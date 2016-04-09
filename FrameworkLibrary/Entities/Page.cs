using System;

namespace FrameworkLibrary
{
    public partial class Page : IMediaDetail
    {
        public new RssItem GetRssItem()
        {
            var rssItem = new RssItem(Title, GetMetaDescription(), AutoCalculatedVirtualPath, CreatedByUser.UserName, (DateTime)PublishDate, this);
            return rssItem;
        }

        public object ToLiquid()
        {
            return this;
        }

        public new Return Validate()
        {
            base.Validate();
            return GenerateValidationReturn();
        }
    }
}