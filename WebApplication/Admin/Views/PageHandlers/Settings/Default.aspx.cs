using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Admin.Views.PageHandlers.Settings
{
    public partial class Default : AdvanceOptionsBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            DefaultLanguageSelector.DataSource = LanguagesMapper.GetAllActive().OrderBy(i => i.Name).ToList();
            DefaultLanguageSelector.DataTextField = "Name";
            DefaultLanguageSelector.DataValueField = "ID";
            DefaultLanguageSelector.DataBind();

            DefaultMasterPageSelector.DataSource = MasterPagesMapper.GetAll().OrderBy(i => i.Name).ToList();
            DefaultMasterPageSelector.DataTextField = "Name";
            DefaultMasterPageSelector.DataValueField = "ID";
            DefaultMasterPageSelector.DataBind();
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);

            if (IsPostBack)
                return;

            UpdateFieldsFromObject(SettingsMapper.GetSettings());
        }

        public void UpdateFieldsFromObject(FrameworkLibrary.Settings settings)
        {
            ShoppingCartTax.Text = settings.ShoppingCartTax.ToString();
            MaxRequestLengthInMB.Text = (settings.MaxRequestLength / (1024 * 1024)).ToString();
            MaxUploadFileSizePerFileInMB.Text = (settings.MaxUploadFileSizePerFile / (1024 * 1024)).ToString();
            GlobalCodeInHead.Text = settings.GlobalCodeInHead;
            GlobalCodeInBody.Text = settings.GlobalCodeInBody;
            OutputCacheDurationInSeconds.Text = settings.OutputCacheDurationInSeconds.ToString();
            PageNotFoundUrl.Text = settings.PageNotFoundUrl;
            EnableGlossaryTerms.Checked = settings.EnableGlossaryTerms;

            if (settings.SiteOnlineAtDateTime > DateTime.MinValue)
                SiteOnlineAtDateTime.Text = settings.SiteOnlineAtDateTime.ToString();

            if (settings.SiteOfflineAtDateTime != null)
                SiteOfflineAtDateTime.Text = settings.SiteOfflineAtDateTime.ToString();

            SiteOfflineUrl.Text = settings.SiteOfflineUrl;

            DefaultLanguageSelector.SelectedValue = settings.DefaultLanguageID.ToString();
            DefaultMasterPageSelector.SelectedValue = settings.DefaultMasterPageID.ToString();
        }

        public void UpdateObjectFromFields(FrameworkLibrary.Settings settings)
        {
            settings.ShoppingCartTax = decimal.Parse(ShoppingCartTax.Text);
            settings.MaxRequestLength = int.Parse(MaxRequestLengthInMB.Text) * (1024 * 1024);
            settings.MaxUploadFileSizePerFile = int.Parse(MaxUploadFileSizePerFileInMB.Text) * (1024 * 1024);
            settings.GlobalCodeInHead = GlobalCodeInHead.Text;
            settings.GlobalCodeInBody = GlobalCodeInBody.Text;
            settings.SiteOfflineUrl = SiteOfflineUrl.Text;
            settings.PageNotFoundUrl = PageNotFoundUrl.Text;
            settings.EnableGlossaryTerms = EnableGlossaryTerms.Checked;
            settings.OutputCacheDurationInSeconds = long.Parse(OutputCacheDurationInSeconds.Text);

            if (!string.IsNullOrEmpty(SiteOnlineAtDateTime.Text))
            {
                settings.SiteOnlineAtDateTime = DateTime.Parse(SiteOnlineAtDateTime.Text);
            }
            else
            {
                settings.SiteOnlineAtDateTime = DateTime.Now;
            }


            if (!string.IsNullOrEmpty(SiteOfflineAtDateTime.Text))
            {
                settings.SiteOfflineAtDateTime = DateTime.Parse(SiteOfflineAtDateTime.Text);
            }
            else
            {
                settings.SiteOfflineAtDateTime = null;
            }

            settings.DefaultLanguageID = long.Parse(DefaultLanguageSelector.SelectedValue);
            settings.DefaultMasterPageID = long.Parse(DefaultMasterPageSelector.SelectedValue);
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            var settings = BaseMapper.GetObjectFromContext(SettingsMapper.GetSettings());

            UpdateObjectFromFields(settings);

            var returnObj = SettingsMapper.Update(settings);

            if (returnObj.IsError)
            {
                DisplayErrorMessage("Error saving settings", returnObj.Error);
            }
            else
            {
                ContextHelper.ClearAllMemoryCache();
                FileCacheHelper.ClearAllCache();

                SettingsMapper.SetSettings(settings);

                DisplaySuccessMessage("Successfully saved settings");
            }
        }
    }
}