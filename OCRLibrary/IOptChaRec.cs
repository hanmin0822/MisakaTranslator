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
        /// <param name="param1">参数一 一般是appID</param>
        /// <param name="param2">参数二 一般是密钥</param>
        bool OCR_Init(string param1, string param2);

        /// <summary>
        /// OCR设置语言代码，在初始化前必须要设置语言代码
        /// </summary>
        /// <param name="langCode">语言代码</param>
        void OCR_SetLangCode(string Code);

        /// <summary>
        /// 返回最后一次错误的ID或原因
        /// </summary>
        string GetLastError();
    }
}
