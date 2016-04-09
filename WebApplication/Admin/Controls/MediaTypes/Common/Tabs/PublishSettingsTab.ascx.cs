using FrameworkLibrary;
using System;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class PublishSettingsTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public void UpdateObjectFromFields()
        {
            if (!string.IsNullOrEmpty(ExpiryDate.Text))
                selectedItem.ExpiryDate = DateTime.Parse(ExpiryDate.Text);
            else
                selectedItem.ExpiryDate = null;

            if (!string.IsNullOrEmpty(PublishDate.Text))
                selectedItem.PublishDate = DateTime.Parse(PublishDate.Text);
        }

        public void UpdateFieldsFromObject()
        {
            PublishDate.Text = selectedItem.PublishDate.ToString();
            ExpiryDate.Text = selectedItem.ExpiryDate.ToString();
        }
    }
}