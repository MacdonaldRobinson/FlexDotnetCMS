using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication.Controls.EmailTemplates.CreateAccount;

namespace WebApplication.Views.PageHandlers
{
    public partial class CreateAnAccount : FrontEndBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Create_OnClick(object sender, EventArgs e)
        {
            var newUser = UsersMapper.CreateObject();
            newUser.UserName = EmailAddress.Text;
            newUser.EmailAddress = EmailAddress.Text;
            newUser.Password = Password.Text;
            newUser.AuthenticationType = AuthType.Forms.ToString();
            newUser.IsActive = true;

            /*var role = RoleEnum.FrontEndUser;
            RoleEnum.TryParse(Category.Text, out role);

            newUser.Roles.Add(BaseMapper.GetObjectFromContext(RolesMapper.GetByEnum(role)));*/

            var returnObj = newUser.Validate();
            var userExists = UsersMapper.GetByEmailAddress(newUser.EmailAddress);

            if (userExists != null)
            {
                returnObj.Error = ErrorHelper.CreateError("Validation Error", "An account with the same email address already exists, <a href=" + URIHelper.BaseUrl + "login>Click Here</a> to login or retrieve your password");
            }

            if (!returnObj.IsError)
                returnObj = UsersMapper.Insert(newUser);

            if (returnObj.IsError)
            {
                Message.Text = returnObj.Error.Exception.Message;

                if ((returnObj.Error.Exception.InnerException.Message != null) && (returnObj.Error.Exception.InnerException.Message != ""))
                    Message.Text = returnObj.Error.Exception.InnerException.Message;
            }
            else
            {
                var returnObjAutoResponder = SendAutoResponderEmail(newUser);
                var returnObjNotification = SendNotificationEmails(newUser);
                Response.Redirect(URIHelper.GetCurrentVirtualPath() + "thank-you/");
            }
        }

        private Return SendAutoResponderEmail(User user)
        {
            var toEmails = new List<MailAddress>();
            toEmails.Add(new MailAddress(user.EmailAddress));

            return EmailHelper.SendTemplate(toEmails, "Thank you for creating a account at " + URIHelper.BaseUrl,
                                            AppSettings.SystemName, AppSettings.SystemEmailAddress,
                                            "~/Controls/EmailTemplates/CreateAccount/AutoResponder.ascx", user, AutoResponder.AutoResponderMode.NewAccount);
        }

        private Return SendNotificationEmails(User user)
        {
            var admins = UsersMapper.GetAllByRole(RolesMapper.GetByEnum(RoleEnum.Administrator));
            var adminEmails = new List<MailAddress>();

            foreach (var admin in admins)
                adminEmails.Add(new MailAddress(admin.EmailAddress));

            return EmailHelper.SendTemplate(adminEmails, "A new account was created at " + URIHelper.BaseUrl,
                                            AppSettings.SystemName, AppSettings.SystemEmailAddress,
                                            "~/Controls/EmailTemplates/CreateAccount/Notification.ascx", user);
        }

        public BasePage BasePage
        {
            get { return (BasePage)this.Page; }
        }
    }
}