using FrameworkLibrary;
using System;

namespace WebApplication.Controls
{
    public partial class ToolTipFromIMediaDetail : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void SetItem(IMediaDetail mediaDetail)
        {
            if (mediaDetail == null)
                return;

            Link.NavigateUrl = URIHelper.ConvertToAbsUrl(mediaDetail.AutoCalculatedVirtualPath);

            var title = mediaDetail.SectionTitle;
            var shortDescription = mediaDetail.ShortDescription;

            var maxCharsTitle = 40;
            var maxCharsShortDescription = 116;

            if (title.Length > maxCharsTitle)
                title = title.Substring(0, maxCharsTitle);

            if (shortDescription.Length > maxCharsShortDescription)
                shortDescription = shortDescription.Substring(0, maxCharsShortDescription) + " ...";

            Link.Attributes["title"] = "<span class='toolTipTitle'>" + title + "</span><br />" + shortDescription;
        }

        /*public string VirtualPath
        {
            set
            {
                var absVirtualPath = value.Replace("~/", FrameworkSettings.RootMediaDetail.VirtualPath);
                var mediaDetail = MediaDetailsMapper.GetByVirtualPath(absVirtualPath, false);
                SetItem(mediaDetail);
            }
        }*/

        public string CssClass
        {
            set { Link.CssClass = value; }
        }
    }
}