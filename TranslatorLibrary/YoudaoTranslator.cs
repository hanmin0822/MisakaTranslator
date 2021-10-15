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
    public class YoudaoTranslator : ITranslator
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

            if (desLang == "zh")
                desLang = "zh_cn";
            if (srcLang == "zh")
                srcLang = "zh_cn";

            if (desLang == "jp")
                desLang = "ja";
            if (srcLang == "jp")
                srcLang = "ja";

            // 原文
            string q = sourceText;
            string retString;

            string trans_type = srcLang + "2" + desLang;
            trans_type = trans_type.ToUpper();
            string url = "https://fanyi.youdao.com/translate?&doctype=json&type=" + trans_type + "&i=" + q;

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

            YoudaoTransResult oinfo;
            try
            {
                oinfo = JsonConvert.DeserializeObject<YoudaoTransResult>(retString);
            }
            catch (JsonException)
            {
                errorInfo = "Deserialize failed. Possible due to quota limits.";
                return null;
            }

            if (oinfo.errorCode == 0)
            {
                //得到翻译结果
                if (oinfo.translateResult.Count == 1)
                {
                    return string.Join("", oinfo.translateResult[0].Select(x => x.tgt));
                }
                else
                {
                    errorInfo = "UnknownError";
                    return null;
                }
            }
            else
            {
                errorInfo = "ErrorID:" + oinfo.errorCode;
                return null;
            }
        }

        public void TranslatorInit(string param1 = "", string param2 = "")
        {
            //不用初始化
        }
    }

    class YoudaoTransResult
    {
        public string type { get; set; }
        public int errorCode { get; set; }
        public int elapsedTime { get; set; }
        public List<List<YoudaoTransData>> translateResult { get; set; }
    }

    class YoudaoTransData {
        public string src { get; set; }
        public string tgt { get; set; }
    }
}
