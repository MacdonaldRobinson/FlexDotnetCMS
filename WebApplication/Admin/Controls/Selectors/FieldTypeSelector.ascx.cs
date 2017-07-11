using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrameworkLibrary;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class FieldTypeSelector : System.Web.UI.UserControl
    {
        private List<FieldType> FieldTypes = new List<FieldType>();
        protected void Page_Init(object sender, EventArgs e)
        {
            FieldTypes = FieldTypesMapper.GetAll().ToList();
            BindFieldTypeDropDown();
        }

        public void BindFieldTypeDropDown()
        {
            foreach (var FieldType in FieldTypes)
            {
                var listItem = FieldTypeDropDown.Items.FindByValue(FieldType.Name);

                if (listItem != null)
                    FieldTypeDropDown.Items.Remove(listItem);
            }

            FieldTypeDropDown.DataSource = FieldTypes;
            FieldTypeDropDown.DataTextField = "Name";
            FieldTypeDropDown.DataValueField = "ID";
            FieldTypeDropDown.DataBind();
        }

        public FieldType SelectedFieldType
        {
            get
            {
                if (FieldTypeDropDown.SelectedValue == "")
                    return null;

                var id = long.Parse(FieldTypeDropDown.SelectedValue);
                var fieldType = FieldTypes.Find(i=>i.ID == id);

                return fieldType;
            }
        }
    }
}