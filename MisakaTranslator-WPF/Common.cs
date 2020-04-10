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
using TextHookLibrary;
using TextRepairLibrary;

namespace MisakaTranslator_WPF
{
    public class Common
    {
        public static IAppSettings appSettings;//应用设置
        public static IRepeatRepairSettings repairSettings;//去重方法参数

        public static int GameID;//全局使用中的游戏ID(数据库)

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
        /// 创建一个新游戏列表库
        /// </summary>
        public static bool CreateNewGameList()
        {
            SQLHelper.CreateNewDatabase(Environment.CurrentDirectory + "\\MisakaGameLibrary.sqlite");
            SQLHelper sqliteH = new SQLHelper();
            int id = sqliteH.ExecuteSql("CREATE TABLE game_library(gameid INTEGER PRIMARY KEY AUTOINCREMENT,gamename TEXT,gamefilepath TEXT,transmode INTEGER,src_lang TEXT,dst_lang TEXT,repair_func TEXT,repair_param_a TEXT,repair_param_b TEXT,hookcode TEXT,isMultiHook TEXT);");
            if (id == -1)
            {
                MessageBox.Show("新建游戏库时发生错误，错误代码:\n" + sqliteH.getLastError(), "数据库错误");
                return false;
            }
            else {
                return true;
            }
        }

        /// <summary>
        /// 得到一个游戏的游戏ID
        /// 如果游戏已经存在于数据库中，则直接返回ID，否则追加新游戏路径并返回新ID，如果返回-1则有数据库错误
        /// </summary>
        /// <param name="gamepath"></param>
        /// <returns>返回游戏ID</returns>
        public static int GetGameID(string gamepath)
        {

            if (File.Exists(Environment.CurrentDirectory + "\\MisakaGameLibrary.sqlite") == false)
            {
                if (CreateNewGameList() == false) {
                    return -1;
                }
            }

            SQLHelper sqliteH = new SQLHelper();

            List<string> ls = sqliteH.ExecuteReader_OneLine(string.Format("SELECT gameid FROM game_library WHERE gamefilepath = '{0}';", gamepath), 1);

            if (ls == null)
            {
                MessageBox.Show("数据库访问时发生错误，错误代码:\n" + sqliteH.getLastError(), "数据库错误");
                return -1;
            }

            if (ls.Count == 0)
            {
                string sql = string.Format("INSERT INTO game_library VALUES(NULL,'{0}','{1}',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL);", 
                    Path.GetFileNameWithoutExtension(gamepath),gamepath);
                sqliteH.ExecuteSql(sql);
                ls = sqliteH.ExecuteReader_OneLine(string.Format("SELECT gameid FROM game_library WHERE gamefilepath = '{0}';", gamepath), 1);
            }

            return int.Parse(ls[0]);
        }
    }
}
