namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Input model for refreshing an access token or logging out.
    /// </summary>
    public class RefreshTokenInputModel
    {
        /// <summary>
        /// The refresh token issued to the client, used to request a new access token or to log out.
        /// </summary>
        public string RefreshToken { get; set; } = null!;

        /// <summary>
        /// Normalizes the input model before processing.
        /// </summary>
        public void Normalize()
        {
            RefreshToken = RefreshToken?.Trim()!;
        }
    }
}
