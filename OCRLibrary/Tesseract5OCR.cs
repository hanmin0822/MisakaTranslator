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
    public class Tesseract5OCR : IOptChaRec
    {
        public string srcLangCode;//OCR识别语言 jpn=日语 eng=英语
        private string errorInfo;

        private IntPtr WinHandle;
        private Rectangle OCRArea;
        private bool isAllWin;

        public string GetLastError()
        {
            return errorInfo;
        }

        public Image GetOCRAreaCap()
        {
            return ScreenCapture.GetWindowRectCapture(WinHandle, OCRArea, isAllWin);
        }

        public string OCRProcess(Bitmap img)
        {
            Bitmap processedImg = (Bitmap)img.Clone();
            try
            {
                processedImg.Save(Environment.CurrentDirectory + "\\temp\\tmp.png", System.Drawing.Imaging.ImageFormat.Png);
                Process p = new Process();
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = "C:\\Program Files\\Tesseract-OCR\\tesseract";
                p.StartInfo.Arguments = "temp\\tmp.png temp\\outputbase -l jpn_vert --psm 5";
                p.Start();
                // Do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // p.WaitForExit();
                // Read the output stream first and then wait.
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                byte[] bs = System.IO.File.ReadAllBytes(Environment.CurrentDirectory + "\\temp\\outputbase.txt");
                string result = Encoding.UTF8.GetString(bs);
                return result;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return null;
            }
        }

        public string OCRProcess()
        {
            if (OCRArea != null)
            {
                Image img = ScreenCapture.GetWindowRectCapture(WinHandle, OCRArea, isAllWin);
                return OCRProcess(new Bitmap(img));
            }
            else
            {
                errorInfo = "未设置截图区域";
                return null;
            }
        }

        public bool OCR_Init(string param1 = "", string param2 = "")
        {
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

        public void SetOCRArea(IntPtr handle, Rectangle rec, bool AllWin)
        {
            WinHandle = handle;
            OCRArea = rec;
            isAllWin = AllWin;
        }

        public void SetOCRSourceLang(string lang)
        {
            srcLangCode = lang;
        }
    }
}
