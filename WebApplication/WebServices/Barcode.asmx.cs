using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Drawing;
using MessagingToolkit.QRCode.Codec;
namespace WebApplication.Services
{
    /// <summary>
    /// Summary description for Barcode
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Barcode : BaseService
    {
        [WebMethod]
        public void GenerateQRCode(string content)
        {
            QRCodeEncoder qe = new QRCodeEncoder();
            WriteImage(qe.Encode(content));
        }
    }
}
