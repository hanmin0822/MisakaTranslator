using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace OCRLibrary
{
    public class BaiduGeneralOCR : IOptChaRec
    {
        public string APIKey;
        public string secretKey;
        private string accessToken;
        private string langCode;
        private string errorInfo;

        private IntPtr WinHandle;
        private Rectangle OCRArea;
        private bool isAllWin;


        public string GetLastError()
        {
            return errorInfo;
        }

        public string OCRProcess(Bitmap img)
        {
            if (img == null || langCode == null || langCode == "") {
                errorInfo = "Param Missing";
                return null;
            }

            string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token=" + accessToken;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            // 图片的base64编码
            string base64 = ImageProcFunc.GetFileBase64(img);
            String str = "language_type=" + langCode + "&image=" + Uri.EscapeDataString(base64);
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();

            string ret = "";
            BaiduOCRresOutInfo oinfo = JsonConvert.DeserializeObject<BaiduOCRresOutInfo>(result);
            if (oinfo.words_result != null)
            {
                for (int i = 0; i < oinfo.words_result_num; i++)
                {
                    ret = ret + oinfo.words_result[i].words + "\n";
                }
                return ret;
            }
            else {
                errorInfo = "UnknownError";
                return null;
            }

        }

        public bool OCR_Init(string param1, string param2)
        {
            APIKey = param1;
            secretKey = param2;

            string ret = BaiduGetToken(APIKey, secretKey);
            BaiduTokenOutInfo btoi = JsonConvert.DeserializeObject<BaiduTokenOutInfo>(ret);
            if (btoi.access_token != null)
            {
                accessToken = btoi.access_token;
                return true;
            }
            errorInfo = "ErrorID:" + btoi.error + " ErrorInfo:" + btoi.error_description;
            return false;
        }

        public static string BaiduGetToken(string clientId, string clientSecret)
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            return result;
        }

        /// <summary>
        /// 百度OCRAPI申请地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_allpyAPI()
        {
            return "https://ai.baidu.com/tech/ocr/general";
        }

        /// <summary>
        /// 百度OCRAPI额度查询地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_bill()
        {
            return "https://console.bce.baidu.com/ai/?fromai=1#/ai/ocr/overview/index";
        }

        /// <summary>
        /// 百度OCRAPI文档地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_Doc()
        {
            return "https://ai.baidu.com/ai-doc/OCR/zk3h7xz52";
        }

        public string OCRProcess()
        {
            if (OCRArea != null)
            {
                Image img = ScreenCapture.GetWindowRectCapture(WinHandle, OCRArea, isAllWin);
                return OCRProcess(new Bitmap(img));
            }
            else {
                errorInfo = "未设置截图区域";
                return null;
            }
        }

        public void SetOCRArea(IntPtr handle, Rectangle rec, bool AllWin)
        {
            WinHandle = handle;
            OCRArea = rec;
            isAllWin = AllWin;
        }

        public Image GetOCRAreaCap()
        {
            return ScreenCapture.GetWindowRectCapture(WinHandle, OCRArea, isAllWin);
        }

        public void SetOCRSourceLang(string lang)
        {
            if (lang == "jpn")
            {
                lang = "jap";
            }

            langCode = lang.ToUpper();
        }
    }

    class BaiduTokenOutInfo
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }

        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string session_key { get; set; }
        public string session_secret { get; set; }
    }

    class BaiduOCRresOutInfo
    {
        public long log_id { get; set; }
        public List<BaiduOCRresDataOutInfo> words_result { get; set; }
        public int words_result_num { get; set; }

    }

    class BaiduOCRresDataOutInfo
    {
        public string words { get; set; }
    }

    class BaiduOCRErrorInfo
    {
        public short error_code{ get; set; }
        public string error_msg{ get; set; }
    }

}
