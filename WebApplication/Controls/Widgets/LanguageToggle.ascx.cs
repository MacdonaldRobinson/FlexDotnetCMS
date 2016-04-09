using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.Widgets
{
    public partial class LanguageToggle : System.Web.UI.UserControl
    {
        private IEnumerable<FrameworkLibrary.Language> activeLanguages = new List<FrameworkLibrary.Language>();

        protected void Page_PreRender(object sender, EventArgs e)
        {
            activeLanguages = LanguagesMapper.GetAllActive();

            LanguageToggleLinks.DataSource = activeLanguages;
            LanguageToggleLinks.DataBind();
        }

        protected void LanguageToggleLinks_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var language = (FrameworkLibrary.Language)e.Item.DataItem;
            var languageLink = (HyperLink)e.Item.FindControl("LanguageLink");

            languageLink.Text = language.Name;

            if (BasePage.CurrentMediaDetail.Language.CultureCode == language.CultureCode)
                languageLink.CssClass = "current";

            var item = MediaDetailsMapper.GetByMedia(BasePage.CurrentMedia, language);

            if (item == null)
            {
                e.Item.Visible = false;
                return;
            }

            languageLink.NavigateUrl = item.AutoCalculatedVirtualPath;
        }

        public FrontEndBasePage BasePage
        {
            get
            {
                return (FrontEndBasePage)this.Page;
            }
        }

        protected void LanguageToggleLinks_DataBound(object sender, EventArgs e)
        {
            var languagesVisible = LanguageToggleLinks.Items.Where(i => i.Visible).Count();

            if (languagesVisible < 2)
                this.Visible = false;
        }
    }
}