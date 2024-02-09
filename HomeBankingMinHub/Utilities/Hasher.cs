using System.Security.Cryptography;
using System.Text;

namespace HomeBankingMinHub.Models
{
    public class Hasher : Utilities.IHasher
    {
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

