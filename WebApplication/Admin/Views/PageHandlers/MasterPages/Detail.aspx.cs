using FrameworkLibrary;
using System;
using MasterPage = FrameworkLibrary.MasterPage;

namespace WebApplication.Admin.Views.PageHandlers.MasterPages
{
    public partial class Detail : AdminBasePage
    {
        private MasterPage selectedItem = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            long id;

            if (long.TryParse(Request["id"], out id))
            {
                selectedItem = MasterPagesMapper.GetByID(id);

                if (!IsPostBack)
                    UpdateFieldsFromObject();
            }

            this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
        }

        private string GetSectionTitle()
        {
            if (selectedItem == null)
                return "New MasterPage";
            else
                return "Editing MasterPage: " + selectedItem.Name;
        }

        private void UpdateObjectFromFields()
        {
            selectedItem.Name = Name.Text;
            selectedItem.PathToFile = PathToFile.GetValue().ToString();
            selectedItem.MobileTemplate = MobileTemplate.GetValue().ToString();
        }

        private void UpdateFieldsFromObject()
        {
            Name.Text = selectedItem.Name;
            PathToFile.SetValue(selectedItem.PathToFile);
            MobileTemplate.SetValue(selectedItem.MobileTemplate);
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (selectedItem == null)
                selectedItem = MasterPagesMapper.CreateObject();
            else
                selectedItem = BaseMapper.GetObjectFromContext<MasterPage>(selectedItem);

            UpdateObjectFromFields();

            Return returnObj = selectedItem.Validate();

            if (!returnObj.IsError)
            {
                if (selectedItem.ID == 0)
                    returnObj = MasterPagesMapper.Insert(selectedItem);
                else
                    returnObj = MasterPagesMapper.Update(selectedItem);
            }

            if (returnObj.IsError)
                DisplayErrorMessage("Error Saving Item", returnObj.Error);
            else
            {
                DisplaySuccessMessage("Successfully Saved Item");
            }
        }
    }
}