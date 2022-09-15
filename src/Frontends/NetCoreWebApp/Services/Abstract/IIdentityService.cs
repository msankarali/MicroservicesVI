using IdentityModel.Client;
using NetCoreWebApp.Dtos;
using Utilities.Results;

namespace NetCoreWebApp.Services.Abstract
{
    public interface IIdentityService
    {
        Task<ApiResponse<bool>> SignIn(SignInDto signInDto);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}