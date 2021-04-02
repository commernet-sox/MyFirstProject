using CoreHtmlToImage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace StudyExtend.HtmlToImage
{
    class HtmlToImage
    {
        private static string toolFilename;

        private static string directory;

        private static string toolFilepath;

        static HtmlToImage()
        {
            toolFilename = "wkhtmltoimage";
            directory = AppContext.BaseDirectory;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                toolFilepath = Path.Combine(directory, toolFilename + ".exe");
                if (File.Exists(toolFilepath))
                {
                    return;
                }

                Assembly assembly = typeof(HtmlConverter).GetTypeInfo().Assembly;
                string @namespace = typeof(HtmlConverter).Namespace;
                using (Stream stream = assembly.GetManifestResourceStream(@namespace + "." + toolFilename + ".exe"))
                {
                    using (FileStream destination = File.OpenWrite(toolFilepath))
                    {
                        stream.CopyTo(destination);
                    }
                }

                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = "/bin/",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = "/bin/bash",
                    Arguments = "which wkhtmltoimage"
                });
                string text = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(text) && text.Contains("wkhtmltoimage"))
                {
                    toolFilepath = "wkhtmltoimage";
                    return;
                }

                throw new Exception("wkhtmltoimage does not appear to be installed on this linux system according to which command; go to https://wkhtmltopdf.org/downloads.html");
            }

            throw new Exception("OSX Platform not implemented yet");
        }
        public void SaveImage(string url, string path, string fileName, int width = 1024, ImageFormat format = ImageFormat.Png, int quality = 100)
        {
            if (path.Equals(""))
                throw new Exception("未指定保存文件的路径");
            byte[] bytes = FromUrl(url, width, format, quality);

            ImageProcessor.ImageFactory imageFactory = new ImageProcessor.ImageFactory(true, false);
            imageFactory.Load(bytes);
            ImageProcessor.Imaging.TextLayer textLayer = new ImageProcessor.Imaging.TextLayer();
            textLayer.DropShadow = false;
            textLayer.Text = "物流科技研究院提供技术支持";//如意仓样本
            textLayer.Opacity = 50;
            textLayer.Style = FontStyle.Bold;
            textLayer.FontFamily = new FontFamily("arial");
            textLayer.FontSize = 28;
            textLayer.FontColor = Color.Red;//Color.AliceBlue
            textLayer.Style = FontStyle.Bold;
            textLayer.Position = new Point(0, 300);
            WatermarkRotate45 watermark = new WatermarkRotate45();
            watermark.DynamicParameter = textLayer;
            var watermark_images = watermark.ProcessImage(imageFactory);
            if (!File.Exists(@"" + path))
            {
                Directory.CreateDirectory(@"" + path);
            }
            var filePath = @"" + path + fileName + ".png";
            watermark_images.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            watermark_images.Dispose();

        }
        public byte[] FromUrl(string url, int width = 1024, ImageFormat format = ImageFormat.Png, int quality = 100)
        {
            string text = format.ToString().ToLower();
            string text2 = Path.Combine(directory, Guid.NewGuid().ToString() + "." + text);
            Process process = Process.Start(new ProcessStartInfo(arguments: (!IsLocalPath(url)) ? $"--quality {quality} --width {width} -f {text} {url} \"{text2}\"" : $"--quality {quality} --width {width} -f {text} \"{url}\" \"{text2}\"", fileName: toolFilepath)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = directory,
                RedirectStandardError = true
            });
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.WaitForExit();
            if (File.Exists(text2))
            {
                byte[] result = File.ReadAllBytes(text2);
                File.Delete(text2);
                return result;
            }

            throw new Exception("Something went wrong. Please check input parameters");
        }
        private bool IsLocalPath(string path)
        {
            if (path.StartsWith("http"))
            {
                return false;
            }

            return new Uri(path).IsFile;
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new Exception(e.Data);
        }
    }
}
