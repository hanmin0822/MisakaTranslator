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
            "BaiduFanyiOCR",
            "TesseractOCR",
            "TesseractCli"
        };

        public static List<string> GetOCRList()
        {
            return lstOCR;
        }

        public static OCREngine OCRAuto(string ocr) {
            switch (ocr)
            {
                case "BaiduOCR":
                    return new BaiduGeneralOCR(); ;
                case "BaiduFanyiOCR":
                    return new BaiduFanyiOCR();
                case "TesseractOCR":
                    return new TesseractOCR(); ;
                case "TesseractCli":
                    return new TesseractCli();
                default:
                    return null;
            }
        }
    }
}
