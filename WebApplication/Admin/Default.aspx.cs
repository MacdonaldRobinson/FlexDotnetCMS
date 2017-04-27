using FrameworkLibrary;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace WebApplication.Admin
{
    public partial class Default : AdminBasePage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            RecentEditsList.DataSource = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0).OrderByDescending(i => i.DateLastModified).Take(5).ToList();
            RecentEditsList.DataBind();
        }

        protected void RecentEditsList_ItemDataBound(object sender, System.Web.UI.WebControls.ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var dataItem = (IMediaDetail)e.Item.DataItem;
            var HyperLink = (HtmlAnchor)e.Item.FindControl("HyperLink");

            HyperLink.HRef = GetAdminUrl(dataItem.MediaTypeID, dataItem.MediaID);
        }
    }
}