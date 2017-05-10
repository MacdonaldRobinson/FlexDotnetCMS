using System;
using System.Collections.Generic;

using System.Linq;

namespace FrameworkLibrary
{
    public class RolesMapper : BaseMapper
    {
        private const string MapperKey = "RolesMapperKey";

        public static IEnumerable<Role> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().Roles.OrderByDescending(i => i.DateCreated));
        }

        public static IEnumerable<Role> FilterByActiveStatus(IEnumerable<Role> items, bool isActive)
        {
            return items.Where(item => item.IsActive == isActive);
        }

        public static IEnumerable<Role> GetAllActive()
        {
            return FilterByActiveStatus(GetAll(), true);
        }

        public static IEnumerable<Role> RemoveRoles(IEnumerable<Role> roles, IEnumerable<Role> rolesToRemove)
        {
            return roles.Where(item => rolesToRemove.Where(i => i.ID == item.ID).Count() == 0);
        }

        public static IEnumerable<Role> GetAllWithPermission(Permission permission)
        {
            return FilterWithPermission(GetAll(), permission);
        }

        public static IEnumerable<Role> FilterWithPermission(IEnumerable<Role> roles, Permission permission)
        {
            return roles.Where(item => item.IsActive).Where(item => item.Permissions.Where(i => i.ID == permission.ID).Count() != 0);
        }

        public static IEnumerable<Role> GetRoles(IEnumerable<RoleMedia> roleMediaDetails)
        {
            var roles = new List<Role>();

            foreach (var item in roleMediaDetails.Where(item => roles.Where(i => i.ID == item.RoleID).Count() == 0))
            {
                roles.Add(item.Role);
            }

            return roles;
        }

        public static Role GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static RoleEnum GetEnumByName(string name)
        {
            return (RoleEnum)Enum.Parse(typeof(RoleEnum), name);
        }

        public static Role GetByEnum(RoleEnum roleEnum)
        {
            var strRoleEnum = roleEnum.ToString();

            return BaseMapper.GetDataModel().Roles.FirstOrDefault(i => i.Name == strRoleEnum);
            //var allItems = GetAll();
            //return allItems.FirstOrDefault(item => item.Name == roleEnum.ToString());
        }

        public static string[] GetUserRolesAsArray(User user)
        {
            var roles = GetUserRoles(user);

            return roles.Select(item => item.Name).ToArray();
        }

        public static IEnumerable<Role> GetUserRoles(User user)
        {
            var allItems = GetAll();
            var userRoles = new List<Role>();

            foreach (var item in allItems)
            {
                userRoles.AddRange(from usr in item.Users where usr.ID == user.ID select item);
            }

            return userRoles;
        }

        public static Role CreateObject()
        {
            return GetDataModel().Roles.Create();
        }

        public static Return Insert(Role obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(Role obj)
        {
            obj.DateLastModified = DateTime.Now;

            return Update(MapperKey, obj);
        }

        public static void RemoveFromMediaDetails(Role obj)
        {
            var items = obj.RolesMedias;

            foreach (var item in items)
                GetDataModel().RolesMedias.Remove(item);
        }

        public static Return DeletePermanently(Role obj)
        {
            obj.Permissions.Clear();
            obj.MediaTypesRoles.Clear();

            RemoveFromMediaDetails(obj);

            return Delete(MapperKey, obj);
        }

        public static void ClearUserRoles(User user)
        {
            var allItems = GetUserRoles(user);

            foreach (var item in allItems)
            {
                foreach (var usr in item.Users.Where(usr => usr.ID == user.ID))
                {
                    item.Users.Remove(usr);
                }
            }
        }
    }
}