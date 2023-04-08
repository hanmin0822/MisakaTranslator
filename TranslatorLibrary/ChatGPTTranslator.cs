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
        private string openai_model = "gpt-3.5-turbo";

        private string apiKey; //ChatGPT翻译API的密钥
        private string apiUrl; //ChatGPT翻译API的URL
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
            hc.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            try
            {
                retString = await (await hc.PostAsync(apiUrl, req)).Content.ReadAsStringAsync();
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
                    errorInfo = "Unknown error";
                    return null;
                }
                return null;
            }
        }

        public void TranslatorInit(string param1, string param2)
        {
            apiKey = param1;
            apiUrl = param2;
        }
    }

#pragma warning disable 0649
    public struct ChatResponse
    {
        public string id;
        public string _object;
        public int created;
        public string model;
        public ChatUsage usage;
        public ChatChoice[] choices;
    }

    public struct ChatUsage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }

    public struct ChatChoice
    {
        public ChatMessage message;
        public string finish_reason;
        public int index;
    }

    public struct ChatMessage
    {
        public string role;
        public string content;
    }

    public struct ChatResErr
    {
        public ChatError error;
    }

    public struct ChatError
    {
        public string message;
        public string type;
        public object param;
        public object code;
    }
}
