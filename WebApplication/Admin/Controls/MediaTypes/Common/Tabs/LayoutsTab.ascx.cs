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
        }

        public void UpdateFieldsFromObject()
        {
            MainLayout.Text = selectedItem.MainLayout;
            SummaryLayout.Text = selectedItem.SummaryLayout;
            FeaturedLayout.Text = selectedItem.FeaturedLayout;
            UseMediaTypeLayouts.Checked = selectedItem.UseMediaTypeLayouts;
        }

        public void UpdateObjectFromFields()
        {
            selectedItem.MainLayout = MediaDetailsMapper.ConvertATagsToShortCodes(MainLayout.Text);
            selectedItem.SummaryLayout = MediaDetailsMapper.ConvertATagsToShortCodes(SummaryLayout.Text);
            selectedItem.FeaturedLayout = MediaDetailsMapper.ConvertATagsToShortCodes(FeaturedLayout.Text);
            selectedItem.UseMediaTypeLayouts = UseMediaTypeLayouts.Checked;
        }
    }
}