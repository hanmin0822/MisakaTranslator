using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRLibrary
{
    public class Tesseract5OCR : OCREngine
    {
        public string srcLangCode;//OCR识别语言 jpn=日语 eng=英语
        private string path;
        private string args;

        public override async Task<string> OCRProcessAsync(Bitmap img)
        {
            Bitmap processedImg = (Bitmap)img.Clone();
            try
            {
                processedImg.Save(Environment.CurrentDirectory + "\\temp\\tmp.png", System.Drawing.Imaging.ImageFormat.Png);
                Process p = new Process();
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = path;
                p.StartInfo.Arguments = "temp\\tmp.png temp\\outputbase " + args;
                p.Start();
                // Wait for the child process to exit before
                // reading to the end of its redirected stream.
                await Task.Run(() =>
                {
                    try
                    {
                        p.WaitForExit();
                    }
                    catch (Exception e)
                    {
                        errorInfo = e.Message;
                    }
                });
                // Read the output stream first and then wait.
                string output = p.StandardOutput.ReadToEnd(); // usually empty
                string err = p.StandardError.ReadToEnd();     // information is all here
                if (err.ToLower().Contains("error"))
                {
                    throw new Exception(err);
                }
                byte[] bs = System.IO.File.ReadAllBytes(Environment.CurrentDirectory + "\\temp\\outputbase.txt");
                string result = Encoding.UTF8.GetString(bs);
                return result;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return "";
            }
        }

        public override bool OCR_Init(string path, string args)
        {
            this.path = path;
            this.args = args;
            try
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\temp");
                return true;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }

        public override void SetOCRSourceLang(string lang)
        {
            srcLangCode = lang;
        }
    }
}
