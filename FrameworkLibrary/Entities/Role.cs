using System.Linq;

namespace FrameworkLibrary
{
    public partial class Role : IMustContainID
    {
        public bool HasPermission(Permission permission, IMediaDetail mediaDetail)
        {
            var items = RolesMediaDetails.Where(i => i.RoleID == ID && i.MediaDetailID == mediaDetail.ID && i.PermissionID == permission.ID);

            return items.Any();
        }
    }
}