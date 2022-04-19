using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;

namespace TranslatorLibrary
{
    public class IBMTranslator : ITranslator
    {
        public string ApiKey;
        public string URL;

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
            if(desLang != "en" && srcLang != "en")
            {
                sourceText = await TranslateAsync(sourceText, "en", srcLang);
                if (sourceText == null)
                    return null;
                srcLang = "en";
            }

            HttpResponseMessage resp;
            var hc = CommonFunction.GetHttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, URL);
            string jsonParam = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                {"text", new string[] {sourceText}},
                {"model_id", srcLang + "-" + desLang}
            });
            req.Content = new StringContent(jsonParam, null,"application/json");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(ApiKey)));

            try
            {
                resp = await hc.SendAsync(req);
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

            string retString = await resp.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Result>(retString, CommonFunction.JsonOP);

            if (!resp.IsSuccessStatusCode){
                errorInfo = result.error;
                return null;
            }

            return result.translations[0].translation;
        }

        public void TranslatorInit(string param1, string param2)
        {
            ApiKey = "apikey:" + param1;
            URL = param2 + "/v3/translate?version=2018-05-01";
        }

        /// <summary>
        /// IBM翻译API申请地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_allpyAPI()
        {
            return "https://cloud.ibm.com/catalog/services/language-translator";
        }

        /// <summary>
        /// IBM翻译API额度查询地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_bill()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// IBM翻译API文档地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_Doc()
        {
            return "https://cloud.ibm.com/apidocs/language-translator#translate";
        }

#pragma warning disable 0649
        struct Result
        {
            public Translations[] translations;
            public string error;
        }

        struct Translations
        {
            public string translation;
        }
    }
}
