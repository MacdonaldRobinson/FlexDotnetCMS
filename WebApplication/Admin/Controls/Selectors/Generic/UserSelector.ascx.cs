using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class UserSelector : System.Web.UI.UserControl
    {
        private IEnumerable<User> allUsers = new List<User>();
        private User selectedUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            Bind();
        }

        public void Bind()
        {
            Items.DataSource = allUsers;
            Items.DataTextField = "UserName";
            Items.DataValueField = "ID";
            Items.DataBind();
        }

        public void SetSelectedUser(User selectedUser)
        {
            Items.SelectedValue = selectedUser.ID.ToString();
        }

        public void SetUsers(IEnumerable<User> Users)
        {
            allUsers = Users;
            Bind();
        }

        public User GetSelectedUser()
        {
            return UsersMapper.GetByID(long.Parse(Items.SelectedValue));
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