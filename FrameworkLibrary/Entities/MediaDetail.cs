using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

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

        public bool CanCache
        {
            get
            {
                if (!this.CanRender)
                    return false;

                if (!this.EnableCaching)
                    return false;

                if(this.CheckEnforceRoleLimitationsOnFrontEnd())
                    return false;

                return true;
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
                var currentLanguage = FrameworkSettings.GetCurrentLanguage();

                if (currentLanguage == null)
                    currentLanguage = LanguagesMapper.GetDefaultLanguage();

                if (this.UseDefaultLanguageLayouts && this.LanguageID != currentLanguage.ID)
                    return this.Media.GetLiveMediaDetail(LanguagesMapper.GetDefaultLanguage())?.UseMainLayout;

                var layout = "";

                if (this.UseMediaTypeLayouts)
                    layout = this.MediaType?.MainLayout;
                else
                    layout = this.MainLayout;

                return layout; //MediaDetailsMapper.ParseSpecialTags(this, layout);
            }
        }

        public string UseSummaryLayout
        {
            get
            {
                var currentLanguage = FrameworkSettings.GetCurrentLanguage();

                if (currentLanguage == null)
                    currentLanguage = LanguagesMapper.GetDefaultLanguage();

                if (this.UseDefaultLanguageLayouts && this.LanguageID != currentLanguage.ID)
                    return this.Media.GetLiveMediaDetail(LanguagesMapper.GetDefaultLanguage())?.UseSummaryLayout;

                var layout = "";

                if (this.UseMediaTypeLayouts)
                    layout = this.MediaType?.SummaryLayout;
                else
                    layout = this.SummaryLayout;

                return layout; //MediaDetailsMapper.ParseSpecialTags(this, layout);
            }
        }

        public string UseFeaturedLayout
        {
            get
            {
                var currentLanguage = FrameworkSettings.GetCurrentLanguage();

                if(currentLanguage == null)
                    currentLanguage = LanguagesMapper.GetDefaultLanguage();

                if (this.UseDefaultLanguageLayouts && this.LanguageID != currentLanguage.ID)
                    return this.Media.GetLiveMediaDetail(LanguagesMapper.GetDefaultLanguage())?.UseSummaryLayout;

                var layout = "";

                if (this.UseMediaTypeLayouts)
                    layout = this.MediaType?.FeaturedLayout;
                else
                    layout = this.FeaturedLayout;

                return layout; //MediaDetailsMapper.ParseSpecialTags(this, layout);
            }
        }
        
        public bool CanUserAccessSection(User user)
        {
            if (user == null)
                return false;

            if (user.IsInRole(RoleEnum.Developer))
                return true;

            var roles = this.GetRoles();

            if (roles.Count == 0)
            {
                roles = this.MediaType.GetRoles();
            }

            var parentLimitedRoles = this.GetParentMediaDetails().Where(i=>i.CanLimitedRolesAccessAllChildPages).SelectMany(i=> ((MediaDetail)i).GetRoles());

            if(parentLimitedRoles.Any())
            {
                if(!user.IsInRoles(parentLimitedRoles))
                {
                    return false;
                }                
            }

            if (roles.Count > 0)
            {
                if (!user.IsInRoles(roles))
                {                    
                    return false;
                }
            }

            return true;
        }

        public string GetCacheKey(RenderVersion renderVersion)
        {
            return (renderVersion.ToString() + this.AutoCalculatedVirtualPath.Replace("~", "")).ToLower();
        }

        public IMediaDetail GetPreviousMediaDetail()
        {
            var children = this.Media?.ParentMedia?.GetLiveMediaDetail()?.ChildMediaDetails?.ToList();

            if (children == null)
                return null;

            var currentIndex = children.FindIndex(i => i.ID == this.ID);
            var previousIndex = currentIndex - 1;

            if (previousIndex < 0)
                previousIndex = 0;

            if(children.Count > 0)
            {
                var previousMediaDetail = children[previousIndex];                

                if (previousMediaDetail.ID == this.ID)
                    return null;

                return previousMediaDetail;
            }

            return null;            
        }

        public IMediaDetail GetNearestParentWhichContainsFieldCode(string FieldCode)
        {
            return MediaDetailsMapper.GetParentsWhichContainsFieldCode(this, Language, FieldCode).FirstOrDefault();
        }

        public IMediaDetail GetNextMediaDetail()
        {
            var children = this.Media?.ParentMedia?.GetLiveMediaDetail()?.ChildMediaDetails?.ToList();

            if (children == null)
                return null;

            var currentIndex = children.FindIndex(i => i.ID == this.ID);
            var nextIndex = currentIndex + 1;

            if (nextIndex >= children.Count)
                nextIndex = 0;

            if(children.Count > 0)
            {
                var nextMediaDetail = children[nextIndex];

                if (nextMediaDetail.ID == this.ID)
                    return null;

                return nextMediaDetail;
            }
            else
            {
                return null;
            }
        }

        public void RemoveFromCache()
        {
            var htmlCacheKey = GetCacheKey(RenderVersion.HTML);
            var mobileCacheKey = GetCacheKey(RenderVersion.Mobile);

            FileCacheHelper.RemoveFromCache(htmlCacheKey);
            RedisCacheHelper.RemoveFromCache(htmlCacheKey);
            ContextHelper.RemoveFromCache(htmlCacheKey);

            RedisCacheHelper.RemoveFromCache(mobileCacheKey);
            FileCacheHelper.RemoveFromCache(mobileCacheKey);
            ContextHelper.RemoveFromCache(mobileCacheKey);

            var language = this.Language;

            if(language == null && this.LanguageID > 0)
            {
                language = LanguagesMapper.GetByID(this.LanguageID);
            }

            var parents = MediaDetailsMapper.GetAllParentMediaDetails(this, language).Where(i=>i.ID != this.ID);

            foreach (var item in parents)
            {
                item.RemoveFromCache();
            }

        }

        public void SaveToMemoryCache(RenderVersion renderVersion, string html)
        {
            if (HasDraft || HistoryVersionNumber != 0)
                return;

            var key = GetCacheKey(renderVersion);            
            ContextHelper.SaveToCache(key, html);
        }

        public void SaveToFileCache(RenderVersion renderVersion, string html)
        {
            if (HasDraft || HistoryVersionNumber != 0)
                return;

            var cacheKey = GetCacheKey(renderVersion);
            FileCacheHelper.SaveToCache(cacheKey, html);
        }

        public void SaveToRedisCache(RenderVersion renderVersion, string html)
        {
            if (HasDraft || HistoryVersionNumber != 0)
                return;

            var cacheKey = GetCacheKey(renderVersion);
            RedisCacheHelper.SaveToCache(cacheKey, html);                
        }

        public Field LoadField(string fieldCode)
        {
            return Fields.FirstOrDefault(i => i.FieldCode == fieldCode);
        }

        public string RenderShortCode(string shortCode, bool includeFieldWrapper = true)
        {
            shortCode = "{" + shortCode + "}";
            return MediaDetailsMapper.ParseSpecialTags(this, shortCode, includeFieldWrapper: includeFieldWrapper);
        }

        public string RenderField(string fieldCode, bool includeFieldWrapper = true)
        {
            var shortCode = "Field:"+fieldCode;
            return RenderShortCode(shortCode, includeFieldWrapper);
        }

        public string RenderField(long fieldId, bool includeFieldWrapper = true)
        {
            var shortCode = "Field:" + fieldId;
            return RenderShortCode(shortCode, includeFieldWrapper);
        }

        public string RenderMainLayout()
        {
            return RenderShortCode("UseMainLayout");
        }

        public string RenderSummaryLayout()
        {
            return RenderShortCode("UseSummaryLayout");
        }

        public string RenderFeaturedLayout()
        {
            return RenderShortCode("UseFeaturedLayout");
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
                var key = this.ID + "_ChildMediaDetails";
                var _childMediaDetails = (IEnumerable<IMediaDetail>)ContextHelper.GetFromRequestContext(key);

                if (_childMediaDetails != null)
                    return _childMediaDetails;

                _childMediaDetails = MediaDetailsMapper.GetAllChildMediaDetails(MediaID, LanguageID).Where(i=>i.MediaType.ShowInSiteTree);

                ContextHelper.SetToRequestContext(key, _childMediaDetails);

                return _childMediaDetails;
            }
        }

        public List<Control> GetTemplateTopAndBottomSegments(System.Web.UI.Page control)
        {
            var masterPage = GetMasterPage();
            var templateTopAndBottomSegments = new List<Control>();

            if (masterPage != null)
            {
                var topAndBottomSegments = StringHelper.SplitByString(masterPage.Layout, "{PageContent}");

                if (topAndBottomSegments.Length > 1)
                {
                    templateTopAndBottomSegments.Add(control.ParseControl(MediaDetailsMapper.ParseSpecialTags(this, topAndBottomSegments.ElementAt(0))));
                    templateTopAndBottomSegments.Add(control.ParseControl(MediaDetailsMapper.ParseSpecialTags(this, topAndBottomSegments.ElementAt(1))));

                    return templateTopAndBottomSegments;
                }
            }

            return templateTopAndBottomSegments;
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
                if (parent is RootPage || parent is Website)
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
            return PublishDate != null ? new RssItem(this) : null;
        }

        private string contextMetaDescription { get; set; } = "";
        public string GetMetaDescription()
        {
            if (!string.IsNullOrEmpty(contextMetaDescription))
                return contextMetaDescription;

            var description = StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(MetaDescription));

            if (string.IsNullOrEmpty(description))
            {
                description = StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(this.ShortDescription)).Trim();
            }

            if(description.Length < 100)
            {
                var mainContent = StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(this.MainContent)).Trim();

                if (mainContent.Contains("<script") || mainContent.Contains("<style"))
                {
                    var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                    htmlDocument.LoadHtml(this.MainContent);

                    htmlDocument.DocumentNode.Descendants()
                                    .Where(n => n.Name == "script" || n.Name == "style" || n.NodeType == HtmlAgilityPack.HtmlNodeType.Comment)
                                    .ToList()
                                    .ForEach(n => n.Remove());

                    mainContent = StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(StringHelper.StripHtmlTags(htmlDocument.DocumentNode.InnerText.Trim())));
                }

                if (mainContent.Length > description.Length)
                {
                    description = mainContent;
                }
            }

            description = StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(StringHelper.StripHtmlTags(description)));

            if(description.Length > 255)
            {
                description = description.Substring(0, 255) + " ...";
            }

            if(string.IsNullOrEmpty(description))
            {
                description = GetPageTitle();
            }

            /*if ((description == "") || (description == LinkTitle))
            {
                description = StringHelper.StripHtmlTags(StringHelper.StripExtraSpaces(StringHelper.StripExtraLines(this.MainContent)));

                if (description.Length > 255)
                    description = description.Substring(0, 255) + " ...";
            }*/

            contextMetaDescription = description;

            return contextMetaDescription;
        }

        private string contextMetaKeywords { get; set; } = "";
        public string GetMetaKeywords()
        {
            if (!string.IsNullOrEmpty(contextMetaKeywords))
                return contextMetaKeywords;

            var metaKeywords = MetaKeywords.Trim();

            if (string.IsNullOrEmpty(metaKeywords) || metaKeywords.Length < 30)
                metaKeywords = GetPageTitle();

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

        public IEnumerable<IMediaDetail> GetParentsWhichContainsFieldCode(string fieldCode)
        {
            return MediaDetailsMapper.GetParentsWhichContainsFieldCode(this, Language, fieldCode);
        }

        public IEnumerable<IMediaDetail> GetParentMediaDetails()
        {
            return MediaDetailsMapper.GetAllParentMediaDetails(this, Language).Reverse().ToList();
        }

        public bool HasDraft
        {
            get
            {
                if (this.Media == null)
                    return false;

                return GetDrafts().Any();
            }
        }

        public IEnumerable<IMediaDetail> GetDrafts()
        {
            var drafts = new List<IMediaDetail>();

            if (this.Media == null)
                return drafts;

            return this.Media.MediaDetails.Where(i => i.IsDraft);
        }

        public IMediaDetail GetLatestDraft()
        {
            var drafts = GetDrafts();

            if (!drafts.Any())
                return null;

            return drafts.OrderBy(i=>i.DateCreated).FirstOrDefault();
        }

        public Return PublishLive()
        {
            var returnObj = new Return();
            var liveVersion = BaseMapper.GetObjectFromContext((MediaDetail)this.Media.GetLiveMediaDetail());
            var selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)this);

            IEnumerable<MediaDetail> items = liveVersion.History.ToList();

            foreach (var item in items)
            {
                if (item.ID != selectedItem.ID)
                {
                    var tmpItem = BaseMapper.GetObjectFromContext(item);
                    item.HistoryForMediaDetailID = selectedItem.ID;
                }
            }

            selectedItem.HistoryVersionNumber = 0;
            selectedItem.HistoryForMediaDetail = null;
            selectedItem.IsDraft = false;
            selectedItem.PublishDate = DateTime.Now;
            //selectedItem.ShowInMenu = true;

            foreach (var fieldAssociation in selectedItem.FieldAssociations)
            {
                var index = 1;
                foreach (var history in fieldAssociation.MediaDetail.History)
                {
                    history.HistoryForMediaDetail = fieldAssociation.MediaDetail;
                    history.HistoryVersionNumber = 1;

                    index++;
                }

                fieldAssociation.MediaDetail.HistoryForMediaDetail = null;
                fieldAssociation.MediaDetail.HistoryVersionNumber = 0;
            }

            foreach (var field in selectedItem.Fields)
            {
                foreach (var fieldAssociation in field.FieldAssociations)
                {
                    var index = 1;

                    foreach (var mediaDetail in fieldAssociation.MediaDetail.Media.MediaDetails)
                    {
                        mediaDetail.HistoryForMediaDetail = fieldAssociation.MediaDetail;
                        mediaDetail.HistoryVersionNumber = 1;

                        index++;
                    }

                    fieldAssociation.MediaDetail.HistoryForMediaDetail = null;
                    fieldAssociation.MediaDetail.HistoryVersionNumber = 0;
                }

                field.FrontEndSubmissions = liveVersion.LoadField(field.FieldCode)?.FrontEndSubmissions;
            }

            foreach (var mediaTypeField in selectedItem.MediaType.Fields)
            {
                if(!selectedItem.Fields.Any(i=>i.FieldCode == mediaTypeField.FieldCode))
                {
                    var mediaDetailField = new MediaDetailField();
                    mediaDetailField.CopyFrom(mediaTypeField);
                    
                    mediaDetailField.UseMediaTypeFieldFrontEndLayout = true;
                    mediaDetailField.UseMediaTypeFieldDescription = true;                    

                    mediaDetailField.MediaTypeField = mediaTypeField;

                    mediaDetailField.DateCreated = mediaDetailField.DateLastModified = DateTime.Now;

                    mediaDetailField.OrderIndex = selectedItem.Fields.Count;
                    selectedItem.Fields.Add(mediaDetailField);
                }
            }


            if (items.Any())
            {
                liveVersion.HistoryVersionNumber = items.OrderByDescending(i => i.HistoryVersionNumber).FirstOrDefault().HistoryVersionNumber + 1;
            }
            else
            {
                liveVersion.HistoryVersionNumber = 1;
            }

            liveVersion.HistoryForMediaDetail = (MediaDetail)selectedItem;

            var associations = BaseMapper.GetDataModel().FieldAssociations.Where(i => i.AssociatedMediaDetailID == liveVersion.ID);

            foreach (var association in associations)
            {
                association.MediaDetail = (MediaDetail)selectedItem;
            }

            returnObj = MediaDetailsMapper.Update(selectedItem);

            if (!returnObj.IsError)
            {
                liveVersion.HistoryForMediaDetailID = selectedItem.ID;                


                returnObj = MediaDetailsMapper.Update(liveVersion);

                if (!returnObj.IsError)
                {
                    ContextHelper.Clear(ContextType.Cache);
                    FileCacheHelper.ClearAllCache();

                    return returnObj;
                }
                else
                {
                    return returnObj;
                }
            }
            else
            {
                return returnObj;
            }
        }

        public bool HasAnyRoles()
        {
            return Media.RolesMedias.Count > 0;
        }

        public bool HasAnyUsers()
        {
            return Media.UsersMedias.Count > 0;
        }

        public bool HasRole(Role role)
        {
            return Media.RolesMedias.Where(i => i.ID == role.ID).Any();
        }

        public bool CheckEnforceRoleLimitationsOnFrontEnd()
        {
            if (this.EnforceRoleLimitationsOnFrontEnd)
                return true;

            var enforceRoleLimitationOnFrontEnds = this.GetParentMediaDetails().Any(i => i.EnforceRoleLimitationsOnFrontEnd);

            return enforceRoleLimitationOnFrontEnds;
        }

        public List<Role> GetRoles()
        {
            return Media.RolesMedias.Select(i => i.Role).ToList();            
        }

        public bool HasUser(User user)
        {
            return Media.UsersMedias.Where(i => i.UserID == user.ID).Any();
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

        public void UpdateField(string fieldCode, string newValue)
        {
            var field = this.Fields.FirstOrDefault(i => i.FieldCode == fieldCode);

            if (field != null)
            {
                field.FieldValue = newValue;
            }
        }

        public string SectionTitle
        {
            get
            {
                return this.RenderField("SectionTitle", false);
            }
            set
            {
                UpdateField("SectionTitle", value);
            }
        }

        public string ShortDescription
        {
            get
            {
                return this.RenderField("ShortDescription", false);
            }
            set
            {
                UpdateField("ShortDescription", value);
            }
        }

        public string MainContent
        {
            get
            {
                return this.RenderField("MainContent", false);
            }
            set
            {
                UpdateField("MainContent", value);
            }
        }

        public string PathToFile
        {
            get
            {
                var pathToFile = this.RenderField("PathToFile", false);

                if(pathToFile.Contains("<"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(pathToFile, "/[/a-zA-Z0-9-._]+");
                    pathToFile = match.Value.Replace("\"", "").Replace("'","");
                }
                
                return pathToFile;
            }
            set
            {
                UpdateField("PathToFile", value);
            }
        }        
    }
}