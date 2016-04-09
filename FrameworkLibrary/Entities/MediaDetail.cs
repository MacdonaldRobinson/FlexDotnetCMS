using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public partial class MediaDetail : IMustContainID, IMediaDetail
    {
        private List<ValidationError> _validationErrors = new List<ValidationError>();
        private List<string> websiteVirtualPaths = WebsitesMapper.GetAllWebsiteVirtualPaths();

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
                    return this.MediaType.MainLayout;
                else
                    return this.MainLayout;
            }
        }

        public string UseSummaryLayout
        {
            get
            {
                if (this.UseMediaTypeLayouts)
                    return this.MediaType.SummaryLayout;
                else
                    return this.SummaryLayout;
            }
        }

        public string UseFeaturedLayout
        {
            get
            {
                if (this.UseMediaTypeLayouts)
                    return this.MediaType.FeaturedLayout;
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

        public void RemoveFromCache()
        {
            var htmlCacheKey = GetCacheKey(RenderVersion.HTML);
            var mobileCacheKey = GetCacheKey(RenderVersion.Mobile);

            FileCacheHelper.ClearCache(htmlCacheKey);
            ContextHelper.RemoveFromCache(htmlCacheKey);

            FileCacheHelper.ClearCache(mobileCacheKey);
            ContextHelper.RemoveFromCache(mobileCacheKey);

            FileCacheHelper.ClearCache(htmlCacheKey + "?version=0");
            ContextHelper.RemoveFromCache(htmlCacheKey + "?version=0");

            FileCacheHelper.ClearCache(mobileCacheKey + "?version=0");
            ContextHelper.RemoveFromCache(mobileCacheKey + "?version=0");
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
                FileCacheHelper.SaveToCache(cacheKey + queryString, html);
            }
        }

        public Field LoadField(string fieldCode)
        {
            return Fields.SingleOrDefault(i => i.FieldCode == fieldCode);
        }

        public MasterPage GetMasterPage()
        {
            if (this.MasterPage != null)
                return MasterPage;

            MediaDetail currentMediaDetail = (MediaDetail)this.ParentMediaDetail;

            while (currentMediaDetail != null)
            {
                if (currentMediaDetail.MasterPage != null)
                {
                    if (currentMediaDetail is FrameworkLibrary.Website)
                        return this.MediaType.MasterPage;

                    return currentMediaDetail.MasterPage;
                }

                currentMediaDetail = (MediaDetail)currentMediaDetail.ParentMediaDetail;
            }

            return MasterPage;
        }

        public FrameworkLibrary.Website GetWebsite()
        {
            var currentItem = this;
            FrameworkLibrary.Website website = null;

            while (currentItem.Media.ParentMediaID != null)
            {
                if (currentItem.MediaTypeID == MediaTypesMapper.GetByEnum(MediaTypeEnum.Website).ID)
                {
                    website = (FrameworkLibrary.Website)currentItem;
                    break;
                }

                currentItem = (MediaDetail)currentItem.ParentMediaDetail;
            }

            return website;
        }

        public IEnumerable<IMediaDetail> ChildMediaDetails
        {
            get
            {
                return MediaDetailsMapper.FilterOutDeletedAndArchived(MediaDetailsMapper.GetAllChildMediaDetails(Media, Language));
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

        public IEnumerable<IMediaDetail> RelatedItems
        {
            get
            {
                /*if (this.VirtualPath == FrameworkSettings.RootMediaDetail.VirtualPath)
                    return new List<IMediaDetail>();*/

                var relatedItems = MediaDetailsMapper.GetRelatedItems(this);

                return relatedItems;
            }
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

        public IMediaDetail ParentMediaDetail
        {
            get
            {
                return Media.ParentMedia == null ? null : MediaDetailsMapper.GetAtleastOneByMedia(Media.ParentMedia, LanguagesMapper.GetByID(LanguageID));
            }
        }

        public long? ParentMediaID
        {
            get
            {
                return Media.ParentMediaID;
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
        public string AutoCalculatedVirtualPath
        {
            get
            {

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

                    if (LanguagesMapper.GetAllActive().Count() == 1)
                        virtualPath = virtualPath.Replace(websiteVirtualPaths.ElementAt(0), URIHelper.ConvertAbsUrlToTilda(URIHelper.BaseUrl.ToLower()));
                    else
                        virtualPath = virtualPath.Replace(websiteVirtualPaths.ElementAt(0), URIHelper.ConvertAbsUrlToTilda(URIHelper.LanguageBaseUrl(this.Language).ToLower()));

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

        public string GetMetaDescription()
        {
            var description = StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(MetaDescription));

            if (string.IsNullOrEmpty(description))
            {
                description = ShortDescription;
            }

            if ((description == "") || (description == LinkTitle))
            {
                description = StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(StringHelper.StripHtmlTags(MainContent)));

                if (description.Length > 255)
                    description = description.Substring(0, 255) + " ...";
            }

            return description;
        }

        public string GetMetaKeywords()
        {
            if (MetaKeywords.Trim() == "")
                return GetMetaDescription();
            else
                return MetaKeywords;
        }

        public string GetPageTitle()
        {
            if (FrameworkSettings.CurrentFrameworkBaseMedia == null)
                return "";

            var pageTitle = "";

            var details = MediaDetailsMapper.GetAllParentMediaDetails(this, Language).Reverse().ToList();

            /*if (FrameworkLibrary.FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail == FrameworkLibrary.FrameworkSettings.RootMediaDetail)
            {
                if (FrameworkSettings.RootMediaDetail != null)
                    details.Add(FrameworkSettings.RootMediaDetail);
            }*/

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

            return pageTitle;
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

        public IMediaDetail NextPage
        {
            get
            {
                var parentChildren = AllParentChildren.ToList();
                var index = parentChildren.FindIndex(delegate (IMediaDetail m) { return m.ID == this.ID; });

                if ((index < 0) || (index >= parentChildren.Count - 1))
                {
                    if (ParentMediaDetail == null)
                        return null;

                    return this.ParentMediaDetail.NextPage;
                }

                return parentChildren[index + 1];
            }
        }

        private IEnumerable<IMediaDetail> AllParentChildren
        {
            get
            {
                if (this.Media.ParentMedia == null)
                    return new List<IMediaDetail>();

                return MediaDetailsMapper.FilterByShowInMenuStatus(MediaDetailsMapper.FilterByCanRenderStatus(MediaDetailsMapper.GetAllChildMediaDetails(this.Media.ParentMedia, this.Language), true), true).OrderBy(i => i.Media.OrderIndex);
            }
        }

        public IMediaDetail PreviousPage
        {
            get
            {
                var parentChildren = AllParentChildren.ToList();
                var index = parentChildren.FindIndex(delegate (IMediaDetail m) { return m.ID == this.ID; });

                if ((index <= 0) || parentChildren.Count == 1)
                {
                    if (ParentMediaDetail == null)
                        return null;

                    return this.ParentMediaDetail.PreviousPage;
                }

                return parentChildren[index - 1];
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