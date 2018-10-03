using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web.Security;
using System.Web.UI.WebControls;
using WebApplication.Controls.EmailTemplates.CreateAccount;
namespace WebApplication.Controls
{
    public partial class Login : System.Web.UI.UserControl
    {

		public enum ViewMode
		{
			Reset,
			Forgot,
			Login
		}

		private ViewMode Mode
		{
			get
			{
				if (string.IsNullOrEmpty(StoredMode.Value))
					return ViewMode.Login;

				return (ViewMode)Enum.Parse(typeof(ViewMode), StoredMode.Value);
			}
			set
			{
				StoredMode.Value = value.ToString();

				switch (value)
				{
					case ViewMode.Forgot:
						ForgotMyPasswordPanel.Visible = true;

						LoginPanel.Visible = false;
						ResetPasswordPanel.Visible = false;
					break;
					case ViewMode.Reset:
						ResetPasswordPanel.Visible = true;

						ForgotMyPasswordPanel.Visible = false;
						LoginPanel.Visible = false;
					break;

					case ViewMode.Login:
					default:
						LoginPanel.Visible = true;

						ForgotMyPasswordPanel.Visible = false;
						ResetPasswordPanel.Visible = false;
					break;

				}
			}
		}

        protected void Page_Init(object sender, EventArgs e)
        {
            ErrorPanel.Visible = false;

			if (Request["mode"] != null && Request["mode"].ToLower() == "reset")
			{
				Mode = ViewMode.Reset;
			}
		}

        protected void LoginButton_Click(object sender, EventArgs e)
        {
			var user = UsersMapper.GetUserByCredentials(Username.Text, Password.Text);

            if(user != null)
            {                
                FormsAuthentication.SetAuthCookie(user.UserName, false);

                FrameworkSettings.CurrentUser = user;

                var returnUrl = (Request["ReturnUrl"] != null) ? Request["ReturnUrl"] : "~/admin/";

                Response.Redirect(returnUrl);
            }
            else
            {
                ErrorPanel.Visible = true;
            }
        }	

		protected void ForgotMyPassword_Click(object sender, EventArgs e)
		{
			Mode = ViewMode.Forgot;
			ForgotMyPasswordPanel.Visible = true;
		}

		protected void ForgotPasswordSend_Click(object sender, EventArgs e)
		{			
			Mode = ViewMode.Forgot;

			if (string.IsNullOrEmpty(EmailAddress.Text))
				return;

			var user = UsersMapper.GetByEmailAddress(EmailAddress.Text);

			if (user != null)
			{
				user.ResetCode = System.Web.Security.Membership.GeneratePassword(5, 0);
				user.ResetCodeIssueDate = DateTime.Now;

				var returnObj = UsersMapper.Update(user);

				if (!returnObj.IsError)
				{
					returnObj = EmailHelper.Send(AppSettings.SystemEmailAddress, EmailHelper.GetMailAddressesFromString(user.EmailAddress), "Password reset", $"We just recieved a password reset request, please click the following link to reset your password: {URIHelper.GetCurrentVirtualPath(true)}?mode=reset&email={user.EmailAddress}, please use the reset code: {user.ResetCode}");

					if (!returnObj.IsError)
					{
						ServerMessage.Text = $"<div class='alert alert-success' role='alert'>An email has been sent to: {user.EmailAddress}</div>";
					}
					else
					{
						ServerMessage.Text = $"<div class='alert alert-danger' role='alert'>Error sending email{returnObj.Error.Message}</div>";
					}
				}
				else
				{
					ServerMessage.Text = $"<div class='alert alert-danger' role='alert'>Error sending email{returnObj.Error.Message}</div>";
				}
			}
			else
			{
				ServerMessage.Text = $"<div class='alert alert-danger' role='alert'>Cannot find an account with the email address: {EmailAddress.Text}</div>";
			}
		}

		protected void ResetPassword_Click(object sender, EventArgs e)
		{
			Mode = ViewMode.Reset;

			var email = Request["email"].ToString();
			var user = UsersMapper.GetByEmailAddress(email);

			if (user == null)
			{
				ResetServerMessage.Text = $"<div class='alert alert-danger' role='alert'>Cannot find an account for email address: {email}</div>";
				return;
			}

			if (user.ResetCode != ResetCode.Text || user.ResetCodeIssueDate == null || ((DateTime)user.ResetCodeIssueDate).AddDays(1) < DateTime.Now)
			{
				ResetServerMessage.Text = $"<div class='alert alert-danger' role='alert'>The Reset Code you entered is incorrect or has expired</div>";
				return;
			}

			var returnObj = user.SetPassword(NewPassword.Text);

			if (!returnObj.IsError)
			{
				ResetServerMessage.Text = $"<div class='alert alert-success' role='alert'>Password for email address '{user.EmailAddress}' has been reset</div>";
				EmailHelper.Send(AppSettings.SystemEmailAddress, EmailHelper.GetMailAddressesFromString(user.EmailAddress), "Password has been reset", "Your password was successfully reset for: " + URIHelper.BaseUrl);

				user.ResetCode = "";
				returnObj = UsersMapper.Update(user);

			}
			else
			{
				ResetServerMessage.Text = $"Error resetting password: { returnObj.Error.Exception.Message }";
			}

		}

		protected void BackToLoginScreen_Click(object sender, EventArgs e)
		{
			Mode = ViewMode.Login;
		}
	}
}