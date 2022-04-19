using System.Text.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            string q = HttpUtility.UrlEncode(sourceText);
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
                oinfo = JsonSerializer.Deserialize<YoudaoTransResult>(retString, CommonFunction.JsonOP);
            }
            catch (JsonException)
            {
                errorInfo = "Deserialize failed. Possibly due to quota limits.";
                return null;
            }

            if (oinfo.errorCode == 0)
            {
                var sb = new StringBuilder(32);
                foreach (var youdaoTransDataList in oinfo.translateResult)
                {
                    foreach (var youdaoTransDataItem in youdaoTransDataList)
                    {
                        sb.Append(youdaoTransDataItem.tgt);
                    }
                }
                return sb.ToString();
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

#pragma warning disable 0649
    struct YoudaoTransResult
    {
        public string type;
        public int errorCode;
        public int elapsedTime;
        public YoudaoTransData[][] translateResult;
    }

    struct YoudaoTransData {
        public string src;
        public string tgt;
    }
}
