using System;

namespace WebApplication.Controls.Generic
{
    public partial class FrickerGallery : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            BasePage.AddJSFile("~/Scripts/galleria/galleria-1.2.7.min.js");
            BasePage.AddJSFile("~/Scripts/galleria/plugins/flickr/galleria.flickr.min.js");
        }

        public string FlickerPhotoSetID { get; set; }

        private FrontEndBasePage BasePage
        {
            get { return (FrontEndBasePage)this.Page; }
        }
    }
}