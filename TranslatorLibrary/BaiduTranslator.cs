using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Web;

namespace TranslatorLibrary
{
    public class BaiduTranslator : ITranslator
    {
        //语言简写列表 https://api.fanyi.baidu.com/product/113

        public string appId;//百度翻译API 的APP ID
        public string secretKey;//百度翻译API 的密钥
        private string errorInfo;//错误信息

        public async Task<string> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            if (sourceText == "" || desLang == "" || srcLang == "") {
                errorInfo = "Param Missing";
                return null;
            }
            
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

            string retString;

            string salt = CommonFunction.RD.Next(100000).ToString();

            string sign = CommonFunction.EncryptString(appId + q + salt + secretKey);
            var sb = new StringBuilder("https://api.fanyi.baidu.com/api/trans/vip/translate?")
                .Append("q=").Append(HttpUtility.UrlEncode(q))
                .Append("&from=").Append(srcLang)
                .Append("&to=").Append(desLang)
                .Append("&appid=").Append(appId)
                .Append("&salt=").Append(salt)
                .Append("&sign=").Append(sign);
            string url = sb.ToString();

            var hc = CommonFunction.GetHttpClient();
            try
            {
                retString = await hc.GetStringAsync(url);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                errorInfo = ex.Message;
                return null;
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                errorInfo = ex.Message;
                return null;
            }

            BaiduTransOutInfo oinfo = JsonSerializer.Deserialize<BaiduTransOutInfo>(retString, CommonFunction.JsonOP);

            if (oinfo.error_code == null || oinfo.error_code == "52000")
            {
                // 52000就是成功
                if (oinfo.trans_result.Length == 0)
                    return "";
                else if(oinfo.trans_result.Length == 1)
                    return oinfo.trans_result[0].dst;
                else
                {
                    var sb2 = new StringBuilder();
                    foreach (var entry in oinfo.trans_result)
                        sb2.AppendLine(entry.dst);
                    return sb2.ToString();
                }
            }
            else
            {
                errorInfo = "ErrorID:" + oinfo.error_code;
                return null;
            }
            
        }

        public void TranslatorInit(string param1, string param2)
        {
            appId = param1;
            secretKey = param2;
        }
        
        
        public string GetLastError()
        {
            return errorInfo;
        }

        /// <summary>
        /// 百度翻译API申请地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_allpyAPI()
        {
            return "https://api.fanyi.baidu.com/product/11";
        }

        /// <summary>
        /// 百度翻译API额度查询地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_bill()
        {
            return "https://api.fanyi.baidu.com/api/trans/product/desktop";
        }

        /// <summary>
        /// 百度翻译API文档地址（错误代码）
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_Doc()
        {
            return "https://api.fanyi.baidu.com/doc/21";
        }
    }

#pragma warning disable 0649
    struct BaiduTransOutInfo
    {
        public string from;
        public string to;
        public BaiduTransResult[] trans_result;
        public string error_code;
    }

    struct BaiduTransResult
    {
        public string src;
        public string dst;
    }
}
