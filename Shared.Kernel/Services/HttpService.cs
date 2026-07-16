using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Kernel.InputModels;
using Shared.Kernel.IServices;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.ViewModels;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Shared.Kernel.Services
{
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

                    request.Headers.Authorization = new AuthenticationHeaderValue(AppConstants.Basic, Convert.ToBase64String(basicAuthByteArray));
                }

                if (!string.IsNullOrWhiteSpace(model.BearerToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(AppConstants.Bearer, model.BearerToken);
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
                    request.Content = new StringContent(content, Encoding.UTF8, MediaTypes.Json);
                }

                using HttpClient client = new(BuildHandler(model.Ssl), disposeHandler: true)
                {
                    Timeout = TimeSpan.FromMilliseconds(model.Timeout)
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