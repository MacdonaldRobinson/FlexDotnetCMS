USE [FlexDotNetCMS]
GO
ALTER TABLE [dbo].[Website] DROP CONSTRAINT [FK_Website_MediaDetails]
GO
ALTER TABLE [dbo].[UsersRoles] DROP CONSTRAINT [FK_UsersRoles_UserID_Users_ID]
GO
ALTER TABLE [dbo].[UsersRoles] DROP CONSTRAINT [FK_UsersRoles_RoleID_Roles_ID]
GO
ALTER TABLE [dbo].[UsersMediaDetails] DROP CONSTRAINT [FK_UsersMediaDetails_UserID]
GO
ALTER TABLE [dbo].[UsersMediaDetails] DROP CONSTRAINT [FK_UsersMediaDetails_PermissionID]
GO
ALTER TABLE [dbo].[UsersMediaDetails] DROP CONSTRAINT [FK_UsersMediaDetails_MediaDetailID]
GO
ALTER TABLE [dbo].[UrlRedirectRules] DROP CONSTRAINT [FK_UrlRedirectRules_MediaDetails]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [FK_Settings_MasterPages]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [FK_Settings_Languages]
GO
ALTER TABLE [dbo].[RootPages] DROP CONSTRAINT [FK_RootPages_MediaDetails]
GO
ALTER TABLE [dbo].[RolesPermissions] DROP CONSTRAINT [FK_RolesPermissions_RolesID]
GO
ALTER TABLE [dbo].[RolesPermissions] DROP CONSTRAINT [FK_RolesPermissions_PermissionID]
GO
ALTER TABLE [dbo].[RolesMediaDetails] DROP CONSTRAINT [FK_RolesMediaDetails_RoleID]
GO
ALTER TABLE [dbo].[RolesMediaDetails] DROP CONSTRAINT [FK_RolesMediaDetails_MediaDetailID]
GO
ALTER TABLE [dbo].[Pages] DROP CONSTRAINT [FK_Pages_MediaDetailID_MediaDetails_ID]
GO
ALTER TABLE [dbo].[MediaTypesRoles] DROP CONSTRAINT [FK_MediaTypesRoles_Roles]
GO
ALTER TABLE [dbo].[MediaTypesRoles] DROP CONSTRAINT [FK_MediaTypesRoles_MediaTypeID]
GO
ALTER TABLE [dbo].[MediaTypesFields] DROP CONSTRAINT [FK_MediaTypesFields_MediaTypes]
GO
ALTER TABLE [dbo].[MediaTypesFields] DROP CONSTRAINT [FK_MediaTypesFields_Fields]
GO
ALTER TABLE [dbo].[MediaTypesChildren] DROP CONSTRAINT [FK_MediaTypesChildren_MediaTypeID]
GO
ALTER TABLE [dbo].[MediaTypesChildren] DROP CONSTRAINT [FK_MediaTypesChildren_AllowedChildMediaTypeID]
GO
ALTER TABLE [dbo].[MediaTypes] DROP CONSTRAINT [FK_MediaTypes_MasterPages]
GO
ALTER TABLE [dbo].[MediaTypeRolesPermissions] DROP CONSTRAINT [FK_MediaTypeRolesPermissions_MediaTypesRoleID]
GO
ALTER TABLE [dbo].[MediaTypeRolesPermissions] DROP CONSTRAINT [FK_MediaTypeRolesPermissions_MediaTypeRolesPermissionID]
GO
ALTER TABLE [dbo].[MediaTags] DROP CONSTRAINT [FK_MediaTags_TagID_Tags_ID]
GO
ALTER TABLE [dbo].[MediaTags] DROP CONSTRAINT [FK_MediaTags_MediaID_Media_ID]
GO
ALTER TABLE [dbo].[MediaDetailsFields] DROP CONSTRAINT [FK_MediaDetailsFields_MediaTypesFields]
GO
ALTER TABLE [dbo].[MediaDetailsFields] DROP CONSTRAINT [FK_MediaDetailsFields_Fields]
GO
ALTER TABLE [dbo].[MediaDetailsFields] DROP CONSTRAINT [FK_MediaDetailFields_MediaDetails]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [FK_MediaDetails_MediaTypeID_MediaType_ID]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [FK_MediaDetails_MediaID_Media_ID]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [FK_MediaDetails_MasterPage]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [FK_MediaDetails_LastUpdatedByUserID]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [FK_MediaDetails_LanguageID_Languages_ID]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [FK_MediaDetails_HistoryForMediaDetailID]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [FK_MediaDetails_CreatedByUserID]
GO
ALTER TABLE [dbo].[Media] DROP CONSTRAINT [FK_Media_ParentMediaID]
GO
ALTER TABLE [dbo].[FieldsAssociations] DROP CONSTRAINT [FK_FieldsAssociations_MediaDetailsFields]
GO
ALTER TABLE [dbo].[FieldsAssociations] DROP CONSTRAINT [FK_FieldAssociations_MediaDetails]
GO
ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_ReplyToCommentID]
GO
ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Media]
GO
ALTER TABLE [dbo].[Website] DROP CONSTRAINT [DF_Website_GoogleAnalyticsTrackingKey]
GO
ALTER TABLE [dbo].[Website] DROP CONSTRAINT [DF_Website_CodeInHead]
GO
ALTER TABLE [dbo].[WebserviceRequests] DROP CONSTRAINT [DF_WebserviceRequests_DateLastModified]
GO
ALTER TABLE [dbo].[WebserviceRequests] DROP CONSTRAINT [DF_WebserviceRequests_DateCreated]
GO
ALTER TABLE [dbo].[WebserviceRequests] DROP CONSTRAINT [DF_WebserviceRequests_Response]
GO
ALTER TABLE [dbo].[WebserviceRequests] DROP CONSTRAINT [DF_WebserviceRequests_UrlReferrer]
GO
ALTER TABLE [dbo].[UsersMediaDetails] DROP CONSTRAINT [DF_UsersMediaDetails_DateLastModified]
GO
ALTER TABLE [dbo].[UsersMediaDetails] DROP CONSTRAINT [DF_UsersMediaDetails_DateCreated]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_Users_AfterLoginStartPage]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_Users_LastName]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_Users_FirstName]
GO
ALTER TABLE [dbo].[UrlRedirectRules] DROP CONSTRAINT [DF_UrlRedirectRules_Is301Redirect]
GO
ALTER TABLE [dbo].[Tags] DROP CONSTRAINT [DF_Topics_DateLastModified]
GO
ALTER TABLE [dbo].[Tags] DROP CONSTRAINT [DF_Topics_DateCreated]
GO
ALTER TABLE [dbo].[Tags] DROP CONSTRAINT [DF_Tags_IsActive]
GO
ALTER TABLE [dbo].[Tags] DROP CONSTRAINT [DF_Tags_ThumbnailPath]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Features_DateLastModified]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Features_DateCreated]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Settings_OutputCacheDurationInSeconds]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Settings_EnableGlossaryTerms]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Settings_PageNotFoundUrl]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Settings_SiteOfflineUrl]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Settings_SiteOnlineAtDateTime]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Settings_MaxUploadFileSizePerFile]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Settings_MaxRequestLength]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Settings_ShoppingCartTax]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Settings_GlobalCodeInBody]
GO
ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [DF_Settings_GlobalCodeInHead]
GO
ALTER TABLE [dbo].[RolesMediaDetails] DROP CONSTRAINT [DF_RolesMediaDetails_DateLastModified]
GO
ALTER TABLE [dbo].[RolesMediaDetails] DROP CONSTRAINT [DF_RolesMediaDetails_DateCreated]
GO
ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [DF__Roles__DateLastM__0880433F]
GO
ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [DF__Roles__DateCreat__078C1F06]
GO
ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [DF_Roles_IsActive]
GO
ALTER TABLE [dbo].[Permissions] DROP CONSTRAINT [DF_Permissions_DateLastModified]
GO
ALTER TABLE [dbo].[Permissions] DROP CONSTRAINT [DF_Permissions_DateCreated]
GO
ALTER TABLE [dbo].[MediaTypesRoles] DROP CONSTRAINT [DF_MediaTypesRoles_DateLastModified]
GO
ALTER TABLE [dbo].[MediaTypesRoles] DROP CONSTRAINT [DF_MediaTypesRoles_DateCreated]
GO
ALTER TABLE [dbo].[MediaTypes] DROP CONSTRAINT [DF_MediaTypes_UseMediaTypeLayouts]
GO
ALTER TABLE [dbo].[MediaTypes] DROP CONSTRAINT [DF_MediaTypes_SummaryLayout1]
GO
ALTER TABLE [dbo].[MediaTypes] DROP CONSTRAINT [DF_MediaTypes_SummaryLayout]
GO
ALTER TABLE [dbo].[MediaTypes] DROP CONSTRAINT [DF_MediaTypes_CustomCode]
GO
ALTER TABLE [dbo].[MediaTypes] DROP CONSTRAINT [DF_MediaTypes_EnableCaching]
GO
ALTER TABLE [dbo].[MediaTypes] DROP CONSTRAINT [DF_MediaTypes_ShowInSiteTree]
GO
ALTER TABLE [dbo].[MediaTypes] DROP CONSTRAINT [DF_MediaTypes_ShowInSearchResults_1]
GO
ALTER TABLE [dbo].[MediaTypeRolesPermissions] DROP CONSTRAINT [DF_MediaTypeRolesPermissions_DateLastModified]
GO
ALTER TABLE [dbo].[MediaTypeRolesPermissions] DROP CONSTRAINT [DF_MediaTypeRolesPermissions_DateCreated]
GO
ALTER TABLE [dbo].[MediaTags] DROP CONSTRAINT [DF_MediaTags_OrderIndex]
GO
ALTER TABLE [dbo].[MediaDetailsFields] DROP CONSTRAINT [DF_MediaDetailsFields_UseMediaTypeFieldFrontEndLayout]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_PathToFile]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_UseMediaTypeLayouts]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_SummaryLayout1]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_SummaryLayout]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_CustomCode]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_CssClasses]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_IsProtected]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_ForceSSL]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_RenderInFooter1]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF__MediaDeta__DateL__3B75D760]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF__MediaDeta__DateC__3A81B327]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_RedirectToFirstChild_1]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_RecurringTimePeriod]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_InStock]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_Price]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_CanAddToShoppingCart]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_IsDraft]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_HistoryVersionNumber]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_AllowCaching]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_ShowInSearchResults_1]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF__MediaDeta__ShowI__398D8EEE]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_EnableCommenting]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_Media_IsDeleted]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_Media_NumberOfStars]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_Media_NumberOfViews]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_MetaDescription]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_MetaKeywords]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_DirectLinkVirtualPath]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_OpenInNewWindow]
GO
ALTER TABLE [dbo].[MediaDetails] DROP CONSTRAINT [DF_MediaDetails_UseDirectLink]
GO
ALTER TABLE [dbo].[Media] DROP CONSTRAINT [DF__Media__OrderInde__1D114BD1]
GO
ALTER TABLE [dbo].[MasterPages] DROP CONSTRAINT [DF_MasterPages_DateLastModified]
GO
ALTER TABLE [dbo].[MasterPages] DROP CONSTRAINT [DF_MasterPages_DateCreated]
GO
ALTER TABLE [dbo].[MasterPages] DROP CONSTRAINT [DF_MasterPages_IsMobileTemplate]
GO
ALTER TABLE [dbo].[Languages] DROP CONSTRAINT [DF_Languages_DateLastModified]
GO
ALTER TABLE [dbo].[Languages] DROP CONSTRAINT [DF_Languages_DateCreated]
GO
ALTER TABLE [dbo].[Languages] DROP CONSTRAINT [DF__Languages__IsAct__0F8D3381]
GO
ALTER TABLE [dbo].[Languages] DROP CONSTRAINT [DF_Languages_DisplayName]
GO
ALTER TABLE [dbo].[IPLocationTracker] DROP CONSTRAINT [DF_IPLocationTracker_DateLastModified]
GO
ALTER TABLE [dbo].[IPLocationTracker] DROP CONSTRAINT [DF_IPLocationTracker_DateCreated]
GO
ALTER TABLE [dbo].[IPLocationTracker] DROP CONSTRAINT [DF_IPLocationTracker_Location]
GO
ALTER TABLE [dbo].[IPLocationTracker] DROP CONSTRAINT [DF_IPLocationTracker_IPAddress]
GO
ALTER TABLE [dbo].[GlossaryTerms] DROP CONSTRAINT [DF_GlossaryTerms_DateLastModified]
GO
ALTER TABLE [dbo].[GlossaryTerms] DROP CONSTRAINT [DF_GlossaryTerms_DateCreated]
GO
ALTER TABLE [dbo].[FieldsAssociations] DROP CONSTRAINT [DF_FieldsAssociations_OrderIndex]
GO
ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [DF_Fields_DateLastModified]
GO
ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [DF_Fields_DateCreated]
GO
ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [DF_Fields_FrontEndLayout]
GO
ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [DF_Fields_GroupName]
GO
ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [DF_Fields_SetValueCode]
GO
ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [DF_Fields_ReturnValuePropertyName]
GO
ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [DF_Fields_FieldValue]
GO
ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [DF_Fields_RenderLabelAfterControl]
GO
ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [DF_Fields_FieldCode]
GO
ALTER TABLE [dbo].[EmailLog] DROP CONSTRAINT [DF__EmailLog__DateLa__21229F2E]
GO
ALTER TABLE [dbo].[EmailLog] DROP CONSTRAINT [DF__EmailLog__DateCr__202E7AF5]
GO
ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [DF__Comments__DateLa__29ACF837]
GO
ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [DF__Comments__DateCr__28B8D3FE]
GO
/****** Object:  Index [UNIQUE_UriSegment]    Script Date: 2016-10-12 9:17:50 AM ******/
ALTER TABLE [dbo].[Languages] DROP CONSTRAINT [UNIQUE_UriSegment]
GO
/****** Object:  Index [Languages_uq2]    Script Date: 2016-10-12 9:17:50 AM ******/
ALTER TABLE [dbo].[Languages] DROP CONSTRAINT [Languages_uq2]
GO
/****** Object:  Index [Languages_uq]    Script Date: 2016-10-12 9:17:50 AM ******/
ALTER TABLE [dbo].[Languages] DROP CONSTRAINT [Languages_uq]
GO
/****** Object:  Table [dbo].[Website]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Website]
GO
/****** Object:  Table [dbo].[WebserviceRequests]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[WebserviceRequests]
GO
/****** Object:  Table [dbo].[UsersRoles]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[UsersRoles]
GO
/****** Object:  Table [dbo].[UsersMediaDetails]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[UsersMediaDetails]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[UrlRedirectRules]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[UrlRedirectRules]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Tags]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Settings]
GO
/****** Object:  Table [dbo].[RootPages]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[RootPages]
GO
/****** Object:  Table [dbo].[RolesPermissions]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[RolesPermissions]
GO
/****** Object:  Table [dbo].[RolesMediaDetails]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[RolesMediaDetails]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Roles]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Permissions]
GO
/****** Object:  Table [dbo].[Pages]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Pages]
GO
/****** Object:  Table [dbo].[MediaTypesRoles]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[MediaTypesRoles]
GO
/****** Object:  Table [dbo].[MediaTypesFields]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[MediaTypesFields]
GO
/****** Object:  Table [dbo].[MediaTypesChildren]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[MediaTypesChildren]
GO
/****** Object:  Table [dbo].[MediaTypes]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[MediaTypes]
GO
/****** Object:  Table [dbo].[MediaTypeRolesPermissions]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[MediaTypeRolesPermissions]
GO
/****** Object:  Table [dbo].[MediaTags]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[MediaTags]
GO
/****** Object:  Table [dbo].[MediaDetailsFields]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[MediaDetailsFields]
GO
/****** Object:  Table [dbo].[MediaDetails]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[MediaDetails]
GO
/****** Object:  Table [dbo].[Media]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Media]
GO
/****** Object:  Table [dbo].[MasterPages]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[MasterPages]
GO
/****** Object:  Table [dbo].[Languages]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Languages]
GO
/****** Object:  Table [dbo].[IPLocationTracker]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[IPLocationTracker]
GO
/****** Object:  Table [dbo].[GlossaryTerms]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[GlossaryTerms]
GO
/****** Object:  Table [dbo].[FieldsAssociations]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[FieldsAssociations]
GO
/****** Object:  Table [dbo].[Fields]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Fields]
GO
/****** Object:  Table [dbo].[EmailLog]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[EmailLog]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 2016-10-12 9:17:50 AM ******/
DROP TABLE [dbo].[Comments]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Status] [nvarchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
	[ReplyToCommentID] [bigint] NULL,
	[MediaID] [bigint] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmailLog]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SenderName] [nvarchar](255) NOT NULL,
	[SenderEmailAddress] [nvarchar](255) NOT NULL,
	[ToEmailAddresses] [nvarchar](max) NOT NULL,
	[FromEmailAddress] [nvarchar](255) NOT NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[VisitorIP] [nvarchar](255) NOT NULL,
	[RequestUrl] [nvarchar](255) NOT NULL,
	[ServerMessage] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_ContactMessages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Fields]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fields](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FieldCode] [nvarchar](255) NOT NULL,
	[FieldLabel] [nvarchar](255) NOT NULL,
	[RenderLabelAfterControl] [bit] NOT NULL,
	[AdminControl] [nvarchar](max) NOT NULL,
	[FieldValue] [nvarchar](max) NOT NULL,
	[GetAdminControlValue] [nvarchar](max) NOT NULL,
	[SetAdminControlValue] [nvarchar](max) NOT NULL,
	[OrderIndex] [bigint] NOT NULL,
	[GroupName] [nvarchar](255) NOT NULL,
	[FrontEndLayout] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Fields] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FieldsAssociations]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FieldsAssociations](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MediaDetailFieldID] [bigint] NOT NULL,
	[AssociatedMediaDetailID] [bigint] NOT NULL,
	[OrderIndex] [int] NOT NULL,
 CONSTRAINT [PK_FieldAssociations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GlossaryTerms]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GlossaryTerms](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Term] [nvarchar](max) NOT NULL,
	[Definition] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_GlossaryTerms] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IPLocationTracker]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IPLocationTracker](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IPAddress] [nvarchar](255) NOT NULL,
	[Location] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_IPLocationTracker] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Languages]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Languages](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DisplayName] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[UriSegment] [nvarchar](50) NOT NULL,
	[CultureCode] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MasterPages]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterPages](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[PathToFile] [nvarchar](255) NOT NULL,
	[MobileTemplate] [nvarchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_MasterPages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Media]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Media](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentMediaID] [bigint] NULL,
	[OrderIndex] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaDetails]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaDetails](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MediaID] [bigint] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[LinkTitle] [nvarchar](255) NOT NULL,
	[UseDirectLink] [bit] NOT NULL,
	[OpenInNewWindow] [bit] NOT NULL,
	[DirectLink] [nvarchar](255) NOT NULL,
	[SectionTitle] [nvarchar](max) NOT NULL,
	[ShortDescription] [nvarchar](500) NOT NULL,
	[MainContent] [nvarchar](max) NOT NULL,
	[MetaKeywords] [nvarchar](500) NOT NULL,
	[MetaDescription] [nvarchar](max) NOT NULL,
	[LanguageID] [bigint] NOT NULL,
	[NumberOfViews] [bigint] NOT NULL,
	[NumberOfStars] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[AllowCommenting] [bit] NOT NULL,
	[ExpiryDate] [datetime] NULL,
	[PublishDate] [datetime] NULL,
	[ShowInMenu] [bit] NOT NULL,
	[ShowInSearchResults] [bit] NOT NULL,
	[EnableCaching] [bit] NOT NULL,
	[SefTitle] [nvarchar](255) NULL,
	[Handler] [nvarchar](255) NULL,
	[CachedVirtualPath] [nvarchar](255) NULL,
	[MediaTypeID] [bigint] NOT NULL,
	[HistoryVersionNumber] [int] NOT NULL,
	[HistoryForMediaDetailID] [bigint] NULL,
	[IsDraft] [bit] NOT NULL,
	[CreatedByUserID] [bigint] NOT NULL,
	[LastUpdatedByUserID] [bigint] NOT NULL,
	[CanAddToCart] [bit] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[QuantityInStock] [bigint] NOT NULL,
	[RecurringTimePeriod] [nvarchar](50) NOT NULL,
	[RedirectToFirstChild] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
	[RenderInFooter] [bit] NOT NULL,
	[ForceSSL] [bit] NOT NULL,
	[IsProtected] [bit] NOT NULL,
	[CssClasses] [nvarchar](50) NOT NULL,
	[MasterPageID] [bigint] NULL,
	[MainLayout] [nvarchar](max) NOT NULL,
	[SummaryLayout] [nvarchar](max) NOT NULL,
	[FeaturedLayout] [nvarchar](max) NOT NULL,
	[UseMediaTypeLayouts] [bit] NOT NULL,
	[PathToFile] [nvarchar](255) NOT NULL,
 CONSTRAINT [MediaDetails_pk] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaDetailsFields]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaDetailsFields](
	[FieldID] [bigint] NOT NULL,
	[MediaDetailID] [bigint] NOT NULL,
	[MediaTypeFieldID] [bigint] NULL,
	[UseMediaTypeFieldFrontEndLayout] [bit] NOT NULL,
 CONSTRAINT [PK_MediaDetailsFields_1] PRIMARY KEY CLUSTERED 
(
	[FieldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaTags]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaTags](
	[TagID] [bigint] NOT NULL,
	[MediaID] [bigint] NOT NULL,
	[OrderIndex] [int] NOT NULL,
 CONSTRAINT [PK_MediaTags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC,
	[MediaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaTypeRolesPermissions]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaTypeRolesPermissions](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MediaTypeRoleID] [bigint] NOT NULL,
	[PermissionID] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_MediaTypeRolesPermissions_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaTypes]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaTypes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Label] [nvarchar](255) NOT NULL,
	[MediaTypeHandler] [nvarchar](255) NOT NULL,
	[MasterPageID] [bigint] NULL,
	[IsActive] [bit] NOT NULL,
	[ShowInMenu] [bit] NOT NULL,
	[ShowInSearchResults] [bit] NOT NULL,
	[ShowInSiteTree] [bit] NOT NULL,
	[EnableCaching] [bit] NOT NULL,
	[MainLayout] [nvarchar](max) NOT NULL,
	[SummaryLayout] [nvarchar](max) NOT NULL,
	[FeaturedLayout] [nvarchar](max) NOT NULL,
	[UseMediaTypeLayouts] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [MediaType_pk] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaTypesChildren]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaTypesChildren](
	[MediaTypeID] [bigint] NOT NULL,
	[AllowedChildMediaTypeID] [bigint] NOT NULL,
 CONSTRAINT [PK_AllowedChildMediaTypes] PRIMARY KEY CLUSTERED 
(
	[MediaTypeID] ASC,
	[AllowedChildMediaTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaTypesFields]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaTypesFields](
	[FieldID] [bigint] NOT NULL,
	[MediaTypeID] [bigint] NOT NULL,
	[Instructions] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_MediaTypesFields_1] PRIMARY KEY CLUSTERED 
(
	[FieldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaTypesRoles]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaTypesRoles](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MediaTypeID] [bigint] NOT NULL,
	[RoleID] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_MediaTypesRoles_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Pages]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pages](
	[MediaDetailID] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MediaDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[EnumName] [varchar](255) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[EnumName] [varchar](255) NOT NULL,
	[Description] [varchar](350) NULL,
	[IsActive] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK__Roles__3214EC2705A3D694] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RolesMediaDetails]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolesMediaDetails](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MediaDetailID] [bigint] NOT NULL,
	[RoleID] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_RolesMediaDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RolesPermissions]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolesPermissions](
	[RoleID] [bigint] NOT NULL,
	[PermissionID] [bigint] NOT NULL,
 CONSTRAINT [PK_RolesPermissions] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC,
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RootPages]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RootPages](
	[MediaDetailID] [bigint] NOT NULL,
 CONSTRAINT [PK_HomePages] PRIMARY KEY CLUSTERED 
(
	[MediaDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[GlobalCodeInHead] [nvarchar](max) NOT NULL,
	[GlobalCodeInBody] [nvarchar](max) NOT NULL,
	[ShoppingCartTax] [decimal](18, 2) NOT NULL,
	[MaxRequestLength] [int] NOT NULL,
	[MaxUploadFileSizePerFile] [int] NOT NULL,
	[SiteOnlineAtDateTime] [datetime] NOT NULL,
	[SiteOfflineAtDateTime] [datetime] NULL,
	[SiteOfflineUrl] [nvarchar](255) NOT NULL,
	[PageNotFoundUrl] [nvarchar](255) NOT NULL,
	[EnableGlossaryTerms] [bit] NOT NULL,
	[OutputCacheDurationInSeconds] [bigint] NOT NULL,
	[DefaultLanguageID] [bigint] NOT NULL,
	[DefaultMasterPageID] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Features] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tags]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SefTitle] [nvarchar](255) NOT NULL,
	[ThumbnailPath] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[OrderIndex] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Topics] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UrlRedirectRules]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UrlRedirectRules](
	[MediaDetailID] [bigint] NOT NULL,
	[VirtualPathToRedirect] [nvarchar](255) NOT NULL,
	[RedirectToUrl] [nvarchar](255) NOT NULL,
	[Is301Redirect] [bit] NOT NULL,
 CONSTRAINT [PK_UrlRedirectRules] PRIMARY KEY CLUSTERED 
(
	[MediaDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProfilePhoto] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[EmailAddress] [nvarchar](255) NOT NULL,
	[AfterLoginStartPage] [nvarchar](255) NOT NULL,
	[AuthenticationType] [nvarchar](255) NOT NULL,
	[LastLoggedInAt] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK__Users__3214EC277B264821] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsersMediaDetails]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersMediaDetails](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[MediaDetailID] [bigint] NOT NULL,
	[PermissionID] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_UsersMediaDetails_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsersRoles]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersRoles](
	[UserID] [bigint] NOT NULL,
	[RoleID] [bigint] NOT NULL,
 CONSTRAINT [PK__UsersRol__AF27604F2DB1C7EE] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WebserviceRequests]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebserviceRequests](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[UrlReferrer] [nvarchar](max) NOT NULL,
	[QueryString] [nvarchar](max) NOT NULL,
	[Method] [nvarchar](255) NOT NULL,
	[Response] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_WebserviceRequests] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Website]    Script Date: 2016-10-12 9:17:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Website](
	[MediaDetailID] [bigint] NOT NULL,
	[CodeInHead] [nvarchar](max) NOT NULL,
	[CodeInBody] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Website] PRIMARY KEY CLUSTERED 
