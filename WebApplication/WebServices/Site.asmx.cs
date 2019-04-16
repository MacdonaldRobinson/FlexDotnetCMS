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
    /// Summary description for Site
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class Site : BaseService
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
            string response = "{}";

            if (SettingsMapper.GetSettings().EnableGlossaryTerms)
            {
                response = JsonConvert.SerializeObject(GlossaryTermsMapper.GetAll());
            }

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

            var currentMonth = DateTime.Now.Month;

            var subdir = "summer/";
            if(currentMonth > 10 || currentMonth < 3)
            {
                subdir = "winter/";
            }

            var filesInDir = Directory.GetFiles(cityBaseDir + subdir);

            var randomIndex = (new Random()).Next(0, filesInDir.Length);

            if (filesInDir.Length > 0)
            {
                var filePath = filesInDir[randomIndex];
                path = filePath.Replace(URIHelper.BasePath, "").Replace("\\","/");
            }

            WriteJSON("{\"path\":\"" + path + "\"}");
        }

        [WebMethod]
        public void GetByID(long id)
        {
            string json = "";

            IMediaDetail detail = MediaDetailsMapper.GetByID(id);

            if (detail != null)
                json = detail.ToJson();

            WriteJSON(json);
        }

		[WebMethod]
		public void GetField(long id, string fieldCode)
		{
			string json = "";

			var field = MediasMapper.GetByID(id).GetLiveMediaDetail()?.LoadField(fieldCode);

			json = field.ToJson(3);

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

            WriteJSON(relatedItems.ToJson());
        }

        [WebMethod]
        public void SaveUseMainLayout(long mediaDetailId, string html)
        {
			if (FrameworkSettings.CurrentUser == null)
			{
				var returnObj = BaseMapper.GenerateReturn("You must be logged in");
				WriteJSON(returnObj.ToJson());
				return;
			}

			var mediaDetail = (MediaDetail)MediaDetailsMapper.GetByID(mediaDetailId);

			if (!FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.Save))
			{
				var returnObj = BaseMapper.GenerateReturn("You do not have permissions to perform this operation");
				WriteJSON(returnObj.ToJson());
				return;
			}

            if(mediaDetail != null)
            {
				mediaDetail.UseMediaTypeLayouts = false;

				html = Uri.UnescapeDataString(html);
				//html = MediaDetailsMapper.ConvertUrlsToShortCodes(html);

				html = html.Replace(URIHelper.BaseUrl, "{BaseUrl}");

				mediaDetail.MainLayout = html;

                var returnObj = MediaDetailsMapper.Update(mediaDetail);

                WriteJSON(returnObj.ToJson());
            }
            else
            {
                WriteJSON(new Return() { Error = new Elmah.Error() { Message = $"MediaDetail with the ID '{mediaDetailId}' was not found" } }.ToJson());
            }            
        }


        [WebMethod]
        public void CanAccessFrontEndEditorForMediaDetail(long id)
        {
            var returnObj = new Return();
            var mediaDetail = MediaDetailsMapper.GetByID(id);

            if(mediaDetail != null && FrameworkSettings.CurrentUser != null)
            {
                returnObj = MediaDetailsMapper.CanAccessMediaDetail(mediaDetail, FrameworkSettings.CurrentUser);
                WriteJSON(StringHelper.ObjectToJson(returnObj));
            }

            returnObj = new Return() { Error = new Elmah.Error(new Exception("Cannot access")) };
            WriteJSON(StringHelper.ObjectToJson(returnObj));
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

            WriteJSON(filterOptions.ToJson());
        }

        [WebMethod]
        public void GetSearchResults(string searchTerm)
        {
            string json = "";

            var items = MediaDetailsMapper.SearchForTerm(searchTerm);

            WriteJSON(items.ToJson());
        }
    }
}