using Shared.Kernel.InputModels;

namespace Shared.Kernel.IServices
{
    /// <summary>
    /// Defines the contract for a service that generates, validates, and parses JSON Web Tokens (JWT),
    /// as well as generating secure random tokens for authentication purposes.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JWT token with the provided claims and optional audience.
        /// </summary>
        /// <param name="model">An object containing all the information required to generate the token.</param>
        /// <returns>A signed JWT token as a string.</returns>
        string GenerateToken(GenerateTokenRequest model);

        /// <summary>
        /// Extracts claims from a JWT token.
        /// </summary>
        /// <param name="token">The JWT token to parse.</param>
        /// <returns>
        /// A dictionary of claims where the key is the claim type and the value is a comma-separated string of claim values.
        /// </returns>
        Dictionary<string, string> GetTokenClaims(string token);
    }
}