/*
 *Namespace         MisakaTranslator
 *Class             Common
 *Description       公共类，包含全局需要的一些变量、方法
 */

using Config.Net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Data.SQLite;

namespace MisakaTranslator
{
    struct TextInfo
    {
        public string TIsrcText;
        public string TIfirstTransText;
        public string TIsecondTransText;
    }


    class Common
    {
        public static int TransMode;//全局的翻译模式 1=TextHook 2=OCR

        public static string srcLang;//全局的翻译API用到的源文本语言
        public static string desLang;//全局的翻译API用到的目标文本语言

        public static string OCRsrcLangCode;//全局的OCR的识别源语言代码
        public static bool isAllWindowCap;//是否全屏截图
        public static IntPtr OCRWinHwnd;//全局的OCR的工作窗口
        public static Rectangle OCRrec;//全局的OCR的工作区域
        public static int OCRdelay;//鼠标点击后进行OCR的延时

        public static TextHookHandle TextractorHandle;//全局的TextractorHandle对象

        public static int GameID;//全局的游戏ID（非进程ID）

        public static string HookCode;//全局的特殊码
        public static string HookCodePlus;//全局的特殊码额外部分，见本软件特殊码定义

        public static string RepeatMethod;//全局的去重方法

        public static Queue<TextInfo> HistoryTextInfos;//全局的历史记录队列

        public static Queue<string> TextractorOutPutHistory;//全局的Textractor的输出记录，用于查错

        public static IAppSettings settings;//全局的设置接口
        public static IRepeatRepairSettings repeatRsettings;//全局的去重方法配置接口


        /// <summary>
        /// 得到一个游戏的游戏ID
        /// 如果游戏已经存在于数据库中，则直接返回ID，否则追加新游戏路径并返回新ID
        /// </summary>
        /// <param name="gamepath"></param>
        /// <returns>返回游戏ID</returns>
        public static int GetGameID(string gamepath)
        {

            if (File.Exists(Environment.CurrentDirectory + "\\settings\\GameList.sqlite") == false)
            {
                CreateNewGameList();
            }

            SQLiteHelper sqliteH = new SQLiteHelper(Environment.CurrentDirectory + "\\settings\\GameList.sqlite");
            
            List<string> ls = sqliteH.ExecuteReader_OneLine(string.Format("SELECT gameID FROM gamelist WHERE gameFilePath = '{0}';", gamepath),1);
            

            if (ls == null)
            {
                string sql = string.Format("INSERT INTO gamelist VALUES(NULL,'{0}',NULL,'False',NULL,NULL,NULL,NULL);", gamepath);
                sqliteH.ExecuteSql(sql);
                ls = sqliteH.ExecuteReader_OneLine(string.Format("SELECT gameID FROM gamelist WHERE gameFilePath = '{0}';", gamepath),1);
            }

            return int.Parse(ls[0]);
        }

        /// <summary>
        /// 创建一个新游戏列表库
        /// </summary>
        public static void CreateNewGameList() {
            SQLiteHelper.CreateNewDatabase(Environment.CurrentDirectory + "\\settings\\GameList.sqlite");
            SQLiteHelper sqliteH = new SQLiteHelper(Environment.CurrentDirectory + "\\settings\\GameList.sqlite");
            sqliteH.ExecuteSql("CREATE TABLE gamelist(gameID INTEGER PRIMARY KEY AUTOINCREMENT,gameFilePath TEXT,gameName TEXT,isHookFunMulti TEXT,hookCode TEXT,srcLang TEXT,dstLang TEXT,RepeatMethod TEXT);");
        }

        /// <summary>
        /// 向历史记录队列中添加消息
        /// </summary>
        /// <param name="srcText"></param>
        /// <param name="firstTransText"></param>
        /// <param name="secondTransText"></param>
        public static void AddHistoryText(string srcText, string firstTransText, string secondTransText)
        {
            if (Common.HistoryTextInfos.Count >= 5)
            {
                Common.HistoryTextInfos.Dequeue();
            }
            Common.HistoryTextInfos.Enqueue(new TextInfo
            {
                TIsrcText = srcText,
                TIfirstTransText = firstTransText,
                TIsecondTransText = secondTransText
            });
        }

        /// <summary>
        /// 记录Textractor的历史输出记录
        /// </summary>
        /// <param name="output"></param>
        public static void AddTextractorHistory(string output)
        {
            if (Common.TextractorOutPutHistory.Count >= 1000)
            {
                Common.TextractorOutPutHistory.Dequeue();
            }
            Common.TextractorOutPutHistory.Enqueue(output);
        }

        /// <summary>
        /// 导出Textractor的历史输出记录
        /// </summary>
        public static void ExportTextractorHistory()
        {
            FileStream fs = new FileStream("TextractorOutPutHistory.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine("=================以下是当前状态Common中各值情况================");
            sw.WriteLine("TransMode=" + TransMode.ToString());
            sw.WriteLine("GameID=" + GameID.ToString());
            sw.WriteLine("HookCode=" + HookCode);
            sw.WriteLine("HookCodePlus=" + HookCodePlus);
            sw.WriteLine("=================以下是Textractor的历史输出记录================");
            string[] history = Common.TextractorOutPutHistory.ToArray();
            for (int i = 0; i < history.Length; i++)
            {
                sw.WriteLine(history[i]);
            }

            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
