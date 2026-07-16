using System.Net;
using System.Text;

namespace Shared.Kernel.ViewModels
{
    /// <summary>
    /// Represents an HTTP response returned by an HTTP request.
    /// </summary>
    /// <param name="response">
    /// The response content.
    /// </param>
    /// <param name="status">
    /// The HTTP status code returned by the server.
    /// </param>
    public class HttpResponse(byte[] response, HttpStatusCode status)
    {
        /// <summary>
        /// Gets or sets the response content returned by the server.
        /// </summary>
        public byte[] Response { get; set; } = response;

        /// <summary>
        /// Gets or sets the HTTP status code returned by the server.
        /// </summary>
        public HttpStatusCode Status { get; set; } = status;

        /// <summary>
        /// Converts the response content from a byte array to a string.
        /// </summary>
        /// <param name="encoding">
        /// The character encoding used to decode the response content.
        /// If not provided, UTF-8 is used.
        /// </param>
        /// <returns>
        /// The response content as a string.
        /// </returns>
        public string GetResponseAsString(Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            return encoding.GetString(Response);
        }
    }
}