(
	[MediaDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[EmailLog] ON 

INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (1, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=59', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T08:20:00.447' AS DateTime), CAST(N'2016-07-18T08:20:00.447' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (2, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=59', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T08:22:10.077' AS DateTime), CAST(N'2016-07-18T08:22:10.077' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (3, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=59', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T08:23:06.353' AS DateTime), CAST(N'2016-07-18T08:23:06.353' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (4, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=59', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T08:23:20.240' AS DateTime), CAST(N'2016-07-18T08:23:20.240' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (5, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=59', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T08:28:36.810' AS DateTime), CAST(N'2016-07-18T08:28:36.810' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (6, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=59', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T08:57:51.470' AS DateTime), CAST(N'2016-07-18T08:57:51.470' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (7, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=59', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T08:58:43.987' AS DateTime), CAST(N'2016-07-18T08:58:43.987' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (8, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=59', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T09:33:20.773' AS DateTime), CAST(N'2016-07-18T09:33:20.773' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (9, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=59', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T09:54:02.067' AS DateTime), CAST(N'2016-07-18T09:54:02.067' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (10, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=63', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T09:57:14.700' AS DateTime), CAST(N'2016-07-18T09:57:14.700' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (11, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=63', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T09:58:41.070' AS DateTime), CAST(N'2016-07-18T09:58:41.070' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (12, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=63', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T10:02:27.000' AS DateTime), CAST(N'2016-07-18T10:02:27.000' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (13, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=63', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T10:09:33.200' AS DateTime), CAST(N'2016-07-18T10:09:33.200' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (14, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T15:18:33.723' AS DateTime), CAST(N'2016-07-18T15:18:33.723' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (15, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T15:58:33.047' AS DateTime), CAST(N'2016-07-18T15:58:33.047' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (16, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T16:06:49.193' AS DateTime), CAST(N'2016-07-18T16:06:49.193' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (17, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T16:18:36.263' AS DateTime), CAST(N'2016-07-18T16:18:36.263' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (18, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T16:24:09.270' AS DateTime), CAST(N'2016-07-18T16:24:09.270' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (19, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T16:25:35.753' AS DateTime), CAST(N'2016-07-18T16:25:35.753' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (20, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T16:27:49.897' AS DateTime), CAST(N'2016-07-18T16:27:49.897' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (21, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-18T16:33:32.310' AS DateTime), CAST(N'2016-07-18T16:33:32.310' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (22, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:32:57.707' AS DateTime), CAST(N'2016-07-19T08:32:57.707' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (23, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:33:43.900' AS DateTime), CAST(N'2016-07-19T08:33:43.900' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (24, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:33:46.693' AS DateTime), CAST(N'2016-07-19T08:33:46.693' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (25, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:33:48.070' AS DateTime), CAST(N'2016-07-19T08:33:48.070' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (26, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:33:49.277' AS DateTime), CAST(N'2016-07-19T08:33:49.277' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (27, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:33:50.473' AS DateTime), CAST(N'2016-07-19T08:33:50.473' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (28, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:33:52.937' AS DateTime), CAST(N'2016-07-19T08:33:52.937' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (29, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:33:54.130' AS DateTime), CAST(N'2016-07-19T08:33:54.130' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (30, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:33:55.303' AS DateTime), CAST(N'2016-07-19T08:33:55.303' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (31, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:33:56.673' AS DateTime), CAST(N'2016-07-19T08:33:56.673' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (32, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:33:58.247' AS DateTime), CAST(N'2016-07-19T08:33:58.247' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (33, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:34:33.223' AS DateTime), CAST(N'2016-07-19T08:34:33.223' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (34, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:58:04.493' AS DateTime), CAST(N'2016-07-19T08:58:04.493' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (35, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:58:07.867' AS DateTime), CAST(N'2016-07-19T08:58:07.867' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (36, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:58:10.497' AS DateTime), CAST(N'2016-07-19T08:58:10.497' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (37, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:58:13.617' AS DateTime), CAST(N'2016-07-19T08:58:13.617' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (38, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:58:17.453' AS DateTime), CAST(N'2016-07-19T08:58:17.453' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (39, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T08:58:19.477' AS DateTime), CAST(N'2016-07-19T08:58:19.477' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (40, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:12:46.880' AS DateTime), CAST(N'2016-07-19T09:12:46.880' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (41, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:12:48.577' AS DateTime), CAST(N'2016-07-19T09:12:48.577' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (42, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:12:49.947' AS DateTime), CAST(N'2016-07-19T09:12:49.947' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (43, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:12:51.003' AS DateTime), CAST(N'2016-07-19T09:12:51.003' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (44, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:12:52.213' AS DateTime), CAST(N'2016-07-19T09:12:52.213' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (45, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:12:54.363' AS DateTime), CAST(N'2016-07-19T09:12:54.363' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (46, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:12:54.620' AS DateTime), CAST(N'2016-07-19T09:12:54.620' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (47, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:22:03.957' AS DateTime), CAST(N'2016-07-19T09:22:03.957' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (48, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:22:05.810' AS DateTime), CAST(N'2016-07-19T09:22:05.810' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (49, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:22:07.740' AS DateTime), CAST(N'2016-07-19T09:22:07.740' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (50, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:22:09.710' AS DateTime), CAST(N'2016-07-19T09:22:09.710' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (51, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:22:13.080' AS DateTime), CAST(N'2016-07-19T09:22:13.080' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (52, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:22:15.337' AS DateTime), CAST(N'2016-07-19T09:22:15.337' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (53, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:25:38.693' AS DateTime), CAST(N'2016-07-19T09:25:38.693' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (54, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:25:40.183' AS DateTime), CAST(N'2016-07-19T09:25:40.183' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (55, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:25:42.443' AS DateTime), CAST(N'2016-07-19T09:25:42.443' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (56, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:25:44.157' AS DateTime), CAST(N'2016-07-19T09:25:44.157' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (57, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:38:02.017' AS DateTime), CAST(N'2016-07-19T09:38:02.017' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (58, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:38:05.230' AS DateTime), CAST(N'2016-07-19T09:38:05.230' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (59, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:38:07.993' AS DateTime), CAST(N'2016-07-19T09:38:07.993' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (60, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:38:10.943' AS DateTime), CAST(N'2016-07-19T09:38:10.943' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (61, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:38:15.120' AS DateTime), CAST(N'2016-07-19T09:38:15.120' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (62, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=64', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T09:38:18.600' AS DateTime), CAST(N'2016-07-19T09:38:18.600' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (63, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:10:45.293' AS DateTime), CAST(N'2016-07-19T10:10:45.293' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (64, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:11:27.753' AS DateTime), CAST(N'2016-07-19T10:11:27.753' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (65, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:11:30.767' AS DateTime), CAST(N'2016-07-19T10:11:30.767' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (66, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:11:33.690' AS DateTime), CAST(N'2016-07-19T10:11:33.690' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (67, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:11:36.830' AS DateTime), CAST(N'2016-07-19T10:11:36.830' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (68, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:47:29.280' AS DateTime), CAST(N'2016-07-19T10:47:29.280' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (69, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:47:32.697' AS DateTime), CAST(N'2016-07-19T10:47:32.697' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (70, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:47:40.773' AS DateTime), CAST(N'2016-07-19T10:47:40.773' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (71, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:47:44.417' AS DateTime), CAST(N'2016-07-19T10:47:44.417' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (72, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:47:47.830' AS DateTime), CAST(N'2016-07-19T10:47:47.830' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (73, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T10:47:52.100' AS DateTime), CAST(N'2016-07-19T10:47:52.100' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (74, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T11:11:02.003' AS DateTime), CAST(N'2016-07-19T11:11:02.003' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (75, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T11:11:04.200' AS DateTime), CAST(N'2016-07-19T11:11:04.200' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (76, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T11:11:05.890' AS DateTime), CAST(N'2016-07-19T11:11:05.890' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (77, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T11:24:56.180' AS DateTime), CAST(N'2016-07-19T11:24:56.180' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (78, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T11:24:59.147' AS DateTime), CAST(N'2016-07-19T11:24:59.147' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (79, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T11:25:02.390' AS DateTime), CAST(N'2016-07-19T11:25:02.390' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (80, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T11:25:04.517' AS DateTime), CAST(N'2016-07-19T11:25:04.517' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (81, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:19:59.867' AS DateTime), CAST(N'2016-07-19T13:19:59.867' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (82, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:20:04.610' AS DateTime), CAST(N'2016-07-19T13:20:04.610' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (83, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:20:07.033' AS DateTime), CAST(N'2016-07-19T13:20:07.033' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (84, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:20:09.693' AS DateTime), CAST(N'2016-07-19T13:20:09.693' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (85, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:20:12.580' AS DateTime), CAST(N'2016-07-19T13:20:12.580' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (86, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:34:11.603' AS DateTime), CAST(N'2016-07-19T13:34:11.603' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (87, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:34:14.477' AS DateTime), CAST(N'2016-07-19T13:34:14.477' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (88, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:34:17.073' AS DateTime), CAST(N'2016-07-19T13:34:17.073' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (89, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:34:20.640' AS DateTime), CAST(N'2016-07-19T13:34:20.640' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (90, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:34:23.457' AS DateTime), CAST(N'2016-07-19T13:34:23.457' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (91, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T13:34:25.763' AS DateTime), CAST(N'2016-07-19T13:34:25.763' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (92, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:04:24.617' AS DateTime), CAST(N'2016-07-19T14:04:24.617' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (93, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:04:27.287' AS DateTime), CAST(N'2016-07-19T14:04:27.287' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (94, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:04:29.027' AS DateTime), CAST(N'2016-07-19T14:04:29.027' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (95, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:04:31.607' AS DateTime), CAST(N'2016-07-19T14:04:31.607' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (96, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:15:25.687' AS DateTime), CAST(N'2016-07-19T14:15:25.687' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (97, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:15:27.197' AS DateTime), CAST(N'2016-07-19T14:15:27.197' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (98, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:15:29.170' AS DateTime), CAST(N'2016-07-19T14:15:29.170' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (99, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:17:59.960' AS DateTime), CAST(N'2016-07-19T14:17:59.960' AS DateTime))
GO
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (100, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:18:01.693' AS DateTime), CAST(N'2016-07-19T14:18:01.693' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (101, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:18:03.047' AS DateTime), CAST(N'2016-07-19T14:18:03.047' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (102, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=30', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:18:04.767' AS DateTime), CAST(N'2016-07-19T14:18:04.767' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (103, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=33', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:44:26.467' AS DateTime), CAST(N'2016-07-19T14:44:26.467' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (104, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=33', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:44:39.407' AS DateTime), CAST(N'2016-07-19T14:44:39.407' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (105, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=33', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:44:43.607' AS DateTime), CAST(N'2016-07-19T14:44:43.607' AS DateTime))
INSERT [dbo].[EmailLog] ([ID], [SenderName], [SenderEmailAddress], [ToEmailAddresses], [FromEmailAddress], [Subject], [Message], [VisitorIP], [RequestUrl], [ServerMessage], [DateCreated], [DateLastModified]) VALUES (106, N'WebApplication', N'system@localhost', N'info@website.com', N'noreply@domain.com', N'Comment Approval Request', N'There was a comment made on a news article with the title ''Blog Post 1'', click on the following link to approve or reject this comment: http://localhost:1112/admin/BlogPost/Edit.aspx?id=33', N'::1', N'http://localhost:1112/blog/blog-post-1/', N'The remote name could not be resolved: ''[SMTP_HOSTNAME_IP]''', CAST(N'2016-07-19T14:44:46.253' AS DateTime), CAST(N'2016-07-19T14:44:46.253' AS DateTime))
SET IDENTITY_INSERT [dbo].[EmailLog] OFF
SET IDENTITY_INSERT [dbo].[Fields] ON 

INSERT [dbo].[Fields] ([ID], [FieldCode], [FieldLabel], [RenderLabelAfterControl], [AdminControl], [FieldValue], [GetAdminControlValue], [SetAdminControlValue], [OrderIndex], [GroupName], [FrontEndLayout], [DateCreated], [DateLastModified]) VALUES (1, N'HideAdminFields', N'HideAdminFields', 0, N'<script type="text/javascript" charset="utf-8">
    $(document).ready(function(){
        $("#VirtualPathHolder,#SectionTitleHolder,#LongDescriptionHolder,#ThumbnailPathHolder, #MasterPageSelectorHolder,#TagsHolder, #PathToFileHolder, #ShortDescriptionHolder").hide();
    });
</script>', N'', N'', N'', 0, N'Scripts', N'', CAST(N'2016-10-12T08:58:50.687' AS DateTime), CAST(N'2016-10-12T08:58:50.687' AS DateTime))
INSERT [dbo].[Fields] ([ID], [FieldCode], [FieldLabel], [RenderLabelAfterControl], [AdminControl], [FieldValue], [GetAdminControlValue], [SetAdminControlValue], [OrderIndex], [GroupName], [FrontEndLayout], [DateCreated], [DateLastModified]) VALUES (2, N'HideAdminFields', N'HideAdminFields', 0, N'<script type="text/javascript" charset="utf-8">
    $(document).ready(function(){
        $("#VirtualPathHolder,#SectionTitleHolder,#LongDescriptionHolder,#ThumbnailPathHolder, #MasterPageSelectorHolder,#TagsHolder, #PathToFileHolder, #ShortDescriptionHolder").hide();
    });
</script>', N'', N'', N'', 0, N'Scripts', N'', CAST(N'2016-10-12T08:58:50.723' AS DateTime), CAST(N'2016-10-12T08:58:50.723' AS DateTime))
INSERT [dbo].[Fields] ([ID], [FieldCode], [FieldLabel], [RenderLabelAfterControl], [AdminControl], [FieldValue], [GetAdminControlValue], [SetAdminControlValue], [OrderIndex], [GroupName], [FrontEndLayout], [DateCreated], [DateLastModified]) VALUES (3, N'ArticlePublishDate', N'ArticlePublishDate', 0, N'<asp:TextBox runat=''server'' class=''datetimepicker'' />', N'', N'Text', N'Text', 0, N'', N'@{
    var field = Model.Field as MediaDetailField;
    if(!string.IsNullOrEmpty(field.FieldValue))
    {
        var dateTime = DateTime.Parse(field.FieldValue);
        @StringHelper.FormatOnlyDate(dateTime)
    }
    else
    {
        @field.FieldValue
    }
}', CAST(N'2016-10-12T09:07:05.683' AS DateTime), CAST(N'2016-10-12T09:07:05.683' AS DateTime))
INSERT [dbo].[Fields] ([ID], [FieldCode], [FieldLabel], [RenderLabelAfterControl], [AdminControl], [FieldValue], [GetAdminControlValue], [SetAdminControlValue], [OrderIndex], [GroupName], [FrontEndLayout], [DateCreated], [DateLastModified]) VALUES (4, N'ArticlePublishDate', N'ArticlePublishDate', 0, N'<asp:TextBox runat=''server'' class=''datetimepicker'' />', N'', N'Text', N'Text', 0, N'', N'@{
    var field = Model.Field as MediaDetailField;
    if(!string.IsNullOrEmpty(field.FieldValue))
    {
        var dateTime = DateTime.Parse(field.FieldValue);
        @StringHelper.FormatOnlyDate(dateTime)
    }
    else
    {
        @field.FieldValue
    }
}', CAST(N'2016-10-12T09:07:05.730' AS DateTime), CAST(N'2016-10-12T09:07:05.730' AS DateTime))
INSERT [dbo].[Fields] ([ID], [FieldCode], [FieldLabel], [RenderLabelAfterControl], [AdminControl], [FieldValue], [GetAdminControlValue], [SetAdminControlValue], [OrderIndex], [GroupName], [FrontEndLayout], [DateCreated], [DateLastModified]) VALUES (5, N'ArticlePublishDate', N'ArticlePublishDate', 0, N'<asp:TextBox runat=''server'' class=''datetimepicker'' />', N'', N'Text', N'Text', 0, N'', N'@{
    var field = Model.Field as MediaDetailField;
    if(!string.IsNullOrEmpty(field.FieldValue))
    {
        var dateTime = DateTime.Parse(field.FieldValue);
        @StringHelper.FormatOnlyDate(dateTime)
    }
    else
    {
        @field.FieldValue
    }
}', CAST(N'2016-10-12T09:07:05.750' AS DateTime), CAST(N'2016-10-12T09:07:05.750' AS DateTime))
SET IDENTITY_INSERT [dbo].[Fields] OFF
SET IDENTITY_INSERT [dbo].[IPLocationTracker] ON 

INSERT [dbo].[IPLocationTracker] ([ID], [IPAddress], [Location], [DateCreated], [DateLastModified]) VALUES (1, N'::1', N'{"ip":"::1","country_code":"","country_name":"","region_code":"","region_name":"","city":"","zip_code":"","time_zone":"","latitude":0,"longitude":0,"metro_code":0}
', CAST(N'2016-06-10T14:16:09.260' AS DateTime), CAST(N'2016-06-10T14:16:09.260' AS DateTime))
SET IDENTITY_INSERT [dbo].[IPLocationTracker] OFF
SET IDENTITY_INSERT [dbo].[Languages] ON 

INSERT [dbo].[Languages] ([ID], [DisplayName], [Name], [UriSegment], [CultureCode], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (5, N'English', N'English', N'en', N'en-CA', N'Canadian English', 1, CAST(N'2010-12-10T22:51:31.000' AS DateTime), CAST(N'2012-01-12T10:23:53.993' AS DateTime))
INSERT [dbo].[Languages] ([ID], [DisplayName], [Name], [UriSegment], [CultureCode], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (6, N'French', N'French', N'fr', N'fr-CA', N'Canadian French', 1, CAST(N'2011-02-05T16:01:55.260' AS DateTime), CAST(N'2016-09-28T13:35:37.933' AS DateTime))
SET IDENTITY_INSERT [dbo].[Languages] OFF
SET IDENTITY_INSERT [dbo].[MasterPages] ON 

INSERT [dbo].[MasterPages] ([ID], [Name], [PathToFile], [MobileTemplate], [DateCreated], [DateLastModified]) VALUES (2, N'Template 1', N'~/Views/MasterPages/SiteTemplates/Template1.Master', N'~/Views/MasterPages/MobileTemplate/Mobile.Master', CAST(N'2012-01-12T15:32:36.367' AS DateTime), CAST(N'2015-02-17T10:17:02.667' AS DateTime))
INSERT [dbo].[MasterPages] ([ID], [Name], [PathToFile], [MobileTemplate], [DateCreated], [DateLastModified]) VALUES (3, N'Template 2', N'~/Views/MasterPages/SiteTemplates/Template2.Master', N'~/Views/MasterPages/MobileTemplate/Mobile.Master', CAST(N'2012-03-05T15:28:40.647' AS DateTime), CAST(N'2015-02-17T10:17:07.783' AS DateTime))
SET IDENTITY_INSERT [dbo].[MasterPages] OFF
SET IDENTITY_INSERT [dbo].[Media] ON 

INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (1, NULL, 0)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (2, 1, 0)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (3, 2, 3)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (4, 2, 4)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (5, 2, 5)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (6, 2, 6)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (7, 6, 0)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (8, 2, 0)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (9, 2, 2)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (10, 2, 1)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (25, 2, 8)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (34, 2, 7)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (35, 34, 0)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (36, 25, 0)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (37, 36, 0)
INSERT [dbo].[Media] ([ID], [ParentMediaID], [OrderIndex]) VALUES (38, 36, 1)
SET IDENTITY_INSERT [dbo].[Media] OFF
SET IDENTITY_INSERT [dbo].[MediaDetails] ON 

INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (3, 1, N'Root', N'Root', 0, 0, N'', N'Root', N'<p>Root</p>', N'<p>Root</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-14T15:33:35.237' AS DateTime), 0, 0, 1, N'', N'', N'~/', 13, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-14T15:33:35.153' AS DateTime), CAST(N'2016-07-14T15:33:35.237' AS DateTime), 0, 0, 0, N'', NULL, N'', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (4, 2, N'Home', N'Home', 0, 0, N'', N'Home', N'<p>Home</p>', N'<p>Home</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-14T15:33:44.000' AS DateTime), 1, 1, 1, N'', N'', N'~/', 19, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-14T15:33:44.390' AS DateTime), CAST(N'2016-10-03T14:46:46.133' AS DateTime), 0, 0, 0, N'', NULL, N'{MainContent}', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (5, 3, N'Page Not Found', N'Page Not Found', 0, 0, N'', N'Page Not Found', N'<p>Page Not Found</p>', N'<p>Page Not Found</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-14T15:33:58.783' AS DateTime), 0, 1, 1, N'', N'', N'~/page-not-found/', 1, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-14T15:33:58.700' AS DateTime), CAST(N'2016-07-14T15:34:03.267' AS DateTime), 0, 0, 0, N'', NULL, N'{MainContent}', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (6, 4, N'Site Offline', N'Site Offline', 0, 0, N'', N'Site Offline', N'<p>Site Offline</p>', N'<p>Site Offline</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-14T15:33:58.000' AS DateTime), 0, 1, 1, N'', N'', N'~/site-offline/', 1, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-14T15:34:10.727' AS DateTime), CAST(N'2016-07-14T15:34:21.797' AS DateTime), 0, 0, 0, N'', NULL, N'{MainContent}', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (8, 5, N'Login', N'Login', 0, 0, N'', N'Login', N'<p>Login</p>', N'<p>Login</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-14T15:34:57.207' AS DateTime), 0, 1, 1, N'', N'/Views/PageHandlers/Login.aspx', N'~/login/', 1, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-14T15:34:57.140' AS DateTime), CAST(N'2016-07-14T15:35:01.940' AS DateTime), 0, 0, 0, N'', NULL, N'{MainContent}', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (9, 6, N'Url Redirect List', N'Url Redirect List', 0, 0, N'', N'Url Redirect List', N'<p>Url Redirect List</p>', N'<p>Url Redirect List</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-14T15:35:29.327' AS DateTime), 0, 0, 1, N'', N'', N'~/url-redirect-list/', 17, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-14T15:35:29.253' AS DateTime), CAST(N'2016-07-14T15:35:29.327' AS DateTime), 0, 0, 0, N'', NULL, N'', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (10, 7, N'/sample -> /', N'/sample -> /', 0, 0, N'', N'/sample -> /', N'<p>/sample -&gt; /</p>', N'<p>/sample -&gt; /</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-14T15:35:57.000' AS DateTime), 0, 0, 1, N'', N'', N'~/url-redirect-list/sample/', 16, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-14T15:35:57.930' AS DateTime), CAST(N'2016-07-14T15:47:12.803' AS DateTime), 0, 0, 0, N'', NULL, N'', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (19, 8, N'Site Map', N'Site Map', 0, 0, N'', N'Site Map', N'<p>Site Map</p>', N'<p>Site Map</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-14T15:33:58.000' AS DateTime), 0, 1, 1, N'', N'/Views/PageHandlers/SiteMap.aspx', N'~/site-map/', 1, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-14T15:53:41.927' AS DateTime), CAST(N'2016-07-14T15:55:15.170' AS DateTime), 0, 0, 0, N'', NULL, N'{MainContent}', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (23, 9, N'Search', N'Search', 0, 0, N'', N'Search', N'<p>Search</p>', N'<p>Search</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-14T15:33:58.000' AS DateTime), 0, 1, 1, N'', N'/Views/PageHandlers/Search.aspx', N'~/search/', 1, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-14T15:55:29.097' AS DateTime), CAST(N'2016-07-14T15:55:56.637' AS DateTime), 0, 0, 0, N'', NULL, N'{MainContent}', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (27, 10, N'Tags', N'Tags', 0, 0, N'', N'Tags', N'<p>Tags</p>', N'<p>Tags</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-14T15:33:58.000' AS DateTime), 0, 1, 1, N'', N'/Views/PageHandlers/Tags.aspx', N'~/tags/', 1, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-14T15:56:27.437' AS DateTime), CAST(N'2016-07-14T15:56:52.243' AS DateTime), 0, 0, 0, N'', NULL, N'{MainContent}', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (51, 25, N'Blog', N'Blog', 0, 0, N'', N'Blog', N'<p>Blog</p>', N'<p>Blog</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-07-15T15:24:59.800' AS DateTime), 1, 0, 0, N'', N'', N'~/blog/', 23, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-07-15T15:24:59.707' AS DateTime), CAST(N'2016-07-15T15:24:59.800' AS DateTime), 0, 0, 0, N'', NULL, N'', N'', N'', 0, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (76, 34, N'Landing Pages', N'Landing Pages', 0, 0, N'', N'Landing Pages', N'<p>Landing Pages</p>', N'<p>Landing Pages</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-10-12T08:55:10.640' AS DateTime), 0, 0, 1, N'', N'', N'~/landing-pages/', 28, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-10-12T08:55:10.497' AS DateTime), CAST(N'2016-10-12T08:55:10.640' AS DateTime), 0, 0, 0, N'', NULL, N'', N'', N'', 1, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (77, 35, N'Landing Page 1', N'Landing Page 1', 0, 0, N'', N'Landing Page 1', N'<p>Landing Page 1</p>', N'<p>Landing Page 1</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-10-12T08:55:26.000' AS DateTime), 0, 0, 1, N'', N'', N'~/landing-pages/landing-page-1/', 27, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-10-12T08:55:26.380' AS DateTime), CAST(N'2016-10-12T08:55:32.450' AS DateTime), 0, 0, 0, N'', NULL, N'', N'', N'', 1, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (79, 36, N'Category 1', N'Category 1', 0, 0, N'', N'Category 1', N'<p>Category 1</p>', N'<p>Category 1</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-10-12T08:59:19.420' AS DateTime), 0, 0, 1, N'', N'', N'~/blog/category-1/', 26, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-10-12T08:59:19.327' AS DateTime), CAST(N'2016-10-12T08:59:19.420' AS DateTime), 0, 0, 0, N'', NULL, N'', N'', N'', 1, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (80, 37, N'Blog Post 1', N'Blog Post 1', 0, 0, N'', N'Blog Post 1', N'<p>Blog Post 1</p>', N'<p>Blog Post 1</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-10-12T08:59:31.387' AS DateTime), 0, 0, 1, N'', N'', N'~/blog/category-1/blog-post-1/', 24, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-10-12T08:59:31.273' AS DateTime), CAST(N'2016-10-12T08:59:31.387' AS DateTime), 0, 0, 0, N'', NULL, N'{MainContent}
<Site:CommentsForm runat="server" />
<Site:CommentsList runat="server" />', N'', N'', 1, N'')
INSERT [dbo].[MediaDetails] ([ID], [MediaID], [Title], [LinkTitle], [UseDirectLink], [OpenInNewWindow], [DirectLink], [SectionTitle], [ShortDescription], [MainContent], [MetaKeywords], [MetaDescription], [LanguageID], [NumberOfViews], [NumberOfStars], [IsDeleted], [AllowCommenting], [ExpiryDate], [PublishDate], [ShowInMenu], [ShowInSearchResults], [EnableCaching], [SefTitle], [Handler], [CachedVirtualPath], [MediaTypeID], [HistoryVersionNumber], [HistoryForMediaDetailID], [IsDraft], [CreatedByUserID], [LastUpdatedByUserID], [CanAddToCart], [Price], [QuantityInStock], [RecurringTimePeriod], [RedirectToFirstChild], [DateCreated], [DateLastModified], [RenderInFooter], [ForceSSL], [IsProtected], [CssClasses], [MasterPageID], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [PathToFile]) VALUES (81, 38, N'Blog Post 2', N'Blog Post 2', 0, 0, N'', N'Blog Post 2', N'<p>Blog Post 2</p>', N'<p>Blog Post 2</p>', N'', N'', 5, 0, 0, 0, 0, NULL, CAST(N'2016-10-12T08:59:31.000' AS DateTime), 0, 0, 1, N'', N'', N'~/blog/category-1/blog-post-2/', 24, 0, NULL, 0, 25, 25, 0, CAST(0.00 AS Decimal(18, 2)), 0, N'', 0, CAST(N'2016-10-12T08:59:37.733' AS DateTime), CAST(N'2016-10-12T08:59:43.573' AS DateTime), 0, 0, 0, N'', NULL, N'{MainContent}
<Site:CommentsForm runat="server" />
<Site:CommentsList runat="server" />', N'', N'', 1, N'')
SET IDENTITY_INSERT [dbo].[MediaDetails] OFF
INSERT [dbo].[MediaDetailsFields] ([FieldID], [MediaDetailID], [MediaTypeFieldID], [UseMediaTypeFieldFrontEndLayout]) VALUES (2, 9, 1, 1)
INSERT [dbo].[MediaDetailsFields] ([FieldID], [MediaDetailID], [MediaTypeFieldID], [UseMediaTypeFieldFrontEndLayout]) VALUES (4, 80, 3, 1)
INSERT [dbo].[MediaDetailsFields] ([FieldID], [MediaDetailID], [MediaTypeFieldID], [UseMediaTypeFieldFrontEndLayout]) VALUES (5, 81, 3, 1)
SET IDENTITY_INSERT [dbo].[MediaTypes] ON 

INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (1, N'Page', N'Page', N'~/Views/MediaTypeHandlers/Page.aspx', NULL, 1, 1, 1, 1, 1, N'{MainContent}', N'', N'', 1, CAST(N'2011-02-25T16:00:51.810' AS DateTime), CAST(N'2016-10-12T08:50:38.013' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (13, N'RootPage', N'RootPage', N'~/Views/MediaTypeHandlers/RootPage.aspx', NULL, 1, 0, 0, 1, 1, N'', N'', N'', 1, CAST(N'2012-01-17T14:36:29.107' AS DateTime), CAST(N'2016-10-12T08:53:58.460' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (16, N'UrlRedirectRule', N'UrlRedirectRule', N'~/Views/MediaTypeHandlers/UrlRedirectRule.aspx', NULL, 1, 0, 0, 1, 1, N'', N'', N'', 1, CAST(N'2012-04-13T15:39:37.113' AS DateTime), CAST(N'2016-10-12T08:51:17.833' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (17, N'UrlRedirectRuleList', N'UrlRedirectRuleList', N'~/Views/MediaTypeHandlers/UrlRedirectRuleList.aspx', NULL, 1, 0, 0, 1, 1, N'', N'', N'', 1, CAST(N'2012-04-13T15:40:04.020' AS DateTime), CAST(N'2016-10-12T08:58:52.720' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (19, N'Website', N'Website', N'~/Views/MediaTypeHandlers/Website.aspx', 2, 1, 1, 1, 1, 1, N'', N'', N'', 1, CAST(N'2012-06-14T16:49:05.997' AS DateTime), CAST(N'2016-10-12T08:54:30.817' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (22, N'NoTemplatePage', N'NoTemplatePage', N'~/Views/MediaTypeHandlers/NoTemplatePage.aspx', NULL, 1, 0, 0, 1, 0, N'', N'', N'', 1, CAST(N'2014-09-26T10:46:02.940' AS DateTime), CAST(N'2016-10-12T08:50:29.067' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (23, N'Blog', N'Blog', N'/Views/MediaTypeHandlers/Blog.aspx', NULL, 1, 1, 0, 1, 0, N'<Site:RenderChildren runat="server" ShowPager="True" PageSize="10" ChildPropertyName="UseSummaryLayout" Where=''MediaType.Name=="BlogPost"'' OrderBy="DateCreated DESC" />', N'', N'', 1, CAST(N'2016-02-26T14:56:06.043' AS DateTime), CAST(N'2016-10-12T09:08:50.630' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (24, N'BlogPost', N'BlogPost', N'~/Views/MediaTypeHandlers/Page.aspx', NULL, 1, 0, 0, 1, 1, N'@model Page
@{
    var relatedItems = Model.GetRelatedItems().Take(3);
    
    var currentArticleDate = Model.RenderShortCode("{Field:ArticlePublishDate}");
    
    if(string.IsNullOrEmpty(currentArticleDate) && Model.PublishDate != null)
    {
        currentArticleDate = StringHelper.FormatOnlyDate((DateTime)Model.PublishDate);
    }    
}
<div class="blog-details">
<article>
    <div class="container">
    <h1 class="blog-title">{SectionTitle}</h1>
    <span class="blog-date">@currentArticleDate</span>
        <div class="blog-social-share">
            <span class=''st_sharethis'' displayText=''ShareThis''></span>
            <span class=''st_facebook'' displayText=''Facebook''></span>
            <span class=''st_twitter'' displayText=''Tweet''></span>
            <span class=''st_linkedin'' displayText=''LinkedIn''></span>
            <span class=''st_email'' displayText=''Email''></span>
        </div>
    <img src="{PathToFile}" />
    {MainContent}
    </div>
</article>
<aside>
    <div class="container related-blog-posts">
          <span class="return-blog-link"><a href="/blog">&laquo; back to blog</a></span>
        <h4>Related Posts</h4>
            @foreach(var item in relatedItems)
            {
                var date = item.RenderShortCode("{Field:ArticlePublishDate}");
                
                if(string.IsNullOrEmpty(date) && item.PublishDate != null)
                {
                    date = StringHelper.FormatOnlyDate((DateTime)item.PublishDate);
                }
                
                <div class="post">
            		<h5>@item.SectionTitle</h5>
            		<small><em>@date</em></small>
            		<p>@Raw(StringHelper.StripHtmlTags(item.ShortDescription))</p>
            		<a href="@item.AbsoluteUrl" class="related-blog-posts-link">Read more &raquo;</a>
        		</div>
            }
    </div>
</aside> 
</div>

<Site:CommentsForm runat="server" />
<Site:CommentsList runat="server" />', N'', N'', 1, CAST(N'2016-03-11T10:23:22.883' AS DateTime), CAST(N'2016-10-12T09:07:56.187' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (25, N'FieldItem', N'FieldItem', N'~/Views/MediaTypeHandlers/Page.aspx', NULL, 1, 0, 0, 0, 0, N'', N'', N'', 1, CAST(N'2016-07-06T09:44:49.593' AS DateTime), CAST(N'2016-10-12T08:50:12.350' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (26, N'BlogCategory', N'BlogCategory', N'/Views/MediaTypeHandlers/Blog.aspx', NULL, 1, 0, 0, 1, 1, N'', N'', N'', 1, CAST(N'2016-10-12T08:48:11.100' AS DateTime), CAST(N'2016-10-12T08:49:28.027' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (27, N'LandingPage', N'LandingPage', N'/Views/MediaTypeHandlers/LandingPage.aspx', NULL, 1, 0, 0, 1, 1, N'', N'', N'', 1, CAST(N'2016-10-12T08:52:47.283' AS DateTime), CAST(N'2016-10-12T08:53:42.297' AS DateTime))
INSERT [dbo].[MediaTypes] ([ID], [Name], [Label], [MediaTypeHandler], [MasterPageID], [IsActive], [ShowInMenu], [ShowInSearchResults], [ShowInSiteTree], [EnableCaching], [MainLayout], [SummaryLayout], [FeaturedLayout], [UseMediaTypeLayouts], [DateCreated], [DateLastModified]) VALUES (28, N'LandingPages', N'LandingPages', N'/Views/MediaTypeHandlers/Page.aspx', NULL, 1, 0, 0, 1, 1, N'', N'', N'', 1, CAST(N'2016-10-12T08:53:22.953' AS DateTime), CAST(N'2016-10-12T08:53:49.327' AS DateTime))
SET IDENTITY_INSERT [dbo].[MediaTypes] OFF
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (1, 1)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (1, 22)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (13, 19)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (17, 16)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (19, 1)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (19, 17)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (19, 22)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (19, 23)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (19, 28)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (22, 22)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (23, 26)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (26, 24)
INSERT [dbo].[MediaTypesChildren] ([MediaTypeID], [AllowedChildMediaTypeID]) VALUES (28, 27)
INSERT [dbo].[MediaTypesFields] ([FieldID], [MediaTypeID], [Instructions]) VALUES (1, 17, N'')
INSERT [dbo].[MediaTypesFields] ([FieldID], [MediaTypeID], [Instructions]) VALUES (3, 24, N'')
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (5)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (6)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (8)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (9)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (19)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (23)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (27)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (51)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (76)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (77)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (79)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (80)
INSERT [dbo].[Pages] ([MediaDetailID]) VALUES (81)
SET IDENTITY_INSERT [dbo].[Permissions] ON 

INSERT [dbo].[Permissions] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (3, N'Delete Items', N'DeleteItems', N'Delete Items', 1, CAST(N'2011-03-19T15:26:49.907' AS DateTime), CAST(N'2011-05-27T09:36:44.640' AS DateTime))
INSERT [dbo].[Permissions] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (5, N'Delete Items Permanently', N'DeleteItemsPermanently', N'Delete Items Permanently', 1, CAST(N'2011-03-25T20:00:56.710' AS DateTime), CAST(N'2011-05-27T09:36:30.840' AS DateTime))
INSERT [dbo].[Permissions] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (6, N'Publish Items', N'PublishItems', N'Publish Items', 1, CAST(N'2011-05-28T14:14:43.507' AS DateTime), CAST(N'2011-05-28T15:02:28.350' AS DateTime))
INSERT [dbo].[Permissions] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (7, N'Save Items', N'SaveItems', N'Save Items', 1, CAST(N'2011-05-28T14:52:56.920' AS DateTime), CAST(N'2011-10-26T14:34:40.003' AS DateTime))
INSERT [dbo].[Permissions] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (8, N'Access CMS', N'AccessCMS', N'Access CMS', 1, CAST(N'2011-05-31T10:09:51.497' AS DateTime), CAST(N'2011-05-31T10:12:50.617' AS DateTime))
INSERT [dbo].[Permissions] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (9, N'Access Tags Manager', N'AccessTagsManager', N'Access Tags Manager', 1, CAST(N'2011-10-26T14:56:49.363' AS DateTime), CAST(N'2011-10-26T14:56:49.363' AS DateTime))
INSERT [dbo].[Permissions] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (10, N'Access Banners Manager', N'AccessBannersManager', N'Access Banners Manager', 1, CAST(N'2011-10-26T14:57:05.257' AS DateTime), CAST(N'2011-10-26T14:57:05.257' AS DateTime))
INSERT [dbo].[Permissions] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (11, N'Access Advance Options', N'AccessAdvanceOptions', N'Access Advance Options', 1, CAST(N'2011-10-26T14:57:35.560' AS DateTime), CAST(N'2011-10-26T14:57:35.560' AS DateTime))
INSERT [dbo].[Permissions] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (12, N'Access Protected Sections', N'AccessProtectedSections', N'AccessProtectedSections', 1, CAST(N'2011-12-13T14:19:11.857' AS DateTime), CAST(N'2013-05-09T11:14:03.823' AS DateTime))
SET IDENTITY_INSERT [dbo].[Permissions] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (3, N'Publisher', N'Publisher', N'Publisher', 1, CAST(N'2011-05-31T10:15:49.637' AS DateTime), CAST(N'2015-09-15T09:13:38.967' AS DateTime))
INSERT [dbo].[Roles] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (4, N'Moderator', N'Moderator', N'Moderator', 1, CAST(N'2011-05-31T10:15:57.677' AS DateTime), CAST(N'2011-12-13T14:20:12.357' AS DateTime))
INSERT [dbo].[Roles] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (18, N'Contributor', N'Contributor', N'Contributor', 1, CAST(N'2011-05-31T14:38:36.053' AS DateTime), CAST(N'2015-10-16T14:20:38.323' AS DateTime))
INSERT [dbo].[Roles] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (20, N'Administrator', N'Administrator', N'Administrator', 1, CAST(N'2011-10-26T14:58:24.577' AS DateTime), CAST(N'2011-12-13T14:19:37.960' AS DateTime))
INSERT [dbo].[Roles] ([ID], [Name], [EnumName], [Description], [IsActive], [DateCreated], [DateLastModified]) VALUES (22, N'Front End User', N'FrontEndUser', N'Front End User', 1, CAST(N'2013-05-09T11:00:25.760' AS DateTime), CAST(N'2013-06-06T14:20:17.563' AS DateTime))
SET IDENTITY_INSERT [dbo].[Roles] OFF
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (3, 6)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (3, 7)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (3, 8)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (3, 12)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (4, 3)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (4, 5)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (4, 6)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (4, 7)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (4, 8)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (4, 9)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (4, 10)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (4, 12)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (18, 7)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (18, 8)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (18, 12)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (20, 3)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (20, 5)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (20, 6)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (20, 7)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (20, 8)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (20, 9)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (20, 10)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (20, 11)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (20, 12)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (22, 3)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (22, 5)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (22, 6)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (22, 7)
INSERT [dbo].[RolesPermissions] ([RoleID], [PermissionID]) VALUES (22, 12)
INSERT [dbo].[RootPages] ([MediaDetailID]) VALUES (3)
SET IDENTITY_INSERT [dbo].[Settings] ON 

INSERT [dbo].[Settings] ([ID], [GlobalCodeInHead], [GlobalCodeInBody], [ShoppingCartTax], [MaxRequestLength], [MaxUploadFileSizePerFile], [SiteOnlineAtDateTime], [SiteOfflineAtDateTime], [SiteOfflineUrl], [PageNotFoundUrl], [EnableGlossaryTerms], [OutputCacheDurationInSeconds], [DefaultLanguageID], [DefaultMasterPageID], [DateCreated], [DateLastModified]) VALUES (1, N'<link rel="stylesheet" href="/Scripts/colorbox-master/example3/colorbox.css" type="text/css" media="all" />
<link rel="stylesheet" href="/Views/MasterPages/SiteTemplates/css/Style.min.css"/>
<link rel="shortcut icon" href="/favicon.ico" />', N'<script src="//cdnjs.cloudflare.com/ajax/libs/jquery/1.12.1/jquery.min.js" type="text/javascript" charset="utf-8"></script>
<script src="//cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js" type="text/javascript" charset="utf-8"></script>
<script src="//cdnjs.cloudflare.com/ajax/libs/jquery.colorbox/1.6.3/jquery.colorbox-min.js" type="text/javascript" charset="utf-8"></script>
<script src="/Scripts/global.js" type="text/javascript" charset="utf-8"></script>
<script src="/Scripts/commonFrontEnd.js" type="text/javascript" charset="utf-8"></script>', CAST(0.00 AS Decimal(18, 2)), 0, 0, CAST(N'2016-06-17T22:39:00.000' AS DateTime), NULL, N'/site-offline/', N'/page-not-found/', 0, 31536000, 5, 3, CAST(N'2012-02-08T10:55:42.713' AS DateTime), CAST(N'2016-07-06T10:32:14.550' AS DateTime))
SET IDENTITY_INSERT [dbo].[Settings] OFF
SET IDENTITY_INSERT [dbo].[Tags] ON 

INSERT [dbo].[Tags] ([ID], [Name], [Description], [SefTitle], [ThumbnailPath], [IsActive], [OrderIndex], [DateCreated], [DateLastModified]) VALUES (14, N'test', N'test', N'test', N'', 0, 0, CAST(N'2016-06-11T22:05:00.043' AS DateTime), CAST(N'2016-06-11T22:05:00.043' AS DateTime))
INSERT [dbo].[Tags] ([ID], [Name], [Description], [SefTitle], [ThumbnailPath], [IsActive], [OrderIndex], [DateCreated], [DateLastModified]) VALUES (15, N'sample', N'sample', N'sample', N'', 0, 1, CAST(N'2016-06-11T22:05:10.903' AS DateTime), CAST(N'2016-06-11T22:05:10.903' AS DateTime))
SET IDENTITY_INSERT [dbo].[Tags] OFF
INSERT [dbo].[UrlRedirectRules] ([MediaDetailID], [VirtualPathToRedirect], [RedirectToUrl], [Is301Redirect]) VALUES (10, N'/sample/', N'/', 0)
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [ProfilePhoto], [UserName], [Password], [FirstName], [LastName], [EmailAddress], [AfterLoginStartPage], [AuthenticationType], [LastLoggedInAt], [IsActive], [DateCreated], [DateLastModified]) VALUES (25, N'/media/uploads/images/profile-photos/batman funny face comics picture.jpg', N'admin', N'[?a??????%l?3~???', N'', N'admin', N'info@website.com', N'', N'Forms', CAST(N'2015-12-08T14:18:13.027' AS DateTime), 1, CAST(N'2011-05-31T15:14:59.767' AS DateTime), CAST(N'2016-09-20T15:01:51.260' AS DateTime))
INSERT [dbo].[Users] ([ID], [ProfilePhoto], [UserName], [Password], [FirstName], [LastName], [EmailAddress], [AfterLoginStartPage], [AuthenticationType], [LastLoggedInAt], [IsActive], [DateCreated], [DateLastModified]) VALUES (29, N'', N'client', N'??Mq0?!}?????l?X', N'client', N'', N'info@website.com', N'', N'Forms', NULL, 1, CAST(N'2016-02-01T10:03:40.057' AS DateTime), CAST(N'2016-04-13T08:34:12.183' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
INSERT [dbo].[UsersRoles] ([UserID], [RoleID]) VALUES (25, 20)
INSERT [dbo].[UsersRoles] ([UserID], [RoleID]) VALUES (29, 3)
INSERT [dbo].[UsersRoles] ([UserID], [RoleID]) VALUES (29, 4)
INSERT [dbo].[UsersRoles] ([UserID], [RoleID]) VALUES (29, 18)
INSERT [dbo].[Website] ([MediaDetailID], [CodeInHead], [CodeInBody]) VALUES (4, N'', N'')
SET ANSI_PADDING ON

GO
/****** Object:  Index [Languages_uq]    Script Date: 2016-10-12 9:17:50 AM ******/
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [Languages_uq] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Languages_uq2]    Script Date: 2016-10-12 9:17:50 AM ******/
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [Languages_uq2] UNIQUE NONCLUSTERED 
(
	[CultureCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UNIQUE_UriSegment]    Script Date: 2016-10-12 9:17:50 AM ******/
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [UNIQUE_UriSegment] UNIQUE NONCLUSTERED 
(
	[UriSegment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comments] ADD  CONSTRAINT [DF__Comments__DateCr__28B8D3FE]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Comments] ADD  CONSTRAINT [DF__Comments__DateLa__29ACF837]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[EmailLog] ADD  CONSTRAINT [DF__EmailLog__DateCr__202E7AF5]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[EmailLog] ADD  CONSTRAINT [DF__EmailLog__DateLa__21229F2E]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[Fields] ADD  CONSTRAINT [DF_Fields_FieldCode]  DEFAULT ('') FOR [FieldCode]
GO
ALTER TABLE [dbo].[Fields] ADD  CONSTRAINT [DF_Fields_RenderLabelAfterControl]  DEFAULT ((0)) FOR [RenderLabelAfterControl]
GO
ALTER TABLE [dbo].[Fields] ADD  CONSTRAINT [DF_Fields_FieldValue]  DEFAULT ('') FOR [FieldValue]
GO
ALTER TABLE [dbo].[Fields] ADD  CONSTRAINT [DF_Fields_ReturnValuePropertyName]  DEFAULT ('') FOR [GetAdminControlValue]
GO
ALTER TABLE [dbo].[Fields] ADD  CONSTRAINT [DF_Fields_SetValueCode]  DEFAULT ('') FOR [SetAdminControlValue]
GO
ALTER TABLE [dbo].[Fields] ADD  CONSTRAINT [DF_Fields_GroupName]  DEFAULT ('Main') FOR [GroupName]
GO
ALTER TABLE [dbo].[Fields] ADD  CONSTRAINT [DF_Fields_FrontEndLayout]  DEFAULT ('') FOR [FrontEndLayout]
GO
ALTER TABLE [dbo].[Fields] ADD  CONSTRAINT [DF_Fields_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Fields] ADD  CONSTRAINT [DF_Fields_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[FieldsAssociations] ADD  CONSTRAINT [DF_FieldsAssociations_OrderIndex]  DEFAULT ((0)) FOR [OrderIndex]
GO
ALTER TABLE [dbo].[GlossaryTerms] ADD  CONSTRAINT [DF_GlossaryTerms_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[GlossaryTerms] ADD  CONSTRAINT [DF_GlossaryTerms_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[IPLocationTracker] ADD  CONSTRAINT [DF_IPLocationTracker_IPAddress]  DEFAULT ('') FOR [IPAddress]
GO
ALTER TABLE [dbo].[IPLocationTracker] ADD  CONSTRAINT [DF_IPLocationTracker_Location]  DEFAULT ('') FOR [Location]
GO
ALTER TABLE [dbo].[IPLocationTracker] ADD  CONSTRAINT [DF_IPLocationTracker_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[IPLocationTracker] ADD  CONSTRAINT [DF_IPLocationTracker_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [DF_Languages_DisplayName]  DEFAULT ('') FOR [DisplayName]
GO
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [DF__Languages__IsAct__0F8D3381]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [DF_Languages_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [DF_Languages_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[MasterPages] ADD  CONSTRAINT [DF_MasterPages_IsMobileTemplate]  DEFAULT ('') FOR [MobileTemplate]
GO
ALTER TABLE [dbo].[MasterPages] ADD  CONSTRAINT [DF_MasterPages_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[MasterPages] ADD  CONSTRAINT [DF_MasterPages_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[Media] ADD  DEFAULT ((0)) FOR [OrderIndex]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_UseDirectLink]  DEFAULT ((0)) FOR [UseDirectLink]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_OpenInNewWindow]  DEFAULT ((0)) FOR [OpenInNewWindow]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_DirectLinkVirtualPath]  DEFAULT ('') FOR [DirectLink]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_MetaKeywords]  DEFAULT ('') FOR [MetaKeywords]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_MetaDescription]  DEFAULT ('') FOR [MetaDescription]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_Media_NumberOfViews]  DEFAULT ((0)) FOR [NumberOfViews]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_Media_NumberOfStars]  DEFAULT ((0)) FOR [NumberOfStars]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_Media_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_EnableCommenting]  DEFAULT ((0)) FOR [AllowCommenting]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF__MediaDeta__ShowI__398D8EEE]  DEFAULT ((1)) FOR [ShowInMenu]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_ShowInSearchResults_1]  DEFAULT ((1)) FOR [ShowInSearchResults]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_AllowCaching]  DEFAULT ((1)) FOR [EnableCaching]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_HistoryVersionNumber]  DEFAULT ((0)) FOR [HistoryVersionNumber]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_IsDraft]  DEFAULT ((0)) FOR [IsDraft]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_CanAddToShoppingCart]  DEFAULT ((0)) FOR [CanAddToCart]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_Price]  DEFAULT ((0.0)) FOR [Price]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_InStock]  DEFAULT ((0)) FOR [QuantityInStock]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_RecurringTimePeriod]  DEFAULT ('') FOR [RecurringTimePeriod]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_RedirectToFirstChild_1]  DEFAULT ((0)) FOR [RedirectToFirstChild]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF__MediaDeta__DateC__3A81B327]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF__MediaDeta__DateL__3B75D760]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_RenderInFooter1]  DEFAULT ((0)) FOR [RenderInFooter]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_ForceSSL]  DEFAULT ((0)) FOR [ForceSSL]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_IsProtected]  DEFAULT ((0)) FOR [IsProtected]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_CssClasses]  DEFAULT ('') FOR [CssClasses]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_CustomCode]  DEFAULT ('') FOR [MainLayout]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_SummaryLayout]  DEFAULT ('') FOR [SummaryLayout]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_SummaryLayout1]  DEFAULT ('') FOR [FeaturedLayout]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_UseMediaTypeLayouts]  DEFAULT ((0)) FOR [UseMediaTypeLayouts]
GO
ALTER TABLE [dbo].[MediaDetails] ADD  CONSTRAINT [DF_MediaDetails_PathToFile]  DEFAULT ('') FOR [PathToFile]
GO
ALTER TABLE [dbo].[MediaDetailsFields] ADD  CONSTRAINT [DF_MediaDetailsFields_UseMediaTypeFieldFrontEndLayout]  DEFAULT ((0)) FOR [UseMediaTypeFieldFrontEndLayout]
GO
ALTER TABLE [dbo].[MediaTags] ADD  CONSTRAINT [DF_MediaTags_OrderIndex]  DEFAULT ((0)) FOR [OrderIndex]
GO
ALTER TABLE [dbo].[MediaTypeRolesPermissions] ADD  CONSTRAINT [DF_MediaTypeRolesPermissions_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[MediaTypeRolesPermissions] ADD  CONSTRAINT [DF_MediaTypeRolesPermissions_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[MediaTypes] ADD  CONSTRAINT [DF_MediaTypes_ShowInSearchResults_1]  DEFAULT ((1)) FOR [ShowInSearchResults]
GO
ALTER TABLE [dbo].[MediaTypes] ADD  CONSTRAINT [DF_MediaTypes_ShowInSiteTree]  DEFAULT ((1)) FOR [ShowInSiteTree]
GO
ALTER TABLE [dbo].[MediaTypes] ADD  CONSTRAINT [DF_MediaTypes_EnableCaching]  DEFAULT ((1)) FOR [EnableCaching]
GO
ALTER TABLE [dbo].[MediaTypes] ADD  CONSTRAINT [DF_MediaTypes_CustomCode]  DEFAULT ('') FOR [MainLayout]
GO
ALTER TABLE [dbo].[MediaTypes] ADD  CONSTRAINT [DF_MediaTypes_SummaryLayout]  DEFAULT ('') FOR [SummaryLayout]
GO
ALTER TABLE [dbo].[MediaTypes] ADD  CONSTRAINT [DF_MediaTypes_SummaryLayout1]  DEFAULT ('') FOR [FeaturedLayout]
GO
ALTER TABLE [dbo].[MediaTypes] ADD  CONSTRAINT [DF_MediaTypes_UseMediaTypeLayouts]  DEFAULT ((0)) FOR [UseMediaTypeLayouts]
GO
ALTER TABLE [dbo].[MediaTypesRoles] ADD  CONSTRAINT [DF_MediaTypesRoles_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[MediaTypesRoles] ADD  CONSTRAINT [DF_MediaTypesRoles_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_Permissions_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_Permissions_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF__Roles__DateCreat__078C1F06]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF__Roles__DateLastM__0880433F]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[RolesMediaDetails] ADD  CONSTRAINT [DF_RolesMediaDetails_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[RolesMediaDetails] ADD  CONSTRAINT [DF_RolesMediaDetails_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_GlobalCodeInHead]  DEFAULT ('') FOR [GlobalCodeInHead]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_GlobalCodeInBody]  DEFAULT ('') FOR [GlobalCodeInBody]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_ShoppingCartTax]  DEFAULT ((0.00)) FOR [ShoppingCartTax]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_MaxRequestLength]  DEFAULT ((100000)) FOR [MaxRequestLength]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_MaxUploadFileSizePerFile]  DEFAULT ((20000)) FOR [MaxUploadFileSizePerFile]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_SiteOnlineAtDateTime]  DEFAULT (getdate()) FOR [SiteOnlineAtDateTime]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_SiteOfflineUrl]  DEFAULT ('') FOR [SiteOfflineUrl]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_PageNotFoundUrl]  DEFAULT ('') FOR [PageNotFoundUrl]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_EnableGlossaryTerms]  DEFAULT ((0)) FOR [EnableGlossaryTerms]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_OutputCacheDurationInSeconds]  DEFAULT ((60)) FOR [OutputCacheDurationInSeconds]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Features_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Features_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[Tags] ADD  CONSTRAINT [DF_Tags_ThumbnailPath]  DEFAULT ('') FOR [ThumbnailPath]
GO
ALTER TABLE [dbo].[Tags] ADD  CONSTRAINT [DF_Tags_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Tags] ADD  CONSTRAINT [DF_Topics_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Tags] ADD  CONSTRAINT [DF_Topics_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[UrlRedirectRules] ADD  CONSTRAINT [DF_UrlRedirectRules_Is301Redirect]  DEFAULT ((0)) FOR [Is301Redirect]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_FirstName]  DEFAULT ('') FOR [FirstName]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_LastName]  DEFAULT ('') FOR [LastName]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_AfterLoginStartPage]  DEFAULT ('') FOR [AfterLoginStartPage]
GO
ALTER TABLE [dbo].[UsersMediaDetails] ADD  CONSTRAINT [DF_UsersMediaDetails_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[UsersMediaDetails] ADD  CONSTRAINT [DF_UsersMediaDetails_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[WebserviceRequests] ADD  CONSTRAINT [DF_WebserviceRequests_UrlReferrer]  DEFAULT ('') FOR [UrlReferrer]
GO
ALTER TABLE [dbo].[WebserviceRequests] ADD  CONSTRAINT [DF_WebserviceRequests_Response]  DEFAULT ('') FOR [Response]
GO
ALTER TABLE [dbo].[WebserviceRequests] ADD  CONSTRAINT [DF_WebserviceRequests_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[WebserviceRequests] ADD  CONSTRAINT [DF_WebserviceRequests_DateLastModified]  DEFAULT (getdate()) FOR [DateLastModified]
GO
ALTER TABLE [dbo].[Website] ADD  CONSTRAINT [DF_Website_CodeInHead]  DEFAULT ('') FOR [CodeInHead]
GO
ALTER TABLE [dbo].[Website] ADD  CONSTRAINT [DF_Website_GoogleAnalyticsTrackingKey]  DEFAULT ('') FOR [CodeInBody]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Media] FOREIGN KEY([MediaID])
REFERENCES [dbo].[Media] ([ID])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Media]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_ReplyToCommentID] FOREIGN KEY([ReplyToCommentID])
REFERENCES [dbo].[Comments] ([ID])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_ReplyToCommentID]
GO
ALTER TABLE [dbo].[FieldsAssociations]  WITH CHECK ADD  CONSTRAINT [FK_FieldAssociations_MediaDetails] FOREIGN KEY([AssociatedMediaDetailID])
REFERENCES [dbo].[MediaDetails] ([ID])
GO
ALTER TABLE [dbo].[FieldsAssociations] CHECK CONSTRAINT [FK_FieldAssociations_MediaDetails]
GO
ALTER TABLE [dbo].[FieldsAssociations]  WITH CHECK ADD  CONSTRAINT [FK_FieldsAssociations_MediaDetailsFields] FOREIGN KEY([MediaDetailFieldID])
REFERENCES [dbo].[MediaDetailsFields] ([FieldID])
GO
ALTER TABLE [dbo].[FieldsAssociations] CHECK CONSTRAINT [FK_FieldsAssociations_MediaDetailsFields]
GO
ALTER TABLE [dbo].[Media]  WITH NOCHECK ADD  CONSTRAINT [FK_Media_ParentMediaID] FOREIGN KEY([ParentMediaID])
REFERENCES [dbo].[Media] ([ID])
GO
ALTER TABLE [dbo].[Media] CHECK CONSTRAINT [FK_Media_ParentMediaID]
GO
ALTER TABLE [dbo].[MediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_MediaDetails_CreatedByUserID] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[MediaDetails] CHECK CONSTRAINT [FK_MediaDetails_CreatedByUserID]
GO
ALTER TABLE [dbo].[MediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_MediaDetails_HistoryForMediaDetailID] FOREIGN KEY([HistoryForMediaDetailID])
REFERENCES [dbo].[MediaDetails] ([ID])
GO
ALTER TABLE [dbo].[MediaDetails] CHECK CONSTRAINT [FK_MediaDetails_HistoryForMediaDetailID]
GO
ALTER TABLE [dbo].[MediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_MediaDetails_LanguageID_Languages_ID] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Languages] ([ID])
GO
ALTER TABLE [dbo].[MediaDetails] CHECK CONSTRAINT [FK_MediaDetails_LanguageID_Languages_ID]
GO
ALTER TABLE [dbo].[MediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_MediaDetails_LastUpdatedByUserID] FOREIGN KEY([LastUpdatedByUserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[MediaDetails] CHECK CONSTRAINT [FK_MediaDetails_LastUpdatedByUserID]
GO
ALTER TABLE [dbo].[MediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_MediaDetails_MasterPage] FOREIGN KEY([MasterPageID])
REFERENCES [dbo].[MasterPages] ([ID])
GO
ALTER TABLE [dbo].[MediaDetails] CHECK CONSTRAINT [FK_MediaDetails_MasterPage]
GO
ALTER TABLE [dbo].[MediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_MediaDetails_MediaID_Media_ID] FOREIGN KEY([MediaID])
REFERENCES [dbo].[Media] ([ID])
GO
ALTER TABLE [dbo].[MediaDetails] CHECK CONSTRAINT [FK_MediaDetails_MediaID_Media_ID]
GO
ALTER TABLE [dbo].[MediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_MediaDetails_MediaTypeID_MediaType_ID] FOREIGN KEY([MediaTypeID])
REFERENCES [dbo].[MediaTypes] ([ID])
GO
ALTER TABLE [dbo].[MediaDetails] CHECK CONSTRAINT [FK_MediaDetails_MediaTypeID_MediaType_ID]
GO
ALTER TABLE [dbo].[MediaDetailsFields]  WITH CHECK ADD  CONSTRAINT [FK_MediaDetailFields_MediaDetails] FOREIGN KEY([MediaDetailID])
REFERENCES [dbo].[MediaDetails] ([ID])
GO
ALTER TABLE [dbo].[MediaDetailsFields] CHECK CONSTRAINT [FK_MediaDetailFields_MediaDetails]
GO
ALTER TABLE [dbo].[MediaDetailsFields]  WITH CHECK ADD  CONSTRAINT [FK_MediaDetailsFields_Fields] FOREIGN KEY([FieldID])
REFERENCES [dbo].[Fields] ([ID])
GO
ALTER TABLE [dbo].[MediaDetailsFields] CHECK CONSTRAINT [FK_MediaDetailsFields_Fields]
GO
ALTER TABLE [dbo].[MediaDetailsFields]  WITH CHECK ADD  CONSTRAINT [FK_MediaDetailsFields_MediaTypesFields] FOREIGN KEY([MediaTypeFieldID])
REFERENCES [dbo].[MediaTypesFields] ([FieldID])
GO
ALTER TABLE [dbo].[MediaDetailsFields] CHECK CONSTRAINT [FK_MediaDetailsFields_MediaTypesFields]
GO
ALTER TABLE [dbo].[MediaTags]  WITH CHECK ADD  CONSTRAINT [FK_MediaTags_MediaID_Media_ID] FOREIGN KEY([MediaID])
REFERENCES [dbo].[Media] ([ID])
GO
ALTER TABLE [dbo].[MediaTags] CHECK CONSTRAINT [FK_MediaTags_MediaID_Media_ID]
GO
ALTER TABLE [dbo].[MediaTags]  WITH CHECK ADD  CONSTRAINT [FK_MediaTags_TagID_Tags_ID] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tags] ([ID])
GO
ALTER TABLE [dbo].[MediaTags] CHECK CONSTRAINT [FK_MediaTags_TagID_Tags_ID]
GO
ALTER TABLE [dbo].[MediaTypeRolesPermissions]  WITH CHECK ADD  CONSTRAINT [FK_MediaTypeRolesPermissions_MediaTypeRolesPermissionID] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([ID])
GO
ALTER TABLE [dbo].[MediaTypeRolesPermissions] CHECK CONSTRAINT [FK_MediaTypeRolesPermissions_MediaTypeRolesPermissionID]
GO
ALTER TABLE [dbo].[MediaTypeRolesPermissions]  WITH CHECK ADD  CONSTRAINT [FK_MediaTypeRolesPermissions_MediaTypesRoleID] FOREIGN KEY([MediaTypeRoleID])
REFERENCES [dbo].[MediaTypesRoles] ([ID])
GO
ALTER TABLE [dbo].[MediaTypeRolesPermissions] CHECK CONSTRAINT [FK_MediaTypeRolesPermissions_MediaTypesRoleID]
GO
ALTER TABLE [dbo].[MediaTypes]  WITH CHECK ADD  CONSTRAINT [FK_MediaTypes_MasterPages] FOREIGN KEY([MasterPageID])
REFERENCES [dbo].[MasterPages] ([ID])
GO
ALTER TABLE [dbo].[MediaTypes] CHECK CONSTRAINT [FK_MediaTypes_MasterPages]
GO
ALTER TABLE [dbo].[MediaTypesChildren]  WITH CHECK ADD  CONSTRAINT [FK_MediaTypesChildren_AllowedChildMediaTypeID] FOREIGN KEY([AllowedChildMediaTypeID])
REFERENCES [dbo].[MediaTypes] ([ID])
GO
ALTER TABLE [dbo].[MediaTypesChildren] CHECK CONSTRAINT [FK_MediaTypesChildren_AllowedChildMediaTypeID]
GO
ALTER TABLE [dbo].[MediaTypesChildren]  WITH CHECK ADD  CONSTRAINT [FK_MediaTypesChildren_MediaTypeID] FOREIGN KEY([MediaTypeID])
REFERENCES [dbo].[MediaTypes] ([ID])
GO
ALTER TABLE [dbo].[MediaTypesChildren] CHECK CONSTRAINT [FK_MediaTypesChildren_MediaTypeID]
GO
ALTER TABLE [dbo].[MediaTypesFields]  WITH CHECK ADD  CONSTRAINT [FK_MediaTypesFields_Fields] FOREIGN KEY([FieldID])
REFERENCES [dbo].[Fields] ([ID])
GO
ALTER TABLE [dbo].[MediaTypesFields] CHECK CONSTRAINT [FK_MediaTypesFields_Fields]
GO
ALTER TABLE [dbo].[MediaTypesFields]  WITH CHECK ADD  CONSTRAINT [FK_MediaTypesFields_MediaTypes] FOREIGN KEY([MediaTypeID])
REFERENCES [dbo].[MediaTypes] ([ID])
GO
ALTER TABLE [dbo].[MediaTypesFields] CHECK CONSTRAINT [FK_MediaTypesFields_MediaTypes]
GO
ALTER TABLE [dbo].[MediaTypesRoles]  WITH CHECK ADD  CONSTRAINT [FK_MediaTypesRoles_MediaTypeID] FOREIGN KEY([MediaTypeID])
REFERENCES [dbo].[MediaTypes] ([ID])
GO
ALTER TABLE [dbo].[MediaTypesRoles] CHECK CONSTRAINT [FK_MediaTypesRoles_MediaTypeID]
GO
ALTER TABLE [dbo].[MediaTypesRoles]  WITH CHECK ADD  CONSTRAINT [FK_MediaTypesRoles_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([ID])
GO
ALTER TABLE [dbo].[MediaTypesRoles] CHECK CONSTRAINT [FK_MediaTypesRoles_Roles]
GO
ALTER TABLE [dbo].[Pages]  WITH CHECK ADD  CONSTRAINT [FK_Pages_MediaDetailID_MediaDetails_ID] FOREIGN KEY([MediaDetailID])
REFERENCES [dbo].[MediaDetails] ([ID])
GO
ALTER TABLE [dbo].[Pages] CHECK CONSTRAINT [FK_Pages_MediaDetailID_MediaDetails_ID]
GO
ALTER TABLE [dbo].[RolesMediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_RolesMediaDetails_MediaDetailID] FOREIGN KEY([MediaDetailID])
REFERENCES [dbo].[MediaDetails] ([ID])
GO
ALTER TABLE [dbo].[RolesMediaDetails] CHECK CONSTRAINT [FK_RolesMediaDetails_MediaDetailID]
GO
ALTER TABLE [dbo].[RolesMediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_RolesMediaDetails_RoleID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([ID])
GO
ALTER TABLE [dbo].[RolesMediaDetails] CHECK CONSTRAINT [FK_RolesMediaDetails_RoleID]
GO
ALTER TABLE [dbo].[RolesPermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolesPermissions_PermissionID] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([ID])
GO
ALTER TABLE [dbo].[RolesPermissions] CHECK CONSTRAINT [FK_RolesPermissions_PermissionID]
GO
ALTER TABLE [dbo].[RolesPermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolesPermissions_RolesID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([ID])
GO
ALTER TABLE [dbo].[RolesPermissions] CHECK CONSTRAINT [FK_RolesPermissions_RolesID]
GO
ALTER TABLE [dbo].[RootPages]  WITH CHECK ADD  CONSTRAINT [FK_RootPages_MediaDetails] FOREIGN KEY([MediaDetailID])
REFERENCES [dbo].[MediaDetails] ([ID])
GO
ALTER TABLE [dbo].[RootPages] CHECK CONSTRAINT [FK_RootPages_MediaDetails]
GO
ALTER TABLE [dbo].[Settings]  WITH CHECK ADD  CONSTRAINT [FK_Settings_Languages] FOREIGN KEY([DefaultLanguageID])
REFERENCES [dbo].[Languages] ([ID])
GO
ALTER TABLE [dbo].[Settings] CHECK CONSTRAINT [FK_Settings_Languages]
GO
ALTER TABLE [dbo].[Settings]  WITH CHECK ADD  CONSTRAINT [FK_Settings_MasterPages] FOREIGN KEY([DefaultMasterPageID])
REFERENCES [dbo].[MasterPages] ([ID])
GO
ALTER TABLE [dbo].[Settings] CHECK CONSTRAINT [FK_Settings_MasterPages]
GO
ALTER TABLE [dbo].[UrlRedirectRules]  WITH CHECK ADD  CONSTRAINT [FK_UrlRedirectRules_MediaDetails] FOREIGN KEY([MediaDetailID])
REFERENCES [dbo].[MediaDetails] ([ID])
GO
ALTER TABLE [dbo].[UrlRedirectRules] CHECK CONSTRAINT [FK_UrlRedirectRules_MediaDetails]
GO
ALTER TABLE [dbo].[UsersMediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_UsersMediaDetails_MediaDetailID] FOREIGN KEY([MediaDetailID])
REFERENCES [dbo].[MediaDetails] ([ID])
GO
ALTER TABLE [dbo].[UsersMediaDetails] CHECK CONSTRAINT [FK_UsersMediaDetails_MediaDetailID]
GO
ALTER TABLE [dbo].[UsersMediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_UsersMediaDetails_PermissionID] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([ID])
GO
ALTER TABLE [dbo].[UsersMediaDetails] CHECK CONSTRAINT [FK_UsersMediaDetails_PermissionID]
GO
ALTER TABLE [dbo].[UsersMediaDetails]  WITH CHECK ADD  CONSTRAINT [FK_UsersMediaDetails_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UsersMediaDetails] CHECK CONSTRAINT [FK_UsersMediaDetails_UserID]
GO
ALTER TABLE [dbo].[UsersRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersRoles_RoleID_Roles_ID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([ID])
GO
ALTER TABLE [dbo].[UsersRoles] CHECK CONSTRAINT [FK_UsersRoles_RoleID_Roles_ID]
GO
ALTER TABLE [dbo].[UsersRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersRoles_UserID_Users_ID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UsersRoles] CHECK CONSTRAINT [FK_UsersRoles_UserID_Users_ID]
GO
ALTER TABLE [dbo].[Website]  WITH CHECK ADD  CONSTRAINT [FK_Website_MediaDetails] FOREIGN KEY([MediaDetailID])
REFERENCES [dbo].[MediaDetails] ([ID])
GO
ALTER TABLE [dbo].[Website] CHECK CONSTRAINT [FK_Website_MediaDetails]
GO
