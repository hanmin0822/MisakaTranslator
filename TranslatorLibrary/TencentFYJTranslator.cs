using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
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

        public async Task<string> TranslateAsync(string sourceText, string desLang, string srcLang)
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

            var sb = new StringBuilder()
                .Append("app_id=").Append(appId)
                .Append("&nonce_str=").Append(salt)
                .Append("&source=").Append(srcLang)
                .Append("&target=").Append(desLang)
                .Append("&text=").Append(HttpUtility.UrlEncode(q).ToUpper())
                .Append("&time_stamp=").Append(CommonFunction.GetTimeStamp());
            sb.Append("&sign=" + CommonFunction.EncryptString(sb.ToString() + "&app_key=" + appKey).ToUpper());
            string req = sb.ToString();

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
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                errorInfo = ex.Message;
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
