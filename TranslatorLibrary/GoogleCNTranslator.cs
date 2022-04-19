using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TranslatorLibrary
{
    public class GoogleCNTranslator : ITranslator
    {
        private string errorInfo;//错误信息

        public string GetTkkJS;

        public string GetLastError()
        {
            return errorInfo;
        }

        public async Task<string> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            if (desLang == "zh")
                desLang = "zh-cn";
            if (srcLang == "zh")
                srcLang = "zh-cn";

            if (desLang == "jp")
                desLang = "ja";
            if (srcLang == "jp")
                srcLang = "ja";

            if (desLang == "kr")
                desLang = "ko";
            if (srcLang == "kr")
                srcLang = "ko";

            string fun = string.Format(@"TL('{0}')", sourceText);

            var tk = ExecuteScript(fun, GetTkkJS);

            string googleTransUrl = "https://translate.google.cn/translate_a/single?client=gtx&dt=t&sl=" + srcLang + "&tl=" + desLang + "&tk=" + tk + "&q=" + HttpUtility.UrlEncode(sourceText);

            var hc = CommonFunction.GetHttpClient();

            try
            {
                var ResultHtml = await hc.GetStringAsync(googleTransUrl);

                dynamic TempResult = System.Text.Json.JsonSerializer.Deserialize<dynamic>(ResultHtml, CommonFunction.JsonOP);

                string ResultText = "";

                if (TempResult != null)
                {

                    for (int i = 0; i < TempResult[0].Count; i++)
                    {
                        if (TempResult[0][i] != null)
                        {
                            ResultText += TempResult[0][i][0];
                        }
                    }
                }

                return ResultText;
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

        public void TranslatorInit(string param1 = "", string param2 = "")
        {
            GetTkkJS = File.ReadAllText($"{Environment.CurrentDirectory}\\lib\\GoogleJS.js");
        }

        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="sExpression">参数体</param>
        /// <param name="sCode">JavaScript代码的字符串</param>
        /// <returns></returns>
        private string ExecuteScript(string sExpression, string sCode)
        {
            MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            scriptControl.UseSafeSubset = true;
            scriptControl.Language = "JScript";
            scriptControl.AddCode(sCode);
            try
            {
                string str = scriptControl.Eval(sExpression).ToString();
                return str;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return null;
        }

    }
}
