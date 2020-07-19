using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRLibrary
{
    public class OCRCommon
    {
        public static List<string> lstOCR = new List<string>()
        {
            "BaiduOCR",
            "TesseractOCR",
            "Tesseract5_vert"
        };

        public static List<string> GetOCRList()
        {
            return lstOCR;
        }

        public static IOptChaRec OCRAuto(string ocr) {
            switch (ocr)
            {
                case "BaiduOCR":
                    return new BaiduGeneralOCR(); ;
                case "TesseractOCR":
                    return new TesseractOCR(); ;
                case "Tesseract5_vert":
                    return new Tesseract5OCR();
                default:
                    return null;
            }
        }
    }
}
