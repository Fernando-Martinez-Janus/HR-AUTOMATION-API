using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Shared.Kernel.InputModels;
using Shared.Kernel.IServices;
using Shared.Kernel.Utils.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace HR_AUTOMATION.Application.Services
{
    /// <summary>
    /// Generates the application's own JWT access tokens using HMAC SHA-256.
    /// </summary>
    /// <param name="configuration">The application configuration provider.</param>
    public class JwtTokenService(IConfiguration configuration, IJwtService jwtService) : IJwtTokenService
    {
        private readonly IJwtService _jwtService = jwtService;
        /// <summary>
        /// The symmetric key used to sign issued tokens.
        /// </summary>
        private readonly string _key = configuration.GetValue<string>(AppConstants.JwtSecretKey)!;

        /// <summary>
        /// The issuer to embed in issued tokens.
        /// </summary>
        private readonly string _issuer = configuration.GetValue<string>(AppConstants.JwtIssuerKey)!;

        /// <summary>
        /// The audience to embed in issued tokens.
        /// </summary>
        private readonly string _audience = configuration.GetValue<string>(AppConstants.JwtDefaultAudienceKey)!;

        /// <summary>
        /// The token lifetime, in milliseconds.
        /// </summary>
        private readonly double _expiresInMilliseconds = configuration.GetValue<double>(AppConstants.JwtExpiresInKey);

        /// <summary>
        /// Generates a signed JWT access token for the specified user.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>The generated access token and its lifetime in seconds.</returns>
        public (string AccessToken, int ExpiresIn) GenerateToken(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(_key)) throw new InvalidOperationException("JWT key no configurada.");

            DateTime now = DateTime.UtcNow;
            DateTime expires = now.AddMilliseconds(_expiresInMilliseconds);

            Dictionary<string, object> claims = new()
            {
                [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
                [JwtRegisteredClaimNames.Email] = user.Email ?? string.Empty,
                [JwtRegisteredClaimNames.Name] = user.FullName ?? string.Empty,
                ["role"] = user.RoleName ?? string.Empty,
                ["roleId"] = user.RoleId.ToString(),
                ["organizationId"] = user.OrganizationId.ToString(),
                ["organizationName"] = user.OrganizationName ?? string.Empty,
                ["userId"] = user.Id.ToString(),
                [JwtRegisteredClaimNames.Iat] = new DateTimeOffset(now).ToUnixTimeSeconds().ToString()
            };

            string accessToken = _jwtService.GenerateToken(new GenerateTokenRequest()
            {
                Claims = claims
            });
            int expiresIn = (int)(_expiresInMilliseconds / 1000);

            return (accessToken, expiresIn);
        }
    }
}
