namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Input model for refreshing an access token.
    /// </summary>
    public class RefreshTokenInputModel
    {
        /// <summary>
        /// The refresh token issued to the client, used to request a new access token.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}