using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace SDT.BaseTool
{
    public sealed class FileUtility
    {
        #region Encrypt And Decrypt Files
        /// <summary>
        /// get file hash value
        /// </summary>
        /// <param name="path">full file path</param>
        /// <returns>result(upper)</returns>
        public static string GetHash<T>(string path)
            where T : HashAlgorithm, new()
        {
            using (var stream = File.OpenRead(path))
            {
                using (var alg = new T())
                {
                    var btsHash = alg.ComputeHash(stream);
                    return StringUtility.HexString(btsHash);
                }
            }
        }

        /// <summary>
        /// encrypt file
        /// </summary>
        /// <typeparam name="T">algorithm</typeparam>
        /// <param name="input">need encrypt file path</param>
        /// <param name="output">encrypted file output path</param>
        /// <param name="key">encrypt key</param>
        public static void Encrypt<T>(string input, string output, string key)
            where T : SymmetricAlgorithm, new()
        {
            using (var fs = File.OpenRead(input))
            {
                var btsFile = new byte[fs.Length];
                fs.Read(btsFile, 0, (int)fs.Length);

                using (var alg = new T())
                {
                    alg.Key = Encoding.ASCII.GetBytes(StringUtility.FormatString(key, alg.KeySize / 8));
                    alg.Mode = CipherMode.ECB;
                    var dector = alg.CreateEncryptor();
                    var btsHash = dector.TransformFinalBlock(btsFile, 0, btsFile.Length);
                    using (var stream = File.OpenWrite(output))
                    {
                        foreach (var b in btsHash)
                        {
                            stream.WriteByte(b);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// decrypt file
        /// </summary>
        /// <typeparam name="T">algorithm</typeparam>
        /// <param name="input">need encrypt file path</param>
        /// <param name="output">encrypted file output path</param>
        /// <param name="key">encrypt key</param>
        public static void Decrypt<T>(string input, string output, string key)
            where T : SymmetricAlgorithm, new()
        {
            using (var fs = File.OpenRead(input))
            {
                var btsFile = new byte[fs.Length];
                fs.Read(btsFile, 0, (int)fs.Length);

                using (var alg = new T())
                {
                    alg.Key = Encoding.ASCII.GetBytes(StringUtility.FormatString(key, alg.KeySize / 8));
                    alg.Mode = CipherMode.ECB;
                    var dector = alg.CreateDecryptor();
                    var btsHash = dector.TransformFinalBlock(btsFile, 0, btsFile.Length);
                    using (var stream = File.OpenWrite(output))
                    {
                        foreach (var b in btsHash)
                        {
                            stream.WriteByte(b);
                        }
                    }
                }
            }
        }
        #endregion

        #region Compression

        public static void Compress(string sourceDir, string destFile) => ZipFile.CreateFromDirectory(sourceDir, destFile);

        public static void CompressAppend(string sourceZip, string destFile)
        {
            using (var zipFs = new FileStream(sourceZip, FileMode.Open))
            {
                using (var archive = new ZipArchive(zipFs, ZipArchiveMode.Update))
                {
                    archive.CreateEntryFromFile(destFile, Path.GetFileName(destFile));
                }
            }
        }

        public static void Decompress(string sourceZip, string extractDir) => ZipFile.ExtractToDirectory(sourceZip, extractDir);

        public static void DecompressPartial(string sourceZip, string extractDir, params string[] sourceEntries)
        {
            using (var archive = ZipFile.OpenRead(sourceZip))
            {
                foreach (var entryName in sourceEntries)
                {
                    var entry = archive.GetEntry(entryName);
                    if (entry != null)
                    {
                        entry.ExtractToFile(Path.Combine(extractDir, entry.FullName));
                    }
                }
            }
        }
        #endregion

        #region Other
        public static Encoding GetEncoding(string filePath, Encoding encode)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var targetEncoding = encode;
                if (stream != null && stream.Length > 2)
                {
                    byte byte3 = 0x00;

                    //save the current seek location
                    var origPos = stream.Seek(0, SeekOrigin.Begin);
                    stream.Seek(0, SeekOrigin.Begin);

                    var readByte = stream.ReadByte();

                    //save the first four bytes of the file stream
                    var byte1 = Convert.ToByte(readByte);
                    var byte2 = Convert.ToByte(stream.ReadByte());
                    if (stream.Length >= 3)
                    {
                        byte3 = Convert.ToByte(stream.ReadByte());
                    }

                    //determines the encoding based on the first four bytes of the file flow
                    if (byte1 == 0xFE && byte2 == 0xFF)
                    {
                        targetEncoding = Encoding.BigEndianUnicode;
                    }
                    else if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)
                    {
                        targetEncoding = Encoding.UTF8;
                    }
                    else if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)
                    {
                        targetEncoding = Encoding.UTF8;
                    }

                    //recovery seek position
                    stream.Seek(origPos, SeekOrigin.Begin);
                }

                return targetEncoding;
            }
        }

        /// <summary>
        /// copy directory
        /// need to determine if a floder exists before
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        public static void CopyDirectory(string sourceDir, string destDir)
        {
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var destFileName = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFileName, true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                var destDirPath = Path.Combine(destDir, Path.GetFileName(dir));
                CopyDirectory(dir, destDirPath);
            }
        }
        #endregion
    }
}
