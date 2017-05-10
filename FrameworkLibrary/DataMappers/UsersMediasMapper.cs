using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace FrameworkLibrary
{
    public class UsersMediasMapper : BaseMapper
    {
        private const string MapperKey = "UsersMediaDetailsMapperKey";

        public static IEnumerable<UserMedia> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().UsersMedias.OrderByDescending(i => i.DateCreated));
        }

        public static UserMedia GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static IEnumerable<User> GetUsers(IEnumerable<UserMedia> userMediaDetails)
        {
            var users = new List<User>();

            foreach (var user in userMediaDetails.Select(item => UsersMapper.GetByID(item.UserID)).Where(user => user != null).Where(user => users.Where(i => i.ID == user.ID).Count() == 0))
            {
                users.Add(user);
            }

            return users;
        }

        public static IEnumerable<UserMedia> GetAllWithPermission(Permission permission)
        {
            var items = GetAll();

            return items.Where(item => item.PermissionID == permission.ID);
        }

        public static IEnumerable<UserMedia> GetByUser(User user, IMediaDetail mediaDetail)
        {
            var allItems = mediaDetail.Media.UsersMedias.Where(i => i.UserID == user.ID);

            return allItems;
        }

        public static IEnumerable<Permission> GetUserRolesPermissions(User user, IMediaDetail mediaDetail)
        {
            return RolesMediasMapper.GetRolesPermissions(user.Roles, mediaDetail);
        }

        public static IEnumerable<Permission> GetNotInRolesPermissions(User user, IMediaDetail mediaDetail)
        {
            return PermissionsMapper.FilterOutList(PermissionsMapper.GetAllActive(), GetUserRolesPermissions(user, mediaDetail));
        }

        public static IEnumerable<Permission> GetUserPermissions(User user, IMediaDetail mediaDetail)
        {
            var allItems = GetByUser(user, mediaDetail);

            return (from item in allItems where item.Permission.IsActive select item.Permission);
        }

        public static UserMedia CreateObject()
        {
            return GetDataModel().UsersMedias.Create();
        }

        public static Return Insert(UserMedia obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(UserMedia obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(UserMedia obj)
        {
            return Delete(MapperKey, obj);
        }
    }
}