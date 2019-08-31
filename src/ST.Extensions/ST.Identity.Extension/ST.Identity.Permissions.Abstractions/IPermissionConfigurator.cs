using System;
using System.Collections.Generic;

namespace ST.Identity.Permissions.Abstractions
{
    public interface IPermissionConfigurator
    {
        /// <summary>
        /// Module permission list
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetModulePermissionsFromTargetModule(Type permissionsConstantsClassType);

        IEnumerable<string> GetPermissions();
    }
}