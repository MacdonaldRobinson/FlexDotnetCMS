using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class LayoutsTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;

            if(selectedItem.LanguageID != LanguagesMapper.GetDefaultLanguage().ID)
            {
                UseDefaultLanguageLayoutsToggleWrapper.Visible = true;
            }
        }

        public void UpdateFieldsFromObject()
        {
            MainLayout.Text = selectedItem.MainLayout;
            SummaryLayout.Text = selectedItem.SummaryLayout;
            FeaturedLayout.Text = selectedItem.FeaturedLayout;
            UseMediaTypeLayouts.Checked = selectedItem.UseMediaTypeLayouts;
            UseDefaultLanguageLayouts.Checked = selectedItem.UseDefaultLanguageLayouts;
            OnPublishExecuteCode.Text = selectedItem.OnPublishExecuteCode;
        }

        public void UpdateObjectFromFields()
        {
            selectedItem.MainLayout = MediaDetailsMapper.ConvertUrlsToShortCodes(MainLayout.Text);
            selectedItem.SummaryLayout = MediaDetailsMapper.ConvertUrlsToShortCodes(SummaryLayout.Text);
            selectedItem.FeaturedLayout = MediaDetailsMapper.ConvertUrlsToShortCodes(FeaturedLayout.Text);
            selectedItem.UseMediaTypeLayouts = UseMediaTypeLayouts.Checked;
            selectedItem.UseDefaultLanguageLayouts = UseDefaultLanguageLayouts.Checked;
            selectedItem.OnPublishExecuteCode = OnPublishExecuteCode.Text;
        }
    }
}