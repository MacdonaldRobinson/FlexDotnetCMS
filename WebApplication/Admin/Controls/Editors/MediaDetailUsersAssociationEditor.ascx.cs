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

            this.ItemList.DataSource = UsersMediasMapper.GetUsers(selectedItem.Media.UsersMedias);
            this.ItemList.DataBind();
        }

        private void HandleAdd()
        {
            EditPanel.Visible = true;
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);
            IEnumerable<UserMedia> UserMediaDetails = UserPermissionsSelector.GetUserMediaDetails();

            User User = UserPermissionsSelector.GetSelectedUser();
            IEnumerable<UserMedia> removeItems = selectedItem.Media.UsersMedias.Where(i => i.UserID == User.ID);

            foreach (UserMedia removeItem in removeItems)
                UsersMediasMapper.DeletePermanently(removeItem);

            foreach (UserMedia UserMediaDetail in UserMediaDetails)
            {
                selectedItem.Media.UsersMedias.Add(UserMediaDetail);
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

        private void HandleDelete(IEnumerable<UserMedia> selectedUserMediaDetails)
        {
            selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);
            Return obj = BaseMapper.GenerateReturn();
            foreach (UserMedia selectedUserMediaDetail in selectedUserMediaDetails)
            {
                UserMedia item = BaseMapper.GetObjectFromContext(selectedUserMediaDetail);
                selectedItem.Media.UsersMedias.Remove(selectedUserMediaDetail);
                obj = UsersMediasMapper.DeletePermanently(selectedUserMediaDetail);

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
                IEnumerable<UserMedia> selectedUserMediaDetails = new List<UserMedia>();

                if (ItemList.SelectedValue != null)
                    selectedUserMediaDetails = UsersMediasMapper.GetByUser(UsersMapper.GetByID(long.Parse(ItemList.SelectedValue.ToString())), selectedItem);

                HandleDelete(selectedUserMediaDetails);
            }
        }
    }
}