using System.Linq;

namespace FrameworkLibrary
{
    public partial class Role : IMustContainID
    {
        public bool HasPermission(Permission permission, IMediaDetail mediaDetail)
        {
            return mediaDetail.Media.RolesMedias.Select(i => i.Role).Any(i => i.Permissions.Any(j => j.ID == permission.ID));
        }
    }
}