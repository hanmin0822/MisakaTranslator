using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRLibrary
{
    public interface IOptChaRec
    {
        /// <summary>
        /// OCR处理，将图片上的文字提取得到一句话
        /// </summary>
        /// <param name="img">欲处理的图片</param>
        /// <returns>返回识别结果，如果为空可通过GetLastError得到错误提示</returns>
        string OCRProcess(Bitmap img);

        /// <summary>
        /// OCR初始化，对于在线API，参数1和参数2有效
        /// </summary>
        /// <param name="lang">指定目标语言 OCR识别语言 jpn=日语 eng=英语</param>
        /// <param name="param1">参数一 一般是appID</param>
        /// <param name="param2">参数二 一般是密钥</param>
        bool OCR_Init(string lang, string param1, string param2);

        /// <summary>
        /// 返回最后一次错误的ID或原因
        /// </summary>
        string GetLastError();
    }
}
