using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FrameworkLibrary
{
    public class JQuerySocialStreamJsonEntry
    {
        public string pageLink { get; set; }

        public string pageName { get; set; }

        public string link { get; set; }

        public string content { get; set; }

        public string thumb { get; set; }

        public string publishedDate { get; set; }
    }

    public class JQuerySocialStreamJsonResponseData
    {
        public JQuerySocialStreamJsonResponseData()
        {
            feed = new JQuerySocialStreamJsonFeed();
        }

        public JQuerySocialStreamJsonFeed feed { get; set; }
    }

    public class JQuerySocialStreamJsonRoot
    {
        public JQuerySocialStreamJsonRoot()
        {
            responseData = new JQuerySocialStreamJsonResponseData();
        }

        public JQuerySocialStreamJsonResponseData responseData { get; set; }
    }

    public class JQuerySocialStreamJsonFeed
    {
        public JQuerySocialStreamJsonFeed()
        {
            entries = new List<JQuerySocialStreamJsonEntry>();
        }

        public string link { get; set; }

        public List<JQuerySocialStreamJsonEntry> entries { get; set; }
    }

    public class JQuerySocialStreamJson
    {
        public string RawFacebookJson { get; set; }

        public dynamic GetObjectFromRawJson()
        {
            return (new JavaScriptSerializer()).Deserialize<dynamic>(RawFacebookJson);
        }

        public string GenerateSocialStreamJson()
        {
            var JQuerySocialStreamJsonRoot = new JQuerySocialStreamJsonRoot();

            var objectFromRawJson = GetObjectFromRawJson();

            foreach (var item in objectFromRawJson["posts"]["data"])
            {
                var jQuerySocialStreamJsonEntry = new JQuerySocialStreamJsonEntry();
                jQuerySocialStreamJsonEntry.link = item["link"];
                jQuerySocialStreamJsonEntry.pageName = item["from"]["name"];
                jQuerySocialStreamJsonEntry.content = item["message"];
                jQuerySocialStreamJsonEntry.thumb = item["picture"];
                jQuerySocialStreamJsonEntry.publishedDate = item["created_time"];

                JQuerySocialStreamJsonRoot.responseData.feed.entries.Add(jQuerySocialStreamJsonEntry);
            }

            var json = (new JavaScriptSerializer()).Serialize(JQuerySocialStreamJsonRoot);

            return json;
        }

        public JQuerySocialStreamJson(string rawFacebookJson)
        {
            RawFacebookJson = rawFacebookJson;
        }
    }

    public class FacebookHelper
    {
        private string accessToken = "";
        private string appId = "";
        private string appSecret = "";
        private WebRequestHelper webRequestHelper = new WebRequestHelper();

        public FacebookHelper(string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        public string MakeFacebookRequest(string id, string limit)
        {
            var fields = "posts.limit(" + limit + ")";
            var url = "https://graph.facebook.com/" + id + "/?key=value&access_token=" + this.appId + "|" + this.appSecret + "&fields=id,message,picture,link,name,description,type,icon,created_time,from,object_id,likes,comments&fields=" + fields;
            var returnJson = webRequestHelper.MakeWebRequest(url);

            var jquerySocialStreamJson = new JQuerySocialStreamJson(returnJson);

            return jquerySocialStreamJson.GenerateSocialStreamJson();
        }
    }
}