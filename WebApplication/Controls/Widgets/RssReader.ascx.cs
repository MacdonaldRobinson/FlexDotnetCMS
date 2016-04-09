using FrameworkLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebApplication.Controls.Widgets
{
    public partial class RssReader : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            var WebRequestHelper = new WebRequestHelper();
            WebRequestHelper.EnableCaching = true;
            WebRequestHelper.CacheDurationInSeconds = ((60 * 60) * 24);

            var rawReturn = WebRequestHelper.MakeWebRequest(RssUrl);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(rawReturn);

            var items = new ArrayList();

            var index = 0;
            foreach (XmlElement item in xmlDocument["rss"]["channel"].ChildNodes)
            {
                if (item.Name == "item")
                {
                    if ((index >= StartIndex) && (index < EndIndex))
                    {
                        items.Add(item);
                    }

                    index++;
                }
            }

            RssItems.DataSource = items;
            RssItems.DataBind();
        }

        public string RssUrl { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        protected void RssItems_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var dataItem = (XmlElement)e.Item.DataItem;

            var Header = (Literal)e.Item.FindControl("Header");
            var Description = (Literal)e.Item.FindControl("Description");
            var PostDate = (Literal)e.Item.FindControl("PostDate");

            var title = dataItem["title"].InnerText;
            var description = dataItem["description"].InnerText;

            var dateTimeString = StringHelper.FormatDateTime(DateTime.Parse(dataItem["pubDate"].InnerText));

            Header.Text = title;
            Description.Text = description;
            PostDate.Text = dateTimeString;
        }
    }
}