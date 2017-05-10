using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class UserPermissionsSelector : System.Web.UI.UserControl
    {
        private Dictionary<User, IEnumerable<Permission>> UsersPermissions
        {
            get
            {
                return (Dictionary<User, IEnumerable<Permission>>)ViewState["UsersPermissions"];
            }
            set
            {
                ViewState["UsersPermissions"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserSelector.ComboBox.AutoPostBack = true;
            UserSelector.ComboBox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
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
            if (UserSelector.ComboBox.SelectedValue != "")
            {
                OnUserSelectPanel.Visible = true;
                User User = UsersMapper.GetByID(long.Parse(UserSelector.ComboBox.SelectedValue));

                if (User == null)
                    return;

                SetUser(User);

                if (AdminBasePage.SelectedMediaDetail != null)
                    PermissionsSelector.SetSelectedPermissions(UsersMediasMapper.GetUserPermissions(User, AdminBasePage.SelectedMediaDetail));
            }
        }

        public void SetUsersPermissions(Dictionary<User, IEnumerable<Permission>> usersPermissions)
        {
            this.UsersPermissions = usersPermissions;
            UserSelector.SetUsers(usersPermissions.Keys);
        }

        public void SetUser(User user)
        {
            UserSelector.SetSelectedUser(user);
            PermissionsSelector.ShowPermissions(UsersPermissions.Where(i => i.Key.ID == user.ID).Single().Value);
        }

        public void ShowPermissions(IEnumerable<Permission> permissions)
        {
            PermissionsSelector.SetSelectedPermissions(permissions);
        }

        public IEnumerable<UserMedia> GetUserMediaDetails()
        {
            User User = GetSelectedUser();
            var selectedPermissions = PermissionsSelector.GetSelectedPermissions();
            var UserMediaDetails = new List<UserMedia>();

            foreach (Permission permission in selectedPermissions)
            {
                var UserMediaDetail = new UserMedia();
                UserMediaDetail.DateCreated = UserMediaDetail.DateLastModified = DateTime.Now;
                UserMediaDetail.UserID = User.ID;
                UserMediaDetail.PermissionID = permission.ID;
                UserMediaDetail.MediaID = AdminBasePage.SelectedMedia.ID;

                UserMediaDetails.Add(UserMediaDetail);
            }

            return UserMediaDetails;
        }

        public User GetSelectedUser()
        {
            return UsersMapper.GetByID(long.Parse(UserSelector.ComboBox.SelectedValue));
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