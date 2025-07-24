using System;
using System.Security.Cryptography;
using System.Text;

namespace CodeEditor
{
    public class Hashing
    {
        // Method to hash the password and return a hexadecimal string
        protected static string hashpassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute hash as a byte array
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a hexadecimal string
                StringBuilder hexString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hexString.Append(b.ToString("x2")); // Converts each byte to a 2-character hex string
                }
                return hexString.ToString();
            }
        }

        // Public method to hash the password
        public static string Pass(string password)
        {
            return hashpassword(password);
        }
    }
}
