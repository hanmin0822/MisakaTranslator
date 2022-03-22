using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Net.Http;
using System.Text.Json;

namespace TranslatorLibrary
{
    public class YoudaoZhiyun : ITranslator
    {
        private static readonly string TRANSLATE_API_URL = "https://openapi.youdao.com/api";
        private string appId, appSecret;
        private string errorInfo;

        public async Task<string> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            if (sourceText == "" || desLang == "" || srcLang == "")
            {
                errorInfo = "Param Missing";
                return null;
            }

            string q = sourceText;
            string input = q.Length <= 20 ? q : q.Substring(0, 10) + q.Length + q.Substring(q.Length - 10);
            string salt = DateTime.Now.Millisecond.ToString();
            string curtime = CommonFunction.GetTimeStamp();
            SHA256 sha = SHA256.Create();
            string sign = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(appId + input + salt + curtime + appSecret))).Replace("-", "").ToLower();
            sha.Dispose();

            Dictionary<String, String> dic = new Dictionary<String, String>
            {
                { "from", LangConversion(srcLang) },
                { "to", LangConversion(desLang) },
                { "signType", "v3" },
                { "curtime", curtime },
                { "appKey", appId },
                { "salt", salt },
                { "sign", sign },
                { "q", HttpUtility.UrlEncode(q) }
            };

            string payload = BuildPayload(dic);

            StringContent request = new StringContent(payload, null, "application/x-www-form-urlencoded");

            try
            {
                HttpResponseMessage response = await CommonFunction.GetHttpClient().PostAsync(TRANSLATE_API_URL, request);
                if (response.IsSuccessStatusCode)
                {
                    string resultStr = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<YoudaoZhiyunResult>(resultStr);
                    if (result.errorCode == "0")
                    {
                        return string.Join("\n", result.translation);
                    }
                    else
                    {
                        errorInfo = "API error code: " + result.errorCode;
                        return null;
                    }
                }
                else
                {
                    errorInfo = "API response code: " + response.StatusCode;
                    return null;
                }
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
        }

        public void TranslatorInit(string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }


        public string GetLastError()
        {
            return errorInfo;
        }

        /// <summary>
        /// 有道智云API申请地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_allpyAPI()
        {
            return "https://ai.youdao.com/product-fanyi-text.s";
        }

        /// <summary>
        /// 有道智云API额度查询地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_bill()
        {
            return "https://ai.youdao.com/console";
        }

        /// <summary>
        /// 有道智云API文档地址（错误代码）
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_Doc()
        {
            return "https://ai.youdao.com/DOCSIRMA/html/自然语言翻译/API文档/文本翻译服务/文本翻译服务-API文档.html";
        }

        private string BuildPayload(Dictionary<string, string> dic)
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }

            return builder.ToString();
        }

        private string LangConversion(string lang)
        {
            if (lang == "zh")
                return "zh-CHS";
            else if (lang == "jp")
                return "ja";
            else if (lang == "kr")
                return "ko";
            else
                return lang;
        }
    }

#pragma warning disable 0649
    struct YoudaoZhiyunResult
    {
        public string errorCode, query, l;
        public string[] translation;
    }
}
