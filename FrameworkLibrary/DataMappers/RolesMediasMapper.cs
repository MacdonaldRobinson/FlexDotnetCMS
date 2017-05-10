using System;
using System.Collections.Generic;

using System.Linq;

namespace FrameworkLibrary
{
    public class RolesMediasMapper : BaseMapper
    {
        private const string MapperKey = "RolesMediaDetailsMapperKey";

        public static IEnumerable<RoleMedia> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().RolesMedias.OrderByDescending(i => i.DateCreated));
        }

        public static RoleMedia GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static IEnumerable<RoleMedia> GetByRole(Role role, IMediaDetail mediaDetail)
        {
            var allItems = mediaDetail.Media.RolesMedias.Where(i => i.RoleID == role.ID);

            return allItems;
        }

        public static IEnumerable<Permission> GetRolePermissions(Role role, IMediaDetail mediaDetail)
        {
            var allItems = GetByRole(role, mediaDetail);

            return allItems.SelectMany(i => i.Role.Permissions.Where(j => j.IsActive));
        }

        public static IEnumerable<RoleMedia> GetAllWithPermission(Permission permission)
        {
            var items = GetAll();

            return items.Where(item => item.Role.Permissions.Any(i => i.ID == permission.ID));
        }

        public static IEnumerable<Permission> GetRolesPermissions(IEnumerable<Role> roles, IMediaDetail mediaDetail)
        {
            var permissions = new List<Permission>();
            foreach (var role in roles)
            {
                permissions.AddRange(GetRolePermissions(role, mediaDetail));
            }

            return permissions.Distinct();
        }

        public static IEnumerable<Role> GetRoles(IEnumerable<RoleMedia> roleMediaDetails)
        {
            var roles = new List<Role>();

            foreach (var role in roleMediaDetails.Select(item => RolesMapper.GetByID(item.RoleID)).Where(role => roles.Where(i => i.ID == role.ID).Count() == 0))
            {
                roles.Add(role);
            }

            return roles;
        }

        public static RoleMedia CreateObject()
        {
            return GetDataModel().RolesMedias.Create();
        }

        public static Return Insert(RoleMedia obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(RoleMedia obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(RoleMedia obj)
        {
            return Delete(MapperKey, obj);
        }
    }
}