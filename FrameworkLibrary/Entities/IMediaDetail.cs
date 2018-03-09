using System;
using System.Collections.Generic;
using System.Web.UI;

namespace FrameworkLibrary
{
    public interface IMediaDetail
    {
        string AbsoluteUrl { get; }
        bool AllowCommenting { get; set; }
        string AutoCalculatedVirtualPath { get; }
        string CachedVirtualPath { get; set; }
        bool CanAddToCart { get; set; }
        bool CanLimitedRolesAccessAllChildPages { get; set; }
        bool EnforceRoleLimitationsOnFrontEnd { get; set; }
        bool CheckEnforceRoleLimitationsOnFrontEnd();
        bool CanRender { get; }
        bool CanCache { get; }
        IEnumerable<IMediaDetail> ChildMediaDetails { get; }
        User CreatedByUser { get; set; }
        long CreatedByUserID { get; set; }
        string CssClasses { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateLastModified { get; set; }
        string DirectLink { get; set; }
        bool EnableCaching { get; set; }
        DateTime? ExpiryDate { get; set; }
        string FeaturedLayout { get; set; }
        ICollection<FieldAssociation> FieldAssociations { get; set; }
        ICollection<MediaDetailField> Fields { get; set; }
        bool ForceSSL { get; set; }
        string Handler { get; set; }
        bool HasDraft { get; }
        ICollection<MediaDetail> History { get; set; }
        MediaDetail HistoryForMediaDetail { get; set; }
        long? HistoryForMediaDetailID { get; set; }
        int HistoryVersionNumber { get; set; }
        long ID { get; set; }
        bool IsArchive { get; }
        bool IsDeleted { get; set; }
        bool IsDraft { get; set; }
        bool IsHistory { get; }
        bool IsProtected { get; set; }
        bool IsPublished { get; }
        string JsonVirtualPath { get; }
        Language Language { get; set; }
        long LanguageID { get; set; }
        User LastUpdatedByUser { get; set; }
        long LastUpdatedByUserID { get; set; }
        string LinkTitle { get; set; }
        string MainContent { get; set; }
        string MainLayout { get; set; }
        MasterPage MasterPage { get; set; }
        long? MasterPageID { get; set; }
        Media Media { get; set; }
        long MediaID { get; set; }
        MediaType MediaType { get; set; }
        long MediaTypeID { get; set; }
        string MetaDescription { get; set; }
        string MetaKeywords { get; set; }
        long NumberOfStars { get; set; }
        long NumberOfViews { get; set; }
        bool OpenInNewWindow { get; set; }
        string PathToFile { get; set; }
        decimal Price { get; set; }
        DateTime? PublishDate { get; set; }
        string QRCodeVirtualPath { get; }
        long QuantityInStock { get; set; }
        string RecurringTimePeriod { get; set; }
        bool RedirectToFirstChild { get; set; }
        bool RenderInFooter { get; set; }
        string RssVirtualPath { get; }
        string SectionTitle { get; set; }
        string SefTitle { get; set; }
        string ShortDescription { get; set; }
        bool ShowInMenu { get; set; }
        bool ShowInSearchResults { get; set; }
        string SummaryLayout { get; set; }
        string Title { get; set; }
        bool UseDefaultLanguageLayouts { get; set; }
        bool UseDirectLink { get; set; }
        string UseFeaturedLayout { get; }
        string UseMainLayout { get; }
        bool UseMediaTypeLayouts { get; set; }
        string UseSummaryLayout { get; }
        List<ValidationError> ValidationErrors { get; }
        string VirtualPath { get; }

        string CalculatedVirtualPath();
        bool CanUserAccessSection(User user);
        void ClearAutoCalculatedVirtualPathCache();
        Return GenerateValidationReturn();
        string GetCacheKey(RenderVersion renderVersion);
        IEnumerable<IMediaDetail> GetDrafts();
        IMediaDetail GetLatestDraft();
        MasterPage GetMasterPage();
        string GetMetaDescription();
        string GetMetaKeywords();
        IMediaDetail GetNearestParentWhichContainsFieldCode(string FieldCode);
        IMediaDetail GetNextMediaDetail();
        string GetPageTitle();
        IEnumerable<IMediaDetail> GetParentMediaDetails();
        IEnumerable<IMediaDetail> GetParentsWhichContainsFieldCode(string fieldCode);
        IMediaDetail GetPreviousMediaDetail();
        IEnumerable<IMediaDetail> GetRelatedItems(long mediaTypeId = 0);
        List<Role> GetRoles();
        RssItem GetRssItem();
        List<Control> GetTemplateTopAndBottomSegments(System.Web.UI.Page control);
        Website GetWebsite();
        bool HasAnyRoles();
        bool HasAnyUsers();
        bool HasRole(Role role);
        bool HasUser(User user);
        Field LoadField(string fieldCode);
        Return PublishLive();
        void RemoveFromCache();
        string RenderField(long fieldId, bool includeFieldWrapper = true);
        string RenderField(string fieldCode, bool includeFieldWrapper = true);
        string RenderMainLayout();
        string RenderShortCode(string shortCode, bool includeFieldWrapper = true);
        void SaveToFileCache(RenderVersion renderVersion, string html);
        void SaveToMemoryCache(RenderVersion renderVersion, string html);
        void SaveToRedisCache(RenderVersion renderVersion, string html);
        void UpdateField(string fieldCode, string newValue);
        Return Validate();
        string OnPublishExecuteCode { get; set; }
    }
}