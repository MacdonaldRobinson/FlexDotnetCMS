using FrameworkLibrary;
using System;

namespace WebApplication.Controls.Mobile
{
    public partial class ToggleView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            this.ToggleViewOptions.SelectedValue = FrontEndBasePage.UserSelectedVersion.ToString();
        }

        protected void ToggleViewOptions_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(URIHelper.GetCurrentVirtualPath() + "?userSelectedVersion=" + this.ToggleViewOptions.SelectedValue);
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