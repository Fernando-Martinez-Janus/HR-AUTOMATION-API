namespace Shared.Kernel.IServices
{
    /// <summary>
    /// Provides methods to retrieve information from the current HTTP context.
    /// </summary>
    public interface IHttpContextService
    {
        /// <summary>
        /// Gets the organization identifier from the current context.
        /// </summary>
        /// <returns>The organization identifier if available; otherwise, <c>null</c>.</returns>
        int? GetOrganizationId();

        /// <summary>
        /// Gets the current user identifier.
        /// </summary>
        /// <returns>The identifier of the current user if available; otherwise, <c>null</c>.</returns>
        int? GetUserId();

        /// <summary>
        /// Gets the remote IP address of the current request.
        /// </summary>
        /// <returns>The IP address if available; otherwise, <c>null</c>.</returns>
        string? GetIpAddress();

        /// <summary>
        /// Gets the User-Agent header of the current request.
        /// </summary>
        /// <returns>The user agent if available; otherwise, <c>null</c>.</returns>
        string? GetUserAgent();

        /// <summary>
        /// Gets the identifier of the currently authenticated user, read from the JWT "userId"
        /// claim of the current request. Unlike <see cref="GetUserId"/>, this reflects the real
        /// signed-in user and must be used wherever an endpoint needs to know who is calling it.
        /// </summary>
        /// <returns>The authenticated user's identifier if available; otherwise, <c>null</c>.</returns>
        int? GetAuthenticatedUserId();
    }
}