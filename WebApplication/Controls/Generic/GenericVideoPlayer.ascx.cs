using FrameworkLibrary;
using System;

namespace WebApplication.Controls.Generic
{
    public partial class GenericVideoPlayer : System.Web.UI.UserControl
    {
        private string flashVideoUrl = "";
        private string previewImageUrl = "";
        private bool autoStart = false;
        private string htmlVideoUrl = "";
        private string downloadVideoUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string PlayerBaseUrl
        {
            get
            {
                return URIHelper.ConvertToAbsUrl("~/Scripts/mediaplayer") + "/";
            }
        }

        public string PlayerSwfUrl
        {
            get
            {
                return this.PlayerBaseUrl + "player.swf";
            }
        }

        public string FlashVideoUrl
        {
            get
            {
                return URIHelper.ConvertToAbsUrl(this.flashVideoUrl);
            }
            set
            {
                this.flashVideoUrl = value;
            }
        }

        public string HtmlVideoUrl
        {
            get
            {
                return URIHelper.ConvertToAbsUrl(this.htmlVideoUrl);
            }
            set
            {
                this.htmlVideoUrl = value;
            }
        }

        public string DownloadVideoUrl
        {
            get
            {
                return URIHelper.ConvertToAbsUrl(this.downloadVideoUrl);
            }
            set
            {
                this.downloadVideoUrl = value;
            }
        }

        public string PreviewImageUrl
        {
            get
            {
                return URIHelper.ConvertToAbsUrl(this.previewImageUrl);
            }
            set
            {
                this.previewImageUrl = value;
            }
        }

        public bool AutoStart
        {
            get
            {
                return autoStart;
            }
            set
            {
                this.autoStart = value;
            }
        }

        public int Width { get; set; }

        public int Height { get; set; }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            BasePage.AddJSFile("~/Scripts/mediaplayer/jwplayer.js");
        }

        private BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}