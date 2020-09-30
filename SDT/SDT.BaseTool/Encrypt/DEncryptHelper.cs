using System;
using System.Security.Cryptography;
using System.Text;

namespace SDT.BaseTool
{
    /// <summary>
    /// 加密解密方法
    /// </summary>
    public class DEncryptHelper
    {
        private const string EncryptKey = "forchnsoft";

        #region 使用 缺省密钥字符串 加密/解密string

        /// <summary>
        /// 使用缺省密钥字符串加密string
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return "";
            }

            return Encrypt(original, EncryptKey);
        }

        /// <summary>
        /// 使用缺省密钥字符串解密string
        /// </summary>
        /// <param name="cypher">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string cypher)
        {
            if (string.IsNullOrEmpty(cypher))
            {
                return "";
            }

            return Decrypt(cypher, EncryptKey);
        }

        #endregion

        #region 使用 给定密钥字符串 加密/解密string
        /// <summary>
        /// 使用给定密钥字符串加密string
        /// </summary>
        /// <param name="original">原始文字</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, string key)
        {
            var buff = Encoding.UTF8.GetBytes(original);
            var kb = Encoding.UTF8.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        /// <summary>
        /// 使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="cypher">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string cypher, string key)
        {
            var buff = Convert.FromBase64String(cypher);
            var kb = Encoding.UTF8.GetBytes(key);
            return Encoding.UTF8.GetString(Decrypt(buff, kb));
        }
        #endregion

        #region  使用 给定密钥 加密/解密/byte[]
        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        private static byte[] Encrypt(byte[] original, byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider
            {
                Key = MakeMD5(key),
                Mode = CipherMode.ECB
            };
            var dector = des.CreateEncryptor();
            var result = dector.TransformFinalBlock(original, 0, original.Length);
            dector.Dispose();
            des.Dispose();

            return result;
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        private static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider
            {
                Key = MakeMD5(key),
                Mode = CipherMode.ECB
            };
            var dector = des.CreateDecryptor();
            var result = dector.TransformFinalBlock(encrypted, 0, encrypted.Length);
            dector.Dispose();
            des.Dispose();

            return result;
        }

        /// <summary>
        /// 生成MD5摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>摘要</returns>
        private static byte[] MakeMD5(byte[] original)
        {
            using (var hashmd5 = new MD5CryptoServiceProvider())
            {
                var keyhash = hashmd5.ComputeHash(original);
                return keyhash;
            }
        }
        #endregion
    }
}
