using System;

namespace FrameworkLibrary
{
    public class RssItem
    {
        private string title;
        private string link;
        private string author;
        private string description;
        private DateTime pubDate;
        private object objectReference;

        public RssItem(string title, string description, string link, string author, DateTime pubDate, object objectReference)
        {
            this.title = title;
            this.author = author;
            this.description = description;
            this.link = link;
            this.pubDate = pubDate;
            this.objectReference = objectReference;
        }

        public string Title
        {
            get
            {
                return title;
            }
        }

        public string Author
        {
            get
            {
                return author;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public string Link
        {
            get
            {
                return link;
            }
        }

        public object ObjectReference
        {
            get
            {
                return objectReference;
            }
        }

        public string PubDate
        {
            get
            {
                return pubDate.ToString("r");
            }
        }

        public string PubDateISO8601
        {
            get
            {
                return pubDate.ToString("yyyy-MM-ddTHH:mm:ss");
            }
        }
    }
}