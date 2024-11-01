using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace TranslatorLibrary
{
    public static class CommonFunction
    {
        public static Dictionary<string, string> lstLanguage = new Dictionary<string, string>() {
            { "中文" , "zh" },
            { "English" , "en" },
            { "日本語" ,  "jp" },
            { "한국어" , "kr" },
            { "Русскийязык" , "ru" },
            { "Français" , "fr" }
        };

        public static Dictionary<string,string> lstTranslator = new Dictionary<string, string>() {
            { "无翻译" , "NoTranslator"},
            { "百度翻译" , "BaiduTranslator" },
            // { "腾讯翻译君" , "TencentFYJTranslator" },
            { "腾讯云翻译" , "TencentOldTranslator" },
            { "彩云小译" , "CaiyunTranslator" },
            { "小牛翻译" , "XiaoniuTranslator"},
            { "IBM翻译" , "IBMTranslator"},
            { "Yandex翻译" , "YandexTranslator"},
            { "有道智云", "YoudaoZhiyun"},
            { "有道翻译(公共接口)" , "YoudaoTranslator" },
            // { "ALAPI免费接口" , "AlapiTranslator"},
            { "谷歌翻译(公共接口)" , "GoogleCNTranslator"},
            { "JBeijing" , "JBeijingTranslator" },
            { "金山快译" , "KingsoftFastAITTranslator" },
            { "译典通", "Dreye"},
            { "DeepL", "DeepLTranslator"},
            {"ChatGPT","ChatGPTTranslator" },
            { "Azure OpenAI", nameof(AzureOpenAITranslator) },
            { "本地人工翻译(见说明)" , "ArtificialTranslator"}
        };

        /// <summary>
        /// 计算MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            md5.Dispose();
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }

        /// <summary>
        /// 计算时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }

        /// <summary>
        /// 获取所有可用的翻译API列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetTranslatorList() {
            return lstTranslator.Keys.ToList();
        }

        /// <summary>
        /// 返回翻译API的值（用于存储的值）的索引
        /// </summary>
        /// <param name="TranslatorValue"></param>
        /// <returns></returns>
        public static int GetTranslatorIndex(string TranslatorValue) {
            for (int i = 0;i < lstTranslator.Count;i++) {
                var kvp = lstTranslator.ElementAt(i);
                if (kvp.Value == TranslatorValue) {
                    return i;
                }
            }
            return -1;
        }

        static HttpClient HC;
        /// <summary>
        /// 获得HttpClinet单例，第一次调用自动初始化
        /// </summary>
        public static HttpClient GetHttpClient()
        {
            if (HC == null)
                lock (typeof(CommonFunction))
                    if (HC == null)
                    {
                        HC = new HttpClient() { Timeout = TimeSpan.FromSeconds(8) };
                        HC.DefaultRequestHeaders.UserAgent.ParseAdd("MisakaTranslator");
                        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; // For FX4.7
                    }
            return HC;
        }
        public static void SetHttpProxiedClient(string addr)
        {
            if (HC == null)
            {
                var px = new WebProxy() { Address = new Uri(addr), UseDefaultCredentials = true };
                var ph = new HttpClientHandler() { Proxy = px };
                HC = new HttpClient(ph) { Timeout = TimeSpan.FromSeconds(8) };
                HC.DefaultRequestHeaders.UserAgent.ParseAdd("MisakaTranslator");
            }
        }

        public static Random RD = new Random();

        public static System.Text.Json.JsonSerializerOptions JsonOP = new()
        {
            IncludeFields = true
        };
    }
}
