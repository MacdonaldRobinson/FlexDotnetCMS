using FrameworkLibrary;
using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json;
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
            string response = JsonConvert.SerializeObject(GeoLocationHelper.GetLocation(GetIP()));

            WriteJSON(response);
        }

        [WebMethod]
        public void GetGlossaryTerms()
        {
            string response = JsonConvert.SerializeObject(GlossaryTermsMapper.GetAll());

            WriteJSON(response);
        }

        [WebMethod]
        public void GetGeoLocation(string ip)
        {
            GeoLocationHelper.APIKey = AppSettings.GeoLocationAPIKey;
            string response = JsonConvert.SerializeObject(GeoLocationHelper.GetLocation(ip));

            WriteJSON(response);
        }

        [WebMethod]
        public void GetLocationBanner()
        {
            var ip = GetIP();

            var location = GeoLocationHelper.GetLocation(ip);
            var imagesBaseDir = URIHelper.BasePath + "/media/uploads/images/banners/homebanners/";

            var cityBaseDir = imagesBaseDir;

            if(location.City.Name != null)
            {
                cityBaseDir = imagesBaseDir + location.City.Name.ToLower() + "/";
            }
            else
            {
                cityBaseDir = imagesBaseDir + "others/";
            }

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
    }
}