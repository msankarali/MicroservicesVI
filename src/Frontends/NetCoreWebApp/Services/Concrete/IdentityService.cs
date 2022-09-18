using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NetCoreWebApp.Dtos;
using NetCoreWebApp.Services.Abstract;
using NetCoreWebApp.Settings;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Utilities.Results;

namespace NetCoreWebApp.Services.Concrete
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient,
                               IHttpContextAccessor httpContextAccessor,
                               IOptions<ClientSettings> clientSettings,
                               IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task RevokeRefreshToken()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> SignIn(SignInDto signInDto)
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.BaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signInDto.Email,
                Password = signInDto.Password,
                Address = discovery.TokenEndpoint
            };

            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();

                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return ApiResponse<bool>.Fail(400, errorDto.Errors.Aggregate((a, b) => a + ", " + b));
            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = discovery.TokenEndpoint
            };

            var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            var claimsIdentity = new ClaimsIdentity(
                claims: userInfo.Claims,
                authenticationType: CookieAuthenticationDefaults.AuthenticationScheme,
                nameType: "name",
                roleType: "role");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)
                }
            });

            authenticationProperties.IsPersistent = signInDto.IsRemember;

            await _httpContextAccessor.HttpContext!.SignInAsync(
                scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                principal: claimsPrincipal,
                properties: authenticationProperties);

            return ApiResponse<bool>.Success(200);
        }
    }
}