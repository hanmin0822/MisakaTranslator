﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

/*
 * DeepL translator integration
 * Author: kjstart
 * API version: v2
 */
namespace TranslatorLibrary
{
    public class DeepLTranslator : ITranslator

    {
        public static readonly string SIGN_UP_URL = "https://www.deepl.com/pro?cta=menu-login-signup";
        public static readonly string BILL_URL = "https://www.deepl.com/pro-account/usage";
        public static readonly string DOCUMENT_URL = "https://www.deepl.com/docs-api/accessing-the-api/error-handling/";

        private static readonly string TRANSLATE_API_URL = "https://api-free.deepl.com/v2/translate";

        private string secretKey; //DeepL翻译API的秘钥
        private string errorInfo; //错误信息

        public string GetLastError()
        {
            return errorInfo;
        }

        public string Translate(string sourceText, string desLang, string srcLang)
        {
            if (sourceText == "" || desLang == "" || srcLang == "")
            {
                errorInfo = "Param Missing";
                return null;
            }

            string payload = "text=" + sourceText
                + "&auth_key=" + secretKey
                + "&source_lang=" + transformSrcLangKey(srcLang)
                + "&target_lang=" + transformDesLangKey(desLang);

            StringContent request = new StringContent(payload, null, "application/x-www-form-urlencoded");

            try
            {
                HttpResponseMessage response = CommonFunction.GetHttpClient().PostAsync(TRANSLATE_API_URL, request).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string resultStr = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    DeepLTranslateResult translateResult = JsonConvert.DeserializeObject<DeepLTranslateResult>(resultStr);
                    if (translateResult != null && translateResult.translations != null && translateResult.translations.Count > 0)
                    {
                        return translateResult.translations[0].text;
                    }else
                    {
                        errorInfo = "Cannot get translation from: " + resultStr;
                        return null;
                    }
                }
                else
                {
                    errorInfo = "API return code: " + response.StatusCode;
                    return null;
                }
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

        public void TranslatorInit(string param1, string param2)
        {
            secretKey = param1;
        }

        private string transformSrcLangKey(String langKey)
        {
            switch (langKey)
            {
                case "zh":
                    return "ZH";
                case "en":
                    return "EN";
                case "jp":
                    return "JA";
                case "kr":
                    errorInfo = "Korean is not supported by DeepL";
                    return null;
                case "ru":
                    return "RU";
                case "fr":
                    return "FR";

            }
            errorInfo = "Unknown language tag: " + langKey;
            return null;
        }

        private string transformDesLangKey(String langKey)
        {
            switch (langKey)
            {
                case "zh":
                    return "ZH";
                case "en":
                    return "EN-US";
                case "jp":
                    return "JA";
                case "kr":
                    errorInfo = "Korean is not supported by DeepL";
                    return null;
                case "ru":
                    return "RU";
                case "fr":
                    return "FR";

            }
            errorInfo = "Unknown language tag: " + langKey;
            return null;
        }
    }

    class DeepLTranslateResult
    {
        public List<DeepLTranslations> translations { get; set; }
    }

    class DeepLTranslations
    {
        public string detected_source_language { get; set; }
        public string text { get; set; }
    }
}
