using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace You.Core.Security
{
    public class AESEncryption
    {

        ///
        /// 获取密钥
        ///
        private static string Key
        {
            get { return ")c[r(2x!+n^+%x-iQ4+vE#b9V&>Z\"[EB"; }
        }

        ///
        /// 获取向量
        ///
        private static string IV
        {
            get { return @"x+/~@4-Wv)z$=o|R"; }
        }

        ///
        /// AES加密
        ///
        /// 明文字符串
        /// 密文
        public static string AESEncrypt(string plainStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

            string encrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return encrypt;
        }

        ///
        /// AES加密
        ///
        /// 明文字符串
        /// 加密失败时是否返回 null，false 返回 String.Empty
        /// 密文
        public static string AESEncrypt(string plainStr, bool returnNull)
        {
            string encrypt = AESEncrypt(plainStr);
            return returnNull ? encrypt : (encrypt == null ? String.Empty : encrypt);
        }

        ///
        /// AES解密
        ///
        /// 密文字符串
        /// 明文
        public static string AESDecrypt(string encryptStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return decrypt;
        }

        ///
        /// AES解密
        ///
        /// 密文字符串
        /// 解密失败时是否返回 null，false 返回 String.Empty
        /// 明文
        public static string AESDecrypt(string encryptStr, bool returnNull)
        {
            string decrypt = AESDecrypt(encryptStr);
            return returnNull ? decrypt : (decrypt == null ? String.Empty : decrypt);
        }      
    }
}
