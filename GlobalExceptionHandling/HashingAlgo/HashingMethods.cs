using System;
using System.Security.Cryptography;

namespace Middleware.HashingAlgo
{
    public class HashingService : IHashingService
    {
        private const int SaltSize = 16;  // 128-bit salt
        private const int HashSize = 32;  // 256-bit hash
        private const int Iterations = 10000; // Number of iterations for PBKDF2

        /// <summary>
        /// Generates a salted hash for the given password.
        /// </summary>
        public string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt); // Generate random salt
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);
                byte[] hashBytes = new byte[SaltSize + HashSize];

                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Verifies a password against a stored hash.
        /// </summary>
        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            try
            {
                byte[] hashBytes = Convert.FromBase64String(storedHash);
                byte[] salt = new byte[SaltSize];
                byte[] storedPasswordHash = new byte[HashSize];

                Array.Copy(hashBytes, 0, salt, 0, SaltSize);
                Array.Copy(hashBytes, SaltSize, storedPasswordHash, 0, HashSize);

                using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    byte[] enteredPasswordHash = pbkdf2.GetBytes(HashSize);
                    return CryptographicOperations.FixedTimeEquals(enteredPasswordHash, storedPasswordHash);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
