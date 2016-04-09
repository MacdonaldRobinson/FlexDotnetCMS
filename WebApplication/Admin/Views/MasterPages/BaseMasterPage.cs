using System;

namespace WebApplication.Admin
{
    public class BaseMasterPage : System.Web.UI.MasterPage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //BasePage.AddCommonIncludes();
        }

        public AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }
    }
}