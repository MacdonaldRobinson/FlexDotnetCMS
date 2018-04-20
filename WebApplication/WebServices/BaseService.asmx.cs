using FrameworkLibrary;
using System;
using System.Collections.Generic;
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
		public BaseService()
		{
		}
		public string GetIP()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];

            if (ip == null)
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }

        public static bool CanAddGZIP()
        {
            var Request = HttpContext.Current.Request;

            string virtualPath = Request.AppRelativeCurrentExecutionFilePath.ToLower();

            if (virtualPath.EndsWith("/"))
                return true;

            var patterns = AppSettings.CompressUriRequestPatterns.Split(',');

            var found = patterns.Where(virtualPath.Contains).Count();

            return found > 0;
        }

        [WebMethod]
        public void MakeWebRequest(string url)
        {
            var Response = HttpContext.Current.Response;

            var WebRequestHelper = new WebRequestHelper();
            WriteRaw(WebRequestHelper.MakeWebRequest(url), Response);
        }

        public static void AddResponseHeaders(bool enableCaching = true, bool enableGzip = true)
        {
            var Request = HttpContext.Current.Request;
            var Response = HttpContext.Current.Response;

            var absPath = Request.Url.AbsolutePath.ToLower();

            /*if ((enableCaching) && (!BasePage.IsInAdminSection))
            {
                Response.Cache.SetExpires(DateTime.Now.AddYears(30));
                Response.Cache.SetMaxAge(TimeSpan.FromDays(365.0 * 3.0));
            }

            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);*/

            var cache = Response.Cache;

            cache.SetMaxAge(TimeSpan.FromDays(0));
            cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            cache.SetValidUntilExpires(false);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();

            if (Request.HttpMethod == "POST")
                return;

            if (((!AppSettings.AttemptCompression) || (!CanAddGZIP())) || (!enableGzip))
                return;

            /*if (absPath.EndsWith(".css") || absPath.EndsWith(".js") || absPath.EndsWith(".axd"))
                return;*/

            string encodings = Request.Headers.Get("Accept-Encoding");

            if (encodings == null)
                return;

            if (encodings != null)
            {                
                // Check the browser accepts deflate or gzip (deflate takes preference)
                encodings = encodings.ToLower();
                if (encodings.Contains("deflate"))
                {
                    Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress);
                    HttpContext.Current.Response.AppendHeader("Content-Encoding", "deflate");
                }
                else if (encodings.Contains("gzip"))
                {
                    Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
                    Response.AppendHeader("Content-Encoding", "gzip");
                }
            }
        }

        public static void WriteObjectAsJSON(object obj)
        {
            var Response = HttpContext.Current.Response;

            Response.ContentType = "application/json";
            var json = StringHelper.ObjectToJson(obj);
            WriteRaw(json, Response);
        }

        public static void WriteJSON(string json)
        {
            var Response = HttpContext.Current.Response;

            Response.ContentType = "application/json";
            WriteRaw(json, Response);
        }

        public static void WriteCsv<T>(List<T> Obj, string filename) where T : class
        {
            var Response = HttpContext.Current.Response;

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", $"attachment; filename={filename}.csv");

            var csv = Obj.ToCsv();
            WriteRaw(csv, Response);
        }

        public static void WriteRawCSV(string csvString, string filename)
        {
            var Response = HttpContext.Current.Response;

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", $"attachment; filename={filename}.csv");

            WriteRaw(csvString, Response);
        }

        public static void WriteXML(string xmlString)
        {
            var Response = HttpContext.Current.Response;

            Response.ContentType = "application/xml";
            WriteRaw(xmlString, Response);
        }

        public static void WriteHtml(string htmlString)
        {
            var Response = HttpContext.Current.Response;

            Response.ContentType = "text/html";
            WriteRaw(htmlString, Response);
        }

        public static void WriteText(string textString)
        {
            var Response = HttpContext.Current.Response;

            Response.ContentType = "text/plain";
            WriteRaw(textString, Response);
        }

        public static void WriteRaw(byte[] bytes, HttpResponse Response)
        {
            Response.Write(bytes);
            Response.End();
        }

        public static void WriteRaw(string str, HttpResponse Response)
        {
            Response.Write(str);
            Response.End();
        }

        public static void WriteImage(Bitmap bitmap)
        {
            var Response = HttpContext.Current.Response;

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