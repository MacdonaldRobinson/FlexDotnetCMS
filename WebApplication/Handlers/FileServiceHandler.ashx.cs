using FrameworkLibrary;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplication.Services
{
    /// <summary>
    /// Summary description for FileServiceHandler
    /// </summary>
    public class FileServiceHandler : IHttpHandler
    {
        private HttpContext context = null;

        public void ProcessRequest(HttpContext context)
        {
            this.context = context;

            var templateBaseUrl = URIHelper.ConvertToAbsUrl(context.Request.QueryString["templateBaseUrl"]);

            var method = "loadfile";
            var path = context.Request.QueryString["path"];

            if (!string.IsNullOrEmpty(context.Request["method"]))
                method = context.Request["method"].ToLower();

            if (string.IsNullOrEmpty(path))
                path = context.Request.FilePath;

            switch (method)
            {
                case "loadfile":
                    LoadFile(path, templateBaseUrl);
                    break;

                /*case "loadjsincludes":
                    LoadJsIncludes(context.Request.QueryString["pageId"], templateBaseUrl);
                    break;

                case "loadcssincludes":
                    LoadCssIncludes(context.Request.QueryString["pageId"], templateBaseUrl);
                    break;*/
            }
        }

        public void LoadFile(string path, string templateBaseUrl)
        {
            if (path == null)
                return;

            var Request = HttpContext.Current.Request;
            var Response = HttpContext.Current.Response;

            //var cacheData = (string)ContextHelper.GetFromCache(path);

            bool isCss = path.EndsWith(".css");
            bool isJs = path.EndsWith(".js");

            if (isCss)
                Response.ContentType = "text/css";
            else if (isJs)
                Response.ContentType = "application/x-javascript";

            string data = "";
            data = LoaderHelper.ReadUrl(path, AppSettings.EnableWebRequestCaching, AppSettings.WebRequestCacheDurationInSeconds);

            //if (AppSettings.MinifyOutput)
            //{
            //    if (isCss)
            //    {
            //        var cssCompressor = new CssCompressor();
            //        data = cssCompressor.Compress(data);
            //    }
            //    else if (isJs)
            //    {
            //        var jsCompressor = new JavaScriptCompressor();
            //        data = jsCompressor.Compress(data);
            //    }
            //}

            var segments = URIHelper.GetUriSegments(path).ToList();
            segments.RemoveAt(segments.Count - 1);

            data = ParserHelper.ParseData(data, BasePage.GetDefaultTemplateVars(templateBaseUrl));

            if (isCss)
            {
                string fileBaseUrl = "/" + string.Join("/", segments) + "/";

                /*data = data.Replace("url('", "url('" + fileBaseUrl);
                data = data.Replace("url(\"", "url(\"" + fileBaseUrl);
                data = data.Replace("url(", "url(" + fileBaseUrl);
                data = data.Replace("url(" + fileBaseUrl + "'", "url('");
                data = data.Replace("url('" + fileBaseUrl + "", "url('");*/

                data = Regex.Replace(data, "url[(][\'|\"]{0,1}(?!.*http)", "${0}" + fileBaseUrl);
            }

            //ContextHelper.SaveToCache(Request.Url.AbsoluteUri, data, DateTime.Now.AddSeconds(AppSettings.WebRequestCacheDurationInSeconds));

            BaseService.WriteRaw(data, Response);
        }

        /*public void LoadJsIncludes(string pageId, string templateBaseUrl)
        {
            HttpContext.Current.Response.ContentType = "application/x-javascript";

            if (!AppSettings.CombineCssAndJsIncludes)
                BaseService.WriteRaw("");

            Dictionary<string, HtmlControl> controls = WebFormHelper.GetJSIncludes(AppSettings.JsIncludesPlaceHolderID);
            var data = "";
            foreach (var control in controls)
            {
                var attr = control.Value.Attributes["src"];

                if (!attr.Contains("templateBaseUrl"))
                    attr += "&templateBaseUrl=" + templateBaseUrl;

                if (!attr.Contains("?"))
                    attr = attr.Replace("&", "?");

                if (!attr.StartsWith(URIHelper.BaseUrl))
                    data += LoaderHelper.ReadUrl(attr, AppSettings.EnableWebRequestCaching, AppSettings.WebRequestCacheDurationInSeconds) + "\r\n";
                else
                    data += File.ReadAllText(URIHelper.ConvertToAbsPath(URIHelper.ConvertAbsUrlToTilda(new Uri(attr).AbsolutePath))) + "\r\n";
            }

            BaseService.WriteRaw(data);
        }

        public void LoadCssIncludes(string pageId, string templateBaseUrl)
        {
            HttpContext.Current.Response.ContentType = "text/css";

            if (!AppSettings.CombineCssAndJsIncludes)
                BaseService.WriteRaw("");

            Dictionary<string, HtmlControl> controls = WebFormHelper.GetCSSIncludes(AppSettings.CssIncludesPlaceHolderID);
            var data = "";
            foreach (var control in controls)
            {
                var attr = control.Value.Attributes["href"];

                if (!attr.Contains("templateBaseUrl"))
                    attr += "&templateBaseUrl=" + templateBaseUrl;

                if (!attr.Contains("?"))
                    attr = attr.Replace("&", "?");

                if (!attr.StartsWith(URIHelper.BaseUrl))
                    data += LoaderHelper.ReadUrl(attr, AppSettings.EnableWebRequestCaching, AppSettings.WebRequestCacheDurationInSeconds) + "\r\n";
                else
                    data += File.ReadAllText(URIHelper.ConvertToAbsPath(URIHelper.ConvertAbsUrlToTilda(new Uri(attr).AbsolutePath))) + "\r\n";
            }

            data = data.Replace(templateBaseUrl + ",", "");

            BaseService.WriteRaw(data);
        }*/

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}