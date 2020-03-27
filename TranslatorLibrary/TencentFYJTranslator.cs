using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace TranslatorLibrary
{
    public class TencentFYJTranslator : ITranslator
    {
        private string errorInfo;//错误信息
        public string appId;//腾讯翻译君API APPID
        public string appKey;//腾讯翻译君API 密钥
        
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

            string url = "https://api.ai.qq.com/fcgi-bin/nlp/nlp_texttranslate?";

            string req = "";
            req += "app_id=" + appId;
            req += "&nonce_str=" + salt;
            req += "&source=" + srcLang;
            req += "&target=" + desLang;
            req += "&text=" + HttpUtility.UrlEncode(q).ToUpper();
            req += "&time_stamp=" + CommonFunction.GetTimeStamp();
            req += "&sign=" + CommonFunction.EncryptString(req + "&app_key=" + appKey).ToUpper();

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
                errorInfo =  "Request Timeout";
                return null;
            }

            TencentTransOutInfo oinfo = JsonConvert.DeserializeObject<TencentTransOutInfo>(retString);

            if (oinfo.ret == "0")
            {
                //得到翻译结果
                return oinfo.data.target_text;
            }
            else
            {
                errorInfo = "ErrorID:" + oinfo.ret + " ErrorInfo:" + oinfo.msg;
                return null;
            }
        }

        public void TranslatorInit(string param1, string param2)
        {
            appId = param1;
            appKey = param2;
        }

        /// <summary>
        /// 翻译君API申请地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_allpyAPI()
        {
            return "https://ai.qq.com/product/nlptrans.shtml";
        }

        /// <summary>
        /// 翻译君API额度查询地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_bill()
        {
            return "https://ai.qq.com/console/home";
        }

        /// <summary>
        /// 翻译君API文档地址（错误代码）
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_Doc()
        {
            return "https://ai.qq.com/doc/nlptrans.shtml";
        }
    }

    class TencentTransOutInfo
    {
        public string ret { get; set; }
        public string msg { get; set; }
        public TencentTransResult data { get; set; }
    }

    class TencentTransResult
    {
        public string source_text { get; set; }
        public string target_text { get; set; }
    }
}
