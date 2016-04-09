using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class MultiRolesSelector : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (RolePermissions == null)
                RolePermissions = new Dictionary<Role, IEnumerable<Permission>>();

            Bind();
        }

        private Dictionary<Role, IEnumerable<Permission>> RolePermissions
        {
            get
            {
                return (Dictionary<Role, IEnumerable<Permission>>)ViewState["MultiRolesSelectorRolePermissions"];
            }
            set
            {
                ViewState["MultiRolesSelectorRolePermissions"] = value;
            }
        }

        public Dictionary<Role, IEnumerable<Permission>> GetSelectedRolePermissions()
        {
            return RolePermissions;
        }

        public void SetRolePermissions(Dictionary<Role, IEnumerable<Permission>> rolesPermissions)
        {
            RolePermissions = rolesPermissions;
        }

        private void Bind()
        {
            this.ItemList.DataSource = RolePermissions;
            this.ItemList.DataBind();

            Dictionary<User, IEnumerable<Permission>> usersPermissions = new Dictionary<User, IEnumerable<Permission>>();

            foreach (KeyValuePair<Role, IEnumerable<Permission>> rolePermissions in RolePermissions)
            {
                foreach (User user in rolePermissions.Key.Users)
                {
                    if (usersPermissions.Keys.Where(i => i.ID == user.ID).Count() == 0)
                        usersPermissions.Add(user, rolePermissions.Value);
                }
            }

            //MultiUserSelector.ShowUsersPermissions(usersPermissions);
        }

        protected void Add_OnClick(object sender, EventArgs e)
        {
            Role addRole = RolePermissionsSelector.GetSelectedRole();

            IEnumerable<KeyValuePair<Role, IEnumerable<Permission>>> rolesPermissions = RolePermissions.Where(i => i.Key.ID == addRole.ID).ToList();

            foreach (KeyValuePair<Role, IEnumerable<Permission>> rolePermissions in rolesPermissions)
                RolePermissions.Remove(rolePermissions.Key);

            RolePermissions.Add(addRole, RolePermissionsSelector.GetSelectedPermissions());
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }
    }
}