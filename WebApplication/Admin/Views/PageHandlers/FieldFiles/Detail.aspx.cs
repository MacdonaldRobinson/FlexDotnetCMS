using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrameworkLibrary;

namespace WebApplication.Admin.Views.PageHandlers.FieldFiles
{
    public partial class Detail : AdminBasePage
    {
        private FieldFile selectedItem = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            long id;

            if (long.TryParse(Request["id"], out id))
            {
                selectedItem = FieldFilesMapper.GetByID(id);

                if (!IsPostBack)
                    UpdateFieldsFromObject();
            }

            this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
        }

        private string GetSectionTitle()
        {
            if (selectedItem == null)
                return "New FieldFile";
            else
                return "Editing FieldFile: " + selectedItem.Name;
        }

        private void UpdateObjectFromFields()
        {
            selectedItem.Name = Name.Text;
            selectedItem.PathToFile = PathToFile.GetValue().ToString();
            selectedItem.Description = ParserHelper.ParseData(Description.Text, TemplateVars, true);
        }

        private void UpdateFieldsFromObject()
        {
            Name.Text = selectedItem.Name;
            PathToFile.SetValue(selectedItem.PathToFile);

            Description.Text = ParserHelper.ParseData(selectedItem.Description, TemplateVars);
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (selectedItem == null)
                selectedItem = FieldFilesMapper.CreateObject();
            else
                selectedItem = BaseMapper.GetObjectFromContext<FieldFile>(selectedItem);

            UpdateObjectFromFields();

            Return returnObj;

            if (selectedItem.ID == 0)
                returnObj = FieldFilesMapper.Insert(selectedItem);
            else
                returnObj = FieldFilesMapper.Update(selectedItem);

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