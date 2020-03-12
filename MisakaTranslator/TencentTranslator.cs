/*
 *Namespace         MisakaTranslator
 *Class             TencentTranslator
 *Description       腾讯翻译（翻译君） API  其余几个类均为用于读取json结果时处理的
 *Author            Hanmin Qi
 *LastModifyTime    2020-03-12
 * ===============================================================
 * 以下是修改记录（任何一次修改都应被记录）
 * 日期   修改内容    作者
 * 2020-03-12       代码注释完成      果冻
 */

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MisakaTranslator
{
    class TencentTranslator
    {

        public static string appId;//腾讯翻译君API APPID
        public static string appKey;//腾讯翻译君API 密钥

        public static void TencentTrans_Init()
        {
            appId = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TencentTranslator", "appID");
            appKey = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TencentTranslator", "appKey");
        }


        /// <summary>
        /// 腾讯翻译君翻译API
        /// https://ai.qq.com/doc/nlptrans.shtml#1-%E6%8E%A5%E5%8F%A3%E6%8F%8F%E8%BF%B0
        /// </summary>
        /// <param name="sourceText"></param>
        /// <param name="desLang"></param>
        /// <param name="srcLang"></param>
        /// <returns></returns>
        public static string Fanyijun_Translate(string sourceText, string desLang, string srcLang)
        {
            // 原文
            string q = sourceText;

            Random rd = new Random();
            string salt = rd.Next(100000).ToString();

            string url = "https://api.ai.qq.com/fcgi-bin/nlp/nlp_texttranslate?";

            string req = "";
            req += "app_id=" + appId;
            req += "&nonce_str=" + salt;
            req += "&source=" + srcLang;
            req += "&target=" + desLang;
            req += "&text=" + HttpUtility.UrlEncode(q).ToUpper();
            req += "&time_stamp=" + GetTimeStamp();
            req += "&sign=" + EncryptString(req + "&app_key=" + appKey).ToUpper();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url+req);
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
            } catch (WebException ex) {
                return "Request Timeout";
            }
            
        }

        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
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
