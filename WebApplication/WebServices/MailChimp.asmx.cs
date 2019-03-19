using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebApplication.Services;
using FrameworkLibrary;

using MailChimp.Net.Models;

namespace WebApplication.WebServices
{
	/// <summary>
	/// Summary description for MailChimp
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	[System.Web.Script.Services.ScriptService]
	public class MailChimp : BaseService
	{
		public MailChimpHelper MailChimpHelper { get; } = new MailChimpHelper("API");
		public string ListID { get; } = "LISTID";

		[WebMethod]
		public void DeleteUnUsedTags()
		{			
			//var returnObj = MailChimpHelper.GenerateUrl(uriSegment, queryString);
			MailChimpHelper.DeleteUnUsedTags(ListID);			
		}

		[WebMethod]
		public void AddUpdateEmailAndTags(string emailAddress, string tagsCsv)
		{
			var tagList = tagsCsv.Split(',').ToList();
			//var returnObj = MailChimpHelper.GenerateUrl(uriSegment, queryString);
			var returnObj = MailChimpHelper.AddUpdateEmailAddress(ListID, new FrameworkLibrary.User() { EmailAddress = emailAddress }, null, tagList);
			WriteObjectAsJSON(returnObj);
		}
	}
}
