using System.Text.Json;
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
    // 此公共API无法使用了，官方推出了需注册的V2，看社区来更新吧。目前在CommonFunction中注释掉了从界面上设置本API的途径
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
            string q = HttpUtility.UrlEncode(sourceText);
            string retString;

            var url = new StringBuilder()
                .Append("https://v1.alapi.cn/api/fanyi?")
                .Append("q=").Append(q)
                .Append("&from=").Append(srcLang)
                .Append("&to=").Append(desLang);

            var hc = CommonFunction.GetHttpClient();
            try
            {
                retString = await hc.GetStringAsync(url.ToString());
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

            AliapiTransResult oinfo = JsonSerializer.Deserialize<AliapiTransResult>(retString);

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
