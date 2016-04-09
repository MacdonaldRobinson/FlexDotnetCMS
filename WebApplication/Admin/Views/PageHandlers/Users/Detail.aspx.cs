using FrameworkLibrary;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Views.Users
{
    public partial class Detail : AdvanceOptionsBasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            BindRolesList();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (SelectedItem != null)
            {
                if (!IsPostBack)
                    UpdateFieldsFromObject();
            }

            this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
        }

        public User SelectedItem
        {
            get
            {
                var user = (User)ContextHelper.GetFromRequestContext("EditSelectedItem");

                if (user == null)
                {
                    long id;

                    if (long.TryParse(Request["id"], out id))
                    {
                        user = UsersMapper.GetByID(id);
                        ContextHelper.SetToRequestContext("EditSelectedItem", user);
                    }
                }

                return user;
            }
            set
            {
                ContextHelper.SetToRequestContext("EditSelectedItem", value);
            }
        }

        private void BindRolesList()
        {
            RolesList.DataSource = RolesMapper.GetAll().ToList();
            RolesList.DataTextField = "Name";
            RolesList.DataValueField = "ID";
            RolesList.DataBind();
        }

        private string GetSectionTitle()
        {
            if (SelectedItem == null)
                return "New User";
            else
                return "Editing User: " + SelectedItem.UserName;
        }

        private void UpdateObjectFromFields()
        {
            SelectedItem.UserName = Username.Text;

            var password = Password.Text.Trim();
            var encryptedPassword = StringHelper.Encrypt(password);

            if (password != "")
            {                         
                SelectedItem.Password = encryptedPassword;
            }

            SelectedItem.EmailAddress = EmailAddress.Text;
            SelectedItem.IsActive = IsActive.Checked;
            SelectedItem.ProfilePhoto = ProfilePhoto.GetValue().ToString();
            SelectedItem.FirstName = FirstName.Text;
            SelectedItem.LastName = LastName.Text;
            SelectedItem.AfterLoginStartPage = AfterLoginStartPage.Text;

            SelectedItem.Roles.Clear();

            foreach (ListItem item in RolesList.Items)
            {
                if (item.Selected)
                    SelectedItem.Roles.Add(BaseMapper.GetObjectFromContext<Role>(RolesMapper.GetByID(long.Parse(item.Value))));
            }
        }

        private void UpdateFieldsFromObject()
        {
            Username.Text = SelectedItem.UserName;            
            //Password.Text = SelectedItem.Password;
            EmailAddress.Text = SelectedItem.EmailAddress;
            IsActive.Checked = SelectedItem.IsActive;
            ProfilePhoto.SetValue(SelectedItem.ProfilePhoto);
            FirstName.Text = SelectedItem.FirstName;
            LastName.Text = SelectedItem.LastName;
            AfterLoginStartPage.Text = SelectedItem.AfterLoginStartPage;

            foreach (Role userRole in SelectedItem.Roles)
                RolesList.Items.FindByValue(userRole.ID.ToString()).Selected = true;
        }

        protected void GetGravatarUrl_OnClick(object sender, EventArgs e)
        {
            ProfilePhoto.SetValue(ImageHelper.GetGravatarImageURL(EmailAddress.Text, 50));
        }

        protected void GetIdenticonUrl_OnClick(object sender, EventArgs e)
        {
            ProfilePhoto.SetValue(ImageHelper.GetGravatarImageURL(EmailAddress.Text, 50, true));
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (SelectedItem == null)
                SelectedItem = UsersMapper.CreateObject();
            else
                SelectedItem = BaseMapper.GetObjectFromContext<User>(SelectedItem);

            SelectedItem.AuthenticationType = AuthType.Forms.ToString();

            UpdateObjectFromFields();

            Return returnObj = SelectedItem.Validate();

            if (!returnObj.IsError)
            {
                if (SelectedItem.ID == 0)
                    returnObj = UsersMapper.Insert(SelectedItem);
                else
                    returnObj = UsersMapper.Update(SelectedItem);
            }

            if (returnObj.IsError)
                DisplayErrorMessage("Error Saving Item", returnObj.Error);
            else
                DisplaySuccessMessage("Successfully Saved Item");
        }
    }
}