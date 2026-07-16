namespace Shared.Kernel.Utils.Constants
{
    /// <summary>
    /// Provides application-wide constants used across the application.
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// Represents the Bearer authentication scheme used in HTTP Authorization headers.
        /// </summary>
        public const string Bearer = "Bearer";

        /// <summary>
        /// Represents the Basic authentication scheme used in HTTP Authorization headers.
        /// </summary>
        public const string Basic = "Basic";

        public const string RedisConnectionStringKey = "Redis:ConnectionString";

        public const string RedisShortExpiration = "Redis:Expiration:Short";
        public const string RedisDefaultExpiration = "Redis:Expiration:Default";
        public const string RedisLongExpiration = "Redis:Expiration:Long";

        public const string SqlServerConnectionStringKey = "ConnectionConfigurations:SqlServerConnection:ConnectionString";
        public const string SqlServerTimeoutKey = "ConnectionConfigurations:SqlServerConnection:Timeout";

        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 10;
    }
}