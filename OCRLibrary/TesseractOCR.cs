extern alias Tesseract;
using tesseract = Tesseract.TesseractOCR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRLibrary
{
    public class TesseractOCR : OCREngine
    {
        private string srcLangCode;  //OCR识别语言 jpn=日语 eng=英语
        private tesseract.Engine engine;

        public override Task<string> OCRProcessAsync(Bitmap img)
        {
            try
            {
                var stream = new MemoryStream();
                img.Save(stream, ImageFormat.Bmp);
                var pix = tesseract.Pix.Image.LoadFromMemory(stream);

                var recog = engine.Process(pix);
                string text = recog.Text;
                stream.Dispose();
                recog.Dispose();
                
                return Task.FromResult(text);
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return Task.FromResult<string>(null);
            }
        }

        public override bool OCR_Init(string param1 = "", string param2 = "")
        {
            try
            {
                engine = new tesseract.Engine(Environment.CurrentDirectory + "\\tessdata", srcLangCode);
                return true;
            }
            catch(Exception ex)
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
