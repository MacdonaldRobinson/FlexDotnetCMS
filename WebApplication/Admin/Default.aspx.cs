using FrameworkLibrary;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
            RecentEditsList.DataSource = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0 && i.MediaType.ShowInSiteTree).OrderByDescending(i => i.DateLastModified).Take(20).ToList();
            RecentEditsList.DataBind();
        }

        protected void RecentEditsList_DataBound(object sender, EventArgs e)
        {
            RecentEditsList.UseAccessibleHeader = true;
            if (RecentEditsList.HeaderRow != null)
            {
                RecentEditsList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
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