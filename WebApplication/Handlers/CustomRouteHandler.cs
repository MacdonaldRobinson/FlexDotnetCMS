using FrameworkLibrary;
using System;
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

            if (FrameworkSettings.CurrentUserInSession == null && Request.HttpMethod == "GET" && (!(bool)BaseMapper.CanConnectToDB || AppSettings.EnableOutputCaching))
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
                    var cacheData = FileCacheHelper.GetFromCache(cacheKey);

                    if (!string.IsNullOrEmpty(cacheData))
                        BaseService.WriteHtml(cacheData + "<!-- Loaded from level 2 - File Cache -->");
                }

                if (AppSettings.EnableLevel3RedisCaching)
                {
                    var cacheData = RedisCacheHelper.GetFromCache(cacheKey);

                    if (!string.IsNullOrEmpty(cacheData))
                        BaseService.WriteHtml(cacheData + "<!-- Loaded from level 3 - Redis Cache -->");
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

            if ((virtualPath != "~/") && (!virtualPath.EndsWith("/")))
            {
                var queryString = HttpContext.Current.Request.QueryString.ToString();

                var path = virtualPath + "/";

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

                FrameworkSettings.SetCurrentLanguage(language);
            }

            if (!isAttemptingAdminLogin && AppSettings.EnableUrlRedirectRules)
            {
                var redirectRule = UrlRedirectRulesMapper.GetRuleForUrl(virtualPath);

                if (redirectRule != null)
                {
                    var newUrl = URIHelper.ConvertToAbsUrl(redirectRule.RedirectToUrl);

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

                if (detail != null && !detail.CanUserAccessSection(FrameworkSettings.CurrentUser))
                {
                    FormsAuthentication.RedirectToLoginPage();
                }

                if (detail != null)
                {
                    if (URIHelper.ConvertAbsUrlToTilda(detail.AbsoluteUrl).Replace("~", "") != Request.Url.AbsolutePath)
                    {
                        Response.Redirect(detail.AbsoluteUrl + Request.Url.Query);
                    }
                }

                if (detail != null)
                {
                    if (detail.ForceSSL || AppSettings.ForceSSL)
                        URIHelper.ForceSSL();
                    else
                        URIHelper.ForceNonSSL();
                }

                if (cmsSettings != null)
                {
                    if ((detail == null) || (!IsValidRequest(detail)))
                    {
                        if (!string.IsNullOrEmpty(cmsSettings.PageNotFoundUrl))
                        {
                            FrameworkSettings.CurrentFrameworkBaseMedia = null;

                            FrameworkSettings.CurrentFrameworkBaseMedia = FrameworkBaseMedia.GetInstanceByVirtualPath(cmsSettings.PageNotFoundUrl, true);
                            detail = (MediaDetail)FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail;

                            ErrorHelper.LogException(new Exception($"Page Not Found: {virtualPath}"));

                            Response.StatusCode = 404;
                        }
                    }
                }

                if (detail != null)
                {
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