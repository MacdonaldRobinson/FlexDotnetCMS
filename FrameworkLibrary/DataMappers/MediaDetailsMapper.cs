using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.IO;
using RazorEngine;

namespace FrameworkLibrary
{
    public class RazorFieldParams
    {
        public Field Field { get; set; }
        public Control Control { get; set; }
        public IMediaDetail MediaDetail { get; set; }
        public Dictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();
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

        public static Dictionary<Tag, List<MediaDetail>> GetRelatedItemsByTags(IMediaDetail item, long mediaTypeId = 0)
        {
            if (item == null)
                return new Dictionary<Tag, List<MediaDetail>>();

            var tags = item.Media.MediaTags.Select(k => k.Tag).ToList();
            var tagItems = new Dictionary<Tag, List<MediaDetail>>();

            foreach (var tag in tags)
            {
                var mediaDetails = new List<MediaDetail>();
                if (mediaTypeId == 0)
                {
                    mediaDetails = BaseMapper.GetDataModel().MediaDetails.Where(i => i.ID != item.ID && i.HistoryForMediaDetail == null && !i.IsDeleted && i.Media.MediaTags.Select(j => j).Any(j => j.TagID == tag.ID)).OrderByDescending(i => (i.PublishDate == null) ? i.DateCreated : i.PublishDate).ToList();
                }
                else
                {
                    mediaDetails = BaseMapper.GetDataModel().MediaDetails.Where(i => i.ID != item.ID && i.HistoryForMediaDetail == null && i.MediaType.ID == mediaTypeId && !i.IsDeleted && i.Media.MediaTags.Select(j => j).Any(j => j.TagID == tag.ID)).OrderByDescending(i => (i.PublishDate == null) ? i.DateCreated : i.PublishDate).ToList();
                }

                if (mediaDetails.Any())
                {
                    tagItems.Add(tag, mediaDetails);
                }
            }

            return tagItems;
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


        public static string ConvertUrlsToShortCodes(string content)
        {
            if (!content.Contains("/"))
                return content;

            if (!content.Contains("<a"))
            {
                var mediaDetail = MediaDetailsMapper.GetByVirtualPath(URIHelper.ConvertAbsUrlToTilda(content));

                if (mediaDetail != null)
                    return mediaDetail.Media.PermaShortCodeLink;

                return content;
            }

            string pattern = @"/[/a-zA-Z0-9-.]{3,}";
            var newString = Regex.Replace(content, pattern, match => {
                var mediaDetail = MediaDetailsMapper.GetByVirtualPath(URIHelper.ConvertAbsUrlToTilda(match.Value));

                if (mediaDetail != null)
                {
                    return mediaDetail.Media.PermaShortCodeLink;
                }
                else
                {
                    return match.Value;
                }
            });

            /*var document = new HtmlAgilityPack.HtmlDocument();
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

            return document.DocumentNode.WriteContentTo();*/

            return newString;
        }

        public static IEnumerable<IMediaDetail> SearchForTerm(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm) || searchTerm.Length < 3)
                return new List<IMediaDetail>();

            var currentLanguage = FrameworkSettings.GetCurrentLanguage();

            searchTerm = searchTerm.ToLower().Trim();
            var foundItems = BaseMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.ShowInSiteTree &&                                                                            
                                                                            !i.IsDeleted &&
                                                                            i.HistoryVersionNumber == 0 &&
                                                                            i.LanguageID == currentLanguage.ID &&
                                                                            (i.MediaID.ToString() == searchTerm ||
                                                                                i.Fields.FirstOrDefault(j => j.FieldCode == "MainContent").FieldValue.ToLower().Contains(searchTerm) ||
                                                                                i.Fields.FirstOrDefault(j => j.FieldCode == "ShortDescription").FieldValue.ToLower().Contains(searchTerm) ||
                                                                                i.Fields.FirstOrDefault(j => j.FieldCode == "SectionTitle").FieldValue.ToLower().Contains(searchTerm) ||
                                                                                i.MainLayout.ToLower().Contains(searchTerm) ||
                                                                                i.MediaType.MainLayout.ToLower().Contains(searchTerm) ||
                                                                                i.Fields.Any(j => (j.FieldAssociations.Count == 0 && j.FieldValue.ToLower().Contains(searchTerm)) ||
                                                                                                j.FieldAssociations.Any(k => !k.MediaDetail.MediaType.ShowInSiteTree &&
                                                                                                                            (k.MediaDetail.Fields.FirstOrDefault(l => l.FieldCode == "SectionTitle").FieldValue.ToLower().Contains(searchTerm) ||
                                                                                                                            k.MediaDetail.Fields.FirstOrDefault(l => l.FieldCode == "MainContent").FieldValue.ToLower().Contains(searchTerm) ||
                                                                                                                            k.MediaDetail.Fields.FirstOrDefault(l => l.FieldCode == "MainLayout").FieldValue.ToLower().Contains(searchTerm))
                                                                                                                        ))
                                                                            )).OrderByDescending(i=>i.PublishDate).ToList().Where(i=>i.CanRender && !i.HasADeletedParent());


