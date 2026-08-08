using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Kernel.InputModels;
using Shared.Kernel.IServices;
using Shared.Kernel.Utils.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shared.Kernel.Services
{
    /// <summary>
    /// Service responsible for generating and validating JSON Web Tokens (JWT).
    /// </summary>
    /// <param name="logger">Instance of Serilog Logger.</param>
    /// <param name="configuration">Instance of IConfiguration to obtain appsettings' JWT configurations.</param>
    public class JwtService(ILogger<JwtService> logger, IConfiguration configuration) : IJwtService
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<JwtService> _logger = logger;

        /// <summary>
        /// Obtains JWT configurations.
        /// </summary>
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Generates a JWT token with the provided claims and optional audience.
        /// </summary>
        /// <param name="model">An object containing all the information required to generate the token.</param>
        /// <returns>A signed JWT token as a string.</returns>
        public string GenerateToken(GenerateTokenRequest model)
        {
            string secret = _configuration.GetValue<string>(AppConstants.JwtSecretKey)!;
            string issuer = _configuration.GetValue<string>(AppConstants.JwtIssuerKey)!;
            string defaultAudience = _configuration.GetValue<string>(AppConstants.JwtDefaultAudienceKey)!;
            bool neverExpires = _configuration.GetValue<bool>(AppConstants.JwtNeverExpiresKey)!;
            double expiresIn = _configuration.GetValue<double>(AppConstants.JwtExpiresInKey)!;

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            IEnumerable<Claim> tokenClaims = model.Claims.Select(claim => new Claim(claim.Key, claim.Value.ToString() ?? string.Empty));

            JwtSecurityToken token = new(
                issuer: issuer,
                audience: string.IsNullOrWhiteSpace(model.Audience) ? defaultAudience : model.Audience,
                claims: tokenClaims,
                notBefore: DateTime.UtcNow,
                expires: neverExpires ? DateTime.MaxValue : DateTime.UtcNow.AddMilliseconds(expiresIn),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Extracts claims from a JWT token.
        /// </summary>
        /// <param name="token">The JWT token to parse.</param>
        /// <returns>
        /// A dictionary of claims where the key is the claim type and the value is a comma-separated string of claim values.
        /// </returns>
        public Dictionary<string, string> GetTokenClaims(string token)
        {
            string secret = _configuration.GetValue<string>(AppConstants.JwtSecretKey)!;
            string issuer = _configuration.GetValue<string>(AppConstants.JwtIssuerKey)!;
            string[] allowedAudiences = _configuration.GetSection(AppConstants.JwtAllowedAudiencesKey).Get<string[]>() ?? [];
            double clockSkew = _configuration.GetValue<double>(AppConstants.JwtClockSkewKey)!;

            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.UTF8.GetBytes(secret);

            TokenValidationParameters parameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudiences = allowedAudiences,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMilliseconds(clockSkew)
            };

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out _);

                Dictionary<string, string> claims =
                    principal.Claims
                    .GroupBy(x => x.Type)
                    .ToDictionary(
                        x => x.Key,
                        y => string.Join(',', y.Select(z => z.Value))
                    );

                return claims;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetTokenClaims));
                throw;
            }
        }
    }
}