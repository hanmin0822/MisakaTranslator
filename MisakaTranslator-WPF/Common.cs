using KeyboardMouseHookLibrary;
using OCRLibrary;
using SQLHelperLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TextHookLibrary;
using TextRepairLibrary;

namespace MisakaTranslator_WPF
{
    public class Common
    {
        public static IAppSettings appSettings;//应用设置
        public static IRepeatRepairSettings repairSettings;//去重方法参数

        public static int transMode;//全局使用中的翻译模式 1=hook 2=ocr

        public static int GameID;//全局使用中的游戏ID(数据库)

        public static TextHookHandle textHooker;//全局使用中的Hook对象
        public static string UsingRepairFunc;//全局使用中的去重方法

        public static string UsingSrcLang;//全局使用中的源语言
        public static string UsingDstLang;//全局使用中的目标翻译语言

        public static IOptChaRec ocr;//全局使用中的OCR对象
        public static bool isAllWindowCap;//是否全屏截图
        public static IntPtr OCRWinHwnd;//全局的OCR的工作窗口
        public static HotKeyInfo UsingHotKey;//全局使用中的触发键信息
        public static int UsingOCRDelay;//全局使用中的OCR延时

        public static Window mainWin;//全局的主窗口对象

        public static GlobalHotKey GlobalOCRHotKey;//全局OCR热键

        /// <summary>
        /// 导出Textractor历史记录，返回是否成功的结果
        /// </summary>
        /// <returns></returns>
        public static bool ExportTextractorHistory()
        {
            try
            {
                if (textHooker != null)
                {
                    FileStream fs = new FileStream("TextractorOutPutHistory.txt", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine("=================以下是Textractor的历史输出记录================");
                    string[] history = textHooker.TextractorOutPutHistory.ToArray();
                    for (int i = 0; i < history.Length; i++)
                    {
                        sw.WriteLine(history[i]);
                    }

                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    return true;
                }
                else {
                    return false;
                }
            }
            catch (System.NullReferenceException) {
                return false;
            }
        }

        /// <summary>
        /// 文本去重方法初始化
        /// </summary>
        public static void RepairFuncInit() {
            TextRepair.SingleWordRepeatTimes = int.Parse(repairSettings.SingleWordRepeatTimes);
            TextRepair.SentenceRepeatFindCharNum = int.Parse(repairSettings.SentenceRepeatFindCharNum);
            TextRepair.regexPattern = repairSettings.Regex;
            TextRepair.regexReplacement = repairSettings.Regex_Replace;
        }

        /// <summary>
        /// 根据进程PID找到程序所在路径
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string FindProcessPath(int pid)
        {
            Process[] ps = Process.GetProcesses();
            string filepath = "";
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Id == pid)
                {
                    try
                    {
                        filepath = ps[i].MainModule.FileName;
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        continue;
                        //这个地方直接跳过，是因为32位程序确实会读到64位的系统进程，而系统进程是不能被访问的
                    }
                    break;
                }
            }
            return filepath;
        }

        /// <summary>
        /// 全局OCR
        /// </summary>
        public static void GlobalOCR() {
            BitmapImage img = ImageProcFunc.ImageToBitmapImage(ScreenCapture.GetAllWindow());
            ScreenCaptureWindow scw = new ScreenCaptureWindow(img,2);
            scw.Width = img.PixelWidth;
            scw.Height = img.PixelHeight;
            scw.Topmost = true;
            scw.Left =0;
            scw.Top = 0;
            scw.Show();
        }
    }
}
