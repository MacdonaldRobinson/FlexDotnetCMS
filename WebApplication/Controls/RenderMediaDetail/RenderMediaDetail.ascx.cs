using FrameworkLibrary;
using System;

namespace WebApplication.Controls
{
    public partial class RenderCurrentMediaDetail : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var mediaDetail = BasePage.CurrentMediaDetail;

            if (MediaDetailID != 0)
                mediaDetail = MediaDetailsMapper.GetByID(MediaDetailID);

            if (string.IsNullOrEmpty(PropertyName))
                PropertyName = "{UseMainLayout}";
            else
                PropertyName = "{" + PropertyName + "}";

            DynamicContent.Controls.Add(this.ParseControl(MediaDetailsMapper.ParseSpecialTags(mediaDetail, PropertyName)));
        }

        public long MediaDetailID
        {
            set;
            get;
        }

        public string PropertyName { get; set; }

        public BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}