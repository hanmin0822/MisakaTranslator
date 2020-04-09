using KeyboardMouseHookLibrary;
using OCRLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TextHookLibrary;
using TextRepairLibrary;

namespace MisakaTranslator_WPF
{
    public class Common
    {
        public static IAppSettings appSettings;//应用设置
        public static IRepeatRepairSettings repairSettings;//去重方法参数

        public static TextHookHandle textHooker;//全局使用中的Hook对象
        public static string UsingRepairFunc;//全局使用中的去重方法

        public static string UsingSrcLang;//全局使用中的源语言
        public static string UsingDstLang;//全局使用中的目标翻译语言

        public static IOptChaRec ocr;//全局使用中的OCR对象
        public static HotKeyInfo UsingHotKey;//全局使用中的触发键信息
        public static int UsingOCRDelay;//全局使用中的OCR延时

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

    }
}
