/*
 *Namespace         MisakaTranslator
 *Class             BaiduTranslator
 *Description       百度翻译 API  其余几个类均为用于读取json结果时处理的
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MisakaTranslator
{
    class BaiduTranslator
    {

        public static string appId;//百度翻译API 的APP ID
        public static string secretKey;//百度翻译API 的密钥
        
        public static void BaiduTrans_Init() {
            appId = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "BaiduTranslator", "appID");
            secretKey = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "BaiduTranslator", "secretKey");
        }
        
        /// <summary>
        /// 调用百度在线API翻译
        /// 语言简写列表 http://api.fanyi.baidu.com/product/113
        /// </summary>
        /// <param name="sourceText">需要翻译的文本</param>
        /// <param name="desLang">目的语言</param>
        /// <param name="srcLang">源语言</param>
        /// <returns></returns>
        public static string Baidu_Translate(string sourceText,string desLang,string srcLang = "auto")
        {
            if (desLang == "kr")
                desLang = "kor";
            if (srcLang == "kr")
                srcLang = "kor";
            if (desLang == "fr")
                desLang = "fra";
            if (srcLang == "fr")
                srcLang = "fra";

            // 原文
            string q = sourceText;
            
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            
            string sign = EncryptString(appId + q + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + srcLang;
            url += "&to=" + desLang;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            try {
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
        
        // 计算MD5值
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }
        
    }

    class BaiduTransOutInfo {
        public string from { get; set; }
        public string to { get; set; }
        public List<BaiduTransResult> trans_result { get; set; }
        public string error_code { get; set; }
    }

    class BaiduTransResult {
        public string src { get; set; }
        public string dst { get; set; }
    }
}
