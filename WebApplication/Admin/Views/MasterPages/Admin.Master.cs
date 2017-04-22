using FrameworkLibrary;
using FrameworkLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin
{
    public partial class Admin : BaseMasterPage
    {
        private Website currentWebsite = WebsitesMapper.GetWebsite();
        private long numberOfActiveLanguages = LanguagesMapper.GetAllActive().Count();

        protected void Page_Init(object sender, EventArgs e)
        {
            InitPage();
        }

        private void InitPage()
        {
            var settings = SettingsMapper.GetSettings();
            var rootMediaDetail = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.MediaType.Name == MediaTypeEnum.RootPage.ToString() && i.LanguageID == AdminBasePage.CurrentLanguage.ID);

            if (rootMediaDetail == null)
            {
                CreateItem.Visible = true;
            }

            if (settings.EnableGlossaryTerms)
            {
                GlossaryTermsNavItem.Visible = true;
            }

            if (numberOfActiveLanguages < 2)
                LanguageSwitcher.Visible = false;
            else
                LanguageSwitcher.Visible = true;

            //AdminBasePage.SelectedMedia = null;
            //AdminBasePage.SelectedMediaDetail = null;

            CreateItem.NavigateUrl = $"{URIHelper.BaseUrl}Admin/Views/PageHandlers/Media/Create.aspx";
        }

        protected void LoginStatus_OnLoggedOut(object sender, EventArgs e)
        {
            this.BasePage.CheckInAll();
        }
    }
}