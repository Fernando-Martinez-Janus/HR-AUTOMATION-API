namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Represents the input data required for a login/authentication request.
    /// </summary>
    public class LoginInputModel
    {
        /// <summary>
        /// The email address of the user attempting to log in.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The user's password for authentication.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}