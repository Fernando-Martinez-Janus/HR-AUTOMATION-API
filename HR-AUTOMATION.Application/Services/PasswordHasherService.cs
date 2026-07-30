using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HR_AUTOMATION.Application.Services
{
    /// <summary>
    /// Verifies user passwords against their stored hash using ASP.NET Core Identity's
    /// <see cref="PasswordHasher{TUser}"/>.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public class PasswordHasherService(ILogger<PasswordHasherService> logger) : IPasswordHasherService
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<PasswordHasherService> _logger = logger;

        /// <summary>
        /// Performs the hash comparison. <see cref="User"/> is only used as the generic type
        /// argument required by <see cref="PasswordHasher{TUser}"/>; it plays no role in hashing.
        /// </summary>
        private readonly PasswordHasher<User> _passwordHasher = new();

        /// <summary>
        /// Verifies that the provided plain-text password matches the stored hash.
        /// </summary>
        /// <param name="passwordHash">The stored password hash.</param>
        /// <param name="providedPassword">The plain-text password to verify.</param>
        /// <returns><c>true</c> if the password is valid; otherwise, <c>false</c>.</returns>
        public bool VerifyPassword(string passwordHash, string providedPassword)
        {
            if (LooksLikeIdentityHash(passwordHash))
            {
                PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(new User(), passwordHash, providedPassword);

                if (result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded)
                {
                    _logger.LogInformation("VerifyPassword: password verified using ASP.NET Identity hash.");

                    return true;
                }

                _logger.LogInformation("VerifyPassword: invalid password.");

                return false;
            }

            // TEMPORARY DEVELOPMENT COMPATIBILITY
            // REMOVE AFTER PASSWORD MIGRATION
            //
            // The stored value does not look like a PasswordHasher<TUser> hash, which means
            // this user's password is still sitting in the database as plain text (the project
            // is still under development). Fall back to a direct comparison so login keeps
            // working during the migration, but never let this take priority over a real hash.
            bool isPlainTextMatch = string.Equals(passwordHash, providedPassword, StringComparison.Ordinal);

            if (isPlainTextMatch)
            {
                _logger.LogWarning(
                    "VerifyPassword: password verified using temporary plain-text compatibility mode. " +
                    "REMOVE AFTER PASSWORD MIGRATION.");

                return true;
            }

            _logger.LogInformation("VerifyPassword: invalid password.");

            return false;
        }

        // TEMPORARY DEVELOPMENT COMPATIBILITY
        // REMOVE AFTER PASSWORD MIGRATION
        //
        // Detects whether a stored value is a real PasswordHasher<TUser> hash instead of relying
        // on VerifyHashedPassword's FormatException as normal control flow. Every hash produced by
        // PasswordHasher<TUser> is Base64 and starts with a 1-byte format marker: 0x00 for the
        // legacy (V2) format or 0x01 for the current (V3) format, followed by at least 12 more
        // bytes of salt/subkey. A short value like "janus1234" either fails Base64 decoding
        // outright or decodes to too few bytes with the wrong marker, so it is treated as plain text.
        private static bool LooksLikeIdentityHash(string storedValue)
        {
            if (string.IsNullOrWhiteSpace(storedValue))
            {
                return false;
            }

            Span<byte> buffer = stackalloc byte[storedValue.Length];

            if (!Convert.TryFromBase64String(storedValue, buffer, out int bytesWritten))
            {
                return false;
            }

            const int minimumHashLength = 13;

            return bytesWritten >= minimumHashLength && buffer[0] is 0x00 or 0x01;
        }
    }
}
