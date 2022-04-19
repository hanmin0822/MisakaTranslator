using System.Text.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Web;

namespace TranslatorLibrary
{
    // 点申请试用会跳转到腾讯云机器翻译，api.ai.qq.com的接口返回502 Bad Gateway
    // 目前在CommonFunction.cs和SettingsWindow.xaml中从界面上隐藏本项
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

            string salt = CommonFunction.RD.Next(100000).ToString();

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

            TencentTransOutInfo oinfo = JsonSerializer.Deserialize<TencentTransOutInfo>(retString, CommonFunction.JsonOP);

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

#pragma warning disable 0649
    struct TencentTransOutInfo
    {
        public string ret;
        public string msg;
        public TencentTransResult data;
    }

    struct TencentTransResult
    {
        public string source_text;
        public string target_text;
    }
}
