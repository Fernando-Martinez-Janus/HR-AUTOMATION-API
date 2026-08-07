using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Kernel.Utils.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HR_AUTOMATION.Application.Services
{
    /// <summary>
    /// Generates the application's own JWT access tokens using HMAC SHA-256.
    /// </summary>
    /// <param name="configuration">The application configuration provider.</param>
    public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
    {
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

            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName ?? string.Empty),
                new Claim("role", user.RoleName ?? string.Empty),
                new Claim("roleId", user.RoleId.ToString()),
                new Claim("organizationId", user.OrganizationId.ToString()),
                new Claim("organizationName", user.OrganizationName ?? string.Empty),
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_key));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: credentials);

            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            int expiresIn = (int)(_expiresInMilliseconds / 1000);

            return (accessToken, expiresIn);
        }
    }
}
