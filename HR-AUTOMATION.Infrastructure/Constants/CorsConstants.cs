namespace HR_AUTOMATION.Infrastructure.Constants
{
    /// <summary>
    /// Provides configuration keys used for Cross-Origin Resource Sharing (CORS).
    /// </summary>
    public static class CorsConstants
    {
        /// <summary>
        /// Configuration key for the collection of allowed origins.
        /// </summary>
        public const string AllowedOriginsKey = "Cors:AllowedOrigins";

        /// <summary>
        /// Configuration key for the collection of allowed request headers.
        /// </summary>
        public const string AllowedHeadersKey = "Cors:AllowedHeaders";

        /// <summary>
        /// Configuration key for the collection of allowed HTTP methods.
        /// </summary>
        public const string AllowedMethodsKey = "Cors:AllowedMethods";

        /// <summary>
        /// Configuration key for the collection of response headers exposed to clients.
        /// </summary>
        public const string ExposedHeadersKey = "Cors:ExposedHeaders";
    }
}