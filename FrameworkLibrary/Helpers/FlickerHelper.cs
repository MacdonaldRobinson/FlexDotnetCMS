using System.Xml;

namespace FrameworkLibrary
{
    public class FlickerHelper
    {
        private string apiKey = "";
        private string returnFormat = "json";
        private static FlickerHelper instance = null;
        private string extras = "oops url_t, url_s, url_m, url_o";
        private WebRequestHelper webRequestHelper = new WebRequestHelper();

        public static FlickerHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new FlickerHelper();

                return instance;
            }
        }

        public void SetReturnFormat(string returnFormat)
        {
            this.returnFormat = returnFormat;
        }

        public void SetFlickerAPIKey(string apiKey)
        {
            this.apiKey = apiKey;
        }

        private FlickerHelper()
        {
        }

        public string GenerateRequestUrl(string method, string userId)
        {
            return "http://api.flickr.com/services/rest/?method=" + method + "&api_key=" + this.apiKey + "&user_id=" + userId + "&format=" + this.returnFormat + "&nojsoncallback=1&extras=" + this.extras;
        }

        public string RequestRaw(string method, string userId, string queryStringParams = "")
        {
            return webRequestHelper.MakeWebRequest(GenerateRequestUrl(method, userId) + "&" + queryStringParams);
        }

        public XmlDocument RequestXML(string method, string userId, string queryStringParams = "")
        {
            string xmlString = RequestRaw(method, userId, queryStringParams);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);

            return xmlDocument;
        }
    }
}