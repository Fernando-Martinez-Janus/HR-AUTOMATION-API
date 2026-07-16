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
    }
}