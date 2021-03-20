using System;
using System.Security.Cryptography;
using System.Text;

namespace Product.API.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class HashFactory
    {
        /// <summary>
        /// Compute hash for the input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHashSHA256(string input)
        {
            using (var sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                var sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
        /// <summary>
        /// Verify a hash against a string.
        /// </summary>
        /// <param name="inputHash"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool VerifyHash(string inputHash, string hash)
        {
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(inputHash, hash) == 0;
        }
    }
}
