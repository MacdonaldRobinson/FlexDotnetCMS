using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebApplication.Services
{
    /// <summary>
    /// Summary description for Twitter
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class Twitter : BaseService
    {
        private TwitterHelper twitterHelper;

        public Twitter()
        {
            twitterHelper = new FrameworkLibrary.TwitterHelper(AppSettings.TwitterApiConsumerKey, AppSettings.TwitterApiConsumerSecret, AppSettings.TwitterApiAccessToken, AppSettings.TwitterApiAccessTokenSecret);
        }

        [WebMethod]
        public void GetTweetsByScreenName(string ScreenName, int NumberOfTweets = 2)
        {
            WriteJSON(twitterHelper.GetTweetsByScreenName(ScreenName, NumberOfTweets, true));
        }

        [WebMethod]
        public void GetTweets(int NumberOfTweets = 2)
        {
            WriteJSON(twitterHelper.GetTweetsByScreenName(AppSettings.TwitterScreenName, NumberOfTweets, true));
        }
    }
}