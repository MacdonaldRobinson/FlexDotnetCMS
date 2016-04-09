using FrameworkLibrary;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.BootStrap
{
    public partial class BootStrapTabbedLayout : System.Web.UI.UserControl
    {
        public BootStrapTabbedLayout()
        {
            DisplayLinkToChildDetailsPage = true;
        }

        public enum Mode
        {
            CurrentChildItems,
            ChildItems
        }

        public void SetItems(IEnumerable<IMediaDetail> mediaDetails)
        {
            NavTabs.DataSource = mediaDetails;
            NavTabs.DataBind();

            NavTabsContents.DataSource = mediaDetails;
            NavTabsContents.DataBind();
        }

        public void SetParentItem(IMediaDetail mediaDetail)
        {
            SetItems(MediaDetailsMapper.GetAllChildMediaDetails(mediaDetail.Media, BasePage.CurrentLanguage));
        }

        public Mode RenderMode
        {
            set
            {
                if (value == Mode.CurrentChildItems)
                {
                    SetItems(MediaDetailsMapper.GetAllChildMediaDetails(BasePage.CurrentMedia, BasePage.CurrentLanguage));
                }
            }
        }

        public bool DisplayLinkToChildDetailsPage { get; set; }

        public FrontEndBasePage BasePage
        {
            get { return (FrontEndBasePage)this.Page; }
        }

        protected void NavTabsContents_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var linkToDetailsPage = (Control)e.Item.FindControl("LinkToDetailsPage");

            if (!DisplayLinkToChildDetailsPage)
                linkToDetailsPage.Visible = false;
        }
    }
}