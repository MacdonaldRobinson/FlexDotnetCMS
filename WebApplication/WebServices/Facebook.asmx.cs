﻿using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebApplication.Services
{
    /// <summary>
    /// Summary description for Facebook
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class Facebook : BaseService
    {
        private FacebookHelper facebookHelper;

        public Facebook()
        {
            facebookHelper = new FacebookHelper(AppSettings.FacebookAppID, AppSettings.FacebookAppSecret);
        }

        [WebMethod]
        public void MakeRequest(string id, string limit)
        {
            WriteJSON(facebookHelper.MakeFacebookRequest(id, limit));
        }
    }
}