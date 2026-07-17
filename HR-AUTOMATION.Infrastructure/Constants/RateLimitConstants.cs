namespace HR_AUTOMATION.Infrastructure.Constants
{
    /// <summary>
    /// Provides constants for rate limiting configuration.
    /// </summary>
    public static class RateLimitConstants
    {
        /// <summary>
        /// Default rate limit policy name.
        /// </summary>
        public const string DefaultPolicy = "default";

        /// <summary>
        /// Unknown rate limiting policy used when no specific policy is defined.
        /// </summary>
        public const string Unknown = "unknown";

        /// <summary>
        /// Default number of requests allowed within the time window.
        /// </summary>
        public const int DefaultPermitLimit = 70;

        /// <summary>
        /// Default time window in milliseconds.
        /// </summary>
        public const int DefaultWindowMilliseconds = 60000;

        /// <summary>
        /// Default maximum number of requests that can wait in the queue when the limit is reached.
        /// </summary>
        public const int DefaultQueueLimit = 0;
    }
}