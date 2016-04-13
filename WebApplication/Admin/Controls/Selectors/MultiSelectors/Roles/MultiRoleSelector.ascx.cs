using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class MultiRolesSelector : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Roles == null)
                Roles = new List<Role>();

            Bind();
        }

        private List<Role> Roles
        {
            get
            {
                return (List<Role>)Session["MultiRolesSelectorRoles"];
            }
            set
            {
                Session["MultiRolesSelectorRoles"] = value;
            }
        }

        public List<Role> GetSelectedRoles()
        {
            return Roles;
        }

        public void SetSelectedRoles(List<Role> roles)
        {
            Roles = roles;
            Bind();
        }

        private void Bind()
        {
            this.ItemList.DataSource = Roles;
            this.ItemList.DataBind();
        }

        protected void Add_OnClick(object sender, EventArgs e)
        {
            Role role = RoleSelector.GetSelectedRole();
            Roles.Add(role);

            Bind();
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            var roleId = long.Parse(((LinkButton)sender).CommandArgument);
            var role = Roles.FirstOrDefault(i => i.ID == roleId);

            Roles.Remove(role);

            Bind();
        }
    }
}