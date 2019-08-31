using System.Collections.Generic;
using ST.Identity.Permissions.Abstractions;

namespace ST.Identity.Razor.Users.Permissions
{
    public class IdentityUsersPermissions
    {
        public const string BpmUserCreate = "Core_UserCreate";
        public const string BpmUserRead = "Core_UserRead";
        public const string BpmUserUpdate = "Core_UserUpdate";
        public const string BpmUserDelete = "Core_UserDelete";
    }

    public class IdentityUsersPermissionsConfigurator : PermissionModuleConfigurator
    {
        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetPermissions()
        {
            return GetModulePermissionsFromTargetModule(typeof(IdentityUsersPermissions));
        }
    }
}
