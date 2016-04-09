using FrameworkLibrary;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.Generic
{
    public partial class GenericSlideShow : System.Web.UI.UserControl
    {
        private Gallery gallery = null;
        private string _mode = "fade";
        private string _speed = "500";
        private string _infiniteLoop = "true";
        private string _controls = "true";
        private string _prevText = "prev";
        private string _prevImage = "";
        private string _nextText = "next";
        private string _nextImage = "";
        private string _hideControlOnEnd = "false";
        private string _captions = "false";
        private string _auto = "false";
        private string _pause = "3000";
        private string _pager = "false";
        private string _pagerType = "full";
        private string _displaySlideQty = "1";
        private string _moveSlideQty = "1";
        private string _autoControls = "true";
        private string _alternativeImageViewerId = "";

        public string Mode
        {
            get { return _mode; }
            set { _mode = value.ToLower(); }
        }

        public string Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public string InfiniteLoop
        {
            get { return _infiniteLoop; }
            set { _infiniteLoop = value.ToLower(); }
        }

        public string Controls
        {
            get { return _controls; }
            set { _controls = value.ToLower(); }
        }

        public string PrevText
        {
            get { return _prevText; }
            set { _prevText = value; }
        }

        public string PrevImage
        {
            get { return _prevImage; }
            set { _prevImage = value; }
        }

        public string NextText
        {
            get { return _nextText; }
            set { _nextText = value; }
        }

        public string NextImage
        {
            get { return _nextImage; }
            set { _nextImage = value; }
        }

        public string HideControlOnEnd
        {
            get { return _hideControlOnEnd; }
            set { _hideControlOnEnd = value.ToLower(); }
        }

        public string Captions
        {
            get { return _captions; }
            set { _captions = value.ToLower(); }
        }

        public string Auto
        {
            get { return _auto; }
            set { _auto = value.ToLower(); }
        }

        public string Pause
        {
            get { return _pause; }
            set { _pause = value.ToLower(); }
        }

        public string Pager
        {
            get { return _pager; }
            set { _pager = value.ToLower(); }
        }

        public string PagerType
        {
            get { return _pagerType; }
            set { _pagerType = value.ToLower(); }
        }

        public string DisplaySlideQty
        {
            get { return _displaySlideQty; }
            set { _displaySlideQty = value; }
        }

        public string MoveSlideQty
        {
            get { return _moveSlideQty; }
            set { _moveSlideQty = value; }
        }

        public string AlternativeImageViewerID
        {
            get { return _alternativeImageViewerId; }
            set { _alternativeImageViewerId = value; }
        }

        public string AutoControls
        {
            get { return _autoControls; }
            set { _autoControls = value.ToLower(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        private void Bind()
        {
            if ((this.gallery == null) || ((this.gallery.Slides.Count == 0)))
            {
                SlidesPanel.Visible = false;
                return;
            }

            if (this.gallery.Slides.Count > 1)
                _auto = "true";

            SlidesList.DataSource = this.gallery.Slides;
            SlidesList.DataBind();
        }

        public void SetGallery(Gallery gallery)
        {
            this.gallery = gallery;
        }

        protected void SlidesList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HyperLink FlashAltLink = (HyperLink)e.Item.FindControl("Link");
                HyperLink Link = (HyperLink)e.Item.FindControl("Link");
                Image Image = (Image)e.Item.FindControl("Image");

                Panel ImageSliderPanel = (Panel)e.Item.FindControl("ImageSliderPanel");
                Panel ContentSliderPanel = (Panel)e.Item.FindControl("ContentSliderPanel");

                Panel NonFlashPanel = (Panel)e.Item.FindControl("NonFlashPanel");

                HtmlControl Slide = (HtmlControl)e.Item.FindControl("Slide");

                Slide Item = (Slide)e.Item.DataItem;

                if ((this.gallery.PathToRenderControl != null && this.gallery.PathToRenderControl != ""))
                {
                    ImageSliderPanel.Visible = false;
                    ContentSliderPanel.Visible = true;

                    Literal ItemRenderedLayout = new Literal();
                    ItemRenderedLayout.Text = LoaderHelper.RenderControl(this.gallery.PathToRenderControl, Item.MediaDetail);
                    ContentSliderPanel.Controls.Add(ItemRenderedLayout);
                }
                else
                {
                    ImageSliderPanel.Visible = true;
                    ContentSliderPanel.Visible = false;

                    if (Item.PathToFile.Trim() != "")
                        Link.NavigateUrl = FlashAltLink.NavigateUrl = Item.Link;

                    this.BasePage.AddToJSPreload(Item.PathToFile);

                    if (!Item.PathToFile.Contains(".swf"))
                    {
                        Image.ImageUrl = Item.PathToFile;
                        Image.Attributes["data-alt-image"] = URIHelper.ConvertToAbsUrl(Item.PathToAlternativeFile);
                        Image.AlternateText = Item.Title;

                        NonFlashPanel.CssClass = NonFlashPanel.CssClass + " InteriorBanner";

                        if (!string.IsNullOrEmpty(Item.BgColor))
                            NonFlashPanel.Style["background-color"] = Item.BgColor;
                    }
                    else
                    {
                        NonFlashPanel.Visible = false;
                    }

                    Link.ToolTip = Item.Title;

                    if (Link.NavigateUrl != "")
                    {
                        Link.CssClass += " popup";
                    }
                }
            }
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