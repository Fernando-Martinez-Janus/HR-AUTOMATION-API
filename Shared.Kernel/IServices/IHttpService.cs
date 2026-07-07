using Shared.Kernel.Services;

namespace Shared.Kernel.IServices
{
    /// <summary>
    /// Defines the contract for an HTTP service that handles HTTP requests.
    /// </summary>
    /// <remarks>
    /// This interface outlines the methods that an HTTP service should implement to perform HTTP requests.
    /// Typically, this service will be used to send requests to external APIs or services and process the responses.
    /// </remarks>
    public interface IHttpService
    {
        /// <summary>
        /// Sends an HTTP request using the provided input model configuration.
        /// </summary>
        /// <param name="model">
        /// An object containing the HTTP method, target URL, headers, optional bearer token,
        /// and optional request body.
        /// </param>
        /// <returns>
        /// A <see cref="HttpResponse"/> containing the response content
        /// and the HTTP status code returned by the server.
        /// </returns>
        /// <remarks>
        /// This method supports custom headers, JSON request bodies, and Bearer token
        /// authentication. The request body, if provided, is serialized to JSON using the
        /// configured JSON serializer settings.
        /// </remarks>
        Task<HttpResponse> SendRequestAsync(HttpRequest model);
    }
}
