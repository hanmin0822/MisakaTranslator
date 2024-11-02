#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
// ReSharper disable ClassNeverInstantiated.Global

namespace TranslatorLibrary
{
    // ReSharper disable once InconsistentNaming
    public class AzureOpenAITranslator : ITranslator
    {
        private const string ROLE_SYSTEM = "system";
        private const string ROLE_USER = "user";
        private const string ROLE_ASSISTANT = "assistant";

        private static readonly string systemPromptTemplate = "You are a professional translator, translating a novel from {0} to {1}. Output the translated text directly.";

        private string? key = string.Empty;
        private string? url = string.Empty;
        private string errorInfo = string.Empty;

        public void TranslatorInit(string param1, string param2)
        {
            key = param1;
            url = param2;
        }

        public async Task<string?> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            if (string.IsNullOrEmpty(sourceText) || string.IsNullOrEmpty(desLang) || string.IsNullOrEmpty(srcLang))
            {
                errorInfo = "Param Missing";
                return null;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                errorInfo = "Invalid Api Url";
                return null;
            }

            var httpClient = CommonFunction.GetHttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            var completionRequest = CreateCompletionRequest(sourceText, desLang, srcLang);

            request.Headers.Add("api-key", key);
            request.Content = new StringContent(JsonSerializer.Serialize(completionRequest), Encoding.UTF8, "application/json");
            ChatResponse? chat;
            try
            {
                var response = await httpClient.SendAsync(request);
                var responseStream = await response.Content.ReadAsStreamAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var error = await JsonSerializer.DeserializeAsync<ChatErrorResponse>(responseStream);
                    errorInfo = error == null ? $"http status code is {(int)response.StatusCode}({response.StatusCode})" : error.Error.Message;
                    return null;
                }

                chat = await JsonSerializer.DeserializeAsync<ChatResponse>(responseStream);
            }
            catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException or JsonException)
            {
                errorInfo = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                throw;
            }
            
            if (chat is not { Choices.Count: > 0 })
            {
                errorInfo = "No completion choices";
                return null;
            }

            if (chat.Choices[0].Message.Content == null)
            {
                var filtered = string.Join(",", chat.Choices[0].ContentFilterResults.EnumerateFiltered().Select(x => x.name));
                errorInfo = $"Result is filtered: {filtered}";
                return null;
            }

            return chat.Choices[0].Message.Content;
        }

        public string GetLastError()
        {
            return errorInfo;
        }

        private static ChatRequest CreateCompletionRequest(string sourceText, string desLang, string srcLang)
        {
            var messages = new List<MessageRequest>
            {
                new()
                {
                    Role = ROLE_SYSTEM,
                    Content = new List<MessageContent>()
                    {
                        new()
                        {
                            Type = "text",
                            Text = string.Format(systemPromptTemplate, srcLang, desLang)
                        }
                    }
                },
                new()
                {
                    Role = ROLE_USER,
                    Content = new List<MessageContent>
                    {
                        new()
                        {
                            Type = "text",
                            Text = sourceText
                        }
                    }
                }
            };
            return new ChatRequest
            {
                Messages = messages,
                Stream = false
            };
        }

        #region DataModel

        public class ChatRequest
        {
            [JsonPropertyName("messages")] public List<MessageRequest> Messages { get; set; } = new();
            [JsonPropertyName("stream")] public bool Stream { get; set; } = false;
        }

        public class ChatResponse
        {
            [JsonPropertyName("choices")] public List<ChatChoice> Choices { get; set; } = new();
        }

        public class ChatErrorResponse
        {
            [JsonPropertyName("error")] public ChatError Error { get; set; } = new();
        }

        public class ChatError
        {
            [JsonPropertyName("code")] public string Code { get; set; } = string.Empty;
            [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
        }

        public class ChatChoice
        {
            [JsonPropertyName("message")] public MessageResponse Message { get; set; } = new();
            [JsonPropertyName("finish_reason")] public string FinishReason { get; set; } = "stop";
            [JsonPropertyName("index")] public int Index { get; set; }
            [JsonPropertyName("content_filter_results")] public ContentFilterResults ContentFilterResults { get; set; } = new();
        }

        public class ContentFilterResults
        {
            [JsonPropertyName("hate")] public FilterResult Hate { get; set; } = new();
            [JsonPropertyName("protected_material_code")] public FilterResult ProtectedMaterialCode { get; set; } = new();
            [JsonPropertyName("self_harm")] public FilterResult SelfHarm { get; set; } = new();
            [JsonPropertyName("sexual")] public FilterResult Sexual { get; set; } = new();
            [JsonPropertyName("violence")] public FilterResult Violence { get; set; } = new();
            
            public IEnumerable<(FilterResult result, string name)> EnumerateFiltered()
            {
                if (Hate.Filtered)
                    yield return (Hate, nameof(Hate));
                if (ProtectedMaterialCode.Filtered)
                    yield return (ProtectedMaterialCode, nameof(ProtectedMaterialCode));
                if (SelfHarm.Filtered)
                    yield return (SelfHarm, nameof(SelfHarm));
                if (Sexual.Filtered)
                    yield return (Sexual, nameof(Sexual));
                if (Violence.Filtered)
                    yield return (Violence, nameof(Violence));
            }
        }

        public struct FilterResult
        {
            public FilterResult()
            {
            }

            [JsonPropertyName("filtered")] public bool Filtered { get; set; } = false;
            [JsonPropertyName("severity")] public string Severity { get; set; } = "safe";
            [JsonPropertyName("detected")] public bool Detected { get; set; } = false;
        }

        public class MessageRequest
        {
            [JsonPropertyName("role")] public string Role { get; set; } = ROLE_SYSTEM;
            [JsonPropertyName("content")] public List<MessageContent> Content { get; set; } = new();
        }

        public class MessageResponse
        {
            [JsonPropertyName("role")] public string Role { get; set; } = ROLE_ASSISTANT;
            [JsonPropertyName("content")] public string? Content { get; set; } = null;  // idk why it will return null
        }

        public class MessageContent
        {
            [JsonPropertyName("type")] public string Type { get; set; } = "text";
            [JsonPropertyName("text")] public string Text { get; set; } = string.Empty;
        }
        
        public class CompletionUsage
        {
            [JsonPropertyName("completion_tokens")] public int CompletionTokens { get; set; }
            [JsonPropertyName("prompt_tokens")] public int PromptTokens { get; set; }
            [JsonPropertyName("total_tokens")] public int TotalTokens { get; set; }
        }
        #endregion
    }
}
