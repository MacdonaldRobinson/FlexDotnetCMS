using System;
using System.Web.Security;

namespace WebApplication.Controls
{
    public partial class LoginSuccess : System.Web.UI.UserControl
    {
        private FrontEndBasePage BasePage
        {
            get
            {
                return (FrontEndBasePage)this.Page;
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            this.BasePage.CurrentUser = null;
            FormsAuthentication.SignOut();
        }
    }
}