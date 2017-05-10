using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls
{
    public partial class MediaDetailRolesAssociationEditor : System.Web.UI.UserControl
    {
        private IMediaDetail selectedItem;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void SetItem(IMediaDetail item)
        {
            this.selectedItem = item;
            var mediaTypeRoles = MediaTypesMapper.GetByID(item.MediaTypeID).GetRoles();

            if (mediaTypeRoles.Count() > 0)
                RolePermissionsSelector.SetRoles(mediaTypeRoles);
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

            this.ItemList.DataSource = RolesMediasMapper.GetRoles(selectedItem.Media.RolesMedias);
            this.ItemList.DataBind();
        }

        private void HandleAdd()
        {
            EditPanel.Visible = true;
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);
            IEnumerable<RoleMedia> RoleMediaDetails = RolePermissionsSelector.GetRoleMediaDetails();

            Role role = RolePermissionsSelector.GetSelectedRole();
            IEnumerable<RoleMedia> removeItems = selectedItem.Media.RolesMedias.Where(i => i.RoleID == role.ID);

            foreach (RoleMedia removeItem in removeItems)
                RolesMediasMapper.DeletePermanently(removeItem);

            foreach (RoleMedia RoleMediaDetail in RoleMediaDetails)
            {
                selectedItem.Media.RolesMedias.Add(RoleMediaDetail);
            }

            Return obj = MediaDetailsMapper.Update(selectedItem);

            if (obj.IsError)
                BasePage.DisplayErrorMessage("Error assigning role", obj.Error);
            else
            {
                BasePage.DisplaySuccessMessage("Successfully assigned role");
                Bind();
                EditPanel.Visible = false;
            }
        }

        protected void Cancel_OnClick(object sender, EventArgs e)
        {
            this.EditPanel.Visible = false;
        }

        private void HandleDelete(IEnumerable<RoleMedia> RoleMediaDetails)
        {
            selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);
            Role role = null;
            Return obj = BaseMapper.GenerateReturn();

            foreach (RoleMedia RoleMediaDetail in RoleMediaDetails)
            {
                obj = RolesMediasMapper.DeletePermanently(RoleMediaDetail);
                role = RoleMediaDetail.Role;
            }

            if (!obj.IsError)
            {
                IEnumerable<UserMedia> userMediaDetails = selectedItem.Media.UsersMedias.Where(i => i.User.IsInRole(role));

                foreach (UserMedia userMediaDetail in userMediaDetails)
                    UsersMediasMapper.DeletePermanently(userMediaDetail);

                Bind();
            }

            if (obj.IsError)
                BasePage.DisplayErrorMessage("Error deleting association", obj.Error);
            else
            {
                BasePage.DisplaySuccessMessage("Successfully deleted association");
                Bind();
            }
        }

        public AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }

        protected void AddRole_Click(object sender, EventArgs e)
        {
            HandleAdd();
        }

        protected void DeleteRole_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            if (!string.IsNullOrEmpty(id))
            {
                var item = selectedItem.History.SingleOrDefault(i => i.ID == long.Parse(id));

                IEnumerable<RoleMedia> RoleMediaDetails = new List<RoleMedia>();

                if (ItemList.SelectedValue != null)
                {
                    Role role = RolesMapper.GetByID(long.Parse(ItemList.SelectedValue.ToString()));
                    RoleMediaDetails = RolesMediasMapper.GetByRole(role, selectedItem);
                }

                HandleDelete(RoleMediaDetails);
            }
        }
    }
}