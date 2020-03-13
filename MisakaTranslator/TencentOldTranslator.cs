/*
 *Namespace         MisakaTranslator
 *Class             TencentOldTranslator
 *Description       腾讯翻译（私人） API  其余几个类均为用于读取json结果时处理的
 */


using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MisakaTranslator
{
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

    class TencentOldTranslator
    {
        public static string SecretId;//腾讯旧版API SecretId
        public static string SecretKey;//腾讯旧版API SecretKey

        public static void TencentOldTrans_Init()
        {
            SecretId = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TencentOldTranslator", "SecretId");
            SecretKey = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TencentOldTranslator", "SecretKey");
        }

        public static string TencentOld_Translate(string sourceText, string desLang, string srcLang) {
            // 原文
            string q = sourceText;

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
            req += "&Timestamp=" + TencentTranslator.GetTimeStamp();
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
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (WebException ex)
            {
                return "Request Timeout";
            }
        }


    }
}
