using FrameworkLibrary;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin
{
    public class Tab
    {
        public Tab(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public ITab TabControl { get; set; }
    }

    public class BaseMediaDetailPanel : System.Web.UI.UserControl
    {
        private List<Tab> tabs = new List<Tab>();

        public virtual void SetObject(IMediaDetail obj)
        {
            SelectedItem = obj;

            var commonPanel = (WebApplication.Admin.Controls.MediaTypes.CommonPanel)WebFormHelper.FindControlRecursive(this, "CommonPanel");

            if (commonPanel != null)
            {
                commonPanel.SetObject(obj);

                var panelFields = (Panel)WebFormHelper.FindControlRecursive(this, "PanelFields");

                if (panelFields != null)
                {
                    commonPanel.SetPanelFields(panelFields);
                }
            }
        }

        protected void UpdateTabsFieldsFromObject()
        {
            foreach (Tab tab in tabs)
            {
                tab.TabControl.SetObject(SelectedItem);
                tab.TabControl.UpdateFieldsFromObject();
            }
        }

        protected void UpdateObjectFromTabsFields()
        {
            foreach (Tab tab in tabs)
            {
                tab.TabControl.SetObject(SelectedItem);
                tab.TabControl.UpdateObjectFromFields();
            }
        }

        protected void AddTabs(List<Tab> tabs)
        {
            foreach (Tab tab in tabs)
                AddTab(tab);
        }

        protected void AddTab(Tab tab)
        {
            if (tabs.Contains(tab))
                return;

            tabs.Add(tab);
        }

        public IMediaDetail SelectedItem
        {
            get
            {
                var item = (IMediaDetail)ViewState["SelectedIMediaDetail"];

                if (item == null)
                    return (IMediaDetail)ContextHelper.Get("SelectedIMediaDetail", ContextType.Session);

                return item;
            }
            set
            {
                ViewState["SelectedIMediaDetail"] = value;
                ContextHelper.Set("SelectedIMediaDetail", value, ContextType.Session);
            }
        }

        public AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }

        public List<Tab> Tabs
        {
            get
            {
                return this.tabs;
            }
        }
    }
}