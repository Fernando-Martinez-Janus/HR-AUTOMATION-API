using System.Security.Cryptography;
using System.Text;

namespace Shared.Kernel.Utils.Helpers
{
    /// <summary>
    /// Provides helper methods for generating cache keys.
    /// </summary>
    public static class CacheKeyHelper
    {
        /// <summary>
        /// Normalizes the specified key and computes a deterministic hash that can be safely used as part of a cache key.
        /// </summary>
        /// <param name="key">The value to normalize and hash.</param>
        /// <returns>An uppercase hexadecimal string representing the computed hash.</returns>
        public static string ComputeHash(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            byte[] encodedKey = Encoding.UTF8.GetBytes(key.Trim().ToLowerInvariant());
            byte[] hashedKey = SHA256.HashData(encodedKey);

            return Convert.ToHexString(hashedKey);
        }

        /// <summary>
        /// Generates a unique cache version identifier.
        /// </summary>
        /// <returns>
        /// A unique identifier that can be used to invalidate related cache entries.
        /// </returns>
        public static string GenerateVersion()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}