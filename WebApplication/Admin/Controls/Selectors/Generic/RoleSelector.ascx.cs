using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls
{
    public partial class RolesSelector : System.Web.UI.UserControl
    {
        private IEnumerable<Role> rolesToRemove = new List<Role>();
        private IEnumerable<Role> allRoles = new List<Role>();
        private Role selectedRole = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (allRoles.Count() == 0)
                allRoles = RolesMapper.RemoveRoles(RolesMapper.GetAll(), rolesToRemove);

            Bind();
        }

        public void SetRolesToRemove(IEnumerable<Role> rolesToRemove)
        {
            this.rolesToRemove = rolesToRemove;
        }

        public void Bind()
        {
            Items.DataSource = allRoles;
            Items.DataTextField = "Name";
            Items.DataValueField = "ID";
            Items.DataBind();
        }

        public void SetSelectedRole(Role selectedRole)
        {
            Items.SelectedValue = selectedRole.ID.ToString();
        }

        public void SetRoles(IEnumerable<Role> roles)
        {
            allRoles = roles;
            Bind();
        }

        public Role GetSelectedRole()
        {
            return RolesMapper.GetByID(long.Parse(Items.SelectedValue));
        }

        public DropDownList ComboBox
        {
            get
            {
                return Items;
            }
        }
    }
}