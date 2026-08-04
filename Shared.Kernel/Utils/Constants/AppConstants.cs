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


        public const string JwtKeyKey = "Jwt:Key";
        //public const string JwtIssuerKey = "Jwt:Issuerop";
        public const string JwtAudienceKey = "Jwt:Audience";
        public const string JwtExpirationMinutesKey = "Jwt:ExpirationMinutes";

        public const string GoogleClientIdKey = "Google:ClientId";
        public const string GoogleIssuer = "https://accounts.google.com";
        public const string GoogleIssuerAlternate = "accounts.google.com";

        ///// Fer
        public const string JwtSecretKey = "JwtConfigurations:Secret";
        public const string JwtIssuerKey = "JwtConfigurations:Issuer";
        public const string JwtDefaultAudienceKey = "JwtConfigurations:DefaultAudience";
        public const string JwtAllowedAudiencesKey = "JwtConfigurations:AllowedAudiences";
        public const string JwtExpiresInKey = "JwtConfigurations:ExpiresIn";
        public const string JwtClockSkewKey = "JwtConfigurations:ClockSkew";
        public const string JwtNeverExpiresKey = "JwtConfigurations:NeverExpires";

        public const string RefreshTokenByteSizeKey = "RefreshTokenConfigurations:SecureTokenByteSize";
        public const string RefreshTokenExpirationDaysKey = "RefreshToken:ExpirationDays";

        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 10;
    }
}