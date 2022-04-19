using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace OCRLibrary
{
    public class BaiduFanyiOCR : OCREngine
    {
        public string appId;
        public string secretKey;
        const string salt = "123456";
        private string langCode;

        public override async Task<string> OCRProcessAsync(Bitmap img)
        {
            if (img == null || langCode == null || langCode == "")
            {
                errorInfo = "Param Missing";
                return null;
            }

            byte[] imgdata = ImageProcFunc.Image2Bytes(img);

            // 计算sign
            MD5 md5 = MD5.Create();
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(appId);
            sw.Write(BitConverter.ToString(md5.ComputeHash(imgdata)).Replace("-", "").ToLower());
            sw.Write(salt);
            sw.Write("APICUIDmac");
            sw.Write(secretKey);
            sw.Flush();
            string sign = BitConverter.ToString(md5.ComputeHash(ms.ToArray())).Replace("-", "").ToLower();
            sw.Close();
            md5.Dispose();

            string endpoint = "https://fanyi-api.baidu.com/api/trans/sdk/picture?cuid=APICUID&mac=mac&salt=" + salt + "&appid=" + appId + "&sign=" + sign + "&to=zh&from=" + langCode;


            HttpWebRequest request = WebRequest.CreateHttp(endpoint);
            request.Method = "POST";
            request.Timeout = 8000;
            request.UserAgent = "MisakaTranslator";
            const string boundary = "boundary";
            request.ContentType = "multipart/form-data;boundary=" + boundary;

            var rsw = new StreamWriter(request.GetRequestStream());
            rsw.WriteLine("--" + boundary);
            rsw.WriteLine("Content-Disposition: form-data;name=image;filename=data.png");
            rsw.WriteLine("Content-Type: application/octet-stream");
            rsw.WriteLine("Content-Transfer-Encoding: binary\r\n");
            rsw.Flush();
            rsw.BaseStream.Write(imgdata, 0, imgdata.Length);
            rsw.WriteLine("\r\n--" + boundary + "--");
            rsw.Close();

            try
            {
                using (var resp = await request.GetResponseAsync())
                {
                    string retStr = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                    var result = JsonSerializer.Deserialize<Result>(retStr, OCRCommon.JsonOP);
                    if (result.error_code == "0")
                        return result.data.sumDst;
                    else
                    {
                        errorInfo = result.error_code + " " + result.error_msg;
                        return null;
                    }
                }
            }
            catch (WebException ex)
            {
                errorInfo = ex.Message;
                return null;
            }
        }

        public override bool OCR_Init(string param1, string param2)
        {
            appId = param1;
            secretKey = param2;
            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(secretKey) || appId.Length == 24)
            {
                errorInfo = "Wrong secret.";
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// 百度翻译OCRAPI申请地址
        /// </summary>
        public static string GetUrl_allpyAPI()
        {
            return "https://fanyi-api.baidu.com/";
        }

        /// <summary>
        /// 百度翻译OCRAPI额度查询地址
        /// </summary>
        public static string GetUrl_bill()
        {
            return "https://fanyi-api.baidu.com/api/trans/product/desktop";
        }

        /// <summary>
        /// 百度翻译OCRAPI文档地址
        /// </summary>
        public static string GetUrl_Doc()
        {
            return "https://fanyi-api.baidu.com/doc/26";
        }

        public override void SetOCRSourceLang(string lang)
        {
            if (lang == "jpn")
                langCode = "jp";
            else if (lang == "eng")
                langCode = "en";
            else
                langCode = lang;
        }

#pragma warning disable 0649
        struct Result
        {
            public string error_code;
            public string error_msg;
            public Data data;
        }
        struct Data
        {
            public string sumDst;
        }
    }
}
