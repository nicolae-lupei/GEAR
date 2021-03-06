﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GR.Core;
using GR.Core.Attributes.Documentation;
using GR.Core.Helpers;
using GR.Core.Helpers.Global;
using GR.Core.Helpers.Responses;
using GR.Identity.Abstractions;
using GR.Identity.Abstractions.Events;
using GR.Identity.Abstractions.Events.EventArgs.Authorization;
using GR.Identity.Abstractions.Extensions;
using GR.Identity.Abstractions.Models.MultiTenants;
using GR.Identity.Abstractions.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GR.Identity
{
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    public class AuthorizeService : IAuthorizeService
    {
        #region Injectable

        /// <summary>
        /// Inject SignIn Manager
        /// </summary>
        private readonly SignInManager<GearUser> _signInManager;

        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        /// <summary>
        /// Inject logger
        /// </summary>
        private readonly ILogger _logger;

        #endregion

        public AuthorizeService(SignInManager<GearUser> signInManager, IUserManager<GearUser> userManager, ILogger<AuthorizeService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> LoginAsync(LoginViewModel model)
        {
            var response = new ResultModel();
            var user = await _userManager.UserManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
            if (user != null)
            {
                if (user.IsDeleted)
                {
                    response.Errors.Add(new ErrorModel(string.Empty, "The user is deleted and cannot log in"));
                    return response;
                }

                if (user.IsPasswordExpired() && !await _userManager.UserManager.IsInRoleAsync(user, GlobalResources.Roles.ADMINISTRATOR))
                {
                    response.Errors.Add(new ErrorModel(string.Empty,
                        "Password has been expired, you need to change the password"));
                    return response;
                }


                var result =
                    await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe,
                        false);

                if (result.Succeeded)
                {
                    user.LastLogin = DateTime.Now;
                    await _userManager.UserManager.UpdateAsync(user);
                    _logger.LogInformation("User logged in.");
                    IdentityEvents.Authorization.UserLogin(new UserLogInEventArgs
                    {
                        IpAdress = _userManager.GetRequestIpAdress(),
                        UserId = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    });

                    var claims = await _userManager.UserManager.GetClaimsAsync(user);
                    var exist = claims.Any(x => x.Type.Equals(nameof(Tenant).ToLowerInvariant()));
                    if (!exist)
                    {
                        var claim = new Claim(nameof(Tenant).ToLowerInvariant(), user.TenantId?.ToString());
                        await _userManager.UserManager.AddClaimAsync(user, claim);
                    }

                    response.IsSuccess = true;
                    return response;
                }

                response.Errors.Add(new ErrorModel(string.Empty, "Invalid login attempt."));
                return response;
            }

            response.Errors.Add(new ErrorModel(string.Empty, "Invalid credentials!"));
            return response;
        }

        /// <summary>
        /// Check password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> CheckPasswordAsync(LoginViewModel model)
        {
            var response = new ResultModel();
            var user = await _userManager.UserManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
            if (user == null)
            {
                response.AddError("User not found");
                return response;
            }
            var checkPassword = await _userManager.UserManager.CheckPasswordAsync(user, model.Password);
            response.IsSuccess = checkPassword;
            return response;
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel> LogoutAsync()
        {
            var result = new ResultModel();
            var userReq = await _userManager.GetCurrentUserAsync();
            if (!userReq.IsSuccess)
            {
                result.Errors.Add(new ErrorModel(nameof(AuthorizationFailure), "Error on logout!!"));
                return result;
            }

            var user = userReq.Result;

            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Errors.Add(new ErrorModel(nameof(Exception), e.Message));
                return result;
            }

            IdentityEvents.Authorization.UserLogout(new UserLogOutEventArgs
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IpAdress = _userManager.GetRequestIpAdress()
            });

            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<AuthenticationScheme>>> GetAuthenticationSchemes()
        {
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return new SuccessResultModel<IEnumerable<AuthenticationScheme>>(schemes);
        }

        #region Helpers

        /// <summary>
        /// Remove claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResultModel> ClearUserClaimsAsync(GearUser user)
        {
            var response = new ResultModel();
            if (user == null) return response;
            var claims = await _userManager.UserManager.GetClaimsAsync(user);
            var identityResult = await _userManager.UserManager.RemoveClaimsAsync(user, claims);
            if (identityResult.Succeeded)
            {
                response.IsSuccess = identityResult.Succeeded;
                return response;
            }

            response.AppendIdentityErrors(identityResult.Errors);
            return response;
        }

        #endregion
    }
}
