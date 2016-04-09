using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Views.MasterPages.SiteTemplate2
{
    public partial class Site2 : System.Web.UI.MasterPage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            Bind();
            //this.BasePage.AddCSSFile(BasePage.TemplateBaseUrl + "css/style.css");
        }

        public void Bind()
        {
            if (this.BasePage.CurrentMediaDetail == null)
                return;

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