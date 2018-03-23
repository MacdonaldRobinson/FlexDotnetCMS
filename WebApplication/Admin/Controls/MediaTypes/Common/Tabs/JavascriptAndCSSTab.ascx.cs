using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class JavascriptAndCSSTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public void UpdateFieldsFromObject()
        {
            Javascript.Text = selectedItem.Javascript;
            CSS.Text = selectedItem.CSS;
        }

        public void UpdateObjectFromFields()
        {
            selectedItem.Javascript = Javascript.Text;
            selectedItem.CSS = CSS.Text;
        }
    }
}