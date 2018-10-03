using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication.Controls.EmailTemplates.CreateAccount;

namespace WebApplication.Controls
{
	public partial class CreateAnAccount : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		private bool IsFieldValueValid(string fieldValue)
		{
			if (string.IsNullOrWhiteSpace(fieldValue))
				return false;

			return true;
		}
		private bool IsValid(User user)
		{
			if (!IsFieldValueValid(user.FirstName)
				|| !IsFieldValueValid(user.Password)
				|| !IsFieldValueValid(user.EmailAddress)				
			)
			{
				return false;
			}

			return true;
		}

		protected void Signup_Click(object sender, EventArgs e)
		{
			var returnObj = BaseMapper.GenerateReturn();

			var foundUser = UsersMapper.GetByEmailAddress(EmailAddress.Text);

			if (foundUser == null)
			{

				var user = new User();
				user.FirstName = FirstName.Text;
				user.LastName = LastName.Text;
				user.UserName = user.EmailAddress = EmailAddress.Text;

				user.SetPassword(Password.Text);

				user.AfterLoginStartPage = "";
				user.AuthenticationType = AuthType.Forms.ToString();
				user.ResetCode = "";

				if (!IsValid(user))
				{
					returnObj = BaseMapper.GenerateReturn("Please make sure you fill out the required fields correctly");
				}
				else
				{
					var role = RolesMapper.GetByEnum(RoleEnum.Member);

					if (role != null)
					{
						user.Roles.Add(role);

						var dashboard = MediaDetailsMapper.GetByMediaType(MediaTypeEnum.Dashboard).FirstOrDefault();
						/*var level = dashboard.ChildMediaDetails.FirstOrDefault(i=>i.MediaType.Name == MediaTypeEnum.Level.ToString());

						if (level != null)
						{
							user.UnlockMedia(level.Media);
						}*/

						returnObj = UsersMapper.Insert(user);

						if (!returnObj.IsError)
						{
							FormsAuthentication.SetAuthCookie(user.UserName, false);
							FrameworkSettings.CurrentUser = user;

							SendAutoResponderEmail(user);
							//SendNotificationEmails(user);

							if (BasePage.CurrentMediaDetail.ChildMediaDetails.Any())
							{
								var firstPageUrl = BasePage.CurrentMediaDetail.ChildMediaDetails.ElementAt(0).AbsoluteUrl;
								Response.Redirect(firstPageUrl);
							}

						}
					}
				}
			}
			else
			{
				returnObj = BaseMapper.GenerateReturn("A user with the same email address already exists in the system.");
			}

			if (returnObj.IsError)
			{
				ErrorPanel.Visible = true;
				ServerMessage.Text = returnObj.Error.Exception.Message;
			}
		}

		public static Return SendAutoResponderEmail(User user)
		{
			var toEmails = new List<MailAddress>();
			toEmails.Add(new MailAddress(user.EmailAddress));

			return EmailHelper.SendTemplate(toEmails, "Thank you for creating a account at " + URIHelper.BaseUrl,
											AppSettings.SystemName, AppSettings.SystemEmailAddress,
											"~/Controls/EmailTemplates/CreateAccount/AutoResponder.ascx", user, AutoResponder.AutoResponderMode.NewAccount);
		}

		/*public static Return SendNotificationEmails(User user)
		{
			var admins = UsersMapper.GetAllByRole(RolesMapper.GetByEnum(RoleEnum.Administrator));
			var adminEmails = new List<MailAddress>();

			foreach (var admin in admins)
				adminEmails.Add(new MailAddress(admin.EmailAddress));

			return EmailHelper.SendTemplate(adminEmails, "A new account was created at " + URIHelper.BaseUrl,
											AppSettings.SystemName, AppSettings.SystemEmailAddress,
											"~/Controls/EmailTemplates/CreateAccount/Notification.ascx", user);
		}*/

		private BasePage BasePage
		{
			get
			{
				return (BasePage)this.Page;
			}
		}

	}
}