            return foundItems;
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
            return items.Where(item => item.Media.RolesMedias.Where(i => i.RoleID == role.ID).Any());
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

        public static Return CanAccessMediaDetail(IMediaDetail mediaDetail, User user, bool checkParents = true)
        {
            var returnObj = GenerateReturn();

            var mediaType = MediaTypesMapper.GetByID(mediaDetail.MediaTypeID);

            if (mediaType == null)
                return returnObj;

            if (FrameworkSettings.CurrentUser.IsInRole(RoleEnum.Developer))
                return returnObj;

            var parentLimitedRoles = mediaDetail.GetParentMediaDetails().Where(i => i.CanLimitedRolesAccessAllChildPages).SelectMany(i => i.GetRoles());

            if (parentLimitedRoles.Any())
            {
                if (user.IsInRoles(parentLimitedRoles))
                {
                    return returnObj;
                }
            }

            if (mediaDetail.Media.RolesMedias.Count != 0)
            {
                var limitedtoRoles = String.Join(",", mediaDetail.Media.RolesMedias.Select(i => i.Role.Name));
                if (user.Roles.Count == 0)
                {
                    returnObj = GenerateReturn("Cannot access item", $"You do not belong to any roles, this item is restricted to certin roles: '{limitedtoRoles}'");
                }
                else if (!mediaDetail.Media.RolesMedias.Where(i => user.IsInRole(i.Role)).Any())
                {
                    returnObj = GenerateReturn("Cannot access item", $"The roles that you belong to do not have permissions to access this item: '{limitedtoRoles}'");
                }
                else if ((mediaDetail.Media.UsersMedias.Count != 0) && (!mediaDetail.Media.UsersMedias.Where(i => i.UserID == user.ID).Any()))
                {
                    returnObj = GenerateReturn("Cannot access item", "You do not have permissions to access this item");
                }

                return returnObj;
            }
            else if (mediaType.MediaTypesRoles.Count != 0)
            {
                if (!user.IsInRoles(mediaType.GetRoles()))
                {
                    returnObj = GenerateReturn("Cannot access item", $"The roles that you belong to do not have permissions to access this media type: '{mediaType.Name}'");
                    return returnObj;
                }
            }

            if (checkParents)
            {
                var parentMediaDetail = mediaDetail.Media.ParentMedia?.GetLiveMediaDetail();

                if(parentMediaDetail != null)
                {
                    returnObj = CanAccessMediaDetail(parentMediaDetail, user);
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
        
        public static IEnumerable<IMediaDetail> GetItemsByMediaTypeAndLanguage(long mediaTypeId, long languageId = 0)
        {
            if(languageId == 0)
            {
                languageId = FrameworkSettings.GetCurrentLanguage().ID;
            }

            return MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.ID == mediaTypeId && i.LanguageID == languageId && i.HistoryVersionNumber == 0 && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now));            
        }

        public static IEnumerable<IMediaDetail> GetItemsWhereFieldAssociationsAreTheSame(long mediaId, List<string> fieldCodes, long mediaTypeId, int take = -1)
        {
            var mediaDetail = MediasMapper.GetByID(mediaId)?.GetLiveMediaDetail();

            if (mediaDetail == null)
                return new List<IMediaDetail>();

            var fieldCodeMediaIds = new Dictionary<string, List<long>>();

            foreach (var fieldCode in fieldCodes)
            {
                var fieldAssociations = ((MediaDetailField)mediaDetail.LoadField(fieldCode))?.FieldAssociations;
                var mediaIds = new List<long>();

                if (fieldAssociations != null)
                {
                    foreach (var association in fieldAssociations)
                    {
                        mediaIds.Add(association.MediaDetail.MediaID);
                    }
                }

                fieldCodeMediaIds.Add(fieldCode, mediaIds);
            }
            
            //return MediaDetailsMapper.GetItemsByMediaTypeAndLanguage(mediaTypeId, mediaDetail.LanguageID).Where(i => i.Fields.Any(j => j.FieldCode == FieldCode && j.FieldAssociations.Any(k => mediaIds.Contains(k.MediaDetail.MediaID)))).ToList();
            var items =  MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.ID == mediaTypeId && i.LanguageID == mediaDetail.LanguageID && i.HistoryVersionNumber == 0 && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now)).OrderByDescending(i=>i.DateCreated).ToList().Where(i=> 
                                                                                    i.Fields.Any(j => fieldCodeMediaIds.Keys.Contains(j.FieldCode) && 
                                                                                                    j.FieldAssociations.Any(k => fieldCodeMediaIds[j.FieldCode].Contains(k.MediaDetail.MediaID))
                                                                                    ));

