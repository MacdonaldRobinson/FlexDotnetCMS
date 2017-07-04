using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FrameworkLibrary
{
    public class RssHelper
    {
        public static RssItem GetRssItem(XmlNode rssFeedItem)
        {
            var titleObj = rssFeedItem.SelectSingleNode("title");
            var title = (titleObj != null) ? titleObj.InnerText : "";

            var descriptionObj = rssFeedItem.SelectSingleNode("description");
            var description = (descriptionObj != null) ? descriptionObj.InnerText : "";

            var linkObj = rssFeedItem.SelectSingleNode("link");
            var link = (linkObj != null) ? linkObj.InnerText : "";

            var authorObj = rssFeedItem.SelectSingleNode("author");
            var author = (authorObj != null) ? authorObj.InnerText : "";

            if (authorObj == null)
            {
                if (rssFeedItem.OwnerDocument != null)
                {
                    var nsMan = new XmlNamespaceManager(rssFeedItem.OwnerDocument.NameTable);
                    nsMan.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

                    var dcCreatorObj = rssFeedItem.SelectSingleNode("author", nsMan);
                    var dcCreator = (dcCreatorObj != null) ? dcCreatorObj.InnerText : "";

                    author = dcCreator;
                }
            }

            var pubDateObj = rssFeedItem.SelectSingleNode("pubDate");
            var pubDate = (pubDateObj != null) ? pubDateObj.InnerText : "";

            RssItem rssItem = new RssItem(title, description, link, author, DateTime.Parse(pubDate), rssFeedItem);
            return rssItem;
        }

        public static IEnumerable<RssItem> GetRssItems(string rssFeed, int count = 5)
        {
            try
            {
                if (!rssFeed.Contains("<"))
                    return new List<RssItem>();

                var rssItems = new List<RssItem>();

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rssFeed);

                var items = xmlDoc.GetElementsByTagName("item");

                int index = 0;
                foreach (XmlNode item in items)
                {
                    if (index >= count)
                        break;

                    rssItems.Add(GetRssItem(item));
                    index++;
                }

                return rssItems;
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);

                return new List<RssItem>();
            }
        }

        private static void WriteRssItem(RssItem rssItem, XmlTextWriter writer)
        {
            writer.WriteStartElement("item");
            writer.WriteElementString("title", rssItem.Title);
            writer.WriteElementString("link", URIHelper.ConvertToAbsUrl(rssItem.Link));

            string shortDesc = StringHelper.StripHtmlTags(rssItem.Description);

            if (shortDesc.Length > 255)
                shortDesc = shortDesc.Substring(0, 255) + " ...";

            writer.WriteElementString("description", shortDesc);
            writer.WriteElementString("author", rssItem.Author);
            writer.WriteElementString("pubDate", rssItem.PubDate);
            writer.WriteElementString("updated", rssItem.Updated);
            writer.WriteEndElement();
        }

        public static void WriteRss(Rss rss, Stream stream)
        {
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);

            writer.WriteStartDocument();
            writer.WriteStartElement("rss");
            writer.WriteAttributeString("version", "2.0");
            writer.WriteStartElement("channel");
            writer.WriteElementString("title", rss.Title);
            writer.WriteElementString("link", URIHelper.ConvertToAbsUrl(rss.Link));
            writer.WriteElementString("description", rss.Description);
            writer.WriteElementString("copyright", rss.Copyright);
            writer.WriteElementString("ttl", rss.TTL);

            foreach (RssItem item in rss.Items)
                WriteRssItem(item, writer);

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
            stream.Close();
        }
    }
}