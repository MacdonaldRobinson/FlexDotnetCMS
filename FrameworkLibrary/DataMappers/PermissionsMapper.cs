using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace FrameworkLibrary
{
    public class PermissionsMapper : BaseMapper
    {
        private const string MapperKey = "PermissionsMapperKey";

        public static IEnumerable<Permission> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().Permissions.OrderByDescending(i => i.DateCreated));
        }

        public static IEnumerable<Permission> FilterByActiveStatus(IEnumerable<Permission> items, bool isActive)
        {
            return items.Where(item => item.IsActive == isActive);
        }

        public static IEnumerable<Permission> GetAllActive()
        {
            return FilterByActiveStatus(GetAll(), true);
        }

        public static Permission GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static Permission GetPermissionsFromEnum(PermissionsEnum permissionsEnum)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.EnumName == permissionsEnum.ToString());
        }

        public static IEnumerable<Permission> GetPermissionsInRole(Role role)
        {
            var items = GetAll();
            var filtered = items.Where(item => role.Permissions.Where(i => i.ID == item.ID).Any());

            return filtered.Distinct();
        }

        public static IEnumerable<Permission> GetPermissionsInRoles(IEnumerable<Role> roles)
        {
            var filtered = new List<Permission>();

            foreach (IEnumerable<Permission> permissions in roles.Select(GetPermissionsInRole))
            {
                filtered.AddRange(permissions);
            }

            return filtered.Distinct();
        }

        public static IEnumerable<Permission> GetPermissionsNotInRole(Role role)
        {
            var items = GetAll();

            return items.Where(item => role.Permissions.Where(i => i.ID == item.ID).Count() == 0);
        }

        public static IEnumerable<Permission> GetUserRolesPermissions(User user)
        {
            var permissions = new List<Permission>();

            foreach (var role in user.Roles)
                permissions.AddRange(GetPermissionsInRole(role));

            return permissions.Distinct();
        }

        public static IEnumerable<Permission> GetPermissionsNotInUserRoles(User user)
        {
            var allPermissions = GetAll();
            var rolesPermissions = GetUserRolesPermissions(user);

            return allPermissions.Where(item => rolesPermissions.Where(i => i.ID == item.ID).Count() == 0);
        }

        public static IEnumerable<Permission> FilterOutList(IEnumerable<Permission> list, IEnumerable<Permission> permissionsToRemove)
        {
            return list.Where(permission => permissionsToRemove.Where(i => i.ID == permission.ID).Count() == 0);
        }

        public static IEnumerable<Permission> RemovePermissions(IEnumerable<Permission> permissions, IEnumerable<Permission> permissionsToRemove)
        {
            return permissions.Where(item => permissionsToRemove.Where(i => i.ID == item.ID).Count() == 0);
        }

        public static Permission CreateObject()
        {
            return GetDataModel().Permissions.Create();
        }

        public static Return Insert(Permission obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert<Permission>(MapperKey, obj);
        }

        public static Return Update(Permission obj)
        {
            obj.DateLastModified = DateTime.Now;

            return Update<Permission>(MapperKey, obj);
        }

        public static Return DeletePermanently(Permission obj)
        {
            var roleMediaDetails = RolesMediaDetailsMapper.GetAllWithPermission(obj);

            foreach (RoleMediaDetail entry in roleMediaDetails)
                RolesMediaDetailsMapper.DeletePermanently(entry);

            var userMediaDetails = UsersMediaDetailsMapper.GetAllWithPermission(obj);

            foreach (var entry in userMediaDetails)
                UsersMediaDetailsMapper.DeletePermanently(entry);

            var roles = RolesMapper.GetAllWithPermission(obj);

            foreach (var entry in roles)
                entry.Permissions.Remove(BaseMapper.GetObjectFromContext(obj));

            return Delete(MapperKey, obj);
        }
    }
}