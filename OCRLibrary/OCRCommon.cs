using System;
using System.Collections.Generic;

namespace OCRLibrary
{
    public static class OCRCommon
    {
        public static List<string> lstOCR = new List<string>()
        {
            "BaiduOCR",
            "BaiduFanyiOCR",
            "TencentOCR",
            "TesseractOCR",
            "TesseractCli"
        };

        static OCRCommon()
        {
            if (Environment.OSVersion.Version.Build >= 10240)
            {
                lstOCR.Add("WindowsOCR");
            }
        }

        public static List<string> GetOCRList()
        {
            return lstOCR;
        }

        public static OCREngine OCRAuto(string ocr)
        {
            switch (ocr)
            {
                case "BaiduOCR":
                    return new BaiduGeneralOCR();
                case "BaiduFanyiOCR":
                    return new BaiduFanyiOCR();
                case "TencentOCR":
                    return new TencentOCR();
                case "TesseractOCR":
                    return new TesseractOCR();
                case "TesseractCli":
                    return new TesseractCli();
                case "WindowsOCR":
                    return new WindowsOCR();
                default:
                    return null;
            }
        }

        public static System.Text.Json.JsonSerializerOptions JsonOP = new()
        {
            IncludeFields = true
        };
    }
}
