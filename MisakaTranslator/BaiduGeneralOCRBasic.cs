/*
 *Namespace         MisakaTranslator
 *Class             BaiduGeneralOCRBasic
 *Description       百度通用OCR API  其余几个类均为用于读取json结果时处理的
 */


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace MisakaTranslator
{
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

    class BaiduOCRresOutInfo {
        public long log_id { get; set; }
        public List<BaiduOCRresDataOutInfo> words_result { get; set; }
        public int words_result_num { get; set; }
        
    }

    class BaiduOCRresDataOutInfo {
        public string words { get; set; }
    }

    class BaiduOCRErrorInfo
    {
        public short error_code
        {
            get;
            set;
        }
        public string error_msg
        {
            get;
            set;
        }
    }

    class BaiduGeneralOCRBasic
    {
        public static string APIKey;
        public static string secretKey;
        public static string accessToken;

        public static bool BaiduGeneralOCRBasic_Init() {
            APIKey = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "BaiduOCR", "APIKEY", "");
            secretKey = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "BaiduOCR", "SecretKey", "");
            string ret = BaiduGetToken(APIKey, secretKey);
            BaiduTokenOutInfo btoi = JsonConvert.DeserializeObject<BaiduTokenOutInfo>(ret);
            if (btoi.access_token!=null) {
                accessToken = btoi.access_token;
                return true;
            }
            return false;
        }

        public static string BaiduGetToken(string clientId,string clientSecret) {
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

        // 通用文字识别
        public static string BaiduGeneralBasicOCR(Image img,string langCode)
        {
            string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token=" + accessToken;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            // 图片的base64编码
            string base64 = getFileBase64(img);
            String str = "language_type=" + langCode + "&image=" + HttpUtility.UrlEncode(base64);
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            return result;
        }

        public static String getFileBase64(Image img)
        {
            string fileName = Environment.CurrentDirectory + "\\OCRRes.png";
            try
            {
                img.Save(fileName, ImageFormat.Png);
            }
            catch (System.NullReferenceException ex)
            {
                
            }
            FileStream filestream = new FileStream(fileName, FileMode.Open);
            byte[] arr = new byte[filestream.Length];
            filestream.Read(arr, 0, (int)filestream.Length);
            string baser64 = Convert.ToBase64String(arr);
            filestream.Close();
            return baser64;
        }
    }
}
