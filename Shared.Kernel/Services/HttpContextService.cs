using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Kernel.IServices;

namespace Shared.Kernel.Services
{
    public class HttpContextService(ILogger<HttpContextService> logger, IHttpContextAccessor httpContextAccessor, IJwtService jwtService) : IHttpContextService
    {
        private readonly ILogger<HttpContextService> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtService _jwtService = jwtService;

        /// <summary>
        /// Gets the raw JWT access token associated with the current execution context.
        /// </summary>
        /// <returns>
        /// The JWT access token if available; otherwise, <c>null</c>.
        /// </returns>
        /// <remarks>
        /// The token is typically obtained from an <c>Authorization</c> header using the <c>Bearer</c> authentication scheme.
        /// </remarks>
        private string? GetToken()
        {
            try
            {
                string? authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader is null || !authHeader.StartsWith("Bearer "))
                {
                    return null;
                }

                return authHeader["Bearer ".Length..].Trim();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtains the value of the attribute within the current token (if found).
        /// </summary>
        /// <param name="attribute">Attribute's name.</param>
        /// <returns>Attibute's value.</returns>
        private string? GetAttributeFromToken(string attribute)
        {
            string? token = GetToken();

            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            Dictionary<string, string>? claims = _jwtService.GetTokenClaims(token);

            if (claims is null)
            {
                return null;
            }

            claims.TryGetValue(attribute, out string? value);

            return value;
        }

        /// <summary>
        /// Gets the organization identifier from the current context.
        /// </summary>
        /// <returns>The organization identifier if available; otherwise, <c>null</c>.</returns>
        public int? GetOrganizationId()
        {
            string? value = GetAttributeFromToken("organizationId");

            return int.TryParse(value, out int result) ? result : 1;
        }

        /// <summary>
        /// Gets the current user identifier.
        /// </summary>
        /// <returns>The identifier of the current user if available; otherwise, <c>null</c>.</returns>
        public int? GetUserId()
        {
            string? value = GetAttributeFromToken("userId");

            return int.TryParse(value, out int result) ? result : 0;
        }

        /// <summary>
        /// Gets the remote IP address of the current request.
        /// </summary>
        /// <returns>The IP address if available; otherwise, <c>null</c>.</returns>
        public string? GetIpAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }

        /// <summary>
        /// Gets the User-Agent header of the current request.
        /// </summary>
        /// <returns>The user agent if available; otherwise, <c>null</c>.</returns>
        public string? GetUserAgent()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
        }

        /// <summary>
        /// Gets the identifier of the currently authenticated user, read from the JWT "userId"
        /// claim of the current request.
        /// </summary>
        /// <returns>The authenticated user's identifier if available; otherwise, <c>null</c>.</returns>
        public int? GetAuthenticatedUserId()
        {
            string? value = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            return int.TryParse(value, out int userId) ? userId : null;
        }
    }
}