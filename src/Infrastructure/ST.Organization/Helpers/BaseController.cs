using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST.Audit.Extensions;
using ST.Entities.Data;
using ST.Identity.Data;
using ST.Identity.Data.MultiTenants;
using ST.Identity.Data.Permissions;
using ST.Identity.Data.UserProfiles;
using ST.Identity.Services.Abstractions;
using ST.MultiTenant.Services.Abstractions;
using ST.Notifications.Abstractions;

namespace ST.MultiTenant.Helpers
{
    [Authorize]
    public class BaseController : Controller, ITenant
    {
        /// <summary>
        /// Inject organization dataService
        /// </summary>
        protected readonly IOrganizationService OrganizationService;

        /// <summary>
        /// Inject notifier
        /// </summary>
        protected readonly INotify<ApplicationRole> Notify;

        /// <summary>
        /// Entity DbContext
        /// </summary>
        protected readonly EntitiesDbContext Context;

        /// <summary>
        /// Application DbContext 
        /// </summary>
        protected readonly ApplicationDbContext ApplicationDbContext;

        /// <summary>
        /// Inject UserManager
        /// </summary>
        protected readonly UserManager<ApplicationUser> UserManager;

        /// <summary>
        /// Inject RoleManager
        /// </summary>
        protected readonly RoleManager<ApplicationRole> RoleManager;

        /// <summary>
        /// Cache Service
        /// </summary>
        protected readonly ICacheService CacheService;


        /// <inheritdoc />
        /// <summary>
        /// Tenant id
        /// </summary>
        public Guid? CurrentUserTenantId
        {
            get
            {
                var tenantId = User?.Claims?.FirstOrDefault(x => x.Type == "tenant")?.Value?.ToGuid();
                if (tenantId != null) return tenantId;
                var user = UserManager.GetUserAsync(User).GetAwaiter().GetResult();
                if (user == null) return null;
                UserManager.AddClaimAsync(user, new Claim("tenant", user.TenantId.ToString())).GetAwaiter()
                    .GetResult();
                return user.TenantId;

            }
        }

        public BaseController(EntitiesDbContext context, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, INotify<ApplicationRole> notify, IOrganizationService organizationService, ICacheService cacheService)
        {
            Context = context;
            ApplicationDbContext = applicationDbContext;
            UserManager = userManager;
            RoleManager = roleManager;
            Notify = notify;
            OrganizationService = organizationService;
            CacheService = cacheService;
        }


        /// <summary>
        /// Get Current User async
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await UserManager.GetUserAsync(User);
        }

        /// <summary>
        /// Get current user
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected ApplicationUser GetCurrentUser()
        {
            return GetCurrentUserAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get User organization
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected async Task<Tenant> GetOrganizationOfUser()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return default;
            return await ApplicationDbContext.Tenants.FirstOrDefaultAsync(x => x.Id == user.TenantId);
        }
    }
}