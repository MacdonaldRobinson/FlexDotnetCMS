using System;
using System.Collections.Generic;

using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace FrameworkLibrary
{
    public class RazorFieldParams
    {
        public Field Field { get; set; }
        public Control Control { get; set; }
        public IMediaDetail MediaDetail { get; set; }
    }

    public class MediaDetailsMapper : BaseMapper
    {
        static MediaDetailsMapper()
        {
            MaxHistory = 30;
        }

        private const string MapperKey = "MediaDetailsMapperKey";

        public static int MaxHistory { get; private set; }

        public static string GetMapperKey()
        {
            return MapperKey;
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(MapperKey, ContextType.Cache);
        }

        public static IEnumerable<string> GetFilterOptions(IMediaDetail item)
        {
            var filterOptions = new List<string> { "Latest", "Alphabetical" };

            var uniqueTags = GetUniqueTagsInChildren(item);

            filterOptions.AddRange(uniqueTags.Select(tag => tag.Name));

            return filterOptions;
        }

        public static IEnumerable<Tag> GetUniqueTagsInChildren(IMediaDetail item)
        {
            var childItems = GetAllChildMediaDetails(item.Media, item.Language);
            var tags = new List<Tag>();

            foreach (var childItem in childItems)
                tags.AddRange(childItem.Media.MediaTags.Select(i => i.Tag));

            return tags.Distinct();
        }

        public static IEnumerable<IMediaDetail> GetRelatedItems(IMediaDetail item)
        {
            var dictionary = new Dictionary<IMediaDetail, int>();

            if (item == null)
                return new List<IMediaDetail>();

            var tags = item.Media.MediaTags.Select(i => i.Tag);
            var allActive = GetAllActiveMediaDetails();

            foreach (var mediaDetail in allActive)
            {
                foreach (var tag in mediaDetail.Media.MediaTags.Select(i => i.Tag))
                {
                    if (tags.Any(i => i.ID == tag.ID))
                    {
                        if (!dictionary.ContainsKey(mediaDetail))
                        {
                            dictionary.Add(mediaDetail, 1);
                        }
                        else
                        {
                            dictionary[mediaDetail] = dictionary[mediaDetail] + 1;
                        }
                    }
                }
            }

            var ordered = dictionary.OrderByDescending(i => i.Value);

            return ordered.Select(i => i.Key);
        }

        /*private static IQueryable<MediaDetail> GetFromDB()
        {
            using (var context = new Entities(FrameworkBaseMedia.ConnectionSettings.ConnectionString))
            {
                context.ContextOptions.LazyLoadingEnabled = true;
                context.ContextOptions.ProxyCreationEnabled = false;

                var items = context.MediaDetails.Where(i => i.Media != null).OrderBy(i => i.Media.OrderIndex);
                var itemsWithEmptyCachedVirtualPaths = context.MediaDetails.Where(i => i.CachedVirtualPath == "");

                if (itemsWithEmptyCachedVirtualPaths.Any())
                {
                    foreach (var item in itemsWithEmptyCachedVirtualPaths)
                    {
                        item.CachedVirtualPath = item.CalculatedVirtualPath();
                    }
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ErrorHelper.LogException(ex);
                    }
                }

                return items;
            }
        }*/

        public static IEnumerable<IMediaDetail> GetAllActiveFooterItems()
        {
            return GetAllActiveMediaDetails().Where(i => i.RenderInFooter);
        }

        public static IEnumerable<IMediaDetail> GetAllActiveMediaDetails()
        {
            return MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0 && !i.IsDeleted);
            //return FilterByCanRenderStatus(FilterByIsHistoryStatus(GetAll(), false), true);
        }

        public static IEnumerable<IMediaDetail> GetOnlyActiveAndVisibleMediaDetails(IEnumerable<IMediaDetail> items, IMediaDetail childOfMediaDetail = null)
        {
            return childOfMediaDetail == null ? FilterByShowInMenuStatus(FilterByCanRenderStatus(FilterByIsHistoryStatus(items, false), true), true) : FilterByShowInMenuStatus(FilterByCanRenderStatus(FilterByIsHistoryStatus(items, false), true).Where(i => i.Media.ParentMediaID == childOfMediaDetail.Media.ID), false);
        }

        public static IEnumerable<IMediaDetail> SearchForTerm(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm) || searchTerm.Length < 3)
                return new List<IMediaDetail>();

            searchTerm = searchTerm.ToLower();
            var allActiveMediaDetails = GetAllActiveMediaDetails();
            //var rootMediaDetail = FrameworkSettings.RootMediaDetail;
            var currentLanguage = FrameworkSettings.GetCurrentLanguage();

            var items = (IEnumerable<IMediaDetail>)allActiveMediaDetails.Where(i => StringHelper.ContainsWord(i.Title, searchTerm) || (StringHelper.ContainsWord(i.ShortDescription, searchTerm)) || (StringHelper.ContainsWord(i.MainContent, searchTerm))).AsEnumerable();

            items = FilterByShowInSearchResultsStatus(FilterByLanguage(FilterByCanRenderStatus(FilterByIsHistoryStatus(items, false), true), currentLanguage), true);

            var filteredItems = new List<IMediaDetail>();

            var tagMatches = (IEnumerable<IMediaDetail>)allActiveMediaDetails.Where(i => i.Media.MediaTags.Any(j => StringHelper.ContainsWord(j.Tag.Name, searchTerm)));
            tagMatches = FilterByShowInSearchResultsStatus(FilterByLanguage(FilterByCanRenderStatus(FilterByIsHistoryStatus(tagMatches, false), true), currentLanguage), true);

            filteredItems.AddRange(tagMatches);
            var maxResults = 10;

            if (filteredItems.Count < maxResults)
            {
                foreach (var item in items)
                {
                    if (filteredItems.Count >= maxResults)
                        break;

                    if (!filteredItems.Contains(item))
                        filteredItems.Add(item);
                }
            }

            return filteredItems;
        }

        public static IEnumerable<IMediaDetail> FilterByIsDraftStatus(IEnumerable<IMediaDetail> items, bool isDraft)
        {
            return items.Where(item => item.IsDraft == isDraft);
        }

        public static IEnumerable<IMediaDetail> FilterByIsHistoryStatus(IEnumerable<IMediaDetail> items, bool isHistory)
        {
            return items.Where(item => item.IsHistory == isHistory);
        }

        public static IEnumerable<IMediaDetail> FilterByDeletedStatus(IEnumerable<IMediaDetail> items, bool isDeleted)
        {
            return items.Where(item => item.IsDeleted == isDeleted);
        }

        public static IEnumerable<IMediaDetail> FilterByCanRenderStatus(IEnumerable<IMediaDetail> items, bool canRender)
        {
            return items.Where(item => item.CanRender == canRender);
        }

        public static IEnumerable<IMediaDetail> FilterByRoot(IEnumerable<IMediaDetail> items, IMediaDetail root)
        {
            return items.Where(item => item.VirtualPath.StartsWith(root.VirtualPath));
        }

        public static IEnumerable<IMediaDetail> FilterByRenderInFooterStatus(IEnumerable<IMediaDetail> items, bool renderOnlyInFooter)
        {
            return items.Where(item => item.RenderInFooter == renderOnlyInFooter);
        }

        public static IEnumerable<IMediaDetail> FilterByPublishedStatus(IEnumerable<IMediaDetail> items, bool isPublished)
        {
            return items.Where(item => item.IsPublished == isPublished);
        }

        public static IEnumerable<IMediaDetail> FilterByTag(IEnumerable<IMediaDetail> items, Tag tag)
        {
            return items.Where(item => item.Media.MediaTags.Select(i => i.Tag).Where(i => i.ID == tag.ID).Any());
        }

        public static IEnumerable<IMediaDetail> FilterByTags(IEnumerable<IMediaDetail> items, IEnumerable<Tag> tags)
        {
            var filtered = new List<IMediaDetail>();

            foreach (var tag in tags)
                filtered.AddRange(FilterByTag(items, tag));

            return filtered.Distinct();
        }

        public static IEnumerable<IMediaDetail> FilterOutHiddenAndDeleted(IEnumerable<IMediaDetail> items)
        {
            var filtered = FilterByShowInMenuStatus(FilterByDeletedStatus(items, false), true);
            return filtered;
        }

        public static IEnumerable<IMediaDetail> FilterOutHiddenDeletedAndArchived(IEnumerable<IMediaDetail> items)
        {
            var filtered = FilterByArchiveStatus(FilterOutHiddenAndDeleted(items), false);
            return filtered;
        }

        public static IEnumerable<IMediaDetail> FilterOutDeletedAndArchived(IEnumerable<IMediaDetail> items)
        {
            var filtered = FilterByArchiveStatus(FilterByDeletedStatus(items, false), false);
            return filtered;
        }

        public static IEnumerable<IMediaDetail> FilterByArchiveStatus(IEnumerable<IMediaDetail> items, bool isArchive)
        {
            return items.Where(item => item.IsArchive == isArchive);
        }

        public static IEnumerable<IMediaDetail> FilterByPublishStatus(IEnumerable<IMediaDetail> items, bool isPublished)
        {
            return items.Where(item => item.IsPublished == isPublished);
        }

        public static IEnumerable<IMediaDetail> FilterByShowInMenuStatus(IEnumerable<IMediaDetail> items, bool showInMenu)
        {
            return items.Where(item => (item.ShowInMenu == showInMenu && (item.IsPublished)));
        }

        public static IEnumerable<IMediaDetail> FilterByShowInSearchResultsStatus(IEnumerable<IMediaDetail> items, bool showInSearchResults)
        {
            return items.Where(item => (item.ShowInSearchResults == showInSearchResults && (item.IsPublished)));
        }

        public static IEnumerable<IMediaDetail> FilterByMediaTypeShowInMenuStatus(IEnumerable<IMediaDetail> items, bool showInMenu)
        {
            return items.Where(item => (MediaTypesMapper.GetByID(item.MediaTypeID).ShowInMenu == showInMenu && (item.IsPublished)));
        }

        public static IEnumerable<IMediaDetail> FilterByLanguageEnum(IEnumerable<IMediaDetail> items, LanguageEnum languageEnum)
        {
            return FilterByLanguage(items, LanguagesMapper.GetByEnum(languageEnum));
        }

        public static IEnumerable<IMediaDetail> FilterByLanguage(IEnumerable<IMediaDetail> items, Language language)
        {
            return items.Where(item => item.LanguageID == language.ID);
        }

        public static IEnumerable<IMediaDetail> FilterByMediaType(IEnumerable<IMediaDetail> items, MediaTypeEnum mediaTypeEnum)
        {
            var mediaTypeId = MediaTypesMapper.GetByEnum(mediaTypeEnum).ID;
            return items.Where(item => item.MediaTypeID == mediaTypeId);
        }

        public static IEnumerable<IMediaDetail> FilterByRole(IEnumerable<IMediaDetail> items, Role role)
        {
            return items.Where(item => item.RolesMediaDetails.Where(i => i.ID == role.ID).Any());
        }

        public static IEnumerable<IMediaDetail> FilterByRoles(List<IMediaDetail> items, IEnumerable<Role> roles)
        {
            var filtered = new List<IMediaDetail>();

            foreach (var role in roles)
            {
                filtered.AddRange(FilterByRole(items, role));
                items.RemoveAll(filtered.Contains);
            }

            return filtered;
        }

        public static Return CanAccessMediaDetail(IMediaDetail mediaDetail, User user)
        {
            var returnObj = GenerateReturn();

            var mediaType = MediaTypesMapper.GetByID(mediaDetail.MediaTypeID);

            if (mediaType == null)
                return returnObj;

            if (FrameworkSettings.CurrentUser.IsInRole(RoleEnum.Administrator))
                return returnObj;

            if (mediaType.MediaTypesRoles.Count != 0)
            {
                if (!user.IsInRoles(mediaType.GetRoles()))
                {
                    returnObj = GenerateReturn("Cannot access item", "The roles that you belong to do not have permissions to access this media type");
                }
            }
            else if (mediaDetail.RolesMediaDetails.Count != 0)
            {
                if (user.Roles.Count == 0)
                {
                    returnObj = GenerateReturn("Cannot access item", "You do not belong to any role, this item is restricted to certin roles");
                }
                else if (!mediaDetail.RolesMediaDetails.Where(i => user.IsInRole(i.Role)).Any())
                {
                    returnObj = GenerateReturn("Cannot access item", "The roles that you belong to do not have permissions to access this item");
                }
                else if ((mediaDetail.UsersMediaDetails.Count != 0) && (!mediaDetail.UsersMediaDetails.Where(i => i.UserID == user.ID).Any()))
                {
                    returnObj = GenerateReturn("Cannot access item", "You do not have permissions to access this item");
                }
            }

            return returnObj;
        }

        public static IMediaDetail ConvertToMediaType(IMediaDetail mediaDetail, MediaTypeEnum mediaTypeEnum)
        {
            var mediaType = MediaTypesMapper.GetByEnum(mediaTypeEnum);

            if (mediaDetail.MediaTypeID == mediaType.ID)
                return mediaDetail;

            var newItem = CreateObject(mediaType.ID, mediaDetail.Media, mediaDetail.Media.ParentMedia);
            newItem.CopyFrom(mediaDetail);

            newItem.MediaTypeID = mediaType.ID;

            var returnObj = Insert(newItem);

            if (!returnObj.IsError)
                DeletePermanently((MediaDetail)mediaDetail);

            return newItem;
        }

        public static IEnumerable<IMediaDetail> GetByMediaType(MediaType mediaType)
        {
            return MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.Name == mediaType.Name);
        }

        public static IEnumerable<IMediaDetail> GetByMediaType(MediaTypeEnum mediaTypeEnum)
        {
            return GetByMediaType(MediaTypesMapper.GetByEnum(mediaTypeEnum));
        }

        public static IMediaDetail GetByID(long id)
        {
            return MediaDetailsMapper.GetDataModel().MediaDetails.FirstOrDefault(item => item.ID == id);
        }

        public static IMediaDetail GetByVirtualPath(string virtualPath, bool selectParentIfPossible = false, bool saveLanguage = true)
        {
            var forceSelectParent = false;

            var activeLanguages = LanguagesMapper.GetAllActive();
            var websiteByCurrentHost = WebsitesMapper.GetWebsite();

            foreach (var activeLanguage in activeLanguages)
            {
                var activeLaunguageBase = URIHelper.ConvertAbsUrlToTilda(URIHelper.GetBaseUrlWithLanguage(activeLanguage));

                if (virtualPath.StartsWith(activeLaunguageBase))
                {
                    if (saveLanguage)
                        FrameworkSettings.SetCurrentLanguage(activeLanguage);

                    /*websiteByCurrentHost = WebsitesMapper.GetWebsite(activeLanguage);

                    virtualPath = virtualPath.Replace(activeLaunguageBase, URIHelper.ConvertAbsUrlToTilda(URIHelper.BaseUrl));

                    if (!virtualPath.EndsWith("/"))
                        virtualPath = virtualPath + "/";

                    var urlSegments = URIHelper.GetUriSegments(virtualPath);*/

                    /*object requestWebsite = null;

                    if (urlSegments.Any())
                    {
                        var firstSegment = urlSegments.ElementAt(0);
                        requestWebsite = MediaDetailsMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.MediaType.Name == MediaTypeEnum.Website.ToString() && i.LanguageID == activeLanguage.ID && i.HistoryVersionNumber == 0);
                    }

                    if (requestWebsite == null)
                    {
                        if (!virtualPath.StartsWith(websiteByCurrentHost.VirtualPath))
                            virtualPath = virtualPath.Replace("~/", websiteByCurrentHost.VirtualPath);
                    }*/

                    var website = WebsitesMapper.GetWebsite(0, activeLanguage);
                    virtualPath = virtualPath.Replace(URIHelper.ConvertAbsUrlToTilda(URIHelper.BaseUrlWithLanguage), website.CachedVirtualPath);

                    var mediaDetail = GetAllActiveMediaDetails().FirstOrDefault(i => i.VirtualPath == virtualPath && i.Language == activeLanguage);

                    if (mediaDetail != null)
                        return mediaDetail;

                    break;
                }
            }

            var activeLanguagesCount = activeLanguages.Count();

            if ((virtualPath == "~/") && (activeLanguagesCount > 1))
                virtualPath = virtualPath + LanguagesMapper.GetDefaultLanguage().CultureCode;

            var versionNumber = 0;

            if ((FrameworkSettings.CurrentUser != null) && (HttpContext.Current.Request["version"] != null) && (virtualPath != "~/"))
            {
                versionNumber = int.Parse(HttpContext.Current.Request["version"]);
            }

            var currentLanguage = FrameworkSettings.GetCurrentLanguage();

            var virtualPathByCurrentHost = virtualPath.Replace("~/", websiteByCurrentHost?.VirtualPath);

            if (virtualPathByCurrentHost.Contains(".aspx"))
                return null;

            var item = BaseMapper.GetDataModel().MediaDetails.Where(i => (i.CachedVirtualPath == virtualPathByCurrentHost || i.CachedVirtualPath == virtualPath) && i.LanguageID == currentLanguage.ID && i.HistoryVersionNumber == versionNumber).ToList().Where(i => i.CanRender || HttpContext.Current.Request["version"] != null).FirstOrDefault();

            if (item != null)
                return item;

            if (((selectParentIfPossible) || (forceSelectParent)) && virtualPath.Contains(websiteByCurrentHost.AutoCalculatedVirtualPath))
            {
                var segments = URIHelper.GetUriSegments(virtualPath).ToList();

                while (segments.Count > 0)
                {
                    segments.RemoveAt(segments.Count - 1);
                    var testVirtualPath = "~/" + string.Join("/", segments.ToArray());

                    if (!testVirtualPath.EndsWith("/"))
                        testVirtualPath = testVirtualPath + "/";

                    var detail = GetByVirtualPath(testVirtualPath);

                    if (detail == null)
                        return null;

                    if (saveLanguage)
                        FrameworkSettings.SetCurrentLanguage(detail.Language);

                    //if ((!forceSelectParent) && (detail.VirtualPath == GetByMedia(FrameworkSettings.RootMedia, detail.Language).VirtualPath))
                    if (!forceSelectParent)
                        return null;

                    if (detail.VirtualPath == "~/")
                        return WebsitesMapper.GetWebsite();

                    return detail;
                }
            }

            return null;
        }

        public IMediaDetail LoadMediaDetailByVirtualPath(string virtualPath)
        {
            var mediaDetail = GetByVirtualPath(virtualPath, false, false);
            var currentLanguage = FrameworkSettings.GetCurrentLanguage();

            if (mediaDetail != null && mediaDetail.LanguageID != currentLanguage.ID)
            {
                var foundMediaDetail = MediaDetailsMapper.FilterByCanRenderStatus(MediaDetailsMapper.FilterByLanguage(mediaDetail.Media.MediaDetails, currentLanguage), true).FirstOrDefault();

                if (foundMediaDetail != null)
                {
                    mediaDetail = foundMediaDetail;
                }
            }

            return mediaDetail;
        }

        public static IEnumerable<IMediaDetail> GetByMedia(Media media)
        {
            return MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.MediaID == media.ID);

            //var allItems = GetAll();
            //return allItems.Where(item => item.MediaID == media.ID);
        }

        public static IMediaDetail GetBySefTitle(string sefTitle)
        {
            var allItems = GetAllActiveMediaDetails();
            return allItems.FirstOrDefault(item => item.SefTitle == sefTitle);
        }

        public static IEnumerable<IMediaDetail> GetByMedias(IEnumerable<Media> medias, Language language, long historyVersion = 0)
        {
            return medias.Select(item => GetByMedia(item, language, historyVersion));
        }

        public static IMediaDetail GetByMedia(Media media, Language language, long historyVersion = 0)
        {
            //var details = FilterByLanguage(GetByMedia(media), language);

            return media.MediaDetails.FirstOrDefault(i => i.HistoryVersionNumber == historyVersion && i.LanguageID == language.ID);
        }

        public static IMediaDetail GetAtleastOneByMedia(Media media, Language language)
        {
            var detail = media.MediaDetails.FirstOrDefault(i => i.HistoryForMediaDetailID == null && i.LanguageID == language.ID);

            if (detail == null)
            {
                var defaultLanguage = LanguagesMapper.GetDefaultLanguage();
                detail = media.MediaDetails.FirstOrDefault(i => i.HistoryForMediaDetailID == null && i.LanguageID == defaultLanguage.ID);
            }                

            return detail;
        }

        public static IMediaDetail GetAtleastOneByMediaID(long mediaId, Language language)
        {
            return GetAtleastOneByMedia(MediasMapper.GetByID(mediaId), language);
        }

        public static IEnumerable<IMediaDetail> GetAllChildMediaDetails(Media media, Language language)
        {
            if (language == null)
                language = LanguagesMapper.GetDefaultLanguage();

            return media.ChildMedias.Select(i => i.MediaDetails.FirstOrDefault(j => j.Media.ParentMediaID == media.ID && j.LanguageID == language.ID && j.HistoryForMediaDetailID == null)).Where(i => i != null && i.MediaType.ShowInSiteTree).OrderBy(i => i.Media.OrderIndex);

            /*var allItems = MediasMapper.GetAllChildMedias(media);

            return allItems.Where(item => item.ParentMediaID == media.ID).Select(item => GetByMedia(item, language)).Where(detail => detail != null);*/
        }

        public static IMediaDetail CreateObject(long mediaTypeId, Media mediaItem, Media parentMedia)
        {
            if (mediaTypeId == 0)
                return new Page();

            IMediaDetail detail;
            MediaTypeEnum mediaTypeEnum = MediaTypeEnum.Page;
            var mediaType = MediaTypesMapper.GetByID(mediaTypeId);

            Enum.TryParse(mediaType.Name, out mediaTypeEnum);

            var language = FrameworkSettings.GetCurrentLanguage();

            detail = CreateByMediaTypeEnum(mediaTypeEnum);

            detail.Media = mediaItem;

            if (parentMedia != null)
                detail.Media.ParentMediaID = parentMedia.ID;

            detail.LanguageID = language.ID;

            detail.MediaTypeID = mediaTypeId;

            return detail;
        }

        public static IMediaDetail CreateObject(MediaTypeEnum mediaTypeEnum)
        {
            IMediaDetail detail;

            switch (mediaTypeEnum)
            {
                case MediaTypeEnum.Page:
                    detail = GetDataModel().MediaDetails.Create<Page>();
                    break;

                case MediaTypeEnum.RootPage:
                    detail = GetDataModel().MediaDetails.Create<RootPage>();
                    break;

                case MediaTypeEnum.UrlRedirectRule:
                    detail = GetDataModel().MediaDetails.Create<UrlRedirectRule>();
                    break;

                case MediaTypeEnum.Website:
                    detail = GetDataModel().MediaDetails.Create<Website>();
                    break;

                default:
                    detail = GetDataModel().MediaDetails.Create<Page>();
                    break;
            }

            return detail;
        }

        public static IMediaDetail CreateByMediaTypeEnum(MediaTypeEnum mediaTypeEnum)
        {
            var detail = CreateObject(mediaTypeEnum);

            var updatableProperties = detail.GetType().GetProperties().Where(i => i.CanWrite && i.PropertyType == typeof(string));

            foreach (var property in updatableProperties)
            {
                property.SetValue(detail, "");
                //ParserHelper.SetValue(detail, property.Name, "");
            }

            detail.LinkTitle = detail.SectionTitle = detail.Title = detail.ShortDescription = detail.MainContent = "New Item";                        
            detail.DateCreated = detail.DateLastModified = DateTime.Now;

            return detail;
        }

        public static string GenerateVirtualPath(MediaDetail obj, Language language)
        {
            var details = GetAllParentMediaDetails(obj, language).ToList();
            var virtualPath = "";

            var counter = 0;

            if (details.Count == 0)
                virtualPath += StringHelper.CreateSlug(obj.LinkTitle);
            else
            {
                foreach (var detail in details)
                {
                    virtualPath += StringHelper.CreateSlug(detail.LinkTitle);

                    if (counter < details.Count() - 1)
                        virtualPath += "/";

                    counter++;
                }
            }

            var uriSegment = "";

            if (LanguagesMapper.GetAllActive().Count() > 1)
                uriSegment = LanguagesMapper.GetByID(obj.LanguageID).UriSegment + "/";

            //virtualPath = "~/" + uriSegment + virtualPath;

            virtualPath = "~/" + virtualPath;

            if (!virtualPath.EndsWith("/"))
                virtualPath = virtualPath + "/";

            return virtualPath;
        }

        public static IEnumerable<IMediaDetail> GetAllParentMediaDetails(IMediaDetail item, Language language)
        {
            var items = new List<IMediaDetail>();
            var absoluteRoot = MediasMapper.GetAbsoluteRoot();

            if ((item.Media.ParentMediaID != null) && (item.Media.ParentMediaID != absoluteRoot.ID))
                items.Add(item);

            while (true)
            {
                if (item == null)
                    break;

                if (item.Media.ParentMediaID == null)
                    break;

                var parentMedia = item.Media.ParentMedia;

                if (parentMedia == null)
                    parentMedia = BaseMapper.GetDataModel().AllMedia.FirstOrDefault(i => i.ID == (long)item.Media.ParentMediaID);
                //parentMedia = MediasMapper.GetByID((long)item.Media.ParentMediaID);

                if (parentMedia == null)
                    break;

                item = parentMedia.MediaDetails.FirstOrDefault(i => i.CanRender && i.LanguageID == language.ID && !i.IsHistory);

                if (item == null)
                    break;

                if (item.Media.ID != absoluteRoot.ID)
                    items.Add(item);
            }

            items.Reverse();

            return items;
        }

        public static Return Insert(IMediaDetail obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;
            obj.CreatedByUserID = FrameworkSettings.CurrentUser.ID;
            obj.LastUpdatedByUserID = FrameworkSettings.CurrentUser.ID;
            obj.DateCreated = obj.DateLastModified = DateTime.Now;
            ((MediaDetail)obj).CachedVirtualPath = obj.CalculatedVirtualPath();

            var returnObj = GenerateReturn();

            if (!obj.IsHistory)
                returnObj = obj.Validate();

            if (!returnObj.IsError)
            {
                returnObj = Insert(MapperKey, (MediaDetail)obj);
            }

            MediaDetailsMapper.ClearCache();
            MediasMapper.ClearCache();

            return returnObj;
        }

        public static Return Update(IMediaDetail obj, bool save = true)
        {
            var mediaDetail = (MediaDetail)obj;
            obj.DateLastModified = DateTime.Now;

            var calculatedVirtualPath = obj.CalculatedVirtualPath();

            if (calculatedVirtualPath != mediaDetail.CachedVirtualPath)
            {
                var childMedias = obj.Media.ChildMedias.Where(i => i.LiveMediaDetail.HistoryVersionNumber == 0 && i.LiveMediaDetail.MediaType.ShowInSiteTree);

                foreach (var item in childMedias)
                {
                    var childMediaDetails = item.MediaDetails.Where(i => i.LanguageID == obj.LanguageID);

                    foreach (var childMediaDetail in childMediaDetails)
                    {
                        Update(childMediaDetail, false);
                    }
                }

                mediaDetail.CachedVirtualPath = calculatedVirtualPath;
            }

            var returnObj = obj.Validate();

            if (!returnObj.IsError)
            {
                if (save)
                {
                    var oldVirtualPath = obj.VirtualPath;

                    returnObj = Update(MapperKey, (MediaDetail)obj);
                }
            }

            return returnObj;
        }

        public static void ClearObjectRelations(IMediaDetail obj)
        {
            var history = obj.History.ToList();
            foreach (MediaDetail item in history)
            {
                ClearObjectRelations(item);                
                GetDataModel().MediaDetails.Remove(item);
            }

            var fields = obj.Fields.ToList();
            foreach (var item in fields)
            {
                var associations = item.FieldAssociations.ToList();
                foreach (var association in associations)
                {
                    if (association.MediaDetail != null && association.MediaDetail.FieldAssociations.Count < 2 && !association.MediaDetail.MediaType.ShowInSiteTree)
                    {
                        ClearObjectRelations(association.MediaDetail);
                        MediaDetailsMapper.DeleteObjectFromContext(association.MediaDetail);
                        //var returnObj = DeletePermanently(association.MediaDetail);
                    }

                    if (association.MediaDetail != null)
                        GetDataModel().FieldAssociations.Remove(association);
                }

                GetDataModel().Fields.Remove(item);
            }

            var roleMediaDetails = obj.RolesMediaDetails.ToList();

            foreach (RoleMediaDetail item in roleMediaDetails)
            {
                GetDataModel().RolesMediaDetails.Remove(item);
            }

            var fieldAssociations = obj.FieldAssociations.ToList();
            foreach (var item in fieldAssociations)
            {
                GetDataModel().FieldAssociations.Remove(item);
            }

            var comments = obj.Media.Comments.ToList();

            foreach (var item in comments)
                GetDataModel().Comments.Remove(item);
        }

        public static Return DeletePermanently(MediaDetail obj)
        {
            obj = GetObjectFromContext(obj);
            var returnObj = GenerateReturn();

            if (obj == null)
            {
                return returnObj;
            }

            var media = obj.Media;
            
            ClearObjectRelations(obj);

            obj.Media.Comments.Clear();

            returnObj = Delete(MapperKey, obj);

            if (media.MediaDetails.Count == 0)
            {
                media.MediaTags.Clear();
                var tmpReturnObj = MediasMapper.DeletePermanently(media);
            }

            return returnObj;
        }

        public static IEnumerable<RssItem> GetRssItems(IEnumerable<IMediaDetail> items)
        {
            return (from item in items where (item.CanRender) && (item.IsPublished) select item.GetRssItem());
        }

        public static string ParseSpecialTags(IMediaDetail mediaDetail, string propertyName = "{UseMainLayout}", int previousCount = 0, object passToParser = null)
        {
            if (mediaDetail == null)
                return "";

            var customCode = propertyName;

            //customCode = ParserHelper.ParseData(propertyName, mediaDetail);

            if (customCode.Contains("{{LoadByMediaDetailID"))
            {
                var loadMediaDetailsProperty = Regex.Matches(customCode, "{{LoadByMediaDetailID:[0-9]+}.[{}a-zA-Z0-9\\[\\]\\(\\=\"\"\\:). }]+");

                foreach (var item in loadMediaDetailsProperty)
                {
                    var loadMediaDetailSegments = Regex.Matches(item.ToString(), "{LoadByMediaDetailID:[0-9]+}");

                    foreach (var loadMediaDetailSegment in loadMediaDetailSegments)
                    {
                        var prop = loadMediaDetailSegment.ToString().Replace("{", "").Replace("}", "");
                        var id = long.Parse(prop.Split(':')[1]);

                        var property = item.ToString().Replace(loadMediaDetailSegment.ToString() + ".", "");

                        if (mediaDetail.Media.ParentMediaID != id)
                        {
                            var selectMediaDetail = MediaDetailsMapper.GetByID(id);
                            var returnValue = ParseSpecialTags(selectMediaDetail, property);

                            customCode = customCode.Replace(item.ToString(), returnValue);
                        }
                    }
                }
            }

            if (customCode.Contains("{RenderChildren:"))
            {
                var renderChildren = Regex.Matches(customCode, "{RenderChildren:.*}");

                foreach (var item in renderChildren)
                {
                    var tempPropertyName = item.ToString().Replace("RenderChildren:", "");
                    var tempCode = "";

                    var childMediaDetails = mediaDetail.ChildMediaDetails.OrderBy(i => i.Media.OrderIndex);

                    foreach (var childMediaDetail in childMediaDetails)
                    {
                        tempPropertyName = tempPropertyName.Replace("{{", "{").Replace("}}", "}");
                        var temp = ParseSpecialTags(childMediaDetail, tempPropertyName);

                        if (temp != tempPropertyName)
                            tempCode += ParseSpecialTags(childMediaDetail, temp);
                    }

                    customCode = customCode.Replace(item.ToString().ToString(), tempCode);
                }
            }

            if (customCode.Contains("{Field:"))
            {
                var fields = Regex.Matches(customCode, "{Field:[a-zA-Z0-9:=\"\". ]+}");

                foreach (var field in fields)
                {
                    var fieldCode = field.ToString().Replace("{Field:", "").Replace("}", "");

                    var segments = fieldCode.Split('=');
                    var firstSegment = segments[0];
                    var tmpIntSegment0 = 0;
                    Field mediaField = null;

                    if (int.TryParse(firstSegment, out tmpIntSegment0))
                    {
                        mediaField = BaseMapper.GetDataModel().Fields.FirstOrDefault(i => i.ID == tmpIntSegment0);
                    }
                    else
                    {
                        mediaField = mediaDetail.Fields.FirstOrDefault(i => i.FieldCode == firstSegment);
                    }

                    if (mediaField == null)
                        continue;

                    if (segments.Count() > 1)
                    {
                        var segment2 = segments[1].Replace("\"", "");

                        if (segment2 == mediaField.FieldValue)
                            customCode = customCode.Replace(field.ToString(), "").Replace("{.", "{");
                    }
                    else
                    {
                        var fieldType = ParserHelper.ParseData(mediaField.AdminControl, new RazorFieldParams { Field = mediaField, MediaDetail = mediaDetail });

                        var parserPage = new System.Web.UI.Page();
                        parserPage.AppRelativeVirtualPath = "~/";
                        var control = parserPage.ParseControl(fieldType);

                        if (control?.Controls.Count > 0 && control.Controls[0].GetType().FullName.StartsWith("ASP") && mediaField.GetAdminControlValue.Contains("@"))
                        {
                            var tag = mediaField.AdminControl.Replace("/>", "FieldID='" + mediaField.ID.ToString() + "' />");
                            customCode = customCode.Replace(field.ToString(), tag);
                        }
                        else
                        {
                            var frontEndLayout = mediaField.FrontEndLayout;

                            if (mediaField is MediaDetailField)
                            {
                                var mediaDetailField = mediaField as MediaDetailField;
                                if (mediaDetailField.UseMediaTypeFieldFrontEndLayout)
                                {
                                    frontEndLayout = mediaDetailField.MediaTypeField?.FrontEndLayout;
                                }
                                else
                                {
                                    frontEndLayout = mediaDetailField.FrontEndLayout;
                                }
                            }

                            if (!string.IsNullOrEmpty(frontEndLayout))
                            {
                                var parsedValue = ParseSpecialTags(mediaDetail, frontEndLayout, 0, new RazorFieldParams { Control = control, Field = mediaField, MediaDetail = mediaDetail });
                                customCode = customCode.Replace(field.ToString(), parsedValue);
                            }
                            else
                            {
                                if (mediaField.GetAdminControlValue.Contains("@"))
                                {
                                    var parsedValue = ParseSpecialTags(mediaDetail, mediaField.FieldValue, 0, new RazorFieldParams { Control = control, Field = mediaField, MediaDetail = mediaDetail });
                                    customCode = customCode.Replace(field.ToString(), parsedValue);
                                }
                                else
                                {
                                    customCode = customCode.Replace(field.ToString(), mediaField.FieldValue);
                                }
                            }
                        }
                    }
                }
            }

            if (customCode.Contains("{Link:"))
            {
                var linkShortCodes = Regex.Matches(customCode, "{Link:[a-zA-Z0-9:=\"\". ]+}");

                foreach (var linkShortCode in linkShortCodes)
                {
                    var mediaId = linkShortCode.ToString().Replace("{Link:", "").Replace("}", "");
                    long id = 0;

                    if (long.TryParse(mediaId, out id))
                    {
                        var media = MediasMapper.GetByID(id);

                        if (media != null)
                        {
                            customCode = customCode.Replace(linkShortCode.ToString(), media.LiveMediaDetail.AbsoluteUrl);
                        }
                        else
                        {
                            customCode = customCode.Replace(linkShortCode.ToString(), "#");
                        }
                    }
                }

            }

            if (passToParser == null)
            {
                passToParser = mediaDetail;
            }

            customCode = ParserHelper.ParseData(customCode, passToParser);

            var matches = Regex.Matches(customCode, "{[a-zA-Z0-9:=\"\".(),\' ]+}");

            if (matches.Count > 0 && matches.Count != previousCount)
            {
                customCode = ParseSpecialTags(mediaDetail, customCode, matches.Count);

                if (customCode == "{UseMainLayout}")
                    customCode = "";
            }

            return customCode;
        }
    }
}