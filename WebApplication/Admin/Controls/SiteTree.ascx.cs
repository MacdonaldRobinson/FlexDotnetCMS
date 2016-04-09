using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls
{
    public partial class SiteTree : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            var items = MediasMapper.GetDataModel().AllMedia.Where(i=>i.ParentMediaID == null).OrderBy(i=>i.OrderIndex);

            ListView.DataSource = items;            
            ListView.DataBind();
        }
        
        protected void ListView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var media = (Media)e.Item.DataItem;
            var link = (HyperLink)e.Item.FindControl("Link");
            var childListView = (ListView)e.Item.FindControl("ChildListView");

            var mediaDetail = MediaDetailsMapper.GetAtleastOneByMedia(media, AdminBasePage.CurrentLanguage);

            if (mediaDetail != null)
            {
                link.Attributes.Add("data-mediaDetailId", mediaDetail.ID.ToString());
                link.Text = mediaDetail.SectionTitle;
                link.NavigateUrl = AdminBasePage.GetRedirectToMediaDetailUrl(mediaDetail.MediaTypeID, mediaDetail.MediaID);

                if(!mediaDetail.ShowInMenu)
                    link.CssClass +=" isHidden";

                if (mediaDetail.IsDeleted)
                    link.CssClass += " isDeleted";

                if (!mediaDetail.IsPublished)
                    link.CssClass += " unPublished";

                if (mediaDetail.ID == AdminBasePage.SelectedMediaDetail.ID)
                {
                    link.CssClass += " selected";
                    link.Attributes.Add("data-jstree", "{\"opened\":true,\"selected\":true}");
                }

            }
            else
            {
                link.Text = "MediaID:"+media.ID+" Does not have a media detail";
            }

            if (childListView != null)
            {
                childListView.DataSource = media.ChildMedias.OrderBy(i=>i.OrderIndex);
                childListView.LayoutTemplate = ListView.LayoutTemplate;
                childListView.ItemTemplate = ListView.ItemTemplate;
                childListView.DataBind();
            }
        }

        public AdminBasePage AdminBasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }
    }
}