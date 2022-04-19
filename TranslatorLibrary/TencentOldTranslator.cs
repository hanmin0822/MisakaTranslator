using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TranslatorLibrary
{
    public class TencentOldTranslator : ITranslator
    {

        private string errorInfo;//错误信息
        public string SecretId;//腾讯旧版API SecretId
        public string SecretKey;//腾讯旧版API SecretKey

        public string GetLastError()
        {
            return errorInfo;
        }

        public async Task<string> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            if (sourceText == "" || desLang == "" || srcLang == "")
            {
                errorInfo = "Param Missing";
                return null;
            }

            string salt = CommonFunction.RD.Next(100000).ToString();
            string ts = CommonFunction.GetTimeStamp().ToString();

            string url = "https://tmt.tencentcloudapi.com/?";

            // 签名，参数使用未URL编码的值
            var sb = new StringBuilder()
                .Append("Action=TextTranslate")
                .Append("&Nonce=").Append(salt)
                .Append("&ProjectId=0")
                .Append("&Region=ap-shanghai")
                .Append("&SecretId=").Append(SecretId)
                .Append("&Source=").Append(srcLang)
                .Append("&SourceText=").Append(sourceText)
                .Append("&Target=").Append(desLang)
                .Append("&Timestamp=").Append(ts)
                .Append("&Version=2018-03-21");
            string req = sb.ToString();

            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(SecretKey));
            byte[] data = Encoding.UTF8.GetBytes("GETtmt.tencentcloudapi.com/?" + req);
            var result = hmac.ComputeHash(data);
            hmac.Dispose();

            // 请求参数，参数使用URL编码后的值
            sb = new StringBuilder()
                .Append("Action=TextTranslate")
                .Append("&Nonce=").Append(salt)
                .Append("&ProjectId=0")
                .Append("&Region=ap-shanghai")
                .Append("&SecretId=").Append(SecretId)
                .Append("&Source=").Append(srcLang)
                .Append("&SourceText=").Append(HttpUtility.UrlEncode(sourceText))
                .Append("&Target=").Append(desLang)
                .Append("&Timestamp=").Append(ts)
                .Append("&Version=2018-03-21")
                .Append("&Signature=").Append(HttpUtility.UrlEncode(Convert.ToBase64String(result)));
            req = sb.ToString();

            string retString;
            var hc = CommonFunction.GetHttpClient();
            try
            {
                retString = await hc.GetStringAsync(url + req);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                errorInfo = ex.Message;
                return null;
            }
            catch (TaskCanceledException ex)
            {
                errorInfo = ex.Message;
                return null;
            }

            TencentOldTransOutInfo oinfo = JsonSerializer.Deserialize<TencentOldTransOutInfo>(retString, CommonFunction.JsonOP);

            if (oinfo.Response.Error == null)
            {
                //得到翻译结果
                return oinfo.Response.TargetText;
            }
            else
            {
                errorInfo = "ErrorID:" + oinfo.Response.Error.Value.Code + " ErrorInfo:" + oinfo.Response.Error.Value.Message;
                return null;
            }

        }

        public void TranslatorInit(string param1, string param2)
        {
            SecretId = param1;
            SecretKey = param2;
        }


        /// <summary>
        /// 腾讯旧版翻译API申请地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_allpyAPI()
        {
            return "https://cloud.tencent.com/product/tmt";
        }

        /// <summary>
        /// 腾讯旧版翻译API额度查询地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_bill()
        {
            return "https://console.cloud.tencent.com/tmt";
        }

        /// <summary>
        /// 腾讯旧版翻译API文档地址（错误代码）
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_Doc()
        {
            return "https://cloud.tencent.com/document/api/551/15619";
        }
    }

#pragma warning disable 0649
    struct TencentOldTransOutInfo
    {
        public TencentOldTransResult Response;
    }

    struct TencentOldTransResult
    {
        public string RequestId;
        public string TargetText;
        public string Source;
        public string Target;
        public TencentOldTransOutError? Error;
    }

    struct TencentOldTransOutError
    {
        public string Code;
        public string Message;
    }

}
