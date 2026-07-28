namespace Shared.Kernel.InputModels
{
    /// <summary>
    /// Model that contains the required information to generate a JWT token.
    /// </summary>
    /// <remarks>
    /// This model encapsulates token-related input data such as claims and audience.
    /// </remarks>
    public class GenerateTokenRequest
    {
        /// <summary>
        /// Collection of claims to be included in the token payload.
        /// The key represents the claim type and the value represents the claim value.
        /// </summary>
        public Dictionary<string, object> Claims { get; set; } = [];

        /// <summary>
        /// The intended audience of the token.
        /// If not provided, the default audience configured in the application settings will be used.
        /// </summary>
        public string? Audience { get; set; }
    }
}