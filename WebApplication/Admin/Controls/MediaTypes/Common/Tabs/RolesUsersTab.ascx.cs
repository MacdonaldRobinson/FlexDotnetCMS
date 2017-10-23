using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class RolesUsersTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public void UpdateFieldsFromObject()
        {
            CanLimitedRolesAccessAllChildPages.Checked = selectedItem.CanLimitedRolesAccessAllChildPages;
            EnforceRoleLimitationOnFrontEnd.Checked = selectedItem.EnforceRoleLimitationsOnFrontEnd;            

            if (!IsPostBack)
            {
                MultiRoleSelector.SetSelectedRoles(this.selectedItem.Media.RolesMedias.Select(i => i.Role).ToList());
            }
        }

        public void UpdateObjectFromFields()
        {
            selectedItem.CanLimitedRolesAccessAllChildPages = CanLimitedRolesAccessAllChildPages.Checked;
            selectedItem.EnforceRoleLimitationsOnFrontEnd = EnforceRoleLimitationOnFrontEnd.Checked;

            var roles = MultiRoleSelector.GetSelectedRoles();

            if (roles == null)
                return;

            var existingRoles = selectedItem.Media.RolesMedias.ToList();

            foreach (var existingRole in existingRoles)
            {
                BaseMapper.GetDataModel().RolesMedias.Remove(existingRole);
            }

            foreach (var role in roles)
            {
                var contextRole = BaseMapper.GetObjectFromContext(role);

                BaseMapper.GetDataModel().RolesMedias.Add(new RoleMedia() { Media = selectedItem.Media, Role = contextRole, DateCreated = DateTime.Now, DateLastModified = DateTime.Now });
                //this.selectedItem.RolesMediaDetails.Add(new RoleMediaDetail() { Role = contextRole, DateCreated = DateTime.Now, DateLastModified = DateTime.Now });
            }
        }
    }
}