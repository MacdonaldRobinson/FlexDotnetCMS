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
            MaxHistory = 20;
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
            var childItems = GetAllChildMediaDetails(item.MediaID, item.LanguageID);
            var tags = new List<Tag>();

            foreach (var childItem in childItems)
                tags.AddRange(childItem.Media.MediaTags.Select(i => i.Tag));

            return tags.Distinct();
        }

        public static IEnumerable<IMediaDetail> GetRelatedItems(IMediaDetail item, long mediaTypeId = 0)
        {
            if (item == null)
                return new List<IMediaDetail>();

            var tagIds = item.Media.MediaTags.Select(k => k.TagID).ToList();

            IEnumerable<IMediaDetail> mediaDetails = new List<IMediaDetail>();

            if (mediaTypeId == 0)
            {
                mediaDetails = BaseMapper.GetDataModel().MediaDetails.Where(i =>i.ID != item.ID && i.HistoryForMediaDetail == null && !i.IsDeleted && i.Media.MediaTags.Select(j => j.TagID).Any(j => tagIds.Contains(j))).OrderByDescending(i => (i.PublishDate == null)? i.DateCreated : i.PublishDate);
            }
            else
            {
                mediaDetails = BaseMapper.GetDataModel().MediaDetails.Where(i => i.ID != item.ID && i.HistoryForMediaDetail == null && i.MediaType.ID == mediaTypeId && !i.IsDeleted && i.Media.MediaTags.Select(j => j.TagID).Any(j => tagIds.Contains(j))).OrderByDescending(i => (i.PublishDate == null) ? i.DateCreated : i.PublishDate);
            }

            return mediaDetails;
        }

        public static IEnumerable<IMediaDetail> GetAllActiveFooterItems()
        {
            return GetAllActiveMediaDetails().Where(i => i.RenderInFooter);
        }

        public static IEnumerable<IMediaDetail> GetAllActiveMediaDetails()
        {
            return MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0 && !i.IsDeleted);
        }

        public static IEnumerable<IMediaDetail> GetOnlyActiveAndVisibleMediaDetails(IEnumerable<IMediaDetail> items, IMediaDetail childOfMediaDetail = null)
        {
            return childOfMediaDetail == null ? FilterByShowInMenuStatus(FilterByCanRenderStatus(FilterByIsHistoryStatus(items, false), true), true) : FilterByShowInMenuStatus(FilterByCanRenderStatus(FilterByIsHistoryStatus(items, false), true).Where(i => i.Media.ParentMediaID == childOfMediaDetail.Media.ID), false);
        }


        public static string ConvertATagsToShortCodes(string content)
        {
            if (!content.Contains("<a"))
                return content;

            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(content);

            var aTags = document.DocumentNode.SelectNodes("//a");

            if (aTags == null)
                return content;

            foreach (var aTag in aTags)
            {
                var href = aTag.Attributes["href"]?.Value;

                if (!string.IsNullOrEmpty(href))
                {
                    if (href.StartsWith("{"))
                        continue;

                    if (href.StartsWith("http") && !href.Contains(URIHelper.BaseUrl))
                        continue;

                    href = URIHelper.ConvertToAbsUrl(href);
                    var uri = new Uri(href);

                    var absPath = URIHelper.ConvertAbsUrlToTilda(uri.AbsolutePath);

                    var mediaDetail = BaseMapper.GetDataModel().MediaDetails.Where(i => i.CachedVirtualPath == absPath && i.HistoryVersionNumber == 0)?.FirstOrDefault();

                    if (mediaDetail != null)
                    {
                        aTag.Attributes["href"].Value = "{Link:" + mediaDetail.MediaID + "}" + uri.Query + uri.Fragment;
                    }
                }
            }

            return document.DocumentNode.WriteContentTo();
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

        public static IEnumerable<IMediaDetail> GetAllActiveByMediaType(long mediaTypeId, int take = -1)
        {
            if(take > 0)
                return MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0 && i.MediaType.ID == mediaTypeId && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate >= DateTime.Now)).Take(take).OrderByDescending(i=>i.Media.OrderIndex);
            else
                return MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0 && i.MediaType.ID == mediaTypeId && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate >= DateTime.Now)).OrderByDescending(i => i.Media.OrderIndex);
        }

        public static IMediaDetail GetByID(long id)
        {
            return MediaDetailsMapper.GetDataModel().MediaDetails.FirstOrDefault(item => item.ID == id);
        }

        public static IMediaDetail GetByVirtualPath(string virtualPath, bool selectParentIfPossible = false, bool saveLanguage = true)
        {
            if(virtualPath.StartsWith("/"))
            {
                virtualPath = $"~{virtualPath}";
            }
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

            var item = BaseMapper.GetDataModel().MediaDetails.Where(i => (i.CachedVirtualPath == virtualPathByCurrentHost || i.CachedVirtualPath == virtualPath) && i.LanguageID == currentLanguage.ID && i.HistoryVersionNumber == versionNumber).ToList().Where(i => i.CanRender || HttpContext.Current.Request["version"] != null).OrderByDescending(i => i.DateLastModified).FirstOrDefault();

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
            return media.MediaDetails.FirstOrDefault(i => i.HistoryVersionNumber == historyVersion && i.LanguageID == language.ID);
        }

        public static IMediaDetail GetAtleastOneByMedia(Media media, Language language)
        {
            var detail = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.HistoryForMediaDetailID == null && i.LanguageID == language.ID && i.MediaID == media.ID && i.MediaType.ShowInSiteTree);

            if (detail == null)
            {
                var defaultLanguage = LanguagesMapper.GetDefaultLanguage();
                detail = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.HistoryForMediaDetailID == null && i.MediaID == media.ID && i.LanguageID == defaultLanguage.ID && i.MediaType.ShowInSiteTree);
            }

            return detail;
        }

        public static IEnumerable<IMediaDetail> GetAtleastOneChildByMedia(Media media, Language language)
        {
            return media.ChildMedias.Select(i => GetAtleastOneByMedia(i, language)).Where(i=>i != null);
        }

        public static IMediaDetail GetAtleastOneByMediaID(long mediaId, Language language)
        {
            return GetAtleastOneByMedia(MediasMapper.GetByID(mediaId), language);
        }

        public static IEnumerable<IMediaDetail> GetAllChildMediaDetails(long mediaId, long languageId)
        {
            if (languageId == 0)
                languageId = LanguagesMapper.GetDefaultLanguage().ID;

            var children = BaseMapper.GetDataModel().MediaDetails.Where(i => i.HistoryForMediaDetailID == null && i.Media.ParentMediaID == mediaId && i.LanguageID == languageId && i.MediaType.ShowInSiteTree);

            if (children.Count() > 0)
            {
                return children.OrderBy(i => i.Media.OrderIndex);
            }
            else
            {
                return new List<IMediaDetail>();
            }
        }

        public static IMediaDetail CreateObject(long mediaTypeId, Media mediaItem, Media parentMedia)
        {
            if (mediaTypeId == 0)
                return new Page();

            IMediaDetail detail;
            MediaTypeEnum mediaTypeEnum = MediaTypeEnum.Page;
            var mediaType = MediaTypesMapper.GetByID(mediaTypeId);

            if (mediaType == null)
                return new Page();

            Enum.TryParse(mediaType.Name, out mediaTypeEnum);

            var language = FrameworkSettings.GetCurrentLanguage();

            detail = CreateByMediaTypeEnum(mediaTypeEnum);

            detail.Media = mediaItem != null ? (GetObjectFromContext(mediaItem) ?? MediasMapper.CreateObject()) : MediasMapper.CreateObject();

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

        public static IEnumerable<Media> GetAllParentMedias(Media item)
        {
            var items = new List<Media>();
            var absoluteRoot = MediasMapper.GetAbsoluteRoot();

            if ((item.ParentMediaID != null) && (item.ParentMediaID != absoluteRoot.ID))
                items.Add(item);

            var parentMedia = item.ParentMedia;

            while (parentMedia != null)
            {
                if (item == null)
                    break;

                if (item.ParentMediaID == null)
                    break;

                if (parentMedia == null)
                    parentMedia = BaseMapper.GetDataModel().AllMedia.FirstOrDefault(i => i.ID == (long)item.ParentMediaID);

                if (parentMedia == null)
                    break;

                if (item.ID != absoluteRoot.ID)
                    items.Add(parentMedia);

                parentMedia = parentMedia.ParentMedia;
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
                var childMedias = obj.Media.ChildMedias.Where(i => {
                    var liveMediaDetail = i.GetLiveMediaDetail();

                    if (liveMediaDetail != null && liveMediaDetail.HistoryVersionNumber == 0 && liveMediaDetail.MediaType.ShowInSiteTree)
                    {
                        return true;
                    }

                    return false;                        
                });

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

        public static string ParseWithTemplate(IMediaDetail mediaDetail)
        {
            var html = ParseSpecialTags(mediaDetail);
            var masterPage = mediaDetail.GetMasterPage();

            if (masterPage != null)
            {
                if (masterPage.UseLayout)
                {
                    html = masterPage.Layout.Replace("{PageContent}", html);
                }
            }

            return html;
        }

        public static string ParseSpecialTags(IMediaDetail mediaDetail, string propertyName = "{UseMainLayout}", int previousCount = 0, object passToParser = null)
        {
            if (mediaDetail == null)
                return "";

            var customCode = propertyName;

            if (customCode.Contains("{{Load"))
            {
                var loadMediaDetailsProperty = Regex.Matches(customCode, "{{Load:[0-9]+}.[{}a-zA-Z0-9\\[\\]\\(\\=\"\"\\:).?&}]+");

                foreach (var item in loadMediaDetailsProperty)
                {
                    var itemAsString = item.ToString();

                    var loadMediaSegments = Regex.Matches(itemAsString, "{Load:[0-9]+}");

                    foreach (var loadMediaSegment in loadMediaSegments)
                    {
                        var prop = loadMediaSegment.ToString().Replace("{", "").Replace("}", "");
                        var id = long.Parse(prop.Split(':')[1]);

                        var property = itemAsString.Replace(loadMediaSegment.ToString() + ".", "");

                        if (mediaDetail.Media.ParentMediaID != id)
                        {
                            var selectMedia = MediasMapper.GetByID(id);

                            if (selectMedia != null)
                            {
                                var returnValue = property;
                                var replaceShortCodes = returnValue.Contains("?ReplaceShortCodes");

                                var liveMediaDetail = selectMedia.GetLiveMediaDetail();

                                returnValue = ParseSpecialTags(liveMediaDetail, returnValue);

                                if (replaceShortCodes)
                                {
                                    returnValue = ParseSpecialTags(mediaDetail, returnValue);
                                }
                                else
                                {
                                    returnValue = ParseSpecialTags(liveMediaDetail, returnValue);
                                }

                                customCode = customCode.Replace(itemAsString, returnValue);
                            }
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
                var fields = Regex.Matches(customCode, "{Field:[a-zA-Z0-9:=\"\".]+}");

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
                var linkShortCodes = Regex.Matches(customCode, "{Link:[a-zA-Z0-9:=\"\".]+}");

                foreach (var linkShortCode in linkShortCodes)
                {
                    var mediaId = linkShortCode.ToString().Replace("{Link:", "").Replace("}", "");
                    long id = 0;

                    if (long.TryParse(mediaId, out id))
                    {
                        var media = MediasMapper.GetByID(id);

                        if (media != null)
                        {
                            var liveMediaDetail = media.GetLiveMediaDetail();

                            if (liveMediaDetail != null)
                            {
                                customCode = customCode.Replace(linkShortCode.ToString(), liveMediaDetail.AbsoluteUrl);
                            }
                        }
                        else
                        {
                            customCode = customCode.Replace(linkShortCode.ToString(), "#");
                        }
                    }
                }
            }

            if (customCode.Contains("{Settings:"))
            {
                var settingsShortCodes = Regex.Matches(customCode, "{Settings:[a-zA-Z0-9:=\"\".]+}");

                foreach (var settingsShortCode in settingsShortCodes)
                {
                    var setting = settingsShortCode.ToString().Replace("Settings:", "");
                    var returnString =  ParserHelper.ParseData(setting, SettingsMapper.GetSettings());

                    customCode = customCode.Replace(settingsShortCode.ToString(), returnString);
                }
            }

            if (passToParser == null)
            {
                passToParser = mediaDetail;
            }

            customCode = ParserHelper.ParseData(customCode, passToParser);

            var matches = Regex.Matches(customCode, "{[a-zA-Z0-9:=\"\".(),\']+}");

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