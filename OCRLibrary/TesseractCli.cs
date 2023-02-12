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
    public class TesseractCli : OCREngine
    {
        private string path;
        private string args;

        public override Task<string> OCRProcessAsync(Bitmap img)
        {
            try
            {
                Process p = Process.Start(new ProcessStartInfo()
                {
                    FileName = path,
                    Arguments = "- - " + args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8
                });
                var imgdata = ImageProcFunc.Image2Bytes(img);
                p.StandardInput.BaseStream.Write(imgdata, 0, imgdata.Length);
                p.StandardInput.Close();
                p.WaitForExit();

                string err = p.StandardError.ReadToEnd();
                if (err.ToLower().Contains("error"))
                {
                    errorInfo = err;
                    return Task.FromResult<string>(null);
                }
         
                string result = p.StandardOutput.ReadToEnd();
                p.Dispose();
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return Task.FromResult<string>(null);
            }
        }

        public override bool OCR_Init(string path, string args)
        {
            if(!File.Exists(path))
                return false;
            this.path = path;
            this.args = args;
            return true;
        }

        public override void SetOCRSourceLang(string lang)
        {
        }
    }
}