            if(take > 0)
            {
                items = items.Take(take).ToList();
            }
            else
            {
                items = items.ToList();
            }

            return items;

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

                    var mediaDetail = GetAllActiveMediaDetails().FirstOrDefault(i => i.VirtualPath == virtualPath && i.Language == activeLanguage && i.MediaType.ShowInSiteTree);

                    if (mediaDetail != null)
                        return mediaDetail;

                    break;
                }
            }

            var activeLanguagesCount = activeLanguages.Count();

            if ((virtualPath == "~/") && (activeLanguagesCount > 1))
                virtualPath = virtualPath + LanguagesMapper.GetDefaultLanguage().CultureCode;

            var versionNumber = 0;

            if ((HttpContext.Current.Request["version"] != null) && (virtualPath != "~/"))
            {
                versionNumber = int.Parse(HttpContext.Current.Request["version"]);
            }

            var currentLanguage = FrameworkSettings.GetCurrentLanguage();

            var virtualPathByCurrentHost = virtualPath.Replace("~/", websiteByCurrentHost?.VirtualPath);

            if (virtualPathByCurrentHost.Contains(".aspx"))
                return null;

            //var item = BaseMapper.GetDataModel().MediaDetails.Where(i => i.LanguageID == currentLanguage.ID && i.HistoryVersionNumber == 0 && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now) && (i.CachedVirtualPath == virtualPathByCurrentHost || i.CachedVirtualPath == virtualPath) && i.LanguageID == currentLanguage.ID && i.HistoryVersionNumber == versionNumber && i.MediaType.ShowInSiteTree).ToList().Where(i => i.CanRender || HttpContext.Current.Request["version"] != null).OrderByDescending(i => i.DateLastModified).FirstOrDefault();



            var item = BaseMapper.GetDataModel().MediaDetails.Where(i => i.LanguageID == currentLanguage.ID && i.HistoryVersionNumber == versionNumber &&
                                                                i.MediaType.ShowInSiteTree && (i.CachedVirtualPath == virtualPathByCurrentHost || i.CachedVirtualPath == virtualPath)
                                                        ).OrderByDescending(i => i.DateLastModified).FirstOrDefault();

			if (item != null)
				return item;

 			/*if (item != null && (item.HasADeletedParent() || item.CanRender))
			{
				if (FrameworkSettings.CurrentUser != null && FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.AccessAdvanceOptions))
				{
					return item;
				}
				else
				{
					item = null;
				}
			}*/

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
            var listMediaDetail = new List<IMediaDetail>();

            var childMedias = BaseMapper.GetDataModel().AllMedia.Where(i => i.ParentMediaID == media.ID && i.MediaDetails.Any(j=>j.MediaType.ShowInSiteTree)).OrderBy(i => i.OrderIndex).ToList();

            foreach (var item in childMedias)
            {
                var mediaDetail = GetAtleastOneByMedia(item, language);

                if(mediaDetail != null)
                {
                    listMediaDetail.Add(mediaDetail);
                }

            }

            return listMediaDetail;
        }

        public static IMediaDetail GetAtleastOneByMediaID(long mediaId, Language language)
        {
            return GetAtleastOneByMedia(MediasMapper.GetByID(mediaId), language);
        }

        public static IEnumerable<IMediaDetail> GetAllChildMediaDetails(long mediaId, long languageId)
        {
            if (languageId == 0)
                languageId = LanguagesMapper.GetDefaultLanguage().ID;

            var children = BaseMapper.GetDataModel().MediaDetails.Where(i => i.HistoryForMediaDetailID == null && i.Media.ParentMediaID == mediaId && i.HistoryVersionNumber == 0 && i.LanguageID == languageId && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now)).OrderBy(i => i.Media.OrderIndex).ToList();

            if (!children.Any())
            {
                return new List<IMediaDetail>();
            }

            return children;
        }

        public static IMediaDetail CreateObject(long mediaTypeId, Media mediaItem, Media parentMedia, bool createMediaTypeFields = true)
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

            detail.UseMediaTypeLayouts = true;
            detail.UseDefaultLanguageLayouts = true;

            if (createMediaTypeFields)
            {
                foreach (var mediaTypeField in mediaType.Fields)
                {
                    var mediaDetailField = new MediaDetailField();
                    mediaDetailField.CopyFrom(mediaTypeField);
                    mediaDetailField.UseMediaTypeFieldFrontEndLayout = true;
                    mediaDetailField.UseMediaTypeFieldDescription = true;
                    mediaDetailField.MediaTypeFieldID = mediaTypeField.ID;

                    detail.Fields.Add(mediaDetailField);
                }
            }

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
            var details = obj.GetAllParentMediaDetails(language.ID).ToList();
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

        public static IMediaDetail GetNearestParentWhichContainsFieldCode(IMediaDetail item, Language language, string FieldCode)
        {
            return GetParentsWhichContainsFieldCode(item, language, FieldCode).FirstOrDefault();
        }

        public static IEnumerable<IMediaDetail> GetParentsWhichContainsFieldCode(IMediaDetail item, Language language, string FieldCode)
        {
            var parents = item.GetAllParentMediaDetails(language.ID);
            var parentsWithField = parents.Where(i => i.ID != item.ID && i.Fields.Any(j => j.FieldCode == FieldCode));

            return parentsWithField.Reverse();
        }

		/*public static IEnumerable<IMediaDetail> GetAllParentMediaDetails(IMediaDetail item, Language language, bool ignoreCanRender = false)
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

				if (ignoreCanRender)
				{
					item = parentMedia.MediaDetails.FirstOrDefault(i => i.LanguageID == language.ID && !i.IsHistory);
				}
				else
				{
					item = parentMedia.MediaDetails.FirstOrDefault(i => i.CanRender && i.LanguageID == language.ID && !i.IsHistory);
				}

                if (item == null)
                    break;

                if (item.Media.ID != absoluteRoot.ID)
                    items.Add(item);
            }

            items.Reverse();

            return items;
        }*/

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
            var currentUser = FrameworkSettings.CurrentUser;

            if (currentUser == null)
            {
                currentUser = UsersMapper.GetAllByRoleEnum(RoleEnum.Developer).ElementAt(0);
            }

            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;
            obj.CreatedByUserID = currentUser.ID;
            obj.LastUpdatedByUserID = currentUser.ID;
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

        public static IMediaDetail ClearObjectRelations(IMediaDetail obj)
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
                        var returnObj = ClearObjectRelations(association.MediaDetail);
                        MediaDetailsMapper.DeleteObjectFromContext((MediaDetail)returnObj);
                    }

                    if (association.MediaDetail != null)
                        GetDataModel().FieldAssociations.Remove(association);
                }

                GetDataModel().Fields.Remove(item);
            }

            /*var roleMedia = obj.Media.RolesMedias.ToList();

            foreach (RoleMedia item in roleMedia)
            {
                GetDataModel().RolesMedias.Remove(item);
            }*/

            var fieldAssociations = obj.FieldAssociations.ToList();
            foreach (var item in fieldAssociations)
            {
                GetDataModel().FieldAssociations.Remove(item);
            }

            var comments = obj.Media.Comments.ToList();

            foreach (var item in comments)
                GetDataModel().Comments.Remove(item);

            return obj;
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
            //var visualLayoutEditor = false;
            //var html = mediaDetail.UseMainLayout;

            /*bool.TryParse(HttpContext.Current.Request["VisualLayoutEditor"], out visualLayoutEditor);

            if (!visualLayoutEditor)
            {
                html = ParseSpecialTags(mediaDetail);
            }*/

            var masterPage = mediaDetail.GetMasterPage();

            if (masterPage != null)
            {
                if (masterPage.UseLayout)
                {
                    var html = masterPage.Layout.Replace("{PageContent}", $"<div id='PageContent' data-mediadetailid='{mediaDetail.ID}' data-mediaid='{mediaDetail.MediaID}'>\r\n{mediaDetail.UseMainLayout}\r\n</div>");
                    
                    var parseTemplateLayout = ParseSpecialTags(mediaDetail, html);

                    //html = masterPage.Layout.Replace("{PageContent}", html);

                    /*if (!visualLayoutEditor)
                    {*/
                    //html = ParseSpecialTags(mediaDetail, html);
                    //}

                    return parseTemplateLayout;
                }
            }

			Engine.Razor.Dispose();

			return mediaDetail.UseMainLayout;
        }

        public static string ReplaceFieldWithParsedValue(string originalText, string textToReplace, IField mediaField, string parsedValue, bool includeFieldWrapper, Dictionary<string, string> arguments)
        {
            var byPassEditorCheck = false;
            var mediaId = "";
            var returnObj = BaseMapper.GenerateReturn();

            if (mediaField is MediaDetailField && FrameworkSettings.CurrentUser != null)
            {
                var mediaDetailField = mediaField as MediaDetailField;
                var mediaDetail = (mediaDetailField.MediaDetail != null) ? mediaDetailField.MediaDetail : MediaDetailsMapper.GetByID(mediaDetailField.MediaDetailID);

                if (mediaDetail != null)
                {
                    mediaId = mediaDetail.MediaID.ToString();
                    returnObj = CanAccessMediaDetail(mediaDetail, FrameworkSettings.CurrentUser);
                }
                else
                {
                    returnObj = BaseMapper.GenerateReturn("Cannot get media detail");
                }

                if(returnObj.IsError)
                {
                    includeFieldWrapper = false;
                    byPassEditorCheck = true;
                }
            }

            if(!byPassEditorCheck && arguments.ContainsKey("editor"))
            {
                if(arguments["editor"].ToLower() == "true" || arguments["editor"].ToLower() == "false")
                {
                    bool.TryParse(arguments["editor"], out includeFieldWrapper);
                    mediaField.ShowFrontEndFieldEditor = includeFieldWrapper;
                }     
            }

            if (!byPassEditorCheck && arguments.ContainsKey("GetPathFromHtml"))
            {
                mediaField.ShowFrontEndFieldEditor = false;
                parsedValue = StringHelper.GetPathFromHtml(parsedValue);
            }

            if (mediaField.ShowFrontEndFieldEditor && includeFieldWrapper)
            {
                parsedValue = $"<div class='field' data-fieldid='{mediaField.ID}' data-mediaid='{mediaId}' data-fieldcode='{mediaField.FieldCode}' data-arguments='{StringHelper.ObjectToJson(arguments)}'>{parsedValue}</div>";
            }

            return originalText.Replace(textToReplace, parsedValue);
        }

        public static System.Web.UI.Page ParserPage { get; } = new System.Web.UI.Page();

        public static string ParseSpecialTags(IMediaDetail mediaDetail, string propertyName = "{UseMainLayout}", int previousCount = 0, object passToParser = null, bool includeFieldWrapper = true)
        {
            if (mediaDetail == null)
                return "";

            var customCode = propertyName;

            if (customCode.Contains("{{Load"))
            {
                /*if (customCode.Contains("@"))
                {
                    customCode = ParserHelper.ParseData(customCode, new RazorFieldParams { MediaDetail = mediaDetail });
                }*/

                /*customCode = Regex.Replace(customCode, "{{Load:[0-9]+}.[{}a-zA-Z0-9\\[\\]\\(\\=\"\"\\:@).?&' }]+", me =>
                {
                    var shortCode = me.Value;

                    var itemAsString = me.Value;

                    var loadMediaSegments = Regex.Matches(itemAsString, "{Load:[0-9]+}");

                    foreach (var loadMediaSegment in loadMediaSegments)
                    {
                        var prop = loadMediaSegment.ToString().Replace("{", "").Replace("}", "");
                        var id = long.Parse(prop.Split(':')[1]);

                        var property = itemAsString.Replace(loadMediaSegment.ToString() + ".", "");
                        var selectMedia = MediasMapper.GetByID(id);

                        if (selectMedia != null)
                        {
                            var returnValue = property;
                            var replaceShortCodes = returnValue.Contains("?ReplaceShortCodes");

                            var liveMediaDetail = selectMedia.GetLiveMediaDetail();

                            if (liveMediaDetail == null)
                            {
                                liveMediaDetail = selectMedia.GetLiveMediaDetail(LanguagesMapper.GetDefaultLanguage());
                            }

                            returnValue = ParseSpecialTags(liveMediaDetail, returnValue);

                            if (replaceShortCodes)
                            {
                                returnValue = ParseSpecialTags(mediaDetail, returnValue);
                            }
                            else
                            {
                                returnValue = ParseSpecialTags(liveMediaDetail, returnValue);
                            }

                            //shortCode = shortCode.Replace(itemAsString, returnValue);
                            shortCode = returnValue;
                        }
                    }

                    return shortCode;
                }, RegexOptions.IgnoreCase);*/

                var loadMediaDetailsProperty = Regex.Matches(customCode, "{{Load:[0-9]+}.[{}a-zA-Z0-9\\[\\]\\(\\=\"\"\\:@).?&' }]+");

                foreach (var item in loadMediaDetailsProperty)
                {
                    var itemAsString = item.ToString();

                    var loadMediaSegments = Regex.Matches(itemAsString, "{Load:[0-9]+}");


                    foreach (var loadMediaSegment in loadMediaSegments)
                    {
                        var prop = loadMediaSegment.ToString().Replace("{", "").Replace("}", "");
                        var id = long.Parse(prop.Split(':')[1]);

                        var property = itemAsString.Replace(loadMediaSegment.ToString() + ".", "");
                        var selectMedia = MediasMapper.GetByID(id);

                        if (selectMedia != null)
                        {
                            var returnValue = property;
                            var replaceShortCodes = returnValue.Contains("?ReplaceShortCodes");

                            var liveMediaDetail = selectMedia.GetLiveMediaDetail();

                            if (liveMediaDetail == null)
                            {
                                liveMediaDetail = selectMedia.GetLiveMediaDetail(LanguagesMapper.GetDefaultLanguage());
                            }

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

            if (customCode.Contains("{IncludeFile:"))
            {
                /*customCode = Regex.Replace(customCode, "{IncludeFile:.*}", me =>
                {
                    var shortCode = me.Value;

                    var path = shortCode.Replace("{IncludeFile:", "").Replace("}", "").Replace("'", "");

                    var absPath = HttpContext.Current.Server.MapPath(path);

                    if (File.Exists(absPath))
                    {
                        var fileContent = File.ReadAllText(absPath);
                        //shortCode = shortCode.Replace(shortCode, fileContent);
                        shortCode = fileContent;
                    }

                    return shortCode;
                }, RegexOptions.IgnoreCase);*/

                var loadFileMatches = Regex.Matches(customCode, "{IncludeFile:.*}");

                foreach (var item in loadFileMatches)
                {
                    var path = item.ToString().Replace("{IncludeFile:", "").Replace("}", "").Replace("'", "");

                    var absPath = HttpContext.Current.Server.MapPath(path);

                    if (File.Exists(absPath))
                    {
                        var fileContent = File.ReadAllText(absPath);
                        customCode = customCode.Replace(item.ToString().ToString(), fileContent);
                    }
                }
            }

            if (customCode.Contains("{RenderChildren:"))
            {
                /*customCode = Regex.Replace(customCode, "{RenderChildren:.*}", me =>
                {
                    var shortCode = me.Value;

                    var tempPropertyName = shortCode.Replace("RenderChildren:", "");
                    var tempCode = "";

                    var childMediaDetails = mediaDetail.ChildMediaDetails.OrderBy(i => i.Media.OrderIndex);

                    foreach (var childMediaDetail in childMediaDetails)
                    {
                        tempPropertyName = tempPropertyName.Replace("{{", "{").Replace("}}", "}");
                        var temp = ParseSpecialTags(childMediaDetail, tempPropertyName);

                        if (temp != tempPropertyName)
                            tempCode += ParseSpecialTags(childMediaDetail, temp);
                    }

                    //shortCode = shortCode.Replace(shortCode, tempCode);
                    shortCode = tempCode;

                    return shortCode;
                }, RegexOptions.IgnoreCase);*/

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

            if (customCode.Contains("{ParentField:"))
            {
                /*customCode = Regex.Replace(customCode, "{ParentField:[a-zA-Z0-9&?=]+}", me =>
                {
                    var shortCode = me.Value;

                    var fieldCode = shortCode.Replace("{ParentField:", "{Field:");

                    var shortCodeValue = ParseSpecialTags(mediaDetail.Media.ParentMedia.GetLiveMediaDetail(), fieldCode);

                    //shortCode = shortCode.Replace(shortCode, shortCodeValue);
                    shortCode = shortCodeValue;

                    return shortCode;
                }, RegexOptions.IgnoreCase);*/

                var fields = Regex.Matches(customCode, "{ParentField:[a-zA-Z0-9&?=]+}");

                foreach (var field in fields)
                {
                    var fieldCode = field.ToString().Replace("{ParentField:", "{Field:");

                    var shortCodeValue = ParseSpecialTags(mediaDetail.Media.ParentMedia.GetLiveMediaDetail(), fieldCode);

                    customCode = customCode.Replace(field.ToString(), shortCodeValue);

                }
            }

            if (customCode.Contains("{Field:"))
            {
                /*customCode = Regex.Replace(customCode, "{Field:[a-zA-Z0-9&?=' ]+}", me =>
                {
                    var shortCode = me.Value;

                    var fieldCode = shortCode.Replace("{Field:", "").Replace("}", "");

                    var split = fieldCode.Split('?');
                    var arguments = new Dictionary<string, string>();

                    if (split.Count() > 1)
                    {
                        fieldCode = split[0];

                        foreach (var argumentString in split[1].Split('&'))
                        {
                            var argumentArray = argumentString.Split('=');
                            if (argumentArray.Count() > 1)
                            {
                                arguments.Add(argumentArray[0].ToString(), argumentArray[1].ToString().Replace("'", ""));
                            }
                        }
                    }

                    long fieldId = 0;
                    Field mediaField = null;

                    if (long.TryParse(fieldCode, out fieldId))
                    {
                        mediaField = BaseMapper.GetDataModel().Fields.FirstOrDefault(i => i.ID == fieldId);
                    }
                    else
                    {
                        mediaField = mediaDetail.Fields.FirstOrDefault(i => i.FieldCode == fieldCode);
                    }

                    if (mediaField != null)
                    {

                        var fieldType = ParserHelper.ParseData(mediaField.AdminControl, new RazorFieldParams { Field = mediaField, MediaDetail = mediaDetail, Arguments = arguments, Control = new Control() });

                        var parserPage = new System.Web.UI.Page();
                        parserPage.AppRelativeVirtualPath = "~/";
                        var control = parserPage.ParseControl(fieldType);

                        if (control?.Controls.Count > 0 && control.Controls[0].GetType().FullName.StartsWith("ASP") && mediaField.GetAdminControlValue.Contains("@"))
                        {
                            var tag = mediaField.AdminControl.Replace("/>", "FieldID='" + mediaField.ID.ToString() + "' />");
                            shortCode = shortCode.Replace(shortCode, tag);
                        }
                        else
                        {
                            var frontEndLayout = mediaField.FrontEndLayout;

                            if (mediaField is MediaDetailField)
                            {
                                var mediaDetailField = mediaField as MediaDetailField;
                                if (mediaDetailField.MediaTypeField != null && mediaDetailField.UseMediaTypeFieldFrontEndLayout)
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
                                var parsedValue = ParseSpecialTags(mediaDetail, frontEndLayout, 0, new RazorFieldParams { Control = control, Field = mediaField, MediaDetail = mediaDetail, Arguments = arguments });
                                shortCode = ReplaceFieldWithParsedValue(shortCode, shortCode, mediaField, parsedValue, includeFieldWrapper, arguments);
                            }
                            else
                            {
                                if (mediaField.GetAdminControlValue.Contains("@"))
                                {
                                    var parsedValue = ParseSpecialTags(mediaDetail, mediaField.FieldValue, 0, new RazorFieldParams { Control = control, Field = mediaField, MediaDetail = mediaDetail, Arguments = arguments });
                                    shortCode = ReplaceFieldWithParsedValue(shortCode, shortCode, mediaField, parsedValue, includeFieldWrapper, arguments);
                                    //customCode = customCode.Replace(field.ToString(), parsedValue);
                                }
                                else
                                {
                                    shortCode = ReplaceFieldWithParsedValue(shortCode, shortCode, mediaField, mediaField.FieldValue, includeFieldWrapper, arguments);
                                    //customCode = customCode.Replace(field.ToString(), mediaField.FieldValue);
                                }
                            }
                        }
                    }

                    return shortCode;
                }, RegexOptions.IgnoreCase);*/

                var fields = Regex.Matches(customCode, "{Field:[a-zA-Z0-9&?=' ]+}");

                foreach (var field in fields)
                {
                    var fieldCode = field.ToString().Replace("{Field:", "").Replace("}", "");

                    var split = fieldCode.Split('?');
                    var arguments = new Dictionary<string, string>();

                    if (split.Count() > 1)
                    {
                        fieldCode = split[0];

                        foreach (var argumentString in split[1].Split('&'))
                        {
                            var argumentArray = argumentString.Split('=');
                            if (argumentArray.Count() > 1)
                            {
                                arguments.Add(argumentArray[0].ToString(), argumentArray[1].ToString().Replace("'", ""));
                            }
                        }
                    }

                    long fieldId = 0;
                    Field mediaField = null;

                    if (long.TryParse(fieldCode, out fieldId))
                    {
                        mediaField = BaseMapper.GetDataModel().Fields.FirstOrDefault(i => i.ID == fieldId);
                    }
                    else
                    {
                        mediaField = mediaDetail.Fields.FirstOrDefault(i => i.FieldCode == fieldCode);
                    }

                    if (mediaField == null)
                        continue;

                    var fieldType = ParserHelper.ParseData(mediaField.AdminControl, new RazorFieldParams { Field = mediaField, MediaDetail = mediaDetail, Arguments = arguments, Control = new Control() });

                    //var parserPage = new System.Web.UI.Page();
                    ParserPage.AppRelativeVirtualPath = "~/";
                    var control = ParserPage.ParseControl(fieldType);

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
                            if (mediaDetailField.MediaTypeField != null && mediaDetailField.UseMediaTypeFieldFrontEndLayout)
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
                            var parsedValue = ParseSpecialTags(mediaDetail, frontEndLayout, 0, new RazorFieldParams { Control = control, Field = mediaField, MediaDetail = mediaDetail, Arguments = arguments });
                            customCode = ReplaceFieldWithParsedValue(customCode, field.ToString(), mediaField, parsedValue, includeFieldWrapper, arguments);
                        }
                        else
                        {
                            if (mediaField.GetAdminControlValue.Contains("@"))
                            {
                                var parsedValue = ParseSpecialTags(mediaDetail, mediaField.FieldValue, 0, new RazorFieldParams { Control = control, Field = mediaField, MediaDetail = mediaDetail, Arguments = arguments });
                                customCode = ReplaceFieldWithParsedValue(customCode, field.ToString(), mediaField, parsedValue, includeFieldWrapper, arguments);
                                //customCode = customCode.Replace(field.ToString(), parsedValue);
                            }
                            else
                            {
                                customCode = ReplaceFieldWithParsedValue(customCode, field.ToString(), mediaField, mediaField.FieldValue, includeFieldWrapper, arguments);
                                //customCode = customCode.Replace(field.ToString(), mediaField.FieldValue);
                            }
                        }
                    }
                }
            }

            if (customCode.Contains("{Link:"))
            {
                /*customCode = Regex.Replace(customCode, "{Link:[a-zA-Z0-9:=\"\".]+}", me =>
                {
                    var shortCode = me.Value;

                    var mediaId = shortCode.Replace("{Link:", "").Replace("}", "");
                    long id = 0;

                    if (long.TryParse(mediaId, out id))
                    {
                        var media = MediasMapper.GetByID(id);

                        if (media != null)
                        {
                            var liveMediaDetail = media.GetLiveMediaDetail();

                            if (liveMediaDetail != null)
                            {
                                //shortCode = shortCode.Replace(shortCode, liveMediaDetail.AbsoluteUrl);
                                shortCode = liveMediaDetail.AbsoluteUrl;
                            }
                        }
                        else
                        {
                            //shortCode = shortCode.Replace(shortCode, "#");
                            shortCode = "#";
                        }
                    }

                    return shortCode;
                }, RegexOptions.IgnoreCase);*/

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
                /*customCode = Regex.Replace(customCode, "{Settings:[a-zA-Z0-9:=\"\".]+}", me =>
                {
                    var shortCode = me.Value;

                    var setting = shortCode.Replace("Settings:", "");
                    var returnString = ParserHelper.ParseData(setting, SettingsMapper.GetSettings());

                    //shortCode = shortCode.Replace(shortCode, returnString);
                    shortCode = returnString;

                    return shortCode;
                }, RegexOptions.IgnoreCase);*/

                var settingsShortCodes = Regex.Matches(customCode, "{Settings:[a-zA-Z0-9:=\"\".]+}");

                var settings = SettingsMapper.GetSettings();

                foreach (var settingsShortCode in settingsShortCodes)
                {
                    var setting = settingsShortCode.ToString().Replace("Settings:", "");
                    var returnString =  ParserHelper.ParseData(setting, settings);
                    
                    customCode = customCode.Replace(settingsShortCode.ToString(), returnString);
                }
            }

            if (passToParser == null)
            {
                passToParser = mediaDetail;
            }

            customCode = ParserHelper.ParseData(customCode, passToParser);

            var matches = Regex.Matches(customCode, "{[a-zA-Z0-9:=\"\".(),\'?&]+}");

            if (matches.Count > 0 && matches.Count != previousCount)
            {
                customCode = ParseSpecialTags(mediaDetail, customCode, matches.Count);

                if (customCode == "{UseMainLayout}")
                    customCode = "";
            }

            if (mediaDetail.ShowInMenu && propertyName == "{UseMainLayout}" && customCode != propertyName)
            {
                customCode = "<div class='UseMainLayout' data-mediadetailid='" + mediaDetail.ID + "' data-mediaid='" + mediaDetail.MediaID + "'>" + customCode + "</div>";
            }

            return customCode;
        }
    }
}