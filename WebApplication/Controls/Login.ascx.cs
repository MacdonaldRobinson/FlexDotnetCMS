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
        protected void Submit_Click(object sender, EventArgs e)
        {
            var user = UsersMapper.GetUserByCredentials(Username.Text, Password.Text);

            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);

                FrameworkSettings.CurrentUser = user;

                var returnUrl = (Request["ReturnUrl"] != null) ? Request["ReturnUrl"] : "~/admin/";

                Response.Redirect(returnUrl);
            }
        }
    }
}