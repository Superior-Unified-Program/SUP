using System;
using System.Text;
using System.Security.Cryptography;

namespace SUP_Library
{
    public class Harsh
    {
        private static string SaltPassword(string password)
        {
            char[] passwordCharArr = password.ToCharArray();
            string saltedPassword = "";
            for (int i = 0; i < passwordCharArr.Length; i++)
            {
                saltedPassword += passwordCharArr[i];
                saltedPassword += "@";
            }
            return saltedPassword;
        }

        public static string HashPassword(string password)
        {
            string saltedPassword = SaltPassword(password);
            byte[] passwordBytes = Encoding.ASCII.GetBytes(saltedPassword);
            HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();
            byte[] hashedBytes = hashAlgorithm.ComputeHash(passwordBytes);
            string hashedPassword = Convert.ToBase64String(hashedBytes);
            return hashedPassword;
        }
    }
}
