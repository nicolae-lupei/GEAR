﻿using System.Threading.Tasks;
using GR.Identity.Clients.Abstractions;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GR.Identity.Clients.Infrastructure
{
    public class ClientsService : IClientsService
    {
        #region Injectable
        private readonly IConfigurationDbContext _configurationDbContext;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurationDbContext"></param>
        public ClientsService(IConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        /// <summary>
        /// Get app name
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public async Task<string> GetAppName(string appId)
        {
            var appName = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId.Equals("core"));
            return appName?.ClientName ?? "No app found";
        }
    }
}