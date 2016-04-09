using FrameworkLibrary;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Controls
{
    public partial class RelatedItems : System.Web.UI.UserControl
    {
        private IMediaDetail item;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void SetItem(IMediaDetail item)
        {
            this.item = item;
            Bind();
        }

        private void Bind()
        {
            if (item == null)
                item = BasePage.CurrentMediaDetail;

            RelatedItemsList.DataSource = MediaDetailsMapper.GetRelatedItems(item).ToList();
            RelatedItemsList.DataBind();
        }

        protected void RelatedItems_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            IMediaDetail DataItem = (IMediaDetail)e.Item.DataItem;

            HyperLink Title = (HyperLink)e.Item.FindControl("Title");
            Literal ShortDesc = (Literal)e.Item.FindControl("ShortDesc");

            Title.Text = DataItem.Title;
            Title.NavigateUrl = DataItem.VirtualPath;
            ShortDesc.Text = DataItem.ShortDescription;
        }

        public BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}