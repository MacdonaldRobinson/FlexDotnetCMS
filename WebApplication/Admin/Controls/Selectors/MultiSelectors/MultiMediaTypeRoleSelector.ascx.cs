using FrameworkLibrary;
using System;

namespace WebApplication.Admin.Controls.Selectors.MultiSelectors
{
    public partial class MultiMediaTypeRoleSelector : System.Web.UI.UserControl
    {
        private MediaType SelectedMediaType
        {
            get
            {
                return (MediaType)ViewState["SelectedMediaType"];
            }
            set
            {
                ViewState["SelectedMediaType"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void SetMediaType(MediaType mediaType)
        {
            this.SelectedMediaType = mediaType;
        }
    }
}