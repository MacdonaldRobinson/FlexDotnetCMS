using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class FieldsTab : BaseTab, ITab
    {        
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;

            if (MediaFieldsEditor != null)
            {
                Bind();
            }
        }

        public void Bind()
        {
            MediaFieldsEditor.SetItems(selectedItem);
        }
        
        public void UpdateFieldsFromObject()
        {
        }

        public void UpdateObjectFromFields()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}