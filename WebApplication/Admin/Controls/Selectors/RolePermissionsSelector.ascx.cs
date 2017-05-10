using FrameworkLibrary;
using System;
using System.Collections.Generic;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class RolePermissionsSelector : System.Web.UI.UserControl
    {
        private IEnumerable<Permission> selectedPermissions = new List<Permission>();

        protected void Page_Load(object sender, EventArgs e)
        {
            RoleSelector.ComboBox.AutoPostBack = true;
            RoleSelector.ComboBox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Bind();
        }

        protected void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            if (RoleSelector.ComboBox.SelectedValue != "")
            {
                OnRoleSelectPanel.Visible = true;
                Role role = RolesMapper.GetByID(long.Parse(RoleSelector.ComboBox.SelectedValue));

                if (role == null)
                    return;

                PermissionsSelector.ShowPermissions(role.Permissions);

                if (AdminBasePage.SelectedMediaDetail != null)
                    PermissionsSelector.SetSelectedPermissions(RolesMediasMapper.GetRolePermissions(role, AdminBasePage.SelectedMediaDetail));
            }
        }

        public void SetRoles(IEnumerable<Role> roles)
        {
            RoleSelector.SetRoles(roles);
        }

        public void SetRole(Role role)
        {
            RoleSelector.SetSelectedRole(role);
        }

        public void SetPermissions(IEnumerable<Permission> permissions)
        {
            PermissionsSelector.SetSelectedPermissions(permissions);
        }

        public IEnumerable<RoleMedia> GetRoleMediaDetails()
        {
            Role role = GetSelectedRole();
            var selectedPermissions = PermissionsSelector.GetSelectedPermissions();
            var RoleMediaDetails = new List<RoleMedia>();

            foreach (Permission permission in selectedPermissions)
            {
                var RoleMediaDetail = new RoleMedia();
                RoleMediaDetail.DateCreated = RoleMediaDetail.DateLastModified = DateTime.Now;
                RoleMediaDetail.RoleID = role.ID;
                //RoleMediaDetail.PermissionID = permission.ID;
                RoleMediaDetail.MediaID = AdminBasePage.SelectedMedia.ID;

                RoleMediaDetails.Add(RoleMediaDetail);
            }

            return RoleMediaDetails;
        }

        public Role GetSelectedRole()
        {
            return RolesMapper.GetByID(long.Parse(RoleSelector.ComboBox.SelectedValue));
        }

        public IEnumerable<Permission> GetSelectedPermissions()
        {
            return PermissionsSelector.GetSelectedPermissions();
        }

        private AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }
    }
}