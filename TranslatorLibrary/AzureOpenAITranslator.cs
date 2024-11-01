#nullable enable
using System;
using System.Collections.Generic;
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
            HttpResponseMessage response;
            try
            {
                response = await httpClient.SendAsync(request);
                var responseStream = await response.Content.ReadAsStreamAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var error = await JsonSerializer.DeserializeAsync<CompletionErrorResponse>(responseStream);
                    errorInfo = error == null ? $"http status code is {(int)response.StatusCode}({response.StatusCode})" : error.Error.Message;
                    return null;
                }

                var completion = await JsonSerializer.DeserializeAsync<CompletionResponse>(responseStream);
                if (completion is not { Choices.Count: > 0 })
                {
                    errorInfo = "No completion choices";
                    return null;
                }

                return completion.Choices[0].Message.Content;
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
        }

        public string GetLastError()
        {
            return errorInfo;
        }

        private static CompletionRequest CreateCompletionRequest(string sourceText, string desLang, string srcLang)
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
            return new CompletionRequest
            {
                Messages = messages,
                Stream = false
            };
        }

        #region DataModel

        public class CompletionRequest
        {
            [JsonPropertyName("messages")] public List<MessageRequest> Messages { get; set; } = new();
            [JsonPropertyName("stream")] public bool Stream { get; set; } = false;
            // [JsonPropertyName("temperature")] public double Temperature { get; set; } = 0.7;
            // [JsonPropertyName("top_p")] public double TopP { get; set; } = 0.95;
            // [JsonPropertyName("max_tokens")] public int MaxTokens { get; set; } = 800;
        }

        public class CompletionResponse
        {
            [JsonPropertyName("choices")] public List<ChatChoice> Choices { get; set; } = new();
            // [JsonPropertyName("created")] public ulong Created { get; set; }
            // [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
            // [JsonPropertyName("model")] public string Model { get; set; } = string.Empty;
            // [JsonPropertyName("object")] public string Object { get; set; } = string.Empty;
            // [JsonPropertyName("system_fingerprint")] public string SystemFingerprint { get; set; } = string.Empty;
            // [JsonPropertyName("usage")] public CompletionUsage Usage { get; set; } = new();
        }

        public class CompletionErrorResponse
        {
            [JsonPropertyName("error")] public CompletionError Error { get; set; } = new();
        }

        public class CompletionError
        {
            [JsonPropertyName("code")] public string Code { get; set; } = string.Empty;
            [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
        }

        public class ChatChoice
        {
            [JsonPropertyName("message")] public MessageResponse Message { get; set; } = new();
            [JsonPropertyName("finish_reason")] public string FinishReason { get; set; } = "stop";
            [JsonPropertyName("index")] public int Index { get; set; }
        }

        public class MessageRequest
        {
            [JsonPropertyName("role")] public string Role { get; set; } = ROLE_SYSTEM;
            [JsonPropertyName("content")] public List<MessageContent> Content { get; set; } = new();
        }

        public class MessageResponse
        {
            [JsonPropertyName("role")] public string Role { get; set; } = ROLE_ASSISTANT;
            [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
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