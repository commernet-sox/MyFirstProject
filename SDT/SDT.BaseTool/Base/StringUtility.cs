using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SDT.BaseTool
{
    /// <summary>
    /// string 工具类
    /// </summary>
    public static class StringUtility
    {
        #region Encoding
        /// <summary>
        /// url encode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(string url) => WebUtility.UrlEncode(url);

        /// <summary>
        /// url decode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(string url) => WebUtility.UrlDecode(url);

        /// <summary>
        /// html encode
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlEncode(string html) => WebUtility.HtmlEncode(html);

        /// <summary>
        /// html decode
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlDecode(string html) => WebUtility.HtmlDecode(html);
        #endregion

        #region Characters Convertsion
        /// <summary>
        /// unicode to string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UnicodeToString(string input)
        {
            var res = input;
            var reg = Regex.Matches(res, @"\\u\w{4}");
            for (var i = 0; i < reg.Count; i++)
            {
                res = res.Replace(reg[i].Groups[0].Value, "" + Regex.Unescape(reg[i].Value.ToString()).ToString());
            }

            return res;
        }

        /// <summary>
        /// convert semiangle
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertSemiangle(string input)
        {
            if (input == null)
            {
                input = "";
            }

            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }

                if (c[i] > 65280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }

            return new string(c);
        }

        /// <summary>
        /// convert holomorph
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertHolomorph(string input)
        {
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }

                if (c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }

            return new string(c);
        }

        /// <summary>
        /// GZIP compress string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Compress(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gzipStream.Write(buffer, 0, buffer.Length);
                    memoryStream.Position = 0;

                    var compressedData = new byte[memoryStream.Length];
                    memoryStream.Read(compressedData, 0, compressedData.Length);

                    var gzipBuffer = new byte[compressedData.Length + 4];
                    Buffer.BlockCopy(compressedData, 0, gzipBuffer, 4, compressedData.Length);
                    Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzipBuffer, 0, 4);
                    return Convert.ToBase64String(gzipBuffer);
                }
            }
        }

        /// <summary>
        /// GZIP decompress string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decompress(string text)
        {
            var gzipBuffer = Convert.FromBase64String(text);
            using (var memoryStream = new MemoryStream())
            {
                var dataLength = BitConverter.ToInt32(gzipBuffer, 0);
                memoryStream.Write(gzipBuffer, 4, gzipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                gzipStream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        #endregion

        #region Character Common Operations
        /// <summary>
        /// get string encoding length
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int GetEncodingLength(string input)
        {
            if (input == null)
            {
                input = "";
            }

            return Encoding.GetEncoding("gb2312").GetBytes(input).Length;
        }

        /// <summary>
        /// cut string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="ellipsis"></param>
        /// <returns></returns>
        public static string CutString(string input, int startIndex, int? length = null, bool ellipsis = false)
        {
            if (input == null)
            {
                input = string.Empty;
            }

            var len = input.Length;
            if (startIndex >= len)
            {
                return string.Empty;
            }

            if (length == null)
            {
                return input.Substring(startIndex);
            }

            if (startIndex + length >= len)
            {
                return input.Substring(startIndex);
            }

            if (ellipsis)
            {
                return input.Substring(startIndex, length.Value) + "…";
            }
            else
            {
                return input.Substring(startIndex, length.Value);
            }
        }

        /// <summary>
        /// cut string (according to the true length)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="ellipsis"></param>
        /// <returns></returns>
        public static string CutString(string input, int length, bool ellipsis)
        {
            if (input == null)
            {
                input = string.Empty;
            }

            var result = string.Empty;
            var byteLen = Encoding.GetEncoding("gb2312").GetByteCount(input);
            var charLen = input.Length;
            var byteCount = 0;//record read process
            var pos = 0;// record cut process
            var ary_ch = input.ToCharArray();

            if (byteLen > length)
            {
                for (var i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(ary_ch[i]) > 255)// if chinese +2
                    {
                        byteCount += 2;
                    }
                    else
                    {
                        byteCount += 1;
                    }

                    if (byteCount > length)// only record the last valid position when out of bounds
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == length)// record current position
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                {
                    result = input.Substring(0, pos);
                    if (ellipsis)
                    {
                        result += "…";
                    }
                }
            }
            else
            {
                result = input;
            }

            return result;
        }
        #endregion

        #region Encrypt And Decrypt
        /// <summary>
        /// byte array to string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string HexString(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var btHash in bytes)
            {
                sb.AppendFormat("{0:X2}", btHash);
            }

            return sb.ToString();
        }

        /// <summary>
        /// string to byte array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] HexBytes(string input)
        {
            var len = input.Length / 2;
            var btsInput = new byte[len];

            for (var i = 0; i < len; i++)
            {
                var c = Convert.ToInt32(input.Substring(i * 2, 2), 16);
                btsInput[i] = (byte)c;
            }

            return btsInput;
        }

        /// <summary>
        /// hash algorithm encryption
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Hash<T>(string input)
            where T : HashAlgorithm, new()
        {
            using (var alg = new T())
            {
                var btsHash = alg.ComputeHash(Encoding.UTF8.GetBytes(input));
                return btsHash;
            }
        }

        /// <summary>
        /// hash algorithm encryption
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HashHex<T>(string input)
            where T : HashAlgorithm, new()
        {
            var btsHash = Hash<T>(input);
            return HexString(btsHash);
        }

        public static byte[] Hash<T>(string input, byte[] key)
            where T : KeyedHashAlgorithm, new()
        {
            using (var alg = new T
            {
                Key = key
            })
            {
                var btsHash = alg.ComputeHash(Encoding.UTF8.GetBytes(input));
                return btsHash;
            }
        }

        public static string HashHex<T>(string input, byte[] key)
            where T : KeyedHashAlgorithm, new()
        {
            var btsHash = Hash<T>(input, key);
            return HexString(btsHash);
        }

        /// <summary>
        /// format string （When the string does not satisfy the length condition, it is encrypted and intercepted）
        /// </summary>
        /// <param name="key">string</param>
        /// <param name="length">required length</param>
        /// <returns></returns>
        public static string FormatString(string key, int length)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length < length)
            {
                var result = HashHex<SHA256CryptoServiceProvider>(key);
                return result.Substring(0, length);
            }
            else
            {
                return key.Substring(0, length);
            }
        }

        public static string Encrypt(string input, string key, string iv = "")
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);
                using (var hmac = new HMACMD5(keyBytes))
                {
                    aes.Key = hmac.ComputeHash(keyBytes);
                }

                var ranIv = iv.IsNull();
                var ivStr = ranIv ? RandomUtility.StringCommon(4).ToUpper() : iv;
                aes.IV = Encoding.ASCII.GetBytes(FormatString(ivStr, aes.KeySize / 8));

                using (var encryptor = aes.CreateEncryptor())
                {
                    var buffer = Encoding.UTF8.GetBytes(input);
                    var cipher = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                    var text = HexString(cipher);
                    if (ranIv)
                    {
                        text = ivStr + text;
                    }

                    var encode = new VigenereEncoder();
                    return encode.Encode(text, key);
                }
            }
        }

        public static string Decrypt(string input, string key, string iv = "")
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);
                using (var hmac = new HMACMD5(keyBytes))
                {
                    aes.Key = hmac.ComputeHash(keyBytes);
                }

                var encode = new VigenereEncoder();
                input = encode.Decode(input, key);

                var ranIv = iv.IsNull();
                var ivStr = ranIv ? input.Substring(0, 4) : iv;
                aes.IV = Encoding.ASCII.GetBytes(FormatString(ivStr, aes.KeySize / 8));
                if (ranIv)
                {
                    input = input.Substring(4);
                }

                var buffer = HexBytes(input);
                using (var decryptor = aes.CreateDecryptor())
                {
                    var cipher = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                    return Encoding.UTF8.GetString(cipher);
                }
            }
        }

        /// <summary>
        /// generate ras private key and public key by asymmetric
        /// </summary>
        /// <typeparam name="T">algorithm</typeparam>
        /// <param name="key">private and public key(to decrypt)</param>
        /// <param name="publicKey">public key(to encrypt)</param>
        public static void GenerateKey<T>(out string key, out string publicKey)
            where T : AsymmetricAlgorithm, new()
        {
            var asy = new T();
            publicKey = asy.ToXmlString(false);
            key = asy.ToXmlString(true);
        }

        /// <summary>
        /// asymmetric encrypt
        /// </summary>
        /// <param name="input">need to encrypt the string</param>
        /// <param name="key">public key</param>
        /// <returns>result</returns>
        public static string RSAEncrypt(string input, string key)
        {
            using (var asy = new RSACryptoServiceProvider())
            {
                asy.FromXmlString(key);
                var btsInput = Encoding.UTF8.GetBytes(input);
                btsInput = asy.Encrypt(btsInput, false);
                return HexString(btsInput);
            }
        }

        /// <summary>
        /// asymmetric decrypt
        /// </summary>
        /// <param name="input">need to decrypt the string</param>
        /// <param name="key">private and public key</param>
        /// <returns>result</returns>
        public static string RSADecrypt(string input, string key)
        {
            using (var asy = new RSACryptoServiceProvider())
            {
                asy.FromXmlString(key);
                var btsInput = HexBytes(input);
                btsInput = asy.Decrypt(btsInput, false);
                return Encoding.UTF8.GetString(btsInput);
            }
        }

        #endregion
    }
}
