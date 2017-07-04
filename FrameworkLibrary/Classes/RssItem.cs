using System;

namespace FrameworkLibrary
{
    public class RssItem
    {
        public RssItem(string title, string description, string link, string author, DateTime pubDate, object objectReference)
        {
            this.Title = title;
            this.Author = author;
            this.Description = description;
            this.Link = link;
            this.PubDate = pubDate.ToString("r");
            this.ObjectReference = objectReference;
        }

        public RssItem(IMediaDetail mediaDetail)
        {
            this.Title = mediaDetail.SectionTitle;
            this.Author = mediaDetail.CreatedByUser.UserName;
            this.Description = mediaDetail.ShortDescription;
            this.Link = mediaDetail.AbsoluteUrl;
            this.PubDate = ((DateTime)mediaDetail.PublishDate).ToString("r");
            this.ObjectReference = mediaDetail;
            this.Updated = mediaDetail.DateLastModified.ToString("r");
        }

        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Description { get; private set; }
        public string Link { get; private set; }
        public object ObjectReference { get; private set; }
        public string PubDate { get; private set; }
        public string Updated { get; private set; }
    }
}