using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Admin.Controls.Selectors.MultiSelectors
{
    public partial class MultiUserSelector : System.Web.UI.UserControl
    {
        private Dictionary<User, IEnumerable<Permission>> showUsersPermissions = new Dictionary<User, IEnumerable<Permission>>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UsersPermissions == null)
                UsersPermissions = new Dictionary<User, IEnumerable<Permission>>();
        }

        private Dictionary<User, IEnumerable<Permission>> UsersPermissions
        {
            get
            {
                return (Dictionary<User, IEnumerable<Permission>>)ViewState["UserPermissions"];
            }
            set
            {
                ViewState["UserPermissions"] = value;
            }
        }

        public Dictionary<User, IEnumerable<Permission>> GetSelectedUserPermissions()
        {
            return UsersPermissions;
        }

        public void SetSelectedUserPermissions(Dictionary<User, IEnumerable<Permission>> usersPermissions)
        {
            this.UsersPermissions = usersPermissions;
        }

        public void ShowUsersPermissions(Dictionary<User, IEnumerable<Permission>> showUsersPermissions)
        {
            this.showUsersPermissions = showUsersPermissions;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Bind();
        }

        private void Bind()
        {
            UserPermissionsSelector.SetUsersPermissions(showUsersPermissions);

            this.ItemList.DataSource = UsersPermissions;
            this.ItemList.DataBind();
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            KeyValuePair<User, IEnumerable<Permission>> selectedUserPermission = UsersPermissions.Where(i => i.Key.ID == long.Parse(ItemList.SelectedValue.ToString())).SingleOrDefault();

            if (selectedUserPermission.Key != null)
                UsersPermissions.Remove(selectedUserPermission.Key);
        }
    }
}