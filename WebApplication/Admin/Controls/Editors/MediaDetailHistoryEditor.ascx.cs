using FrameworkLibrary;
using System;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Editors
{
    public partial class MediaDetailHistoryEditor : System.Web.UI.UserControl
    {
        private IMediaDetail selectedItem;

        protected void Page_Init(object sender, EventArgs e)
        {
            Bind();
        }

        public void SetItem(IMediaDetail item)
        {
            selectedItem = item;
        }

        protected override void OnPreRender(EventArgs e)
        {
            Bind();
            base.OnPreRender(e);
        }

        private void Bind()
        {
            if (selectedItem == null)
                return;

            this.ItemList.DataSource = selectedItem.History.OrderByDescending(i => i.HistoryVersionNumber).ToList();
            this.ItemList.DataBind();
        }

        private void HandleLoad(IMediaDetail item)
        {
            WebApplication.BasePage.RedirectToMediaDetail(item, item.HistoryVersionNumber);
        }

        private void HandleDelete(IMediaDetail item)
        {
            Return returnObj = MediaDetailsMapper.DeletePermanently((MediaDetail)item);

            if (returnObj.IsError)
                BasePage.DisplayErrorMessage("Error deleting association", returnObj.Error);
            else
            {
                BasePage.DisplaySuccessMessage("Successfully deleted association");
                selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);

                Bind();
            }
        }

        /*protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            ItemList.Rebind();
        }*/

        public AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }

        /*protected void ItemList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            var item = (IMediaDetail)e.Item.DataItem;

            if (item == null)
                return;

            switch (e.CommandName)
            {
                case "LoadHistory":
                    break;

                case "DeleteHistory":
                    break;
            }
        }*/

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        /*protected void ItemList_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            var sortDirection = ((e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending) ? "ASC" : "DESC");
            selectedItem.History = selectedItem.History.OrderBy(e.SortExpression + " " + sortDirection).ToList();
            Bind();
        }*/


        public IMediaDetail GetDataItemFromSender(Control sender)
        {
            var dataItemIndex = ((ItemList.PageSize * ItemList.PageIndex) + ((GridViewRow)(sender).Parent.Parent).DataItemIndex);
            var dataItem = ((System.Collections.Generic.List<FrameworkLibrary.MediaDetail>)ItemList.DataSource).ElementAt(dataItemIndex);

            return dataItem;
        }

        protected void LoadHistory_Click(object sender, EventArgs e)
        {
            var dataItem = GetDataItemFromSender((LinkButton)sender);

            if (dataItem != null)
            {
                HandleLoad(dataItem);
            }
        }

        protected void DeleteHistory_Click(object sender, EventArgs e)
        {
            var dataItem = GetDataItemFromSender((LinkButton)sender);

            if (dataItem != null)
            {
                HandleDelete(dataItem);
            }
        }
    }
}