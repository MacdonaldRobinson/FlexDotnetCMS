using FrameworkLibrary;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class SEOSettingsTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public void UpdateFieldsFromObject()
        {
            PageTitle.Text = selectedItem.Title;
            MetaDescription.Text = selectedItem.MetaDescription;
            MetaKeywords.Text = selectedItem.MetaKeywords;
			MetaRobots.Text = selectedItem.MetaRobots;
		}

        public void UpdateObjectFromFields()
        {
            selectedItem.Title = PageTitle.Text;
            selectedItem.MetaDescription = MetaDescription.Text;
            selectedItem.MetaKeywords = MetaKeywords.Text;
			selectedItem.MetaRobots = MetaRobots.Text;			
		}
    }
}