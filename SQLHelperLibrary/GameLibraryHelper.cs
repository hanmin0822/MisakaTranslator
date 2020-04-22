using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLHelperLibrary
{
    public class GameInfo
    {
        /// <summary>
        /// 游戏名（非进程名，但在游戏名未知的情况下先使用进程名替代）
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 游戏文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 游戏ID
        /// </summary>
        public int GameID { get; set; }

        /// <summary>
        /// 翻译模式,1=hook 2=ocr
        /// </summary>
        public int TransMode { get; set; }

        /// <summary>
        /// 源语言代码，同翻译API中语言代码
        /// </summary>
        public string Src_Lang { get; set; }

        /// <summary>
        /// 目标语言代码，同翻译API中语言代码
        /// </summary>
        public string Dst_Lang { get; set; }

        /// <summary>
        /// 去重方法，仅在hook模式有效
        /// </summary>
        public string Repair_func { get; set; }

        /// <summary>
        /// 去重方法所需参数1，仅在hook模式有效
        /// </summary>
        public string Repair_param_a { get; set; }

        /// <summary>
        /// 去重方法所需参数2，仅在hook模式有效
        /// </summary>
        public string Repair_param_b { get; set; }

        /// <summary>
        /// 特殊码值，仅在hook模式有效
        /// </summary>
        public string Hookcode { get; set; }

        /// <summary>
        /// 是否需要下次启动时重选Hook方法，仅在hook模式有效，值为True或False
        /// </summary>
        public bool IsMultiHook { get; set; }

        /// <summary>
        /// 检查是否是64位应用程序
        /// </summary>
        public bool Isx64 { get; set; }
    }

    public class GameLibraryHelper
    {
        /// <summary>
        /// 创建一个新游戏列表库
        /// </summary>
        public static bool CreateNewGameList()
        {
            SQLHelper.CreateNewDatabase(Environment.CurrentDirectory + "\\MisakaGameLibrary.sqlite");
            SQLHelper sqliteH = new SQLHelper();
            int id = sqliteH.ExecuteSql("CREATE TABLE game_library(gameid INTEGER PRIMARY KEY AUTOINCREMENT,gamename TEXT,gamefilepath TEXT,transmode INTEGER,src_lang TEXT,dst_lang TEXT,repair_func TEXT,repair_param_a TEXT,repair_param_b TEXT,hookcode TEXT,isMultiHook TEXT,isx64 TEXT);");
            if (id == -1)
            {
                MessageBox.Show("新建游戏库时发生错误，错误代码:\n" + sqliteH.getLastError(), "数据库错误");
                return false;
            }
            else
            {
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
                if (CreateNewGameList() == false)
                {
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
                string sql = string.Format("INSERT INTO game_library VALUES(NULL,'{0}','{1}',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);",
                    Path.GetFileNameWithoutExtension(gamepath), gamepath);
                sqliteH.ExecuteSql(sql);
                ls = sqliteH.ExecuteReader_OneLine(string.Format("SELECT gameid FROM game_library WHERE gamefilepath = '{0}';", gamepath), 1);
            }

            return int.Parse(ls[0]);
        }

        /// <summary>
        /// 得到游戏库中所有游戏的信息
        /// </summary>
        public static List<GameInfo> GetAllGameLibrary()
        {
            if (File.Exists(Environment.CurrentDirectory + "\\MisakaGameLibrary.sqlite") == false)
            {
                if (CreateNewGameList() == false)
                {
                    return null;
                }
            }

            SQLHelper sqliteH = new SQLHelper();
            List <List<string>> ls = sqliteH.ExecuteReader("SELECT * FROM game_library;", 12);

            if (ls == null)
            {
                MessageBox.Show("数据库访问时发生错误，错误代码:\n" + sqliteH.getLastError(), "数据库错误");
                return null;
            }
            
            if (ls.Count == 0)
            {
                return null;
            }

            List<GameInfo> ret = new List<GameInfo>();

            for (int i = 0;i < ls.Count;i++) {
                List<string> gameI = ls[i];

                GameInfo gi = new GameInfo();

                if (gameI[4] == "" || gameI[5] == "" || gameI[6] == "" || gameI[9] == "" || gameI[10] == "" || gameI[11] == "")
                {
                    //没有完整走完导航的游戏，这时就不需要显示这个库
                    continue;   
                }

                gi.GameID = int.Parse(gameI[0]);
                gi.GameName = gameI[1];
                gi.FilePath = gameI[2];
                gi.TransMode = int.Parse(gameI[3]);
                gi.Src_Lang = gameI[4];
                gi.Dst_Lang = gameI[5];
                gi.Repair_func = gameI[6];
                gi.Repair_param_a = gameI[7];
                gi.Repair_param_b = gameI[8];
                gi.Hookcode = gameI[9];
                gi.IsMultiHook = Convert.ToBoolean(gameI[10]);
                gi.Isx64 = Convert.ToBoolean(gameI[11]);

                ret.Add(gi);
            }

            return ret;
        }

        /// <summary>
        /// 通过游戏ID得到游戏信息
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        public static GameInfo GetGameInfoByID(int gameID) {
            if (File.Exists(Environment.CurrentDirectory + "\\MisakaGameLibrary.sqlite") == false)
            {
                if (CreateNewGameList() == false)
                {
                    return null;
                }
            }

            SQLHelper sqliteH = new SQLHelper();
            List<string> ls = sqliteH.ExecuteReader_OneLine(string.Format("SELECT * FROM game_library WHERE gameid = {0};",gameID), 12);

            if (ls == null)
            {
                MessageBox.Show("数据库访问时发生错误，错误代码:\n" + sqliteH.getLastError(), "数据库错误");
                return null;
            }

            if (ls.Count == 0)
            {
                return null;
            }
            
            GameInfo gi = new GameInfo();
            gi.GameID = int.Parse(ls[0]);
            gi.GameName = ls[1];
            gi.FilePath = ls[2];
            gi.TransMode = int.Parse(ls[3]);
            gi.Src_Lang = ls[4];
            gi.Dst_Lang = ls[5];
            gi.Repair_func = ls[6];
            gi.Repair_param_a = ls[7];
            gi.Repair_param_b = ls[8];
            gi.Hookcode = ls[9];
            gi.IsMultiHook = Convert.ToBoolean(ls[10]);
            gi.Isx64 = Convert.ToBoolean(ls[11]);

            return gi;
        }

        /// <summary>
        /// 从数据库中删除这个游戏
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        public static bool DeleteGameByID(int gameID) {
            SQLHelper sqliteH = new SQLHelper();
            int ret = sqliteH.ExecuteSql(string.Format("DELETE FROM game_library WHERE gameid = {0};",gameID));
            if (ret == -1) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改数据库中的游戏名
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool UpdateGameNameByID(int gameID, string name) {
            SQLHelper sqliteH = new SQLHelper();
            int ret = sqliteH.ExecuteSql(string.Format("UPDATE game_library SET gamename = '{0}' WHERE gameid = {1};",name,gameID));
            if (ret == -1)
            {
                return false;
            }
            return true;
        }
    }
}
