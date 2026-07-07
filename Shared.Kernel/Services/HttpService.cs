using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Kernel.IServices;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Shared.Kernel.Services
{
    #region Models
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
        /// When <c>null</c>, the .NET default of 100 seconds applies.
        /// </summary>
        public int Timeout { get; set; } = 100;
    }

    /// <summary>
    /// Represents per-request SSL/TLS configuration for an HTTP call.
    /// </summary>
    /// <remarks>
    /// The caller is responsible for loading certificate data from disk or any other source
    /// before building this model. Options are not mutually exclusive —
    /// for example, you can send a client certificate AND trust a specific server cert.
    /// </remarks>
    public class SslConfig
    {
        /// <summary>
        /// When <c>true</c>, bypasses all server certificate validation.
        /// Use only for internal APIs with self-signed or untrusted certificates.
        /// </summary>
        public bool IgnoreAllErrors { get; set; }

        /// <summary>
        /// DER-encoded bytes of the trusted server certificate (public key only, e.g. from a .cer file).
        /// When set, the server certificate is accepted only if its thumbprint matches these bytes.
        /// Load the file with <c>File.ReadAllBytes</c> before assigning.
        /// </summary>
        public byte[]? TrustedCertificate { get; set; }

        /// <summary>
        /// PKCS#12 / PFX bytes of the client certificate used for mutual TLS (mTLS).
        /// The data must include the private key.
        /// Load the file with <c>File.ReadAllBytes</c> before assigning.
        /// </summary>
        public byte[]? ClientCertificate { get; set; }

        /// <summary>
        /// Password for the client certificate, if it is password-protected.
        /// </summary>
        public string? ClientCertificatePassword { get; set; }
    }

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
    #endregion

    /// <summary>
    /// A service that handles HTTP requests using <see cref="HttpClient"/>.
    /// </summary>
    /// <remarks>
    /// This class provides functionality to send HTTP requests and process the responses.
    /// Each call creates its own <see cref="HttpClient"/> backed by a handler built from the
    /// request's <see cref="SslConfig"/>, so SSL behaviour is fully consistent regardless of
    /// whether custom certificate options are supplied.
    /// </remarks>
    /// <param name="logger">Instance of Serilog Logger.</param>
    public class HttpService(ILogger<HttpService> logger) : IHttpService
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<HttpService> _logger = logger;

        /// <summary>
        /// The settings used for serializing and deserializing JSON data.
        /// </summary>
        /// <remarks>
        /// This instance of <see cref="JsonSerializerSettings"/> is configured to:
        /// - Use camelCase for property names.
        /// - Handle DateTime values in UTC format.
        /// - Include null values when serializing objects.
        /// </remarks>
        private readonly JsonSerializerSettings _jsonSerializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            NullValueHandling = NullValueHandling.Include
        };

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
        public async Task<HttpResponse> SendRequestAsync(HttpRequest model)
        {
            try
            {
                HttpRequestMessage request = new(model.Method, model.Url);

                foreach (KeyValuePair<string, string> header in model.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                if (model.BasicAuthentication.HasValue)
                {
                    KeyValuePair<string, string> basicAuth = model.BasicAuthentication.Value;

                    byte[] basicAuthByteArray = Encoding.ASCII.GetBytes($"{basicAuth.Key}:{basicAuth.Value}");

                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(basicAuthByteArray));
                }

                if (!string.IsNullOrWhiteSpace(model.BearerToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", model.BearerToken);
                }

                string? content = null;

                if (model.FormContent.Count > 0)
                {
                    FormUrlEncodedContent formContent = new(model.FormContent);
                    request.Content = formContent;

                    content = string.Join(",", model.FormContent.Select(x => $"{x.Key}={x.Value}"));
                }

                if (model.Body != null)
                {
                    content = JsonConvert.SerializeObject(model.Body, _jsonSerializerSettings);
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                }

                using HttpClient client = new(BuildHandler(model.Ssl), disposeHandler: true)
                {
                    Timeout = TimeSpan.FromSeconds(model.Timeout)
                };
                HttpResponseMessage response = await client.SendAsync(request);

                return new(await response.Content.ReadAsByteArrayAsync(), response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SendRequestAsync));

                throw;
            }
        }

        /// <summary>
        /// Builds an <see cref="HttpClientHandler"/> configured according to the provided <see cref="SslConfig"/>.
        /// When <paramref name="ssl"/> is <c>null</c>, returns a default handler with standard SSL validation.
        /// </summary>
        private static HttpClientHandler BuildHandler(SslConfig? ssl)
        {
            HttpClientHandler handler = new();

            if (ssl is null)
            {
                return handler;
            }

            if (ssl.IgnoreAllErrors)
            {
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            }

            if (ssl.TrustedCertificate is { Length: > 0 })
            {
                X509Certificate2 trustedCert = X509CertificateLoader.LoadCertificate(ssl.TrustedCertificate);
                string expectedThumbprint = trustedCert.Thumbprint;

                handler.ServerCertificateCustomValidationCallback = (_, serverCert, _, _) =>
                    string.Equals(serverCert?.GetCertHashString(), expectedThumbprint, StringComparison.OrdinalIgnoreCase);
            }

            if (ssl.ClientCertificate is { Length: > 0 })
            {
                X509Certificate2 clientCert = X509CertificateLoader.LoadPkcs12(
                    ssl.ClientCertificate,
                    ssl.ClientCertificatePassword);

                handler.ClientCertificates.Add(clientCert);
            }

            return handler;
        }
    }
}
