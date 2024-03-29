﻿using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using WebApplication.Admin;
using WebApplication.Services;

namespace WebApplication.Handlers
{
    public class CustomRouteHandler : IRouteHandler, IRequiresSessionState
    {
        private string virtualPath = "";

        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

		private void AttemptDBConnection()
		{
			if (BaseMapper.CanConnectToDB != null && !(bool)BaseMapper.CanConnectToDB)
				BaseService.WriteHtml("Cannot connect to the database");
		}

        private void AttemptToLoadFromCache()
        {
            if (!IsRunningOnLocalHost && AppSettings.ForceSSL && !URIHelper.IsSSL())
            {
                URIHelper.ForceSSL();
            }

			//if (Request.Url.AbsolutePath != "/" && !BasePage.IsAjaxRequest)
			//{
			//	Response.Redirect("/#" + Request.Url.PathAndQuery, true);
			//}

			if (Request.QueryString.Count == 0 || (Request.QueryString.Count == 1 && Request.QueryString[BasePage.HomePagePath] != null))
            {
				if (Request.HttpMethod == "GET" && ((BaseMapper.CanConnectToDB != null && !(bool)BaseMapper.CanConnectToDB) || AppSettings.EnableOutputCaching))
				{
                    var userSelectedVersion = RenderVersion.HTML;

                    if (BasePage.IsMobile)
                        userSelectedVersion = RenderVersion.Mobile;

					var cacheKey = (userSelectedVersion.ToString() + Request.Url.LocalPath).ToLower();

                    if (AppSettings.EnableLevel1MemoryCaching)
                    {
                        var cacheData = (string)ContextHelper.GetFromCache(cacheKey);

                        if (!string.IsNullOrEmpty(cacheData))
                            BaseService.WriteHtml(cacheData + "<!-- Loaded from level 1 - Memory Cache -->");
                    }

                    if (AppSettings.EnableLevel2FileCaching)
                    {
                        var cache = FileCacheHelper.GetFromCache(cacheKey);

                        if (!cache.IsError)
                        {
                            var cacheData = cache.GetRawData<string>();

                            if (!string.IsNullOrEmpty(cacheData))
                                BaseService.WriteHtml($"{cacheData} <!-- Loaded from level 2 - File Cache -->");
                        }
                    }

                    if (AppSettings.EnableLevel3RedisCaching)
                    {
                        var cacheData = RedisCacheHelper.GetFromCache(cacheKey);

                        if (!string.IsNullOrEmpty(cacheData))
                            BaseService.WriteHtml(cacheData + "<!-- Loaded from level 3 - Redis Cache -->");
                    }
                }

                AttemptDBConnection();

            }
        }

		public bool IsRunningOnLocalHost
		{
			get
			{
				return Request.Url.Host.StartsWith("localhost");
			}
		}

