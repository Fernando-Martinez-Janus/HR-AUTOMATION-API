using HR_AUTOMATION.Domain.Entities;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Generates the application's own JWT access tokens.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generates a signed JWT access token for the specified user.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>The generated access token and its lifetime in seconds.</returns>
        (string AccessToken, int ExpiresIn) GenerateToken(User user);
    }
}
