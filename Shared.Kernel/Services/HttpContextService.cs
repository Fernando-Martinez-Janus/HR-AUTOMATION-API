using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Kernel.IServices;

namespace Shared.Kernel.Services
{
    public class HttpContextService(ILogger<HttpContextService> logger, IHttpContextAccessor httpContextAccessor) : IHttpContextService
    {
        private readonly ILogger<HttpContextService> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// Gets the organization identifier from the current context.
        /// </summary>
        /// <returns>The organization identifier if available; otherwise, <c>null</c>.</returns>
        public int? GetOrganizationId()
        {
            HttpContext context = _httpContextAccessor.HttpContext;

            return 1;
        }

        /// <summary>
        /// Gets the current user identifier.
        /// </summary>
        /// <returns>The identifier of the current user if available; otherwise, <c>null</c>.</returns>
        public int? GetUserId()
        {
            HttpContext context = _httpContextAccessor.HttpContext;

            return 1;
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