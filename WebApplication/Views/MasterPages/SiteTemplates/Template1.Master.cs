using System;
using System.Collections.Generic;

namespace WebApplication
{
    public partial class SiteMaster : System.Web.UI.MasterPage
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

            /*RelatedItems.SetItem(this.BasePage.CurrentMediaDetail);
            SectionTitle.Text = this.BasePage.CurrentMediaDetail.SectionTitle;*/
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