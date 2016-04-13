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
            if (!IsPostBack)
            {
                MultiRoleSelector.SetSelectedRoles(this.selectedItem.RolesMediaDetails.Select(i => i.Role).ToList());
            }
        }

        public void UpdateObjectFromFields()
        {
            var roles = MultiRoleSelector.GetSelectedRoles();

            if (roles == null)
                return;

            var existingRoles = selectedItem.RolesMediaDetails.ToList();

            foreach (var existingRole in existingRoles)
            {
                BaseMapper.GetDataModel().RolesMediaDetails.Remove(existingRole);
            }

            foreach (var role in roles)
            {
                var contextRole = BaseMapper.GetObjectFromContext(role);

                BaseMapper.GetDataModel().RolesMediaDetails.Add(new RoleMediaDetail() { MediaDetail = (MediaDetail)selectedItem, Role = contextRole, DateCreated = DateTime.Now, DateLastModified = DateTime.Now });
                //this.selectedItem.RolesMediaDetails.Add(new RoleMediaDetail() { Role = contextRole, DateCreated = DateTime.Now, DateLastModified = DateTime.Now });
            }
        }
    }
}