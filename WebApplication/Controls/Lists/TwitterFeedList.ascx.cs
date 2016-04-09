using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.Lists
{
    public partial class TwitterFeedList : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TwitterScreenName))
                TwitterScreenName = AppSettings.TwitterScreenName;

            if (!TwitterScreenName.Contains("["))
            {
                var twitterHelper = new TwitterHelper(AppSettings.TwitterApiConsumerKey, AppSettings.TwitterApiConsumerSecret, AppSettings.TwitterApiAccessToken, AppSettings.TwitterApiAccessTokenSecret);
                var tweets = twitterHelper.GetTweetsByScreenName(TwitterScreenName, NumberOfItems);

                ItemsList.DataSource = tweets;
                ItemsList.DataBind();
            }
        }

        protected void ItemsList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var tweet = (LinqToTwitter.Status)e.Item.DataItem;
            var text = (Literal)e.Item.FindControl("Text");

            text.Text = LinkTwitterTags(tweet.Text);
        }

        private string LinkTwitterTags(string tweet)
        {
            var patterns = new Dictionary<string, string>();
            patterns.Add("(http://[^ ]*[a-zA-Z0-9])", "<a href=\"$1\" target=\"_blank\">$1</a>");
            patterns.Add("(http://[^ ]*[a-zA-Z0-9]target=\"_blank\")", "<a href=\"$1\" target=\"_blank\">$1</a>");
            patterns.Add("(#[^ ]*[a-zA-Z0-9])", "<a href=\"http://twitter.com/$1\" target=\"_blank\">$1</a>");
            patterns.Add("(@[^ ]*[a-zA-Z0-9])", "<a href=\"http://twitter.com/$1\" target=\"_blank\">$1</a>");
            patterns.Add("(username)", "<a href=\"http://twitter.com/" + TwitterScreenName + "\" target=\"_blank\">$1</a>");

            foreach (KeyValuePair<string, string> i in patterns)
            {
                tweet = RegReplace(tweet, i.Key, i.Value);
            }

            return tweet;
        }

        private string RegReplace(string strInput, string strPattern, string strReplace)
        {
            return System.Text.RegularExpressions.Regex.Replace(strInput, strPattern, strReplace);
        }

        public string TwitterScreenName { get; set; }

        public int NumberOfItems { get; set; }
    }
}