using FrameworkLibrary;
using System;

namespace WebApplication.Controls.RenderMedia
{
    public partial class RenderMedia : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var mediaDetail = BasePage.CurrentMediaDetail;

            if (MediaID != 0)
                mediaDetail = MediasMapper.GetByID(MediaID)?.GetLiveMediaDetail();

            if (mediaDetail != null)
            {

                if (string.IsNullOrEmpty(PropertyName))
                    PropertyName = "{UseMainLayout}";
                else
                    PropertyName = "{" + PropertyName + "}";

                DynamicContent.Controls.Add(this.ParseControl(MediaDetailsMapper.ParseSpecialTags(mediaDetail, PropertyName)));
            }
        }

        public long MediaID
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