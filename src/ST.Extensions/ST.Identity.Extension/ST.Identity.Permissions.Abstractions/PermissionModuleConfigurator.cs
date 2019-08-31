using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ST.Core.Helpers;
using ST.Identity.Abstractions;

namespace ST.Identity.Permissions.Abstractions
{
    public abstract class PermissionModuleConfigurator : IPermissionConfigurator
    {
        /// <summary>
        /// Inject identity context
        /// </summary>
        protected IIdentityContext IdentityContext => IoC.Resolve<IIdentityContext>();

        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<string> GetPermissions();

        /// <summary>
        /// Get module permissions
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetModulePermissionsFromTargetModule(Type permissionsConstantsClassType)
        {
            var fieldInfo = permissionsConstantsClassType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var permissions = fieldInfo.Select(_ => _.GetValue(null).ToString()).ToList();
            return permissions;
        }


        public virtual async Task SeedModulePermissions()
        {
            var permissions = GetPermissions();
            if (permissions.Any()) return;
            var dbPermissions = await IdentityContext.Permissions.Where(q => !q.IsDeleted).ToListAsync();
        }
    }
}
