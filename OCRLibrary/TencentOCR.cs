using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.Text.Json;

namespace OCRLibrary
{
    public class TencentOCR : OCREngine
    {
        public string SecretId;
        public string SecretKey;
        private string langCode;
        static int SessionUuid = 0;

        public override async Task<string> OCRProcessAsync(Bitmap img)
        {
            if (img == null || langCode == null || langCode == "")
            {
                errorInfo = "Param Missing";
                return null;
            }

            string data = ImageProcFunc.GetFileBase64(img);
            string ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            SessionUuid++;

            // 拼接除了Signature以外的参数，必须按字母排序
            var sb = new StringBuilder()
                .Append("Action=ImageTranslate")
                .Append("&Data=").Append(data)
                .Append("&Nonce=").Append(SessionUuid)
                .Append("&ProjectId=0")
                .Append("&Region=ap-shanghai")
                .Append("&Scene=doc")
                .Append("&SecretId=").Append(SecretId)
                .Append("&SessionUuid=").Append(SessionUuid)
                .Append("&Source=").Append(langCode)
                .Append("&Target=zh")
                .Append("&Timestamp=").Append(ts)
                .Append("&Version=2018-03-21");

            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(SecretKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes("GETtmt.tencentcloudapi.com/?" + sb.ToString()));
            hmac.Dispose();

            // 计算完Signature后，base64编码，再URL编码，加上去。另外Data是base64编码后的，也要URL编码
            var sb2 = new StringBuilder("https://tmt.tencentcloudapi.com/?")
                .Append("Action=ImageTranslate")
                .Append("&Data=").Append(HttpUtility.UrlEncode(data))
                .Append("&Nonce=").Append(SessionUuid)
                .Append("&ProjectId=0")
                .Append("&Region=ap-shanghai")
                .Append("&Scene=doc")
                .Append("&SecretId=").Append(SecretId)
                .Append("&SessionUuid=").Append(SessionUuid)
                .Append("&Source=").Append(langCode)
                .Append("&Target=zh")
                .Append("&Timestamp=").Append(ts)
                .Append("&Version=2018-03-21")
                .Append("&Signature=").Append(HttpUtility.UrlEncode(Convert.ToBase64String(hashBytes)));

            HttpWebRequest request = WebRequest.CreateHttp(sb2.ToString());
            request.Timeout = 8000;
            request.UserAgent = "MisakaTranslator";

            try
            {
                using (var resp = await request.GetResponseAsync())
                {
                    string retStr = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                    var result = JsonSerializer.Deserialize<Result>(retStr, OCRCommon.JsonOP).Response;
                    if (result.Error != null)
                    {
                        errorInfo = result.Error?.Code + " " + result.Error?.Message;
                        return null;
                    }

                    return String.Join("\n", result.ImageRecord?.Value.Select(x => x.TargetText));
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
            SecretId = param1;
            SecretKey = param2;
            return true;
        }

        public static string GetUrl_allpyAPI()
        {
            return "https://cloud.tencent.com/product/tmt";
        }

        public static string GetUrl_bill()
        {
            return "https://console.cloud.tencent.com/tmt";
        }

        public static string GetUrl_Doc()
        {
            return "https://cloud.tencent.com/document/product/551/17232";
        }

        public override void SetOCRSourceLang(string lang)
        {
            if (lang == "jpn")
                langCode = "ja";
            else if (lang == "eng")
                langCode = "en";
            else
                langCode = lang;
        }

#pragma warning disable 0649
        struct Result
        {
            public Response Response;
        }
        struct Response
        {
            public string SessionUuid;
            public string Source;
            public string Target;
            public string RequestId;
            public Record? ImageRecord;
            public Error? Error;
        }
        struct Record
        {
            public ItemValue[] Value;
        }
        struct ItemValue
        {
            public string SourceText;
            public string TargetText;
            public int X, Y, W, H;
        }
        struct Error
        {
            public string Code, Message;
        }
    }
}
