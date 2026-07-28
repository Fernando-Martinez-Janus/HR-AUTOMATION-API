namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Represents the input data required for a login/authentication request.
    /// </summary>
    public class LoginInputModel
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user's password, in plain text as submitted by the client.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Normalizes the input model before processing.
        /// </summary>
        public void Normalize()
        {
            Email = Email?.Trim()!;
        }
    }
}
