using System;

namespace WebApplication.Controls
{
    public partial class Header1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public FrontEndBasePage BasePage
        {
            get
            {
                return (FrontEndBasePage)this.Page;
            }
        }
    }
}