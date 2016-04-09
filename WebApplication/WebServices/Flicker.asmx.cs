using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using FrameworkLibrary;

namespace WebApplication.Services
{
    /// <summary>
    /// Summary description for Flicker
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Flicker : BaseService
    {
        public Flicker()
        {
            FlickerHelper.Instance.SetReturnFormat("json");
            FlickerHelper.Instance.SetFlickerAPIKey(AppSettings.FlickerAPIKey);
        }

        [WebMethod]
        public void GetPhotosInPhotoSet(string PhotoSetID)
        {
            WriteJSON(FlickerHelper.Instance.RequestRaw("flickr.photosets.getPhotos", "", "photoset_id=" + PhotoSetID));
        }

        [WebMethod]
        public void GetPhotoSizes(string PhotoID)
        {
            WriteJSON(FlickerHelper.Instance.RequestRaw("flickr.photos.getSizes", "", "photo_id=" + PhotoID));
        }
    }
}
