using IdentityModel;
using IdentityServer4.Validation;
using IdentityServerV4.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServerV4.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var appUser = await _userManager.FindByEmailAsync(context.UserName);

            if (appUser == null)
            {
                var errors = new Dictionary<string, object>
                {
                    { "errors", new List<string> { "Wrong email or password" } }
                };

                context.Result.CustomResponse = errors;

                return;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(appUser, context.Password);


            if (!passwordCheck)
            {
                var errors = new Dictionary<string, object>
                {
                    { "errors", new List<string> { "Wrong email or password" } }
                };

                context.Result.CustomResponse = errors;

                return;
            }

            context.Result = new GrantValidationResult(
                subject: appUser.Id.ToString(),
                authenticationMethod: OidcConstants.AuthenticationMethods.Password);
        }
    }
}