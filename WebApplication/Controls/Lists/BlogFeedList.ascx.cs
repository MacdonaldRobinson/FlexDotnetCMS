using FrameworkLibrary;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.Lists
{
    public partial class BlogFeedList : System.Web.UI.UserControl
    {
        private string feedUrl = "";
        private int numberOfItemsToPull = 1;
        private bool showPostDescription = true;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Bind();
        }

        public void SetBlogUrl(string feedUrl, int numberOfItemsToPull)
        {
            this.feedUrl = feedUrl;
            this.numberOfItemsToPull = numberOfItemsToPull;
        }

        public bool ShowPostDescription
        {
            get
            {
                return showPostDescription;
            }
            set
            {
                showPostDescription = value;
            }
        }

        private void Bind()
        {
            var webRequestHelper = new WebRequestHelper();
            webRequestHelper.EnableCaching = true;
            webRequestHelper.CacheDurationInSeconds = AppSettings.WebRequestCacheDurationInSeconds;
            ItemsList.DataSource = RssHelper.GetRssItems(webRequestHelper.MakeWebRequest(feedUrl), numberOfItemsToPull);
            ItemsList.DataBind();
        }

        protected void ItemsList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Literal PublishDate = (Literal)e.Item.FindControl("PublishDate");
                HyperLink Link = (HyperLink)e.Item.FindControl("Link");
                HyperLink Title = (HyperLink)e.Item.FindControl("Title");
                Literal ShortDesc = (Literal)e.Item.FindControl("ShortDesc");
                HtmlContainerControl PostDesc = (HtmlContainerControl)e.Item.FindControl("PostDesc");

                PostDesc.Visible = this.ShowPostDescription;

                RssItem item = (RssItem)e.Item.DataItem;

                PublishDate.Text = item.PubDate;

                Link.NavigateUrl = item.Link;
                Title.Text = item.Title;
                Title.NavigateUrl = item.Link;

                string shortDesc = item.Description;

                if (item.Description.Length > 130)
                    shortDesc = item.Description.Substring(0, 120) + "...";

                ShortDesc.Text = shortDesc;
            }
        }
    }
}