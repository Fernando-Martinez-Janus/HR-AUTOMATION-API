namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Verifies user passwords against their stored hash.
    /// </summary>
    public interface IPasswordHasherService
    {
        /// <summary>
        /// Verifies that the provided plain-text password matches the stored hash.
        /// </summary>
        /// <param name="passwordHash">The stored password hash.</param>
        /// <param name="providedPassword">The plain-text password to verify.</param>
        /// <returns><c>true</c> if the password is valid; otherwise, <c>false</c>.</returns>
        bool VerifyPassword(string passwordHash, string providedPassword);
    }
}
