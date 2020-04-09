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
        /// OCR处理，根据设定的截图区域自动截图后识别
        /// </summary>
        /// <returns>返回识别结果，如果为空可通过GetLastError得到错误提示</returns>
        string OCRProcess();

        /// <summary>
        /// 设定截图区域
        /// </summary>
        /// <param name="handle">(窗口模式)窗口句柄</param>
        /// <param name="rec">截图区域</param>
        /// <param name="isAllWin">是否全屏截取</param>
        void SetOCRArea(IntPtr handle, Rectangle rec, bool AllWin);

        /// <summary>
        /// 得到OCR区域截图
        /// </summary>
        /// <returns></returns>
        Image GetOCRAreaCap();

        /// <summary>
        /// 设置OCR源语言
        /// </summary>
        /// <param name="lang">指定目标语言 OCR识别语言 jpn=日语 eng=英语</param>
        void SetOCRSourceLang(string lang);

        /// <summary>
        /// OCR初始化，对于在线API，参数1和参数2有效
        /// </summary>
        /// <param name="param1">参数一 一般是appID</param>
        /// <param name="param2">参数二 一般是密钥</param>
        bool OCR_Init(string param1, string param2);

        /// <summary>
        /// 返回最后一次错误的ID或原因
        /// </summary>
        string GetLastError();
    }
}
