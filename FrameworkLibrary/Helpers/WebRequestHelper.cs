using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FrameworkLibrary
{
    public class WebRequestHelper
    {
        public WebRequestHelper()
        {
            OmmitList.Add("jquery");
            OmmitList.Add("google");            
            OmmitList.Add("twitter");
            OmmitList.Add("facebook");
            OmmitList.Add("youtube");
        }

        private long cacheDurationInSeconds = 60;
        private bool enableCaching = false;
        private List<string> _ommitList = new List<string>();

        public List<string> OmmitList
        {
            get
            {
                return _ommitList;
            }
        }

        public enum RequestMethod
        {
            GET,
            POST
        }

        public bool EnableCaching
        {
            get { return enableCaching; }
            set { enableCaching = value; }
        }

        public long CacheDurationInSeconds
        {
            get { return cacheDurationInSeconds; }
            set { cacheDurationInSeconds = value; }
        }

        public string MakeWebRequest(string urlString, ICredentials credentialCache = null, RequestMethod method = RequestMethod.GET, string queryString = "")
        {
            var storageItem = WebserviceRequestsMapper.GetByUrl(urlString);
            var data = "";

            if ((storageItem != null) && (EnableCaching) && !storageItem.Response.Contains("unavailable") && storageItem.DateLastModified.AddSeconds(CacheDurationInSeconds) > DateTime.Now)
                return storageItem.Response;

            WebserviceRequest request = null;

            if (storageItem == null)
                request = new WebserviceRequest();
            else
                request = storageItem;

            request.Url = urlString;
            request.QueryString = queryString;
            request.Response = "";
            request.Method = method.ToString();
            request.UrlReferrer = "";

            if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
                request.UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri;

            WebRequest webRequest = WebRequest.Create(urlString);
            if (credentialCache != null)
            {
                webRequest.Credentials = credentialCache;
                webRequest.PreAuthenticate = true;
            }

            webRequest.Method = method.ToString();

            try
            {
                if (method == RequestMethod.POST)
                {
                    var encoding = new ASCIIEncoding();
                    byte[] bytes = encoding.GetBytes(queryString);

                    webRequest.ContentLength = bytes.Length;
                    webRequest.ContentType = "application/x-www-form-urlencoded";

                    using (var webRequestStream = webRequest.GetRequestStream())
                    {
                        webRequestStream.Write(bytes, 0, bytes.Length);
                        webRequestStream.Flush();
                        webRequestStream.Close();
                    }
                }

                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    Stream dataStream = webResponse.GetResponseStream();

                    using (StreamReader streamReader = new StreamReader(dataStream))
                    {
                        data = streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                ErrorHelper.LogException(ex);

                data = ex.Message;

                if (ex.Response != null)
                {
                    // can use ex.Response.Status, .StatusDescription
                    if (ex.Response.ContentLength != 0)
                    {
                        using (var stream = ex.Response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                data = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }

            if (request != null)
            {
                request.Response = data;

                var ommit = false;

                foreach (var item in OmmitList)
                {
                    if (request.Url.ToLower().Contains(item))
                    {
                        ommit = true;
                        break;
                    }
                }

                if (EnableCaching)
                {
                    if (request.ID == 0)
                        WebserviceRequestsMapper.Insert(request);
                    else
                    {
                        request = BaseMapper.GetObjectFromContext(request);
                        WebserviceRequestsMapper.Update(request);
                    }
                }
            }

            return data;
        }
    }
}