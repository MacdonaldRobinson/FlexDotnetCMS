using FrameworkLibrary;
using System;

namespace WebApplication.Admin.Controls.MediaTypes.Website
{
    public partial class WebsitePanel : BaseMediaDetailPanel, IMediaDetailPanel
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            LoadTabs();
        }

        private void LoadTabs()
        {
            //AddWebsiteSettingsTab();

            if (this.Tabs.Count == 0)
                this.AddTabs(CommonPanel.Tabs);
        }

        private void AddWebsiteSettingsTab()
        {
            if (BasePage.CurrentUser.IsInRole(RoleEnum.Developer))
            {
                CommonPanel.AddTab("Website Settings", "~/Admin/Controls/MediaTypes/Website/Tabs/WebsiteSettingsTab.ascx");
            }
        }

        public void UpdateObjectFromFields()
        {
            SelectedItem.CodeInBody = "";
            SelectedItem.CodeInHead = "";

            CommonPanel.UpdateObjectFromFields();
        }

        public void UpdateFieldsFromObject()
        {
            CommonPanel.UpdateFieldsFromObject();
        }

        public new FrameworkLibrary.Website SelectedItem
        {
            get { return (FrameworkLibrary.Website)base.SelectedItem; }
        }
    }
}