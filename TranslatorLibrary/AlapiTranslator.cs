using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorLibrary
{

    public class AlapiTranslator : ITranslator
    {
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

            
            string url = "https://v1.alapi.cn/api/fanyi?q=" + q + "&from=" + srcLang + "&to=" + desLang;

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

            AliapiTransResult oinfo = JsonConvert.DeserializeObject<AliapiTransResult>(retString);

            if (oinfo.msg == "success")
            {
                //得到翻译结果
                if (oinfo.data.trans_result.Count == 1)
                {
                    return string.Join("", oinfo.data.trans_result.Select(x => x.dst));
                }
                else
                {
                    errorInfo = "UnknownError";
                    return null;
                }
            }
            else
            {
                errorInfo = "Error:" + oinfo.msg;
                return null;
            }
        }

        public void TranslatorInit(string param1 = "", string param2 = "")
        {
            //不用初始化
        }

        class AliapiTransResult
        {
            public int code { get; set; }
            public string msg { get; set; }
            public AliapiTransData data { get; set; }
        }

        class AliapiTransData
        {
            public string from { get; set; }
            public string to { get; set; }
            public List<AliapiTransResData> trans_result { get; set; }
        }

        class AliapiTransResData
        {
            public string src { get; set; }
            public string dst { get; set; }
        }
    }
}