		public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {            
            RedisCacheHelper.SetRedisCacheConnectionString(AppSettings.RedisCacheConnectionString);
            FileCacheHelper.SetFileSystemCacheDirPath(AppSettings.FileSystemCacheDirPath);

            virtualPath = Request.Url.AbsolutePath.ToLower();

			var queryString = HttpContext.Current.Request.QueryString.ToString();
            queryString = System.Web.HttpUtility.UrlDecode(queryString);


            if (!Request.Path.EndsWith("/") || ((virtualPath != "~/") && (!virtualPath.EndsWith("/"))))
            {
                var path = Request.Path + "/";

                if (!string.IsNullOrEmpty(queryString))
                    path = path + "?" + queryString;

                HttpContext.Current.Response.RedirectPermanent(path, true);
            }

            Settings cmsSettings = null;
            bool isAttemptingAdminLogin = false;

            if ((virtualPath != "~/login/") && (virtualPath != "~/admin/") && string.IsNullOrEmpty(Request.QueryString["format"]))
            {
				AttemptToLoadFromCache();

				//cmsSettings = SettingsMapper.GetSettings();

				//if (cmsSettings != null)
				//{
				//    var isSiteOnline = cmsSettings.IsSiteOnline();

				//    if (isSiteOnline)
				//    {
				//        if (virtualPath.Contains(cmsSettings.SiteOfflineUrl))
				//            Response.Redirect("~/");

				//        AttemptToLoadFromCache();
				//    }
				//    else
				//    {
				//        if (!virtualPath.Contains(cmsSettings.SiteOfflineUrl))
				//            Response.Redirect(cmsSettings.SiteOfflineUrl);
				//    }
				//}
				//else
				//{
				//    AttemptToLoadFromCache();
				//}
			}
            else
            {
                isAttemptingAdminLogin = true;
            }


			var languageSegment = FrameworkSettings.GetCurrentLanguage().UriSegment;

			if (LanguagesMapper.GetAllActive().Count() > 1 && !Request.Url.PathAndQuery.Contains($"/{languageSegment}/"))
			{
				var url = URIHelper.ConvertToAbsUrl("/" + languageSegment + Request.Url.PathAndQuery);
				Response.RedirectPermanent(url, true);
			}

			var segments = URIHelper.GetUriSegments(virtualPath).ToList();

            string firstSegment = "";

            if (segments.Count > 0)
            {
                firstSegment = segments[0];

                var language = LanguagesMapper.GetAllActive().SingleOrDefault(i => i.UriSegment == firstSegment);

                if(language != null)
                    FrameworkSettings.SetCurrentLanguage(language);
            }

            if (!isAttemptingAdminLogin && AppSettings.EnableUrlRedirectRules)
            {
                var path = virtualPath;

                if (!string.IsNullOrEmpty(queryString))
                    path = path + "?" + queryString;

                var redirectRule = UrlRedirectRulesMapper.GetRuleForUrl(path);

                if (redirectRule != null)
                {
                    var newUrl = redirectRule.RedirectToUrl;

                    if (newUrl.Contains("{"))
                    {
                        newUrl = MediaDetailsMapper.ParseSpecialTags(redirectRule, newUrl);
                    }

                    newUrl = URIHelper.ConvertToAbsUrl(newUrl);
                    
                    var possibleLoopRules = UrlRedirectRulesMapper.GetRulesFromUrl(URIHelper.ConvertAbsUrlToTilda(newUrl));
                    var foundActiveVirtualPath = MediaDetailsMapper.GetByVirtualPath(path);

                    if (possibleLoopRules.Any())
                    {
                        foreach (var rule in possibleLoopRules)
                        {
                            var returnObj = MediaDetailsMapper.DeletePermanently(rule);
                        }
                    }

                    if(foundActiveVirtualPath != null)
                    {
                        var returnObj = MediaDetailsMapper.DeletePermanently(redirectRule);
                    }

                    if (Request.QueryString.Count > 0)
                        newUrl += "?" + Request.QueryString;

                    if (redirectRule.Is301Redirect)
                        Response.RedirectPermanent(newUrl, true);
                    else
                        Response.Redirect(newUrl);
                }
            }

            if (!File.Exists(HttpContext.Current.Server.MapPath(virtualPath)) && !virtualPath.Contains(ParserHelper.OpenToken) && !virtualPath.Contains(ParserHelper.CloseToken))
            {
                string viewPath = "";

                long mediaDetailId = 0;
                long.TryParse(requestContext.HttpContext.Request["MediaDetailID"], out mediaDetailId);

                long mediaId = 0;
                long.TryParse(requestContext.HttpContext.Request["MediaID"], out mediaId);

                MediaDetail detail = null;

                if (mediaDetailId == 0 && mediaId == 0)
                {
                    FrameworkSettings.Current = FrameworkBaseMedia.GetInstanceByVirtualPath(Request.Url.AbsolutePath.ToLower(), true);
                    detail = (MediaDetail)FrameworkSettings.Current.CurrentMediaDetail;
                }
                else if (mediaDetailId != 0)
                {
                    var mediaDetail = MediaDetailsMapper.GetByID(mediaDetailId);

                    FrameworkSettings.Current = FrameworkBaseMedia.GetInstanceByMediaDetail(mediaDetail);
                    detail = (MediaDetail)FrameworkSettings.Current.CurrentMediaDetail;
                }
                else if (mediaId != 0)
                {
                    var media = MediasMapper.GetByID(mediaId);

                    FrameworkSettings.Current = FrameworkBaseMedia.GetInstanceByMedia(media);
                    detail = (MediaDetail)FrameworkSettings.Current.CurrentMediaDetail;
                }

                /*if (detail != null && !detail.CanUserAccessSection(FrameworkSettings.CurrentUser))
                {
                    FormsAuthentication.RedirectToLoginPage();
                }*/

                /*if (detail != null)
                {
                    var absUrlBase = URIHelper.ConvertAbsUrlToTilda(detail.AbsoluteUrl).Replace("~", "");
                    var absPathBase = URIHelper.ConvertAbsUrlToTilda(Request.Url.AbsolutePath).Replace("~", "");

                    if (absUrlBase != absPathBase)
                    {
                        Response.Redirect(detail.AbsoluteUrl + Request.Url.Query);
                    }
                }*/

                if (detail != null)
                {
                    if ((!IsRunningOnLocalHost) && (detail.ForceSSL || AppSettings.ForceSSL))
                        URIHelper.ForceSSL();
                }
                else
                {
                    var currentLanguageId = FrameworkSettings.GetCurrentLanguage().ID;

                    var historyVersion = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.LanguageID == currentLanguageId && i.CachedVirtualPath == virtualPath && i.MediaType.ShowInSiteTree && i.HistoryVersionNumber != 0 && i.HistoryForMediaDetail != null);

                    if (historyVersion != null && historyVersion.VirtualPath != historyVersion.HistoryForMediaDetail.VirtualPath)
                    {
                        var foundRedirectUrl = UrlRedirectRulesMapper.GetRuleForUrl(virtualPath);

                        if (foundRedirectUrl == null)
                        {
                            var urlRedirectRule = UrlRedirectRulesMapper.CreateUrlRedirect(virtualPath, historyVersion.HistoryForMediaDetail.Media);

                            if (urlRedirectRule != null)
                            {
                                var returnObj = UrlRedirectRulesMapper.Insert(urlRedirectRule);
                                HttpContext.Current.Response.RedirectPermanent(historyVersion.HistoryForMediaDetail.CachedVirtualPath, true);
                            }
                            else
                            {
								RedirectToHomePage();
							}
                        }
                    }
                    else
                    {
						RedirectToHomePage();
					}
                }

				if ((detail == null) || (!IsValidRequest(detail)))
				{
					detail = null;
					if (cmsSettings != null)
					{
                        if (!string.IsNullOrEmpty(cmsSettings.PageNotFoundUrl))
                        {
                            ErrorHelper.LogException(new Exception($"Page Not Found: {virtualPath}"));

                            Response.Redirect(cmsSettings.PageNotFoundUrl);

                            /*FrameworkSettings.CurrentFrameworkBaseMedia = null;

                            FrameworkSettings.CurrentFrameworkBaseMedia = FrameworkBaseMedia.GetInstanceByVirtualPath(cmsSettings.PageNotFoundUrl, true);
                            detail = (MediaDetail)FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail;

                            ErrorHelper.LogException(new Exception($"Page Not Found: {virtualPath}"));*/

                            //Response.StatusCode = 301;
                        }
                    }
                }

                if (detail != null)
                {
                    var draft = detail.GetLatestDraft();

                    if (draft != null && (draft.PublishDate - detail.PublishDate) > TimeSpan.FromSeconds(10) && draft.CanRender)
                    {
                        var returnObj = draft.PublishLive();

                        if(!returnObj.IsError)
                        {
                            detail.RemoveFromCache();
                            draft.RemoveFromCache();

                            FrameworkSettings.Current = FrameworkBaseMedia.GetInstanceByMediaDetail(draft);
                            detail = (MediaDetail)FrameworkSettings.Current.CurrentMediaDetail;
                        }
                    }

                    if (detail.RedirectToFirstChild)
                    {                        
                        var child = detail.ChildMediaDetails.FirstOrDefault();

                        if (child != null)
                        {
                            var redirectPath = child.AutoCalculatedVirtualPath;

                            if(!string.IsNullOrEmpty(queryString))
                            {
                                redirectPath = redirectPath + "?" + queryString;
                            }

                            HttpContext.Current.Response.Redirect(redirectPath);
                        }                            
                    }

                    viewPath = FrameworkSettings.Current.CurrentMediaDetail.Handler;

                    if ((viewPath == null) || (viewPath.Trim() == ""))
                        viewPath = MediaTypesMapper.GetByID(FrameworkSettings.Current.CurrentMediaDetail.MediaTypeID).MediaTypeHandler;

                    viewPath = URIHelper.ConvertAbsUrlToTilda(viewPath);

                    if (!string.IsNullOrEmpty(Request.QueryString["format"]))
                    {
                        FrontEndBasePage.HandleFormatQueryString(detail, Request.QueryString["format"], Request.QueryString["depth"]);
                    }

                    return CreateInstanceFromVirtualPath(viewPath, typeof(BasePage));
                }
            }

            return new DefaultHttpHandler();
        }

		private void RedirectToHomePage()
		{
			var homePagePath = "/";

			if (!string.IsNullOrEmpty(HttpContext.Current.Request[BasePage.HomePagePath]))
			{
				homePagePath = HttpContext.Current.Request[BasePage.HomePagePath];
			}

			HttpContext.Current.Response.RedirectPermanent(homePagePath, true);
		}

		private bool IsValidRequest(IMediaDetail detail)
        {
			if (detail == null)
                return false;

			if (FrameworkSettings.CurrentUser != null && FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.AccessAdvanceOptions))
				return true;

            if (!detail.CanRender || detail.HasADeletedParent())
                return false;

            return true;
        }

        private IHttpHandler CreateInstanceFromVirtualPath(string viewPath, Type type)
        {
            var display = BuildManager.CreateInstanceFromVirtualPath(viewPath, type);
            return (IHttpHandler)display;
        }
    }
}