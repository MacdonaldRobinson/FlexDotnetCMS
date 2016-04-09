using System;

namespace WebApplication.Controls
{
    public partial class Header : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.PreRender += new EventHandler(Page_PreRender);            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        public BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}