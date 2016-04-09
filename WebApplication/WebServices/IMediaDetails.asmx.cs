using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace WebApplication.Services
{
    /// <summary>
    /// Summary description for IMediaDetails
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class IMediaDetails : BaseService
    {
        [WebMethod]
        public void GetLocation()
        {
            string response = GetGeoLocation(GetIP());

            WriteJSON(response);
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

        [WebMethod]
        public string GetGeoLocation(string ip)
        {
            GeoLocationHelper.APIKey = AppSettings.GeoLocationAPIKey;
            string response = GeoLocationHelper.GetLocation(ip);

            return response;
        }

        [WebMethod]
        public void GetLocationBanner()
        {
            var ip = GetIP();

            var location = (new JavaScriptSerializer()).Deserialize<Location>(GetGeoLocation(ip));
            var imagesBaseDir = URIHelper.BasePath + "/media/uploads/images/banners/homebanners/";

            if (string.IsNullOrEmpty(location.city))
                location.city = "others";

            var cityBaseDir = imagesBaseDir + location.city.ToLower() + "/";

            var path = "";

            if (!Directory.Exists(cityBaseDir))
                cityBaseDir = imagesBaseDir + "others/";

            var filesInDir = Directory.GetFiles(cityBaseDir);

            var randomIndex = (new Random()).Next(0, filesInDir.Length);

            var filePath = filesInDir[randomIndex];

            path = filePath.Replace(URIHelper.BasePath, "");

            WriteJSON("{\"path\":\"" + path + "\"}");
        }

        [WebMethod]
        public void GetByID(long id)
        {
            string json = "";

            IMediaDetail detail = MediaDetailsMapper.GetByID(id);

            if (detail != null)
                json = detail.ToJSON();

            WriteJSON(json);
        }

        [WebMethod]
        public void GetRelatedItemsByID(long id)
        {
            string json = "";
            IEnumerable<IMediaDetail> relatedItems = new List<IMediaDetail>();
            IMediaDetail detail = MediaDetailsMapper.GetByID(id);

            if (detail != null)
                relatedItems = MediaDetailsMapper.GetRelatedItems(detail);

            WriteJSON(relatedItems.ToJSON());
        }

        [WebMethod]
        public void ClearCache(long id)
        {
            if (id != 0)
            {
                var item = MediaDetailsMapper.GetByID(id);

                if (item != null)
                {
                    item.RemoveFromCache();
                }
            }
        }

        [WebMethod]
        public void GetFilterOptions(long id)
        {
            string json = "";

            IEnumerable<string> filterOptions = new List<string>();
            IMediaDetail detail = MediaDetailsMapper.GetByID(id);

            if (detail != null)
                filterOptions = MediaDetailsMapper.GetFilterOptions(detail);

            WriteJSON(filterOptions.ToJSON());
        }

        [WebMethod]
        public void GetSearchResults(string searchTerm)
        {
            string json = "";

            var items = MediaDetailsMapper.SearchForTerm(searchTerm);

            WriteJSON(items.ToJSON());
        }

        //[WebMethod]
        //public void GetFilteredChildItems(long id, string filterOption)
        //{
        //    string json = "";

        //    IMediaDetail item = MediaDetailsMapper.GetByID(id);

        //    if(item == null)
        //        return;

        //    IEnumerable<IMediaDetail> items = new List<IMediaDetail>();
        //    Tag tag = TagsMapper.GetByName(filterOption);

        //    Language language = LanguagesMapper.GetByID(item.LanguageID);

        //    if (language != null)
        //    {
        //        if (tag != null)
        //            items = MediaDetailsMapper.GetOnlyActiveAndVisibleMediaDetails(MediaDetailsMapper.GetByMedias(MediasMapper.GetByTag(tag), language, 0), item);
        //        else
        //        {
        //            switch(filterOption)
        //            {
        //                case "Latest":
        //                    items = MediaDetailsMapper.GetOnlyActiveAndVisibleMediaDetails(MediaDetailsMapper.GetAll(), item).OrderBy(i => i.DateCreated);
        //                break;
        //                case "Alphabetical":
        //                    items = MediaDetailsMapper.GetOnlyActiveAndVisibleMediaDetails(MediaDetailsMapper.GetAll(), item).OrderBy(i => i.LinkTitle);
        //                break;
        //            }
        //        }
        //    }

        //    WriteJSON(items.ToJSON());
        //}
    }
}