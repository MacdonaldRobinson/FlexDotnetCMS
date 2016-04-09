using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkLibrary;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class MultiRolesSelector : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RolePermissions == null)
                RolePermissions = new Dictionary<Role, List<Permission>>();

        }

        private Dictionary<Role, List<Permission>> RolePermissions
        {
            get
            {
                return (Dictionary<Role, List<Permission>>)ViewState["RolePermissions"];
            }
            set
            {
                ViewState["RolePermissions"] = value;
            }
        }

        public Dictionary<Role, List<Permission>> GetSelectedRolePermissions()
        {
            return RolePermissions;
        }

        public void SetRolePermissions(Dictionary<Role, List<Permission>> rolesPermissions)
        {
            RolePermissions = rolesPermissions;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Bind();
        }

        private void Bind()
        {
            this.ItemList.DataSource = RolePermissions;
            this.ItemList.DataBind();
        }

        protected void ItemList_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (RolePermissions == null)
                return;

            RadGrid itemList = (RadGrid)sender;
            ItemList.DataSource = RolePermissions.Skip(itemList.CurrentPageIndex * itemList.PageSize).Take(itemList.PageSize);
            ItemList.VirtualItemCount = RolePermissions.Count();
        }

        protected void MediaGridToolbar_OnButtonClick(object sender, RadToolBarEventArgs e)
        {
            string commandName = ((RadToolBarButton)e.Item).CommandName;

            switch (commandName)
            {
                case "Delete":
                    KeyValuePair<Role, List<Permission>> selectedRolePermission = RolePermissions.Where(i => i.Key.ID == long.Parse(ItemList.SelectedValue.ToString())).SingleOrDefault();

                    if (selectedRolePermission.Key != null)
                        RolePermissions.Remove(selectedRolePermission.Key);
                    break;
            }
        }

        protected void Add_OnClick(object sender, EventArgs e)
        {
            Role addRole = RolePermissionsSelector.GetSelectedRole();

            if (RolePermissions.Where(i => i.Key.ID == addRole.ID).Count() == 0)
                RolePermissions.Add(addRole, RolePermissionsSelector.GetSelectedPermissions());
        }
    }
}