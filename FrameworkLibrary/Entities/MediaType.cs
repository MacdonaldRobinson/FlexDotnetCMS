using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FrameworkLibrary
{
    public partial class MediaType : IMustContainID
    {
        public Return Validate()
        {
            var returnOnj = BaseMapper.GenerateReturn();
            MediaTypeHandler = MediaTypeHandler.Trim();

            if (!File.Exists(URIHelper.ConvertToAbsPath(MediaTypeHandler)))
            {
                var ex = new Exception("Media Type MediaTypeHandler is invalid", new Exception("File (" + MediaTypeHandler + ") does not exist"));
                returnOnj.Error = ErrorHelper.CreateError(ex);
            }

            return returnOnj;
        }

        public IEnumerable<Role> GetRoles()
        {
            return MediaTypesRoles.Select(mediaTypeRole => RolesMapper.GetByID(mediaTypeRole.RoleID));
        }

        public void AddChildMediaTypes(IEnumerable<MediaType> items)
        {
            MediaTypes.Clear();

            foreach (MediaType item in items.Where(item => MediaTypes.Where(i => i.ID == item.ID).Count() == 0))
            {
                MediaTypes.Add(BaseMapper.GetObjectFromContext(item));
            }
        }

        public void AddRolesPermissions(Dictionary<Role, IEnumerable<Permission>> rolesPermissions)
        {
            var roles = MediaTypesRoles;

            foreach (var mediaTypeRole in roles)
                MediaTypesRolesMapper.DeletePermanently(mediaTypeRole);

            foreach (var rolePermissions in rolesPermissions)
            {
                if (MediaTypesRoles.Where(i => i.RoleID == rolePermissions.Key.ID).Count() != 0) continue;
                var mediaTypeRole = new MediaTypeRole { MediaTypeID = ID, RoleID = rolePermissions.Key.ID };

                mediaTypeRole.DateCreated = mediaTypeRole.DateLastModified = DateTime.Now;

                foreach (var mediaTypeRolesPermission in rolePermissions.Value.Select(permission => new MediaTypeRolePermission
                                                                                                        {
                                                                                                            MediaTypeRoleID = mediaTypeRole.RoleID,
                                                                                                            PermissionID = permission.ID
                                                                                                        }))
                {
                    mediaTypeRolesPermission.DateCreated = mediaTypeRolesPermission.DateLastModified = DateTime.Now;

                    mediaTypeRole.MediaTypeRolesPermissions.Add(mediaTypeRolesPermission);
                }

                MediaTypesRoles.Add(mediaTypeRole);
            }
        }
    }
}