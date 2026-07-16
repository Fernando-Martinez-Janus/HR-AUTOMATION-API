namespace HR_AUTOMATION.Infrastructure.Constants
{
    /// <summary>
    /// Provides constants used to configure API versioning.
    /// </summary>
    public static class VersioningConstants
    {
        /// <summary>
        /// Indicates whether the default API version should be assumed when no version is specified in the request.
        /// </summary>
        public const bool AssumeDefaultVersionWhenUnspecified = true;

        /// <summary>
        /// Defines the default major API version.
        /// </summary>
        public const int DefaultApiVersion = 1;

        /// <summary>
        /// Indicates whether supported and deprecated API versions should be reported in response headers.
        /// </summary>
        public const bool ReportApiVersions = true;

        /// <summary>
        /// Specifies the format used to group API versions in the API documentation.
        /// </summary>
        public const string GroupNameFormat = "'v'VVV";

        /// <summary>
        /// Indicates whether the API version placeholder in route templates should be replaced with the actual version.
        /// </summary>
        public const bool SubstituteApiVersionInUrl = true;
    }
}