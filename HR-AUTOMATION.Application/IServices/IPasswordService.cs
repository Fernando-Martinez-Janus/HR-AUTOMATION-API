namespace HR_AUTOMATION.Application.IServices
{
    public interface IPasswordService
    {

        /// <summary>
        /// Toma una contraseña en texto plano y genera un hash seguro e impredecible.
        /// </summary>
        Task<string> HashPassword(string password);

        /// <summary>
        /// Compara una contraseña en texto plano contra un hash guardado previamente.
        /// </summary>
        /// <returns>True si la contraseña es correcta, de lo contrario False.</returns>
        bool VerifyPassword(string password, string savedHash);


    }
}
