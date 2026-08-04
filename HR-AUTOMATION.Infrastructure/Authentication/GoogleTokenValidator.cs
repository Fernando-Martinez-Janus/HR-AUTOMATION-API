using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Infrastructure.Authentication
{
    /// <summary>
    /// Validates Google ID tokens using Google's official token verification library.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The application configuration provider.</param>
    public class GoogleTokenValidator(
        ILogger<GoogleTokenValidator> logger,
        IConfiguration configuration
    ) : IGoogleTokenValidator
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<GoogleTokenValidator> _logger = logger;

        /// <summary>
        /// The configured Google OAuth client identifier, used to validate the token audience.
        /// </summary>
        private readonly string _clientId = configuration.GetValue<string>(AppConstants.GoogleClientIdKey)!;

        /// <summary>
        /// Validates a Google ID token against Google's public keys and issuer/audience/expiration rules.
        /// </summary>
        /// <param name="idToken">The Google ID token to validate.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The identity information extracted from the validated token.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the token is missing, invalid, expired, or the email is not verified.</exception>
        public async Task<GoogleUserInfo> ValidateAsync(string idToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(idToken))
            {
                throw new ResponseExceptionFactory(Exceptions.GoogleIdTokenRequired);
            }

            try
            {
                GoogleJsonWebSignature.ValidationSettings settings = new()
                {
                    Audience = [_clientId]
                };

                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                bool isIssuerValid = payload.Issuer is AppConstants.GoogleIssuer or AppConstants.GoogleIssuerAlternate;

                if (!isIssuerValid || !payload.EmailVerified)
                {
                    throw new ResponseExceptionFactory(Exceptions.InvalidGoogleToken);
                }

                return new GoogleUserInfo
                {
                    Email = payload.Email,
                    Name = payload.Name
                };
            }
            catch (ResponseExceptionFactory)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ValidateAsync));

                throw new ResponseExceptionFactory(Exceptions.InvalidGoogleToken);
            }
        }
    }
}
