using HR_AUTOMATION.Application.IServices;
using System.Security.Cryptography;


namespace HR_AUTOMATION.Application.Services
{
    public class PasswordService : IPasswordService
    {

        public async Task<string> HashPassword(string password)
        {
            try
            {
                /// <summary>
                /// salt aleatorios e impredecibles
                /// 
                /// 100,000 hashes del primer resultado. (int iterations = 100000;)
                /// </summary>
                byte[] saltBytes = RandomNumberGenerator.GetBytes(32);
                int iterations = 100000;


                /// <summary>
                /// Ejecutamos el algoritmo PBKDF2 en un hilo secundario usando Task.Run
                /// y hace un hash final de 32 bytes (256 bits) utilizando SHA256 como función hash subyacente.
                /// </summary>
                byte[] hashBytes = await Task.Run(() =>
                {
                    return Rfc2898DeriveBytes.Pbkdf2(
                        password: password,
                        salt: saltBytes,
                        iterations: iterations,
                        hashAlgorithm: HashAlgorithmName.SHA256,
                        outputLength: 32
                    );
                });


                /// <summary>
                ///  union de salt, iteraciones y el hash usando base64 para almacenarlo en la base de datos.
                /// </summary>
                string saltString = Convert.ToBase64String(saltBytes);
                string hashString = Convert.ToBase64String(hashBytes);

                /// <summary>
                /// Returnamos el formato: ITERACIONES.SALT.HASH
                /// </summary>
                return $"{iterations}.{saltString}.{hashString}";
            }
            catch (Exception ex)
            {
                throw new Exception("The password could not be hashed.", ex);
            }
        }



        public bool VerifyPassword(string password, string savedHash)
        {

            /// <summary>
            ///     Separacion de las partes del hash guardado (iteraciones, salt y hash original) para verificar la contraseña ingresada por el usuario.
            /// </summary>
            var parts = savedHash.Split('.');
            int iterations = int.Parse(parts[0]);
            byte[] saltBytes = Convert.FromBase64String(parts[1]);
            byte[] hashOriginalBytes = Convert.FromBase64String(parts[2]);


            /// <summary>
            /// calculamos el hash de la contraseña que acaba de ingresar, 
            /// </summary>
            byte[] hashNuevoBytes = Rfc2898DeriveBytes.Pbkdf2(
                password,
                saltBytes,
                iterations,
                HashAlgorithmName.SHA256,
                outputLength: 32
            );


            /// <summary>
            /// CryptographicOperations.FixedTimeEquals compara los bytes de forma segura
            /// evitando ataques de tiempo (Timing Attacks)
            /// </summary>
            return CryptographicOperations.FixedTimeEquals(hashOriginalBytes, hashNuevoBytes);
        }



    }
}

