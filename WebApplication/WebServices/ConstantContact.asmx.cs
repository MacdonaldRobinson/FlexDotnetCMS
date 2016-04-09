using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using FrameworkLibrary;
using WebApplication.Services;

namespace WebApplication.WebServices
{
    /// <summary>
    /// Summary description for ConstantContact
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ConstantContact : BaseService
    {
        [WebMethod]
        public void MakeRequest(string webServiceSegment)
        {
            var username = "scottlandstomp";
            var apiKey = "75dd6e4e-162d-4570-8ae7-cd61efa53706";
            var address = new Uri("https://api.constantcontact.com/ws/customers/" + username + "/" + webServiceSegment);
            var networkCred = new NetworkCredential(apiKey + "%" + username, "stomp2011");
            var cacheCred = new CredentialCache();
            cacheCred.Add(address, "Basic", networkCred);

            var webRequestHelper = new WebRequestHelper();
            var xmlString = webRequestHelper.MakeWebRequest(address.AbsoluteUri, networkCred);
            WriteXML(xmlString);
        }
    }
}
