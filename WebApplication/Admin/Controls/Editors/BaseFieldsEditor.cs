using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Editors
{
    public class SelectorFieldOption
    {
        public string OptionText { get; set; }
        public string OptionValue { get; set; }
        public string AdminControl { get; set; }
        public string GetAdminControlValue { get; set; }
        public string SetAdminControlValue { get; set; }
        public string FrontEndLayout { get; set; }
    }

    public class BaseFieldsEditor : System.Web.UI.UserControl
    {
        public List<FieldType> FieldTypes { get; } = new List<FieldType>();

        public BaseFieldsEditor()
        {
            FieldTypes = FieldTypesMapper.GetAll().ToList();
        }

        public void BindFieldTypeDropDown(DropDownList fieldTypeDropDown)
        {
            var firstItem = fieldTypeDropDown.Items.FindByValue("");
            fieldTypeDropDown.Items.Clear();
            fieldTypeDropDown.Items.Add(firstItem);

            foreach (var FieldType in FieldTypes)
            {
                var listItem = fieldTypeDropDown.Items.FindByValue(FieldType.Name);

                if (listItem != null)
                    fieldTypeDropDown.Items.Remove(listItem);
            }

            fieldTypeDropDown.DataSource = FieldTypes;
            fieldTypeDropDown.DataTextField = "Name";
            fieldTypeDropDown.DataValueField = "ID";
            fieldTypeDropDown.DataBind();
        }

        public void FieldTypeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fieldTypeDropDown = ((DropDownList)sender);

            var selectorFieldOption = FieldTypes.SingleOrDefault(i => i.ID.ToString() == fieldTypeDropDown.SelectedValue);

            if (selectorFieldOption != null)
            {
                var adminControl = (TextBox)this.FindControl("AdminControl");
                var getAdminControlValue = (TextBox)this.FindControl("GetAdminControlValue");
                var setAdminControlValue = (TextBox)this.FindControl("SetAdminControlValue");
                var frontEndLayout = (TextBox)this.FindControl("FrontEndLayout");
                var fieldDescription = (dynamic)this.FindControl("FieldDescription");
				var showFrontEndFieldEditor = (CheckBox)this.FindControl("ShowFrontEndFieldEditor");

				//var fieldValue = (TextBox)this.FindControl("FieldValue");

				if (adminControl != null)
                {
                    adminControl.Text = selectorFieldOption.CodeToRenderAdminControl;
                }

                if (getAdminControlValue != null)
                {
                    getAdminControlValue.Text = selectorFieldOption.CodeToGetAdminControlValue;
                }

                if (setAdminControlValue != null)
                {
                    setAdminControlValue.Text = selectorFieldOption.CodeToSetAdminControlValue;
                }

                if (frontEndLayout != null)
                {
                    frontEndLayout.Text = selectorFieldOption.CodeToRenderFrontEndLayout;
                }

                if (fieldDescription != null)
                {
                    fieldDescription.SetValue(selectorFieldOption.FieldDescription);
                }

				/*if (fieldValue != null)
                {
                    fieldValue.Text = "";
                }*/

				showFrontEndFieldEditor.Checked = true;

			}

            fieldTypeDropDown.SelectedValue = "";			

		}
    }
}