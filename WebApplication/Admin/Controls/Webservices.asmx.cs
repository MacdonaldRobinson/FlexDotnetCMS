using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace WebApplication.Admin.Controls
{
    /// <summary>
    /// Summary description for Webservices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class Webservices : System.Web.Services.WebService
    {
        [WebMethod]
        public int ReOrderMediaFields(List<FrameworkLibrary.MediaDetailField> items)
        {
            var index = 0;
            foreach (var item in items)
            {
                var mediaField = BaseMapper.GetDataModel().MediaDetailFields.SingleOrDefault(i => i.ID == item.ID);

                if (mediaField != null)
                {
                    mediaField.OrderIndex = index;

                    index++;
                }
            }

            return BaseMapper.GetDataModel().SaveChanges();
        }
    }
}