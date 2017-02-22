using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrameworkLibrary;

namespace WebApplication.Admin.Views.PageHandlers.FieldTypes
{
    public partial class Detail : AdminBasePage
    {
        private FieldType selectedItem = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            long id;

            if (long.TryParse(Request["id"], out id))
            {
                selectedItem = FieldTypesMapper.GetByID(id);

                if (!IsPostBack)
                    UpdateFieldsFromObject();
            }

            this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
        }

        private string GetSectionTitle()
        {
            if (selectedItem == null)
                return "New FieldType";
            else
                return "Editing FieldType: " + selectedItem.Name;
        }

        private void UpdateObjectFromFields()
        {
            selectedItem.Name = Name.Text;
            selectedItem.CodeToRenderAdminControl = CodeToRenderAdminControl.Text;
            selectedItem.CodeToGetAdminControlValue = CodeToGetAdminControlValue.Text;
            selectedItem.CodeToSetAdminControlValue = CodeToSetAdminControlValue.Text;
            selectedItem.CodeToRenderFrontEndLayout = CodeToRenderFrontEndLayout.Text;
        }

        private void UpdateFieldsFromObject()
        {
            Name.Text = selectedItem.Name;
            CodeToRenderAdminControl.Text = selectedItem.CodeToRenderAdminControl;
            CodeToGetAdminControlValue.Text = selectedItem.CodeToGetAdminControlValue;
            CodeToSetAdminControlValue.Text = selectedItem.CodeToSetAdminControlValue;
            CodeToRenderFrontEndLayout.Text = selectedItem.CodeToRenderFrontEndLayout;
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (selectedItem == null)
                selectedItem = FieldTypesMapper.CreateObject();
            else
                selectedItem = BaseMapper.GetObjectFromContext<FieldType>(selectedItem);

            UpdateObjectFromFields();

            Return returnObj;

            if (selectedItem.ID == 0)
                returnObj = FieldTypesMapper.Insert(selectedItem);
            else
                returnObj = FieldTypesMapper.Update(selectedItem);

            if (returnObj.IsError)
                DisplayErrorMessage("Error Saving Item", returnObj.Error);
            else
            {
                UpdateFieldsFromObject();
                DisplaySuccessMessage("Successfully Saved Item");
            }
        }
    }
}
