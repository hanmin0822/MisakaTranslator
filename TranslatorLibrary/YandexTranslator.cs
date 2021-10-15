using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace TranslatorLibrary
{
    public class YandexTranslator : ITranslator
    {
        public string ApiKey;

        private string errorInfo;

        public string GetLastError()
        {
            return errorInfo;
        }

        public async Task<string> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            if (desLang == "kr")
                desLang = "ko";
            if (srcLang == "kr")
                srcLang = "ko";
            if (desLang == "jp")
                desLang = "ja";
            if (srcLang == "jp")
                srcLang = "ja";

            var hc = CommonFunction.GetHttpClient();
            string apiurl = "https://translate.yandex.net/api/v1.5/tr.json/translate?key=" + ApiKey + "&lang=" + srcLang + "-" + desLang + "&text=";

            try
            {
                string retString = await hc.GetStringAsync(apiurl + sourceText);
                var doc = JsonConvert.DeserializeObject<dynamic>(retString);
                return doc.text[0];
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

        public void TranslatorInit(string param1, string param2="")
        {
            ApiKey = param1;
        }

        /// <summary>
        /// Yandex翻译API申请地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_allpyAPI()
        {
            return "https://translate.yandex.com/developers/keys";
        }

        /// <summary>
        /// Yandex翻译API额度查询地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_bill()
        {
            return "https://translate.yandex.com/developers/stat";
        }

        /// <summary>
        /// Yandex翻译API文档地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_Doc()
        {
            return "https://yandex.com/dev/translate/doc/dg/reference/translate.html";
        }
    }
}
