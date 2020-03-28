using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace OCRLibrary
{
    public class TesseractOCR : IOptChaRec
    {
        public string srcLangCode;//OCR识别语言 jpn=日语 eng=英语
        private TesseractEngine TessOCR;
        private string errorInfo;

        public string OCRProcess(Bitmap img)
        {
            try {
                var page = TessOCR.Process(img);
                string res = page.GetText();
                page.Dispose();
                return res;
            }
            catch (Exception ex) {
                errorInfo = ex.Message;
                return null;
            }
        }

        public bool OCR_Init(string param1 = "", string param2 = "")
        {
            try
            {
                TessOCR = new TesseractEngine(Environment.CurrentDirectory + "\\tessdata", srcLangCode, EngineMode.Default);
                return true;
            }
            catch(Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }

        public void OCR_SetLangCode(string Code)
        {
            srcLangCode = Code;
        }

        public string GetLastError()
        {
            return errorInfo;
        }
    }
}
