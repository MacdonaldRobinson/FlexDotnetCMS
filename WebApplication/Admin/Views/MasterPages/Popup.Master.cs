using System;

namespace WebApplication.Admin.Views.MasterPages
{
    public partial class Popup : BaseMasterPage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.BasePage.AddCSSFile("~/Admin/Styles/popup.css");
        }
    }
}