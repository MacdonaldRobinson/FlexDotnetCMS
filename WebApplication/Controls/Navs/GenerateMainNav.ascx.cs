using System;

namespace WebApplication.Controls
{
    public partial class GenerateMainNav : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (this.BasePage.FrameworkBaseMedia.RootMedia != null)
            GenerateNav.RootMedia = this.BasePage.CurrentWebsite.Media;
        }

        public bool RenderRootMedia
        {
            get { return GenerateNav.RenderRootMedia; }
            set { GenerateNav.RenderRootMedia = value; }
        }

        public string RootUlClasses
        {
            get { return GenerateNav.RootULClasses; }
            set { GenerateNav.RootULClasses = value; }
        }

        public bool IsFooterMenu
        {
            get { return GenerateNav.IsFooterMenu; }
            set { GenerateNav.IsFooterMenu = value; }
        }

        public int RenderDepth
        {
            get { return GenerateNav.RenderDepth; }
            set { GenerateNav.RenderDepth = value; }
        }

        public bool RenderParentItemInChildNav
        {
            get { return GenerateNav.RenderParentItemInChildNav; }
            set { GenerateNav.RenderParentItemInChildNav = value; }
        }

        private BasePage BasePage
        {
            get { return (BasePage)this.Page; }
        }
    }
}