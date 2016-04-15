using FrameworkLibrary;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebApplication.Services
{
    /// <summary>
    /// Summary description for BaseService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class BaseService : System.Web.Services.WebService
    {
        private static HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }

        private static HttpResponse Response
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }

        public static bool CanAddGZIP()
        {
            string virtualPath = Request.AppRelativeCurrentExecutionFilePath.ToLower();

            if (virtualPath.EndsWith("/"))
                return true;

            var patterns = AppSettings.GzipUriRequestPatterns.Split(',');

            var found = patterns.Where(virtualPath.Contains).Count();

            return found > 0;
        }

        [WebMethod]
        public void MakeWebRequest(string url)
        {
            var WebRequestHelper = new WebRequestHelper();
            WriteRaw(WebRequestHelper.MakeWebRequest(url));
        }

        public static void AddResponseHeaders(bool enableCaching = true, bool enableGzip = true)
        {
            var absPath = Request.Url.AbsolutePath.ToLower();

            if ((enableCaching) && (!BasePage.IsInAdminSection))
            {
                Response.Cache.SetExpires(DateTime.Now.AddYears(30));
                Response.Cache.SetMaxAge(TimeSpan.FromDays(365.0 * 3.0));
            }

            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);

            if (HttpContext.Current.Request.HttpMethod == "POST")
                return;

            if (((!AppSettings.AttemptGzipCompression) || (!CanAddGZIP())) || (!enableGzip))
                return;

            string encodings = HttpContext.Current.Request.Headers.Get("Accept-Encoding");

            if (encodings == null)
                return;

            if (absPath.EndsWith(".css") || absPath.EndsWith(".js") || absPath.EndsWith(".axd"))
                return;

            Stream s = HttpContext.Current.Response.Filter;
            encodings = encodings.ToLower();

            if (encodings.Contains("gzip"))
            {
                Response.Filter = new GZipStream(s, CompressionMode.Compress);
                Response.AppendHeader("Content-Encoding", "gzip");
                //HttpContext.Current.Trace.Warn("GZIP Compression on");
            }
        }

        public static void WriteJSON(string json)
        {
            Response.ContentType = "application/json";
            WriteRaw(json);
        }

        public static void WriteXML(string xmlString)
        {
            Response.ContentType = "application/xml";
            WriteRaw(xmlString);
        }

        public static void WriteHtml(string htmlString)
        {
            Response.ContentType = "text/html";
            WriteRaw(htmlString);
        }

        public static void WriteRaw(string str)
        {
            Response.Write(str);
            Response.End();
        }

        public static void WriteImage(Bitmap bitmap)
        {
            Response.ContentType = "image/png";

            Graphics graph = Graphics.FromImage(bitmap);
            MemoryStream temp = new MemoryStream();

            bitmap.Save(temp, ImageFormat.Png);
            byte[] buffer = temp.GetBuffer();
            Response.OutputStream.Write(buffer, 0, buffer.Length);
            Response.Flush();
            Response.End();
        }
    }
}