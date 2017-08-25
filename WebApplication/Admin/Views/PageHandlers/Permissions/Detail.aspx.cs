using FrameworkLibrary;
using System;

namespace WebApplication.Admin.Views.PageHandlers.Permissions
{
    public partial class Detail : AdvanceOptionsBasePage
    {
        private Permission selectedItem = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            long id;

            if (long.TryParse(Request["id"], out id))
            {
                selectedItem = PermissionsMapper.GetByID(id);

                if (!IsPostBack)
                    UpdateFieldsFromObject();
            }

            this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
        }

        private string GetSectionTitle()
        {
            if (selectedItem == null)
                return "New Permission";
            else
                return "Editing Permission: " + selectedItem.Name;
        }

        private void UpdateObjectFromFields()
        {
            selectedItem.Name = Name.Text;
            selectedItem.Description = Description.Text;
            selectedItem.EnumName = EnumName.Text;
            selectedItem.IsActive = IsActive.Checked;
        }

        private void UpdateFieldsFromObject()
        {
            Name.Text = selectedItem.Name;
            Description.Text = selectedItem.Description;
            EnumName.Text = selectedItem.EnumName;
            IsActive.Checked = selectedItem.IsActive;
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (!canAccessSection)
                return;

            if (selectedItem == null)
                selectedItem = PermissionsMapper.CreateObject();
            else
                selectedItem = BaseMapper.GetObjectFromContext<Permission>(selectedItem);

            UpdateObjectFromFields();

            Return returnObj;

            if (selectedItem.ID == 0)
                returnObj = PermissionsMapper.Insert(selectedItem);
            else
                returnObj = PermissionsMapper.Update(selectedItem);

            if (returnObj.IsError)
                DisplayErrorMessage("Error Saving Item", returnObj.Error);
            else
                DisplaySuccessMessage("Successfully Saved Item");
        }
    }
}