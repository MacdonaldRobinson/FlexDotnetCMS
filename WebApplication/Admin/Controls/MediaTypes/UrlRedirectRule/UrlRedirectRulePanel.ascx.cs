using FrameworkLibrary;
using System;

namespace WebApplication.Admin.Controls.MediaTypes.UrlRedirectRule
{
    public partial class UrlRedirectRulePanel : BaseMediaDetailPanel, IMediaDetailPanel
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            LoadTabs();
        }

        public override void SetObject(IMediaDetail obj)
        {
            base.SetObject(obj);
            UpdateFieldsFromObject();
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

            if (!selectedItem.VirtualPathToRedirect.EndsWith("/"))
                selectedItem.VirtualPathToRedirect = selectedItem.VirtualPathToRedirect + "/";

            selectedItem.RedirectToUrl = RedirectToUrl.Text;
            selectedItem.Is301Redirect = Is301Redirect.Checked;

            CommonPanel.UpdateObjectFromFields();
        }

        public void UpdateFieldsFromObject()
        {
            var selectedItem = (FrameworkLibrary.UrlRedirectRule)SelectedItem;
            VirtualPathToRedirect.Text = selectedItem.VirtualPathToRedirect;
            RedirectToUrl.Text = MediaDetailsMapper.ConvertUrlsToShortCodes(selectedItem.RedirectToUrl);
            Is301Redirect.Checked = selectedItem.Is301Redirect;

            CommonPanel.UpdateFieldsFromObject();
        }
    }
}