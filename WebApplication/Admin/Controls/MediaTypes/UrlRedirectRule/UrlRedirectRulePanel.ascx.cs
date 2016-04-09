using System;

namespace WebApplication.Admin.Controls.MediaTypes.UrlRedirectRule
{
    public partial class UrlRedirectRulePanel : BaseMediaDetailPanel, IMediaDetailPanel
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            LoadTabs();
        }

        private void LoadTabs()
        {
            if (this.Tabs.Count == 0)
                this.AddTabs(CommonPanel.Tabs);
        }

        public void UpdateObjectFromFields()
        {
            var selectedItem = (FrameworkLibrary.UrlRedirectRule)SelectedItem;
            selectedItem.VirtualPathToRedirect = VirtualPathToRedirect.Text;
            selectedItem.RedirectToUrl = RedirectToUrl.Text;
            selectedItem.Is301Redirect = Is301Redirect.Checked;

            CommonPanel.UpdateObjectFromFields();
        }

        public void UpdateFieldsFromObject()
        {
            var selectedItem = (FrameworkLibrary.UrlRedirectRule)SelectedItem;
            VirtualPathToRedirect.Text = selectedItem.VirtualPathToRedirect;
            RedirectToUrl.Text = selectedItem.RedirectToUrl;
            Is301Redirect.Checked = selectedItem.Is301Redirect;

            CommonPanel.UpdateFieldsFromObject();
        }
    }
}