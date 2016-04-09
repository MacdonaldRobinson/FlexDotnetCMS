using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Editors
{
    public partial class MediaDetailUsersAssociationEditor : System.Web.UI.UserControl
    {
        private IMediaDetail selectedItem;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void SetItem(IMediaDetail item)
        {
            this.selectedItem = item;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Bind();
        }

        private void Bind()
        {
            if (selectedItem == null)
                return;

            this.ItemList.DataSource = UsersMediaDetailsMapper.GetUsers(selectedItem.UsersMediaDetails);
            this.ItemList.DataBind();
        }

        private void HandleAdd()
        {
            EditPanel.Visible = true;
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);
            IEnumerable<UserMediaDetail> UserMediaDetails = UserPermissionsSelector.GetUserMediaDetails();

            User User = UserPermissionsSelector.GetSelectedUser();
            IEnumerable<UserMediaDetail> removeItems = selectedItem.UsersMediaDetails.Where(i => i.UserID == User.ID);

            foreach (UserMediaDetail removeItem in removeItems)
                UsersMediaDetailsMapper.DeletePermanently(removeItem);

            foreach (UserMediaDetail UserMediaDetail in UserMediaDetails)
            {
                selectedItem.UsersMediaDetails.Add(UserMediaDetail);
            }

            Return obj = MediaDetailsMapper.Update(selectedItem);

            if (obj.IsError)
                BasePage.DisplayErrorMessage("Error assigning User", obj.Error);
            else
            {
                BasePage.DisplaySuccessMessage("Successfully assigned User");
                Bind();
                EditPanel.Visible = false;
            }
        }

        protected void Cancel_OnClick(object sender, EventArgs e)
        {
            this.EditPanel.Visible = false;
        }

        private void HandleDelete(IEnumerable<UserMediaDetail> selectedUserMediaDetails)
        {
            selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);
            Return obj = BaseMapper.GenerateReturn();
            foreach (UserMediaDetail selectedUserMediaDetail in selectedUserMediaDetails)
            {
                UserMediaDetail item = BaseMapper.GetObjectFromContext(selectedUserMediaDetail);
                selectedItem.UsersMediaDetails.Remove(selectedUserMediaDetail);
                obj = UsersMediaDetailsMapper.DeletePermanently(selectedUserMediaDetail);

                if (obj.IsError)
                    break;
            }

            if (obj.IsError)
                BasePage.DisplayErrorMessage("Error deleting association", obj.Error);
            else
            {
                BasePage.DisplaySuccessMessage("Successfully deleted association");
                Bind();
            }
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        public AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }

        protected void AddUser_Click(object sender, EventArgs e)
        {
            HandleAdd();
        }

        protected void DeleteUser_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            if (!string.IsNullOrEmpty(id))
            {
                IEnumerable<UserMediaDetail> selectedUserMediaDetails = new List<UserMediaDetail>();

                if (ItemList.SelectedValue != null)
                    selectedUserMediaDetails = UsersMediaDetailsMapper.GetByUser(UsersMapper.GetByID(long.Parse(ItemList.SelectedValue.ToString())), selectedItem);

                HandleDelete(selectedUserMediaDetails);
            }
        }
    }
}