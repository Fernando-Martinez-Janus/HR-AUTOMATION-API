namespace HR_AUTOMATION.Application.InputModels;

/// <summary>
/// Represents the login credentials used to authenticate against the job portal.
/// </summary>
public class ScrapeCredentialsInputModel
{
    /// <summary>
    /// Gets or sets the account email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Cleans and normalizes the input values by trimming string properties.
    /// </summary>
    /// <remarks>
    /// <see cref="Password"/> is intentionally not trimmed since leading or trailing
    /// whitespace may be part of the actual password.
    /// </remarks>
    public void Normalize()
    {
        Email = Email.Trim();
    }
}