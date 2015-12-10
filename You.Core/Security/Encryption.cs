using System;
using System.Security.Cryptography;
using System.Text;

namespace You.Data.Security
{
    /// <summary>
    /// 加密类
    /// <remarks>
    /// 创建：2014.12.13
    /// </remarks>
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// 256位散列加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public static string Sha256(string plainText)
        {
            SHA256Managed _sha256 = new SHA256Managed();
            byte[] _cipherText = _sha256.ComputeHash(Encoding.Default.GetBytes(plainText));
            return Convert.ToBase64String(_cipherText);
        }
    }
}
