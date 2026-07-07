using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Infrastructure.Middlewares
{
    /// <summary>
    /// Middleware that handles errors.
    /// If an error is thrown, it will return the status code and the error message.
    /// </summary>
    /// <param name="next">The next delegate in the request pipeline.</param>
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate next)
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

        /// <summary>
        /// Set the next delegate to be executed in the pipeline.
        /// </summary>
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// Json serializer settings.
        /// </summary>
        private readonly JsonSerializerSettings _jsonSerializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
        };

        /// <summary>
        /// Handles the HTTP request.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles detected exception, returning an instance of ResponseFactory.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="ex">Thrown exception.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, nameof(HandleExceptionAsync));

            ResponseFactory response =
                ex is IResponse _response
                ? new(_response)
                : new(Exceptions.InternalServerError);

            string result = JsonConvert.SerializeObject(response, _jsonSerializerSettings);

            context.Response.StatusCode = response.Code;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(result);
        }
    }
}
