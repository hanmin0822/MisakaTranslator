using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public string Translate(string sourceText, string desLang, string srcLang)
        {
            if (sourceText == "" || desLang == "" || srcLang == "")
            {
                errorInfo = "Param Missing";
                return null;
            }
            
            // 原文
            string q = sourceText;

            string retString;

            Random rd = new Random();
            string salt = rd.Next(100000).ToString();

            string url = "https://tmt.tencentcloudapi.com/?";

            string req = "Action=TextTranslate";
            req += "&Nonce=" + salt;
            req += "&ProjectId=0";
            req += "&Region=ap-shanghai";
            req += "&SecretId=" + SecretId;
            req += "&Source=" + srcLang;
            req += "&SourceText=" + sourceText;
            req += "&Target=" + desLang;
            req += "&Timestamp=" + CommonFunction.GetTimeStamp();
            req += "&Version=2018-03-21";

            HMACSHA1 hmac = new HMACSHA1()
            {
                Key = System.Text.Encoding.UTF8.GetBytes(SecretKey)
            };
            byte[] data = Encoding.UTF8.GetBytes("GETtmt.tencentcloudapi.com/?" + req);
            var result = hmac.ComputeHash(data);
            req = req + "&Signature=" + HttpUtility.UrlEncode(Convert.ToBase64String(result));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + req);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                
            }
            catch (WebException ex)
            {
                errorInfo = "Request Timeout";
                return null;
            }

            TencentOldTransOutInfo oinfo = JsonConvert.DeserializeObject<TencentOldTransOutInfo>(retString);

            if (oinfo.Response.Error == null)
            {
                //得到翻译结果
                return oinfo.Response.TargetText;
            }
            else
            {
                errorInfo = "ErrorID:" + oinfo.Response.Error.Code + " ErrorInfo:" + oinfo.Response.Error.Message;
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

    class TencentOldTransOutInfo
    {
        public TencentOldTransResult Response { get; set; }
    }

    class TencentOldTransResult
    {
        public string RequestId { get; set; }
        public string TargetText { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public TencentOldTransOutError Error { get; set; }
    }

    class TencentOldTransOutError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

}
