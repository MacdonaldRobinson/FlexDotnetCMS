using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameworkLibrary
{
    public partial class MediaDetail : IMustContainID, IMediaDetail
    {
        private List<ValidationError> _validationErrors = new List<ValidationError>();

        public bool CanRender
        {
            get
            {
                return !IsDeleted && IsPublished;
            }
        }

        public bool IsHistory
        {
            get
            {
                return (HistoryVersionNumber > 0);
            }
        }

        public bool IsPublished
        {
            get
            {
                if (PublishDate == null)
                    return false;

                return (((!IsArchive) && (PublishDate <= DateTime.Now)) && ((ExpiryDate == null) || (ExpiryDate > DateTime.Now)));
            }
        }

        public string UseMainLayout
        {
            get
            {
                if (this.UseMediaTypeLayouts)
                    return this.MediaType?.MainLayout;
                else
                    return this.MainLayout;
            }
        }

        public string UseSummaryLayout
        {
            get
            {
                if (this.UseMediaTypeLayouts)
                    return this.MediaType?.SummaryLayout;
                else
                    return this.SummaryLayout;
            }
        }

        public string UseFeaturedLayout
        {
            get
            {
                if (this.UseMediaTypeLayouts)
                    return this.MediaType?.FeaturedLayout;
                else
                    return this.FeaturedLayout;
            }
        }

        public bool CanUserAccessSection(User user)
        {
            if (IsProtected)
            {
                if (user == null)
                    return false;

                if (!user.HasPermission(PermissionsEnum.AccessProtectedSections))
                    return false;
            }

            return true;
        }

        public string GetCacheKey(RenderVersion renderVersion)
        {
            return renderVersion.ToString() + "_" + this.AutoCalculatedVirtualPath.Replace("~", "");
        }

        public IMediaDetail PreviousMediaDetail
        {
            get
            {
                var children = this.Media?.ParentMedia?.LiveMediaDetail?.ChildMediaDetails?.ToList();

                if (children == null)
                    return null;

                var currentIndex = children.FindIndex(i => i.ID == this.ID);
                var previousIndex = currentIndex - 1;

                if (previousIndex < 0)
                    previousIndex = 0;

                var previousMediaDetail = children[previousIndex];

                if (previousMediaDetail.ID == this.ID)
                    return null;

                return previousMediaDetail;
            }
        }

        public IMediaDetail NextMediaDetail
        {
            get
            {
                var children = this.Media?.ParentMedia?.LiveMediaDetail?.ChildMediaDetails?.ToList();

                if (children == null)
                    return null;

                var currentIndex = children.FindIndex(i => i.ID == this.ID);
                var nextIndex = currentIndex + 1;

                if (nextIndex >= children.Count)
                    nextIndex = 0;

                var nextMediaDetail = children[nextIndex];

                if (nextMediaDetail.ID == this.ID)
                    return null;

                return nextMediaDetail;
            }
        }

        public void RemoveFromCache()
        {
            var htmlCacheKey = GetCacheKey(RenderVersion.HTML);
            var mobileCacheKey = GetCacheKey(RenderVersion.Mobile);

            FileCacheHelper.ClearCache(htmlCacheKey);
            RedisCacheHelper.ClearCache(htmlCacheKey);
            ContextHelper.RemoveFromCache(htmlCacheKey);

            FileCacheHelper.ClearCache(mobileCacheKey);
            ContextHelper.RemoveFromCache(mobileCacheKey);

            FileCacheHelper.ClearCache(htmlCacheKey + "?version=0");
            RedisCacheHelper.ClearCache(htmlCacheKey + "?version=0");
            ContextHelper.RemoveFromCache(htmlCacheKey + "?version=0");

            FileCacheHelper.ClearCache(mobileCacheKey + "?version=0");
            RedisCacheHelper.ClearCache(mobileCacheKey + "?version=0");
            ContextHelper.RemoveFromCache(mobileCacheKey + "?version=0");

            var parents = MediaDetailsMapper.GetAllParentMediaDetails(this, this.Language).Where(i=>i.ID != this.ID);

            foreach (var item in parents)
            {
                item.RemoveFromCache();
            }

        }

        public void SaveToMemoryCache(RenderVersion renderVersion, string html, string queryString = "")
        {
            var key = GetCacheKey(renderVersion);

            if (string.IsNullOrEmpty(queryString))
            {
                ContextHelper.SetToCache(key, html);
                ContextHelper.SetToCache(key + "?version=0", html);
            }
            else
            {
                ContextHelper.SetToCache(key + queryString, html);
            }
        }

        public void SaveToFileCache(RenderVersion renderVersion, string html, string queryString = "")
        {
            var cacheKey = GetCacheKey(renderVersion);
            var cache = FileCacheHelper.GetFromCache(cacheKey);

            if (string.IsNullOrEmpty(queryString) && string.IsNullOrEmpty(cache))
            {
                FileCacheHelper.SaveToCache(cacheKey, html);
                FileCacheHelper.SaveToCache(cacheKey + "?version=0", html);
            }
            else if (string.IsNullOrEmpty(cache))
            {
                var items = HttpUtility.ParseQueryString(queryString);

                foreach (var item in items)
                {
                    var str = item + "=" + items[item.ToString()];

                    cacheKey = cacheKey.Replace(str, "");
                }

                cacheKey = cacheKey + queryString;
                cacheKey = cacheKey.Replace("?&", "?").Replace("??", "?").ToLower();

                FileCacheHelper.SaveToCache(cacheKey, html);
            }
        }

        public void SaveToRedisCache(RenderVersion renderVersion, string html, string queryString = "")
        {
            var cacheKey = GetCacheKey(renderVersion);

            if (string.IsNullOrEmpty(queryString))
            {
                RedisCacheHelper.SaveToCache(cacheKey, html);
                RedisCacheHelper.SaveToCache(cacheKey + "?version=0", html);
            }
            else
            {
                var items = HttpUtility.ParseQueryString(queryString);

                foreach (var item in items)
                {
                    var str = item + "=" + items[item.ToString()];

                    cacheKey = cacheKey.Replace(str, "");
                }

                cacheKey = cacheKey + queryString;
                cacheKey = cacheKey.Replace("?&", "?").Replace("??", "?").ToLower();

                RedisCacheHelper.SaveToCache(cacheKey, html);
            }
        }

        public Field LoadField(string fieldCode)
        {
            return Fields.SingleOrDefault(i => i.FieldCode == fieldCode);
        }

        public string RenderShortCode(string shortCode)
        {
            return MediaDetailsMapper.ParseSpecialTags(this, shortCode);
        }

        public string RenderMainLayout()
        {
            return RenderShortCode("{UseMainLayout}");
        }
        public MasterPage GetMasterPage()
        {
            if (this.MasterPage != null)
                return MasterPage;

            if (this.MediaType.MasterPage == null)
                return SettingsMapper.GetSettings().DefaultMasterPage;

            return this.MediaType.MasterPage;

            /*MediaDetail currentMediaDetail = (MediaDetail)this.Media.ParentMedia?.MediaDetails.FirstOrDefault(i=>i.HistoryVersionNumber == 0 && i.LanguageID == this.LanguageID);

            while (currentMediaDetail != null)
            {
                if (currentMediaDetail.MasterPage != null)
                    return currentMediaDetail.MasterPage;

                currentMediaDetail = (MediaDetail)currentMediaDetail.Media.ParentMedia?.MediaDetails.FirstOrDefault(i => i.HistoryVersionNumber == 0 && i.LanguageID == this.LanguageID);
            }

            return MasterPage;*/
        }

        public FrameworkLibrary.Website GetWebsite()
        {
            return WebsitesMapper.GetWebsite(0, this.Language);

            /*var currentItem = this;
            FrameworkLibrary.Website website = null;

            while (currentItem.Media.ParentMediaID != null)
            {
                if (currentItem.MediaTypeID == MediaTypesMapper.GetByEnum(MediaTypeEnum.Website).ID)
                {
                    website = (FrameworkLibrary.Website)currentItem;
                    break;
                }

                currentItem = (MediaDetail)currentItem.Media.ParentMedia.MediaDetails.FirstOrDefault(i => i.HistoryVersionNumber == 0 && i.LanguageID == this.LanguageID);
            }

            return website;*/
        }

        public IEnumerable<IMediaDetail> ChildMediaDetails
        {
            get
            {
                return MediaDetailsMapper.GetAllChildMediaDetails(MediaID, LanguageID).Where(i=>!i.IsDeleted && i.IsPublished);
            }
        }

        public string VirtualPath
        {
            get
            {
                if (this.CachedVirtualPath == "")
                    this.CachedVirtualPath = CalculatedVirtualPath();

                return this.CachedVirtualPath;
            }
        }

        public string CalculatedVirtualPath()
        {
            var parents = MediaDetailsMapper.GetAllParentMediaDetails(this, LanguagesMapper.GetByID(this.LanguageID)).Reverse();

            var virtualPath = "";

            foreach (var parent in parents)
            {
                if (parent is Website)
                    continue;

                virtualPath = StringHelper.CreateSlug(parent.LinkTitle) + "/" + virtualPath;
            }

            if (virtualPath == "")
                virtualPath = "~/";

            virtualPath = URIHelper.ConvertAbsUrlToTilda(virtualPath);

            return virtualPath;
        }

        public IEnumerable<IMediaDetail> GetRelatedItems(long mediaTypeId = 0)
        {
            var relatedItems = MediaDetailsMapper.GetRelatedItems(this, mediaTypeId);

            return relatedItems;
        }

        public bool IsArchive
        {
            get
            {
                return ExpiryDate <= DateTime.Now;
            }
        }

        public List<ValidationError> ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
        }

        public Return Validate()
        {
            if (_validationErrors == null)
                _validationErrors = new List<ValidationError>();

            /*if (Title.Trim() == "")
                _validationErrors.Add(new ValidationError("Title cannot be blank"));

            if (ShortDescription.Trim() == "")
                _validationErrors.Add(new ValidationError("Short Description cannot be blank"));

            /*if (LongDescription.Trim() == "")
                _validationErrors.Add(new ValidationError("Long Description cannot be blank"));*/

            if (LinkTitle.Trim() == "")
                _validationErrors.Add(new ValidationError("Link Title cannot be blank"));

            if (ExpiryDate < PublishDate)
                _validationErrors.Add(new ValidationError("The Publish Date of the item cannot be greater then its expiry date, please check your Publish Settings"));

            return GenerateValidationReturn();
        }

        public Return GenerateValidationReturn()
        {
            var returnObj = BaseMapper.GenerateReturn();

            if (_validationErrors.Count == 0)
                return returnObj;

            var validationMessages = "";

            foreach (var validationError in _validationErrors)
                validationMessages += validationError.Message + "<br />";

            returnObj.Error = ErrorHelper.CreateError(new System.Exception("Validation Error", new System.Exception(validationMessages)));

            return returnObj;
        }

        private string _absoluteUrl = "";

        public string AbsoluteUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_absoluteUrl))
                {
                    _absoluteUrl = URIHelper.ConvertToAbsUrl(AutoCalculatedVirtualPath);
                }

                return _absoluteUrl;
            }
        }

        private string _autoCalculatedVirtualPath = "";
        public void ClearAutoCalculatedVirtualPathCache()
        {
            _autoCalculatedVirtualPath = "";
        }
        public string AutoCalculatedVirtualPath
        {
            get
            {
                var websiteVirtualPaths = WebsitesMapper.GetAllWebsiteVirtualPaths();

                if (websiteVirtualPaths.Count == 0)
                    return "";

                if (!string.IsNullOrEmpty(_autoCalculatedVirtualPath))
                {
                    return _autoCalculatedVirtualPath;
                }

                var httpContext = System.Web.HttpContext.Current;

                if (httpContext != null && httpContext.Request.Url.Segments.Count() >= 2 && websiteVirtualPaths.Contains(httpContext.Request.Url.Segments[1]))
                    return this.VirtualPath;

                if (this.VirtualPath != null)
                {
                    var virtualPath = this.VirtualPath;

                    if (websiteVirtualPaths.Count > 1)
                    {
                        foreach (var websiteVirtualPath in websiteVirtualPaths)
                        {
                            virtualPath = virtualPath.Replace(websiteVirtualPath, URIHelper.ConvertAbsUrlToTilda(URIHelper.LanguageBaseUrl(this.Language)));
                        }

                        return virtualPath;
                    }

                    if (LanguagesMapper.CountAllActive() == 1)
                        virtualPath = virtualPath.Replace(websiteVirtualPaths.ElementAt(0), URIHelper.ConvertAbsUrlToTilda(URIHelper.BaseUrl.ToLower()));
                    else
                        virtualPath = virtualPath.Replace(websiteVirtualPaths.ElementAt(0), URIHelper.ConvertAbsUrlToTilda(URIHelper.LanguageBaseUrl(LanguagesMapper.GetByID(this.LanguageID)).ToLower()));

                    _autoCalculatedVirtualPath = virtualPath;

                    return _autoCalculatedVirtualPath;
                }

                return "";
            }
        }

        public RssItem GetRssItem()
        {
            return PublishDate != null ? new RssItem(Title, GetMetaDescription(), AutoCalculatedVirtualPath, CreatedByUser.UserName, (DateTime)PublishDate, this) : null;
        }

        private string contextMetaDescription { get; set; } = "";
        public string GetMetaDescription()
        {
            if (!string.IsNullOrEmpty(contextMetaDescription))
                return contextMetaDescription;

            var description = StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(MetaDescription));

            if (string.IsNullOrEmpty(description))
            {
                description = StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(this.ShortDescription));
            }

            description = StringHelper.StripHtmlTags(description);

            if ((description == "") || (description == LinkTitle))
            {
                description = StringHelper.StripHtmlTags(StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(this.MainContent)));

                if (description.Length > 255)
                    description = description.Substring(0, 255) + " ...";
            }

            contextMetaDescription = StringHelper.StripHtmlTags(description);

            return contextMetaDescription;
        }

        private string contextMetaKeywords { get; set; } = "";
        public string GetMetaKeywords()
        {
            if (!string.IsNullOrEmpty(contextMetaKeywords))
                return contextMetaKeywords;

            var metaKeywords = MetaKeywords.Trim();

            if (MetaKeywords.Trim() == "")
                contextMetaKeywords = GetMetaDescription();
            else
                contextMetaKeywords = metaKeywords;

            return contextMetaKeywords;
        }

        private string contextPageTitle { get; set; } = "";
        public string GetPageTitle()
        {
            if (FrameworkSettings.CurrentFrameworkBaseMedia == null)
                return "";

            if (!string.IsNullOrEmpty(contextPageTitle))
                return contextPageTitle;

            if (this.Title != this.LinkTitle)
                return this.Title;

            var pageTitle = "";

            var details = MediaDetailsMapper.GetAllParentMediaDetails(this, Language).Reverse().ToList();


            if (details.Count == 0)
                details.Add(WebsitesMapper.GetWebsite());

            var counter = 0;
            foreach (MediaDetail detail in details)
            {
                pageTitle += detail.Title;

                counter++;

                if (counter < details.Count)
                    pageTitle += " - ";
            }

            contextPageTitle = pageTitle;

            return contextPageTitle;
        }

        public bool HasAnyRoles()
        {
            return UsersMediaDetails.Count > 0;
        }

        public bool HasAnyUsers()
        {
            return UsersMediaDetails.Count > 0;
        }

        public bool HasRole(Role role)
        {
            return RolesMediaDetails.Where(i => i.ID == role.ID).Any();
        }

        public bool HasUser(User user)
        {
            return UsersMediaDetails.Where(i => i.UserID == user.ID).Any();
        }

        public string RssVirtualPath
        {
            get
            {
                return AutoCalculatedVirtualPath + "rss/";
            }
        }

        public string JsonVirtualPath
        {
            get
            {
                return AutoCalculatedVirtualPath + "json/";
            }
        }

        public string QRCodeVirtualPath
        {
            get
            {
                return AutoCalculatedVirtualPath + "qrcode/";
            }
        }
    }
}