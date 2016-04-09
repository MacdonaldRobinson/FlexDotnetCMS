using System;

namespace WebApplication.Admin.Controls.MediaTypes.HomePage
{
    public partial class HomePagePanel : BaseMediaDetailPanel, IMediaDetailPanel
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
            CommonPanel.UpdateObjectFromFields();
        }

        public void UpdateFieldsFromObject()
        {
            CommonPanel.UpdateFieldsFromObject();
        }
    }
}