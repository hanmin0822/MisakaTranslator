using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;

namespace TranslatorLibrary
{
    public class CaiyunTranslator : ITranslator
    {
        public string caiyunToken;//彩云小译 令牌
        private string errorInfo;//错误信息

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

            if (desLang == "jp")
                desLang = "ja";
            if (srcLang == "jp")
                srcLang = "ja";

            // 原文
            string q = sourceText;
            string retString;

            string trans_type = srcLang + "2" + desLang;

            string url = "https://api.interpreter.caiyunai.com/v1/translator";
            //json参数
            string jsonParam = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                {"source", new string[] {q}},
                {"trans_type", trans_type},
                {"request_id", "demo"},
                {"detect", true}
            });

            var hc = CommonFunction.GetHttpClient();
            var req = new StringContent(jsonParam, null, "application/json");
            req.Headers.Add("X-Authorization", "token " + caiyunToken);
            try
            {
                retString = await (await hc.PostAsync(url, req)).Content.ReadAsStringAsync();
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
            finally
            {
                req.Dispose();
            }

            CaiyunTransResult oinfo;
            try
            {
                oinfo = JsonSerializer.Deserialize<CaiyunTransResult>(retString, CommonFunction.JsonOP);
            }
            catch {
                errorInfo = "JsonConvert Error";
                return null;
            }

            if (oinfo.target?.Length >= 1)
            {
                //得到翻译结果
                return string.Join("", oinfo.target.Select(x => Regex.Unescape(x)));
            }
            else
            {
                errorInfo = "ErrorInfo:" + oinfo.message;
                return null;
            }
            
        }

        public void TranslatorInit(string param1, string param2 = "")
        {
            caiyunToken = param1;
        }


        /// <summary>
        /// 彩云小译API申请地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_allpyAPI()
        {
            return "https://dashboard.caiyunapp.com/user/sign_in/";
        }

        /// <summary>
        /// 彩云小译API额度查询地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_bill()
        {
            return "https://dashboard.caiyunapp.com/";
        }

        /// <summary>
        /// 彩云小译API文档地址（错误代码）
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_Doc()
        {
            return "https://fanyi.caiyunapp.com/#/api";
        }
    }

#pragma warning disable 0649
    struct CaiyunTransResult
    {
        public string message;
        public double confidence;
        public int rc;
        public string[] target;
    }


}
