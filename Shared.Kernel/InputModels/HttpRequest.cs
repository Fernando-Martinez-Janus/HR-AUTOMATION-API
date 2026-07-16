namespace Shared.Kernel.InputModels
{
    /// <summary>
    /// Represents the input configuration for an HTTP request.
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// Gets or sets the target URL for the HTTP request.
        /// </summary>
        public required string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method to be used for the request
        /// (e.g., GET, POST, PUT, DELETE).
        /// </summary>
        public required HttpMethod Method { get; set; }

        /// <summary>
        /// Gets or sets the collection of custom HTTP headers to be sent with the request.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = [];

        /// <summary>
        /// Gets or sets the basic authentication credentials.
        /// The key represents the username and the value represents the password.
        /// When provided, they are used to generate the Authorization header using the Basic scheme.
        /// </summary>
        public KeyValuePair<string, string>? BasicAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the Bearer token used for authorization.
        /// When provided, it is added to the Authorization header using the Bearer scheme.
        /// </summary>
        public string? BearerToken { get; set; }

        /// <summary>
        /// Gets or sets the request body object.
        /// If provided, the body is serialized to JSON before being sent.
        /// </summary>
        public object? Body { get; set; }

        /// <summary>
        /// Gets or sets the form fields to be sent as application/x-www-form-urlencoded content.
        /// Each key-value pair represents a form field name and its corresponding value.
        /// </summary>
        public Dictionary<string, string> FormContent { get; set; } = [];

        /// <summary>
        /// Gets or sets optional SSL/TLS settings for this specific request.
        /// When <c>null</c>, the default <see cref="System.Net.Http.HttpClient"/> (no custom cert handling) is used.
        /// </summary>
        public SslConfig? Ssl { get; set; }

        /// <summary>
        /// Gets or sets the timeout for this request.
        /// When <c>null</c>, the .NET default of 100000 milliseconds applies.
        /// </summary>
        public int Timeout { get; set; } = 100000;
    }
}