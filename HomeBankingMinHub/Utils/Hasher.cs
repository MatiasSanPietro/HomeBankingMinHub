using System.Security.Cryptography;
using System.Text;
using HomeBankingMinHub.Utils.Interfaces;

namespace HomeBankingMinHub.Models
{
    public class Hasher : IHasher
    {
        // Genero 32 bytes de datos aleatorios, se pasa al salt, se concatena con la
        // contraseña y se calcula el hash con SHA256
        public string HashPassword(string password, out string salt)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var saltBytes = new byte[32];
                rng.GetBytes(saltBytes);

                salt = Convert.ToBase64String(saltBytes);

                using (var sha256 = SHA256.Create())
                {
                    var saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                    var hashedBytes = sha256.ComputeHash(saltedPassword);
                    return Convert.ToBase64String(hashedBytes);
                }
            }
        }

        // Toma el salt asociado a la contraseña, se contatenan y se utiliza SHA256
        // para calcular el hash de la contraseña
        public bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                var hashedBytes = sha256.ComputeHash(saltedPassword);
                var hashedPasswordToCheck = Convert.ToBase64String(hashedBytes);

                return hashedPasswordToCheck == hashedPassword;
            }
        }
    }
}

