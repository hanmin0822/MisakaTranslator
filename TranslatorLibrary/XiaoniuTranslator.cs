using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorLibrary
{
    public class XiaoniuTranslator : ITranslator
    {
        public string apiKey;//小牛翻译API 的APIKEY
        private string errorInfo;//错误信息

        public string GetLastError()
        {
            return errorInfo;
        }

        public string Translate(string sourceText, string desLang, string srcLang)
        {
            if (desLang == "kr")
                desLang = "ko";
            if (srcLang == "kr")
                srcLang = "ko";
            if (desLang == "jp")
                desLang = "ja";
            if (srcLang == "jp")
                srcLang = "ja";

            // 原文
            string q = sourceText;

            string retString;

            var sb = new StringBuilder("https://api.niutrans.com/NiuTransServer/translation?")
                .Append("&from=").Append(srcLang)
                .Append("&to=").Append(desLang)
                .Append("&apikey=").Append(apiKey)
                .Append("&src_text=").Append(Uri.EscapeDataString(q));

            string url = sb.ToString();

            var hc = CommonFunction.GetHttpClient();
            try
            {
                retString = hc.GetStringAsync(url).GetAwaiter().GetResult();
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

            XiaoniuTransOutInfo oinfo = JsonConvert.DeserializeObject<XiaoniuTransOutInfo>(retString);

            if (oinfo.error_code == null || oinfo.error_code == "52000")
            {
                //得到翻译结果
                if (oinfo.tgt_text != null)
                {
                    return oinfo.tgt_text;
                }
                else
                {
                    errorInfo = "UnknownError";
                    return null;
                }
            }
            else
            {
                if (oinfo.error_msg != null)
                {
                    errorInfo = "ErrorID:" + oinfo.error_msg;
                    return null;
                }
                else
                {
                    errorInfo = "UnknownError";
                    return null;
                }
            }

        }

        public void TranslatorInit(string param1, string param2 = "")
        {
            //第二参数无用
            apiKey = param1;
        }

        /// <summary>
        /// 小牛翻译API申请地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_allpyAPI()
        {
            return "https://niutrans.com/API";
        }

        /// <summary>
        /// 小牛翻译API额度查询地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_bill()
        {
            return "https://niutrans.com/cloud/console/statistics/free";
        }

        /// <summary>
        /// 小牛翻译API文档地址（错误代码）
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_Doc()
        {
            return "https://niutrans.com/documents/develop/develop_text/free#error";
        }


    }

    class XiaoniuTransOutInfo
    {
        public string from { get; set; }
        public string to { get; set; }
        public string src_text { get; set; }
        public string tgt_text { get; set; }
        public string error_code { get; set; }
        public string error_msg { get; set; }
    }
}
