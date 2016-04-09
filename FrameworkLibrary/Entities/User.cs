using System.Collections.Generic;
using System.Security.Principal;

namespace FrameworkLibrary
{
    public partial class User : IMustContainID, IPrincipal
    {
        private readonly CustomIdentity _identity;

        public User(CustomIdentity identity)
        {
            _identity = identity;
            AuthenticationType = identity.AuthenticationType;
            UserName = identity.Name;
            Password = "";
            EmailAddress = "";
        }

        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        public bool HasPermission(PermissionsEnum permissionsEnum)
        {
            return UsersMapper.HasPermission(permissionsEnum, this);
        }

        public bool IsInRole(Role role)
        {
            return UsersMapper.IsUserInRole(this, role);
        }

        public bool IsInRoles(IEnumerable<Role> roles)
        {
            return UsersMapper.IsUserInRoles(this, roles);
        }

        public bool IsInRole(RoleEnum roleEnum)
        {
            return IsInRole(RolesMapper.GetByEnum(roleEnum));
        }

        public bool IsInRole(string role)
        {
            return IsInRole(RolesMapper.GetEnumByName(role));
        }

        public Return Validate()
        {
            var returnObj = BaseMapper.GenerateReturn();

            if (Roles.Count == 0)
            {
                returnObj.Error = ErrorHelper.CreateError(new System.Exception("Validation Error", new System.Exception("Must be assigned to at lease one role")));
            }

            return returnObj;
        }
    }
}