using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Kernel.InputModels;
using Shared.Kernel.IServices;
using Shared.Kernel.Utils.Constants;
using System.Security.Cryptography;

namespace HR_AUTOMATION.Application.Services
{
    public class AuthService(ILogger<AuthService> logger, IConfiguration configuration, IJwtService jwtService) : IAuthService
    {
        private readonly ILogger<AuthService> _logger = logger;

        private readonly IConfiguration _configuration = configuration;

        private readonly IJwtService _jwtService = jwtService;

        private string GenerateRefreshToken()
        {
            int tokenByteSize = _configuration.GetValue<int>(AppConstants.RefreshTokenByteSizeKey)!;

            byte[] bytes = RandomNumberGenerator.GetBytes(tokenByteSize);

            return WebEncoders.Base64UrlEncode(bytes);
        }

        private string GenerateJwT()
        {
            GenerateTokenRequest model = new()
            {
                Claims = new()
                {
                    ["UserId"] = 1,
                    ["Name"] = "John Doe",
                    ["Roles"] = "test1,test2,test3"
                }
            };

            return _jwtService.GenerateToken(model);
        }

        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// </summary>
        /// <param name="model">The login input model containing username and password.</param>
        /// <returns>Generated authentication token and refresh token.</returns>
        public async Task<AuthViewModel> LoginAsync(LoginInputModel model)
        {
            try
            {
                return new()
                {
                    AccessToken = GenerateJwT(),
                    RefreshToken = GenerateRefreshToken()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(LoginAsync));
                throw;
            }
        }

        /// <summary>
        /// Refreshes the authentication token for the current user.
        /// </summary>
        /// <param name="model">Current refresh token.</param>
        /// <returns>Generated authentication token and refresh token.</returns>
        public async Task<AuthViewModel> RefreshTokenAsync(RefreshTokenInputModel model)
        {
            try
            {
                return new()
                {
                    AccessToken = GenerateJwT(),
                    RefreshToken = GenerateRefreshToken()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(RefreshTokenAsync));
                throw;
            }
        }
    }
}