using FrameworkLibrary;
using System;

namespace WebApplication.Admin.Controls.MediaTypes.Website.Tabs
{
    public partial class WebsiteSettingsTab : BaseTab, ITab
    {
        private Settings Settings { get; set; } = SettingsMapper.GetSettings();
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public new FrameworkLibrary.Website SelectedItem
        {
            get { return (FrameworkLibrary.Website)base.selectedItem; }
        }

        public void UpdateFieldsFromObject()
        {
            CodeInHead.Text = SelectedItem.CodeInHead;
            CodeInBody.Text = SelectedItem.CodeInBody;
        }

        public void UpdateObjectFromFields()
        {
            SelectedItem.CodeInHead = CodeInHead.Text;
            SelectedItem.CodeInBody = CodeInBody.Text;
        }
    }
}