using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Controls
{
    public partial class GenerateSubNav : System.Web.UI.UserControl
    {
        private bool renderParentNav = false;

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (renderParentNav)
            {
                Media parentMedia = null;

                if (this.BasePage.CurrentMedia.ParentMediaID != null)
                    parentMedia = MediasMapper.GetByID((long)this.BasePage.CurrentMedia.ParentMediaID);

                if (parentMedia == null)
                    return;

                var children = MediaDetailsMapper.FilterByMediaTypeShowInMenuStatus(MediaDetailsMapper.FilterByShowInMenuStatus(MediaDetailsMapper.FilterByCanRenderStatus(MediaDetailsMapper.GetAllChildMediaDetails(parentMedia, this.BasePage.CurrentLanguage), true), true), true);

                if (children.Count() > 0)
                    GenerateNav.RootMedia = parentMedia;
                else
                {
                    if (parentMedia.ParentMediaID == null)
                        return;

                    parentMedia = MediasMapper.GetByID((long)parentMedia.ParentMediaID);

                    //if ((parentMedia != null) && (parentMedia.ID != FrameworkSettings.RootMedia.ID))
                    if (parentMedia != null)
                        GenerateNav.RootMedia = parentMedia;
                }
            }
            else
                GenerateNav.RootMedia = this.BasePage.CurrentMedia;
        }

        public bool RenderParentNavIfNoChildren
        {
            set
            {
                if (value)
                {
                    if (this.BasePage.CurrentMedia == null)
                        return;

                    //var children = MediaDetailsMapper.FilterByMediaTypeShowInMenuStatus(MediaDetailsMapper.FilterByShowInMenuStatus(MediaDetailsMapper.FilterByCanRenderStatus(MediaDetailsMapper.GetAllChildMediaDetails(this.BasePage.CurrentMedia, this.BasePage.CurrentLanguage), true), true), true).ToList();

                    var children = this.BasePage.CurrentMediaDetail.ChildMediaDetails;

                    if (!children.Any())
                        renderParentNav = true;
                    else
                        renderParentNav = false;
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