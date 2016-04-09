using FrameworkLibrary;
using System;
using System.Collections.Generic;

namespace WebApplication.Admin.Views.Roles
{
    public partial class Detail : AdvanceOptionsBasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            PermissionsSelector.ShowPermissions(PermissionsMapper.GetAll());
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (SelectedItem != null)
            {
                if (!IsPostBack)
                    UpdateFieldsFromObject();
            }

            this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
        }

        public Role SelectedItem
        {
            get
            {
                var user = (Role)ContextHelper.GetFromRequestContext("EditSelectedItem");

                if (user == null)
                {
                    long id;

                    if (long.TryParse(Request["id"], out id))
                    {
                        user = RolesMapper.GetByID(id);
                        ContextHelper.SetToRequestContext("EditSelectedItem", user);
                    }
                }

                return user;
            }
            set
            {
                ContextHelper.SetToRequestContext("EditSelectedItem", value);
            }
        }

        private string GetSectionTitle()
        {
            if (SelectedItem == null)
                return "New Role";
            else
                return "Editing Role: " + SelectedItem.Name;
        }

        private void UpdateObjectFromFields()
        {
            SelectedItem.IsActive = IsActive.Checked;
            SelectedItem.Name = Name.Text;
            SelectedItem.Description = Description.Text;
            SelectedItem.EnumName = EnumName.Text;

            IEnumerable<Permission> items = PermissionsSelector.GetSelectedPermissions();

            SelectedItem.Permissions.Clear();

            foreach (Permission item in items)
                SelectedItem.Permissions.Add(BaseMapper.GetObjectFromContext(item));
        }

        private void UpdateFieldsFromObject()
        {
            Name.Text = SelectedItem.Name;
            Description.Text = SelectedItem.Description;
            IsActive.Checked = SelectedItem.IsActive;
            EnumName.Text = SelectedItem.EnumName;

            PermissionsSelector.SetSelectedPermissions(SelectedItem.Permissions);
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (!canAccessSection)
                return;

            if (SelectedItem == null)
                SelectedItem = RolesMapper.CreateObject();
            else
                SelectedItem = BaseMapper.GetObjectFromContext<Role>(SelectedItem);

            UpdateObjectFromFields();

            Return returnObj;

            if (SelectedItem.ID == 0)
                returnObj = RolesMapper.Insert(SelectedItem);
            else
                returnObj = RolesMapper.Update(SelectedItem);

            if (returnObj.IsError)
                DisplayErrorMessage("Error Saving Item", returnObj.Error);
            else
            {
                DisplaySuccessMessage("Successfully Saved Item");
                UpdateFieldsFromObject();
            }
        }
    }
}