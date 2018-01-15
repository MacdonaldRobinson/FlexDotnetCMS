using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class UsersMapper : BaseMapper
    {
        private const string MapperKey = "UsersMapperKey";

        /*public static IEnumerable<User> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().Users.OrderByDescending(i => i.DateCreated));
        }*/

        public static User GetUserByCredentials(string username, string password)
        {
            var encryptedPassword = StringHelper.Encrypt(password);

            return GetDataModel().Users.FirstOrDefault(item => (username == item.UserName) && (encryptedPassword == item.Password) && (item.IsActive));
        }

        public static User GetByEmailAddress(string emailAddress)
        {
            return GetDataModel().Users.FirstOrDefault(item => item.EmailAddress == emailAddress);
        }

        public static Return TransferOwnership(User fromUser, User toUser)
        {
            var obj = GenerateReturn();

            var userMedias = fromUser.UsersMedias.ToList();
            var createdMediaDetails = fromUser.CreatedMediaDetails.ToList();
            var lastUpdatedMediaDetails = fromUser.LastUpdatedMediaDetails.ToList();

            foreach (var userMediaDetail in userMedias)
            {
                userMediaDetail.UserID = toUser.ID;
                obj = UsersMediasMapper.Update(userMediaDetail);
            }

            foreach (var createdMediadetail in createdMediaDetails)
            {
                createdMediadetail.CreatedByUserID = toUser.ID;
                obj = MediaDetailsMapper.Update(createdMediadetail);
            }

            foreach (var lastUpdatedMediaDetail in lastUpdatedMediaDetails)
            {
                lastUpdatedMediaDetail.LastUpdatedByUserID = toUser.ID;
                obj = MediaDetailsMapper.Update(lastUpdatedMediaDetail);
            }

            return obj;
        }

        public static User GetByID(long id)
        {
            return GetDataModel().Users.FirstOrDefault(item => item.ID == id);
        }

        public static User GetByUserName(string username)
        {
            return GetDataModel()?.Users.FirstOrDefault(item => item.UserName == username);
        }

        public static IEnumerable<User> FilterByActiveStatus(IEnumerable<User> items, bool isActive)
        {
            return items.Where(item => item.IsActive == isActive);
        }

        public static IEnumerable<User> GetAllActive()
        {
            return GetDataModel().Users.Where(i => i.IsActive);
        }

        public static IEnumerable<User> GetAllByRole(Role role, IEnumerable<User> ommitUsers = null)
        {
            return GetDataModel().Users.ToList().Where(item => IsUserInRole(item, role)).Where(item => (ommitUsers == null) || (ommitUsers.Where(i => i.ID == item.ID).Count() == 0));
        }

        public static IEnumerable<User> GetAllByRoles(IEnumerable<Role> roles, IEnumerable<User> ommitUsers = null)
        {
            var filtered = new List<User>();

            foreach (var role in roles)
                filtered.AddRange(GetAllByRole(role, ommitUsers));

            return filtered.Distinct();
        }

        public static IEnumerable<User> GetAllByRoleEnum(RoleEnum roleEnum)
        {
            return GetAllByRole(RolesMapper.GetByEnum(roleEnum));
        }

        public static bool IsUserInRole(User user, Role role)
        {
            return user.Roles.Where(i => i.ID == role.ID).Any();
        }

        public static bool HasPermission(PermissionsEnum permissionsEnum, User user)
        {
            var permission = PermissionsMapper.GetPermissionsFromEnum(permissionsEnum);

            if (permission == null)
                return false;

            var roles = RolesMapper.FilterWithPermission(user.Roles, permission);

            return roles.Any();
        }

        public static bool IsUserInRoles(User user, IEnumerable<Role> roles)
        {
            return roles.Any(role => IsUserInRole(user, role));
        }

        public static User CreateObject()
        {
            return GetDataModel().Users.Create();
        }

        public static Return Insert(User obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(User obj)
        {
            obj.DateLastModified = DateTime.Now;

            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(User obj)
        {
            obj.Roles.Clear();
            return Delete(MapperKey, obj);
        }
    }
}