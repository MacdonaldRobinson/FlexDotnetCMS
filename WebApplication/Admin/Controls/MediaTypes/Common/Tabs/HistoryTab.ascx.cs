using FrameworkLibrary;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class HistoryTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
            UpdateFieldsFromObject();
        }

        public void UpdateFieldsFromObject()
        {
            MediaDetailHistoryEditor.SetItem(selectedItem);
        }

        public void UpdateObjectFromFields()
        {
        }
    }
}