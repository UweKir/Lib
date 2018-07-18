using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace DataManager.Security
{

    public class Cipher
    {

        #region Class Members

        /// <summary>
        /// Pwd for the encryption
        /// </summary>
        private static String pwd = "!rheinGold?";

        /// <summary>
        /// Salt for the encryption
        /// </summary>
        private static String slt = "Claudius";

        #endregion

        #region Standard functions

        /// <summary>
        /// Encrypts in standard the plain text
        /// </summary>
        /// <param name="plain">plain text to encrypt</param>
        /// <returns>encrypted text</returns>
        public String getEncrypted(String plain)
        {
            return Encrypt<RijndaelManaged>(plain, pwd, slt);
        }

        /// <summary>
        /// Decrypts in standard the cipher text
        /// </summary>
        /// <param name="cipher">Cipher text to decrypt</param>
        /// <returns>Decrypted text</returns>
        public String getDecrypted(String cipher)
        {
            return Decrypt<RijndaelManaged>(cipher, pwd, slt);
        }

        #endregion

        #region Encryption/ Decryption

        /// <summary>
        /// Function for direct encryption
        /// </summary>
        /// <typeparam name="T">Algorithm</typeparam>
        /// <param name="value">Text to encrypt</param>
        /// <param name="password">Pwd</param>
        /// <param name="salt">Salt</param>
        /// <returns>Encrypted text</returns>
        public static string Encrypt<T>(string value, string password, string salt)
             where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }

                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        /// <summary>
        /// Function for direct decryption
        /// </summary>
        /// <typeparam name="T">Algorithm</typeparam>
        /// <param name="value">Text to decrypt</param>
        /// <param name="password">Pwd</param>
        /// <param name="salt">Salt</param>
        /// <returns>decrypted text</returns>
        public static string Decrypt<T>(string text, string password, string salt)
           where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(text)))
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        #endregion
    }
}