using FrameworkLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class PermissionsSelector : System.Web.UI.UserControl
    {
        private IEnumerable<Permission> showPermissions = new List<Permission>();
        private IEnumerable<Permission> selectedPermissions = new List<Permission>();

        private void Bind()
        {
            ItemsList.DataSource = showPermissions.ToList();
            ItemsList.DataTextField = "Name";
            ItemsList.DataValueField = "ID";
            ItemsList.DataBind();

            BindSelectedPermissions();
        }

        private void BindSelectedPermissions()
        {
            foreach (ListItem item in ItemsList.Items)
            {
                if (selectedPermissions.Where(i => i.ID.ToString() == item.Value).Count() > 0)
                    item.Selected = true;
            }
        }

        public IEnumerable<Permission> GetSelectedPermissions()
        {
            var selectedPermissions = new List<Permission>();

            foreach (ListItem item in ItemsList.Items)
            {
                if (item.Selected)
                    selectedPermissions.Add(PermissionsMapper.GetByID(long.Parse(item.Value)));
            }

            return selectedPermissions;
        }

        public void ClearSelection()
        {
            foreach (ListItem item in ItemsList.Items)
            {
                item.Selected = false;
            }
        }

        public void ShowPermissions(IEnumerable<Permission> showPermissions)
        {
            this.showPermissions = showPermissions;
            Bind();
        }

        public void SetSelectedPermissions(IEnumerable<Permission> selectedPermissions)
        {
            this.selectedPermissions = selectedPermissions;
            BindSelectedPermissions();
        }
    }
}