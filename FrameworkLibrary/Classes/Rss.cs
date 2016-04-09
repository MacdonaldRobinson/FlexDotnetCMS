using System.Collections.Generic;

namespace FrameworkLibrary
{
    public class Rss
    {
        private IEnumerable<RssItem> items = new List<RssItem>();
        private string title = "";
        private string link = "";
        private string description = "";
        private string copyright = "";
        private string ttl = "5";
        private string version = "2.0";
        private string encoding = "utf-8";
        private string xmlVersion = "1.0";

        public Rss(string title, string link, string description, string copyright = "", string ttl = "5", string version = "2.0", string encoding = "utf-8", string xmlVersion = "1.0")
        {
            this.title = title;
            this.link = link;
            this.description = description;
            this.copyright = copyright;
            this.ttl = ttl;
            this.version = version;
            this.encoding = encoding;
            this.xmlVersion = xmlVersion;
        }

        public IEnumerable<RssItem> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public string Link
        {
            get
            {
                return link;
            }
            set
            {
                link = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public string Copyright
        {
            get
            {
                return copyright;
            }
            set
            {
                copyright = value;
            }
        }

        public string TTL
        {
            get
            {
                return ttl;
            }
            set
            {
                ttl = value;
            }
        }

        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }

        public string Encoding
        {
            get
            {
                return encoding;
            }
            set
            {
                encoding = value;
            }
        }

        public string XmlVersion
        {
            get
            {
                return xmlVersion;
            }
            set
            {
                xmlVersion = value;
            }
        }
    }
}