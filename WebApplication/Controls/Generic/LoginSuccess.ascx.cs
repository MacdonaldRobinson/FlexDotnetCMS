using System;
using System.Web.Security;

namespace WebApplication.Controls
{
    public partial class LoginSuccess : System.Web.UI.UserControl
    {
        protected void LoginStatus_OnLoggedOut(object sender, EventArgs e)
        {
            this.BasePage.CurrentUser = null;
            FormsAuthentication.SignOut();
        }

        private FrontEndBasePage BasePage
        {
            get
            {
                return (FrontEndBasePage)this.Page;
            }
        }
    }
}