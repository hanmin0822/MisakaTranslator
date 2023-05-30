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
        public string SrcLang { get; set; }

        /// <summary>
        /// 目标语言代码，同翻译API中语言代码
        /// </summary>
        public string DstLang { get; set; }

        /// <summary>
        /// 去重方法，仅在hook模式有效
        /// </summary>
        public string RepairFunc { get; set; }

        /// <summary>
        /// 去重方法所需参数1，仅在hook模式有效
        /// </summary>
        public string RepairParamA { get; set; }

        /// <summary>
        /// 去重方法所需参数2，仅在hook模式有效
        /// </summary>
        public string RepairParamB { get; set; }

        /// <summary>
        /// 特殊码值，仅在hook模式有效
        /// </summary>
        public string Hookcode { get; set; }

        /// <summary>
        /// 用户自定的特殊码值，如果用户这一项不是自定义的，那么应该为'NULL'，仅在hook模式有效，注意下次开启游戏时这里就需要注入一下
        /// </summary>
        public string HookCodeCustom { get; set; }

        /// <summary>
        /// 是否需要下次启动时重选Hook方法，仅在hook模式有效，值为True或False
        /// </summary>
        public bool IsMultiHook { get; set; }

        /// <summary>
        /// 检查是否是64位应用程序
        /// </summary>
        public bool Isx64 { get; set; }
    }

    public static class GameLibraryHelper
    {
        public static readonly SQLHelper sqlHelper = new($"{Environment.CurrentDirectory}\\MisakaGameLibrary.sqlite");

        /// <summary>
        /// 创建一个新游戏列表库
        /// </summary>
        static GameLibraryHelper()
        {
            var id = sqlHelper.ExecuteSql("CREATE TABLE IF NOT EXISTS game_library(gameid INTEGER PRIMARY KEY AUTOINCREMENT,gamename TEXT,gamefilepath TEXT,transmode INTEGER,src_lang TEXT,dst_lang TEXT,repair_func TEXT,repair_param_a TEXT,repair_param_b TEXT,hookcode TEXT,isMultiHook TEXT,isx64 TEXT,hookcode_custom TEXT);");
            if (id == -1)
            {
                MessageBox.Show($"初始化游戏库发生错误，数据库错误代码:\n{sqlHelper.GetLastError()}");
                throw new Exception(sqlHelper.GetLastError());
            }
        }

        /// <summary>
        /// 得到一个游戏的游戏ID
        /// 如果游戏已经存在于数据库中，则直接返回ID，否则追加新游戏路径并返回新ID，如果返回-1则有数据库错误
        /// </summary>
        /// <param name="gamePath"></param>
        /// <returns>返回游戏ID</returns>
        public static int GetGameID(string gamePath)
        {
            var ls = sqlHelper.ExecuteReader_OneLine(
                $"SELECT gameid FROM game_library WHERE gamefilepath = '{gamePath}';", 1);

            if (ls == null)
            {
                MessageBox.Show($"数据库访问时发生错误，错误代码:\n{sqlHelper.GetLastError()}", "数据库错误");
                return -1;
            }

            if (ls.Count == 0)
            {
                var sql =
                    $"INSERT INTO game_library VALUES(NULL,'{Path.GetFileNameWithoutExtension(gamePath)}','{gamePath}',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);";
                sqlHelper.ExecuteSql(sql);
                ls = sqlHelper.ExecuteReader_OneLine(
                    $"SELECT gameid FROM game_library WHERE gamefilepath = '{gamePath}';", 1);
            }

            return int.Parse(ls[0]);
        }

        /// <summary>
        /// 得到游戏库中所有游戏的信息
        /// </summary>
        public static List<GameInfo> GetAllGameLibrary()
        {
            var ls = sqlHelper.ExecuteReader("SELECT * FROM game_library;", 13);

            if (ls == null)
            {
                MessageBox.Show($"数据库访问时发生错误，错误代码:\n{sqlHelper.GetLastError()}", "数据库错误");
                return null;
            }
            
            if (ls.Count == 0)
            {
                return null;
            }

            var ret = new List<GameInfo>();

            foreach (var gameI in ls)
            {
                var gameInfo = new GameInfo();

                if (gameI[4] == "" || gameI[5] == "" || gameI[6] == "" || gameI[9] == "" || gameI[10] == "" || gameI[11] == "")
                {
                    //没有完整走完导航的游戏，这时就不需要显示这个库
                    continue;   
                }

                gameInfo.GameID = int.Parse(gameI[0]);
                gameInfo.GameName = gameI[1];
                gameInfo.FilePath = gameI[2];
                gameInfo.TransMode = int.Parse(gameI[3]);
                gameInfo.SrcLang = gameI[4];
                gameInfo.DstLang = gameI[5];
                gameInfo.RepairFunc = gameI[6];
                gameInfo.RepairParamA = gameI[7];
                gameInfo.RepairParamB = gameI[8];
                gameInfo.Hookcode = gameI[9];
                gameInfo.IsMultiHook = Convert.ToBoolean(gameI[10]);
                gameInfo.Isx64 = Convert.ToBoolean(gameI[11]);
                gameInfo.HookCodeCustom = gameI[12];

                ret.Add(gameInfo);
            }

            return ret;
        }

        /// <summary>
        /// 通过游戏ID得到游戏信息
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        public static GameInfo GetGameInfoByID(int gameID) {
            var ls = sqlHelper.ExecuteReader_OneLine($"SELECT * FROM game_library WHERE gameid = {gameID};", 13);

            if (ls == null)
            {
                MessageBox.Show($"数据库访问时发生错误，错误代码:\n{sqlHelper.GetLastError()}", "数据库错误");
                return null;
            }

            if (ls.Count == 0)
            {
                return null;
            }

            GameInfo gameInfo = new GameInfo
            {
                GameID = int.Parse(ls[0]),
                GameName = ls[1],
                FilePath = ls[2],
                TransMode = int.Parse(ls[3]),
                SrcLang = ls[4],
                DstLang = ls[5],
                RepairFunc = ls[6],
                RepairParamA = ls[7],
                RepairParamB = ls[8],
                Hookcode = ls[9],
                IsMultiHook = Convert.ToBoolean(ls[10]),
                Isx64 = Convert.ToBoolean(ls[11]),
                HookCodeCustom = ls[12]
            };

            return gameInfo;
        }

        /// <summary>
        /// 从数据库中删除这个游戏
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        public static bool DeleteGameByID(int gameID) {
            var ret = sqlHelper.ExecuteSql($"DELETE FROM game_library WHERE gameid = {gameID};");
            return ret != -1;
        }

        /// <summary>
        /// 修改数据库中的游戏名
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool UpdateGameNameByID(int gameID, string name) {
            var ret = sqlHelper.ExecuteSql($"UPDATE game_library SET gamename = '{name}' WHERE gameid = {gameID};");
            return ret != -1;
        }
    }
}
