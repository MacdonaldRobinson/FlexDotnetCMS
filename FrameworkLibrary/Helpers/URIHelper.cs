using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameworkLibrary
{
    public class URIHelper
    {
        public static string GetCurrentVirtualPath(bool asAbsPath = false, bool includeQueryString = false)
        {
            string virtualPath = HttpContext.Current.Request.Path;

            if (virtualPath.StartsWith("~/" + HttpContext.Current.Request.Url.Host))
                virtualPath = virtualPath.Replace("~/" + HttpContext.Current.Request.Url.Host, "~");

            if (HttpContext.Current.Request.ApplicationPath != "/")
            {
                virtualPath = virtualPath.ToLower().Replace(HttpContext.Current.Request.ApplicationPath.ToLower(), "");
            }

            if (virtualPath.ToLower().Contains("~/default.aspx"))
                virtualPath = "~/";

            virtualPath = URIHelper.ConvertAbsUrlToTilda(virtualPath);

            if (asAbsPath)
                virtualPath = ConvertToAbsUrl(virtualPath);

            if (virtualPath == "")
                virtualPath = "~/";

            /*var details = MediaDetailsMapper.GetByVirtualPath(virtualPath, false);

            if (details == null)
            {
                if (!virtualPath.Contains(FrameworkSettings.RootMediaDetail.VirtualPath))
                    virtualPath = virtualPath.Replace("~/", FrameworkSettings.RootMediaDetail.VirtualPath);
            }*/

            return virtualPath;
        }

        public static string ConvertAbsPathToAbsUrl(string absPath)
        {
            var basePath = HttpContext.Current.Server.MapPath("~/");
            var absUrl = ConvertToAbsUrl(absPath.Replace(basePath, "~/"));

            return absUrl;
        }

        public static string RemoveTrailingSlash(string url)
        {
            if (url.EndsWith("/"))
            {
                int lastIndex = url.LastIndexOf('/');
                return url.Remove(lastIndex);
            }
            else
                return url;
        }

        public static string Prepair(string url)
        {
            return RemoveTrailingSlash(url.ToLower());
        }

        public static string PrepairUri(string uri)
        {
            uri = StringHelper.RemoveSpecialChars(uri);

            return uri.Trim().Replace(" ", "-").ToLower();
        }

        public static bool IsSame(string url1, string url2)
        {
            url1 = URIHelper.ConvertToAbsUrl(url1);
            url2 = URIHelper.ConvertToAbsUrl(url2);

            if (Prepair(url1) == Prepair(url2))
                return true;

            return false;
        }

        public static bool Uri1ContainsUri2(string url1, string url2)
        {
            if (Prepair(url1).Contains(Prepair(url2)))
                return true;

            return false;
        }

        public static string UrlEncode(string str)
        {
            return HttpContext.Current.Server.UrlEncode(str);
        }

        public static string UrlDecode(string str)
        {
            return HttpContext.Current.Server.UrlDecode(str);
        }

        public static IEnumerable<string> GetUriSegments(string url, string ignoreUri = "")
        {
            IEnumerable<string> uriSegments = new List<string>();

            if (ignoreUri != "")
                url = url.Replace(ignoreUri, "");

            url = url.Replace(ConvertAbsUrlToTilda(BaseUrl), "");

            if (url.StartsWith("/"))
                url = url.Remove(0, 1);

            if (url.EndsWith("/"))
                url = url.Remove(url.Length - 1);

            if (url != "")
                uriSegments = url.Split('/');

            return uriSegments;
        }

        public static string GetParentPath(string url, int levelsUp)
        {
            var segments = GetParentPathList(url, levelsUp);
            var path = String.Join("/", segments);

            if (!path.EndsWith("/"))
                path += "/";

            path = URIHelper.ConvertAbsUrlToTilda(path);

            return path;
        }

        public static IEnumerable<string> GetParentPathList(string url, int levelsUp)
        {
            var segments = GetUriSegments(url).ToList();
            var moveUpBy = (segments.Count - levelsUp);

            if (moveUpBy > 0)
            {
                var count = segments.Count - moveUpBy;
                segments.RemoveRange(moveUpBy, count);
            }

            return segments;
        }

        public static string ConvertToAbsUrl(string relOrTildaToRootPath)
        {
            if (relOrTildaToRootPath == null || relOrTildaToRootPath.Trim() == "")
                return "";

            if (relOrTildaToRootPath.Contains("://"))
                return relOrTildaToRootPath;

            var segments = GetUriSegments(relOrTildaToRootPath).ToList();
            var baseUrlSegments = GetUriSegments(HttpContext.Current.Request.ApplicationPath).ToList();

            for (int i = 0; i < segments.Count; i++)
            {
                if (baseUrlSegments.Count <= i)
                    break;

                if (segments[i] == baseUrlSegments[i])
                    segments.RemoveAt(i);
            }

            relOrTildaToRootPath = "";

            for (int i = 0; i < segments.Count; i++)
                relOrTildaToRootPath += segments[i] + "/";

            if (segments.Count > 0)
            {
                if (segments[segments.Count - 1].Contains("."))
                    relOrTildaToRootPath = RemoveTrailingSlash(relOrTildaToRootPath);
            }

            var url = (BaseUrl + relOrTildaToRootPath).Replace("\\", "/");

            if((url.Contains("?") || url.Contains("#")) && url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return url;
        }

        public static bool IsCMSLoginRequest(string url)
        {
            url = url.ToLower();

            if (url.Contains("/admin/") || url.Contains("/login/"))
                return true;

            return false;
        }


        public static string BaseUrl
        {
            get
            {
                var baseUrl = (string)ContextHelper.GetFromRequestContext("BaseUrl");

                if (!string.IsNullOrEmpty(baseUrl))
                    return baseUrl;

                string appPath = HttpContext.Current.Request.ApplicationPath;

                if (!appPath.EndsWith("/"))
                    appPath = appPath + "/";

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + appPath;

                if (IsSSL())
                    url = url.Replace("http:", "https:");

                url = url.ToLower();

                ContextHelper.SetToRequestContext("BaseUrl", url);

                return url;
            }
        }

        public static string BaseUrlWithLanguage
        {
            get
            {
                var baseUrlWithLanguage = (string)ContextHelper.GetFromRequestContext("BaseUrlWithLanguage");

                if (!string.IsNullOrEmpty(baseUrlWithLanguage))
                    return baseUrlWithLanguage;

                var url = LanguageBaseUrl(FrameworkSettings.GetCurrentLanguage());

                ContextHelper.SetToRequestContext("BaseUrlWithLanguage", url);

                return url.ToLower();
            }
        }

        public static string LanguageBaseUrl(Language language)
        {
            var url = BaseUrl;

            if (LanguagesMapper.CountAllActive() > 1)
                url += language.UriSegment + "/".ToLower();

            return url;
        }

        public static string GetBaseUrlWithLanguage(Language language)
        {
            return BaseUrl + language.UriSegment + "/";
        }

        public static bool StartsWithLanguage(string url)
        {
            IEnumerable<Language> languages = LanguagesMapper.GetAllActive();
            url = ConvertAbsUrlToTilda(url);

            foreach (Language language in languages)
            {
                string url2 = URIHelper.ConvertAbsUrlToTilda(GetBaseUrlWithLanguage(language));

                if (url2.EndsWith("/"))
                    url2 = url2.Remove(url2.Length - 1);

                if (url.StartsWith(url2))
                    return true;
            }

            return false;
        }

        public static string BasePath
        {
            get
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;

                if (!appPath.EndsWith("/"))
                    appPath = appPath + "/";

                return appPath;
            }
        }

        public static string ConvertAbsUrlToTilda(string absUrl)
        {
            absUrl = absUrl.ToLower().Trim();

            if (absUrl == "")
                return "";

            if (absUrl.StartsWith("~/"))
                return absUrl;

            if (!absUrl.Contains("://"))
            {
                if(absUrl.StartsWith("/"))
                {
                    absUrl = absUrl.Substring(1, absUrl.Length-1);
                    absUrl = (BaseUrl + absUrl);
                }
            }                

            if (absUrl.StartsWith("/"))
                absUrl = BaseUrl + absUrl.Remove(0, 1);

            absUrl = absUrl.Replace(BaseUrl, "~/").Replace("~//", "~/");

            if (!absUrl.EndsWith("/") && !absUrl.Contains("?") && !absUrl.Contains("#") && !absUrl.Contains("."))
            {
                absUrl = absUrl + "/";
            }

            return absUrl;
        }

        public static string ConvertToAbsPath(string relOrTildaPath)
        {
            if (relOrTildaPath.Contains(":/") || relOrTildaPath.Contains(":\\"))
                return relOrTildaPath;

            return ConvertToAbsUrl(relOrTildaPath).Replace(BaseUrl, BasePath);
        }

        public static bool IsSSL()
        {
            bool isSSL = false;

            if (HttpContext.Current.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] != null && HttpContext.Current.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on")
                isSSL = true;

            if (!isSSL)
                isSSL = Request.IsSecureConnection;

            return isSSL;
        }

        public static void ForceSSL()
        {
            if (!IsSSL())
                Response.Redirect(Request.Url.AbsoluteUri.Replace("http:", "https:"));
        }

        public static void ForceNonSSL()
        {
            if (IsSSL())
                Response.Redirect(Request.Url.AbsoluteUri.Replace("https:", "http:"));
        }

        private static HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private static HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }
    }
}