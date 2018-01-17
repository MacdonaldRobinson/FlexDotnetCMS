using FrameworkLibrary;
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

        private void AttemptToLoadFromCache()
        {
            if (AppSettings.ForceSSL && !URIHelper.IsSSL())
            {
                URIHelper.ForceSSL();
            }

            if (FrameworkSettings.CurrentUser == null)
            {
                if (Request.HttpMethod == "GET" && (!(bool)BaseMapper.CanConnectToDB || AppSettings.EnableOutputCaching))
                {
                    var userSelectedVersion = RenderVersion.HTML;

                    if (BasePage.IsMobile)
                        userSelectedVersion = RenderVersion.Mobile;

                    var cacheKey = (userSelectedVersion.ToString() + "_" + Request.Url.PathAndQuery).ToLower();

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

                if(!(bool)BaseMapper.CanConnectToDB)
                    BaseService.WriteHtml("Cannot connect to the database");

            }
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {            
            RedisCacheHelper.SetRedisCacheConnectionString(AppSettings.RedisCacheConnectionString);
            FileCacheHelper.SetFileSystemCacheDirPath(AppSettings.FileSystemCacheDirPath);

            virtualPath = URIHelper.GetCurrentVirtualPath().ToLower();

            var queryString = HttpContext.Current.Request.QueryString.ToString();
            queryString = System.Web.HttpUtility.UrlDecode(queryString);


            if (!Request.Path.EndsWith("/") || ((virtualPath != "~/") && (!virtualPath.EndsWith("/"))))
            {
                var path = Request.Path + "/";

                if (!string.IsNullOrEmpty(queryString))
                    path = path + "?" + queryString;

                HttpContext.Current.Response.RedirectPermanent(path);
            }

            Settings cmsSettings = null;
            bool isAttemptingAdminLogin = false;

            if ((virtualPath != "~/login/") && (virtualPath != "~/admin/") && string.IsNullOrEmpty(Request.QueryString["format"]))
            {
                cmsSettings = SettingsMapper.GetSettings();

                if (cmsSettings != null)
                {
                    var isSiteOnline = cmsSettings.IsSiteOnline();

                    if (isSiteOnline)
                    {
                        if (virtualPath.Contains(cmsSettings.SiteOfflineUrl))
                            Response.Redirect("~/");

                        AttemptToLoadFromCache();
                    }
                    else
                    {
                        if (!virtualPath.Contains(cmsSettings.SiteOfflineUrl))
                            Response.Redirect(cmsSettings.SiteOfflineUrl);
                    }
                }
                else
                {
                    AttemptToLoadFromCache();
                }
            }
            else
            {
                isAttemptingAdminLogin = true;
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
                        Response.RedirectPermanent(newUrl);
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
                    FrameworkSettings.CurrentFrameworkBaseMedia = FrameworkBaseMedia.GetInstanceByVirtualPath(virtualPath, true);
                    detail = (MediaDetail)FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail;
                }
                else if (mediaDetailId != 0)
                {
                    var mediaDetail = MediaDetailsMapper.GetByID(mediaDetailId);

                    FrameworkSettings.CurrentFrameworkBaseMedia = FrameworkBaseMedia.GetInstanceByMediaDetail(mediaDetail);
                    detail = (MediaDetail)FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail;
                }
                else if (mediaId != 0)
                {
                    var media = MediasMapper.GetByID(mediaId);

                    FrameworkSettings.CurrentFrameworkBaseMedia = FrameworkBaseMedia.GetInstanceByMedia(media);
                    detail = (MediaDetail)FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail;
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
                    if (detail.ForceSSL || AppSettings.ForceSSL)
                        URIHelper.ForceSSL();
                    else
                        URIHelper.ForceNonSSL();
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
                                HttpContext.Current.Response.RedirectPermanent(historyVersion.HistoryForMediaDetail.CachedVirtualPath);
                            }
                            else
                            {
                                HttpContext.Current.Response.RedirectPermanent("/");
                            }
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.RedirectPermanent("/");
                    }
                }

                if (cmsSettings != null)
                {
                    if ((detail == null) || (!IsValidRequest(detail)))
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

                            FrameworkSettings.CurrentFrameworkBaseMedia = FrameworkBaseMedia.GetInstanceByMediaDetail(draft);
                            detail = (MediaDetail)FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail;
                        }
                    }

                    if (detail.RedirectToFirstChild)
                    {
                        var items = MediaDetailsMapper.GetAllChildMediaDetails(detail.MediaID, detail.LanguageID).ToList();

                        if (items.Count > 0)
                            HttpContext.Current.Response.Redirect(items[0].AutoCalculatedVirtualPath);
                    }

                    viewPath = FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.Handler;

                    if ((viewPath == null) || (viewPath.Trim() == ""))
                        viewPath = MediaTypesMapper.GetByID(FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.MediaTypeID).MediaTypeHandler;

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

        private bool IsValidRequest(IMediaDetail detail)
        {
            if (detail == null)
                return false;

            if ((!detail.CanRender) && (Request["version"] == null))
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