using System;
using System.Collections.Generic;
using System.Data.Entity.Core;

using System.Linq;

namespace FrameworkLibrary
{
    public class RolesMediaDetailsMapper : BaseMapper
    {
        private const string MapperKey = "RolesMediaDetailsMapperKey";

        public static IEnumerable<RoleMediaDetail> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().RolesMediaDetails.OrderByDescending(i => i.DateCreated));
        }

        public static RoleMediaDetail GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static IEnumerable<RoleMediaDetail> GetByRole(Role role, IMediaDetail mediaDetail)
        {
            var allItems = mediaDetail.RolesMediaDetails.Where(i => i.RoleID == role.ID);

            return allItems;
        }

        public static IEnumerable<Permission> GetRolePermissions(Role role, IMediaDetail mediaDetail)
        {
            var allItems = GetByRole(role, mediaDetail);

            return (from item in allItems where item.Permission.IsActive select item.Permission);
        }

        public static IEnumerable<RoleMediaDetail> GetAllWithPermission(Permission permission)
        {
            var items = GetAll();

            return items.Where(item => item.PermissionID == permission.ID);
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

        public static IEnumerable<Role> GetRoles(IEnumerable<RoleMediaDetail> roleMediaDetails)
        {
            var roles = new List<Role>();

            foreach (var role in roleMediaDetails.Select(item => RolesMapper.GetByID(item.RoleID)).Where(role => roles.Where(i => i.ID == role.ID).Count() == 0))
            {
                roles.Add(role);
            }

            return roles;
        }

        public static RoleMediaDetail CreateObject()
        {
            return GetDataModel().RolesMediaDetails.Create();
        }

        public static Return Insert(RoleMediaDetail obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(RoleMediaDetail obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(RoleMediaDetail obj)
        {
            return Delete(MapperKey, obj);
        }
    }
}