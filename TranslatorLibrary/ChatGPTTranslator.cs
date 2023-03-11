using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Linq;

/*
 * ChatGPT translator integration
 * Author: bychv
 * API version: v1
 */
namespace TranslatorLibrary
{
    public class ChatGPTTranslator : ITranslator

    {
        public static readonly string SIGN_UP_URL = "https://platform.openai.com";
        public static readonly string BILL_URL = "https://platform.openai.com/account/usage";
        public static readonly string DOCUMENT_URL = "https://platform.openai.com/docs/introduction/overview";
        public static readonly string TRANSLATE_API_URL = "https://api.openai.com/v1/chat/completions";
        private string openai_model = "gpt-3.5-turbo";

        private string apiKey; //ChatGPT翻译API的密钥
        private string errorInfo; //错误信息

        public string GetLastError()
        {
            return errorInfo;
        }

        public async Task<string> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            string q = sourceText;

            if (sourceText == "" || desLang == "" || srcLang == "")
            {
                errorInfo = "Param Missing";
                return null;
            }
            string retString;
            string jsonParam = $"{{\"model\": \"{openai_model}\",\"messages\": [{{\"role\": \"system\", \"content\": \"Translate {srcLang} To {desLang}\"}},{{\"role\": \"user\", \"content\": \"{q}\"}}]}}";
            var hc = CommonFunction.GetHttpClient();
            var req = new StringContent(jsonParam, null, "application/json");
            //req.Headers.Add("Authorization", "Bearer " + apiKey);
            hc.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            try
            {
                retString = await (await hc.PostAsync(TRANSLATE_API_URL, req)).Content.ReadAsStringAsync();
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

            ChatResponse oinfo;

            try
            {
                oinfo = JsonSerializer.Deserialize<ChatResponse>(retString, CommonFunction.JsonOP);
            }
            catch
            {
                errorInfo = "JsonConvert Error";
                return null;
            }
            
            try
            {
                return oinfo.choices[0].message.content;
            }
            catch
            {
                try
                {
                    var err = JsonSerializer.Deserialize<ChatResErr>(retString, CommonFunction.JsonOP);
                    errorInfo = err.error.message;
                    return null;
                }
                catch
                {
                    errorInfo = "未知错误";
                    return null;
                }
                return null;
            }
                
            
        }

        public void TranslatorInit(string param1, string param2 = "")
        {
            apiKey = param1;
        }


    }

#pragma warning disable 0649
    public class ChatResponse
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public ChatUsage usage { get; set; }
        public ChatChoice[] choices { get; set; }
    }

    public class ChatUsage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    public class ChatChoice
    {
        public ChatMessage message { get; set; }
        public string finish_reason { get; set; }
        public int index { get; set; }
    }

    public class ChatMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class ChatResErr
    {
        public ChatError error { get; set; }
    }

    public class ChatError
    {
        public string message { get; set; }
        public string type { get; set; }
        public object param { get; set; }
        public object code { get; set; }
    }





}