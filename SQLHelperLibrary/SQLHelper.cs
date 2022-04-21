using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppEnvironmentLibrary;

namespace SQLHelperLibrary
{
    public class SQLHelper
    {
        private readonly string _mDbConnectionString;
        private string _errorInfo;//最后一次错误信息

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dataSource">数据库文件路径</param>
        public SQLHelper(string dataSource)
        {
            try
            {
                var connectionStringBuilder = new SQLiteConnectionStringBuilder
                {
                    Version = 3,
                    Pooling = true,
                    FailIfMissing = false,
                    DataSource = dataSource
                };
                _mDbConnectionString = connectionStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 专用于游戏库的初始化
        /// </summary>
        public SQLHelper() : this($"{AppEnvironment.LocalFolder}\\MisakaGameLibrary.sqlite") { }


        /// <summary>
        /// 创建一个新的sqlite数据库，后缀名.sqlite
        /// </summary>
        /// <param name="Filepath">数据库路径</param>
        public static void CreateNewDatabase(string Filepath)
        {
            SQLiteConnection.CreateFile(Filepath);
        }

        /// <summary>
        /// 执行一条非查询语句,失败会返回-1，可通过getLastError获取失败原因
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回影响的结果数</returns>
        public int ExecuteSql(string sql)
        {
            using (var mDbConnection = new SQLiteConnection(_mDbConnectionString))
            {
                try
                {
                    mDbConnection.Open();
                    using (var command = new SQLiteCommand(sql, mDbConnection))
                    {
                        var res = command.ExecuteNonQuery();
                        return res;
                    }
                }
                catch (SQLiteException ex)
                {
                    _errorInfo = ex.Message;
                    return -1;
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回单行结果（适用于执行查询可确定只有一条结果的）,失败返回null,可通过getLastError获取失败原因
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="columns">结果应包含的列数</param>
        /// <returns></returns>
        public List<string> ExecuteReader_OneLine(string sql, int columns)
        {
            using (var mDbConnection = new SQLiteConnection(_mDbConnectionString))
            {
                try
                {
                    mDbConnection.Open();
                    using (var cmd = new SQLiteCommand(sql, mDbConnection))
                    {
                        using (var myReader = cmd.ExecuteReader())
                        {
                            var ret = new List<string>();
                            while (myReader.Read())
                            {
                                for (var i = 0; i < columns; i++)
                                {
                                    ret.Add(myReader[i].ToString());
                                }
                            }
                            return ret;
                        }
                    }

                }
                catch (SQLiteException e)
                {
                    _errorInfo = e.Message;
                    return null;
                }
            }
        }

        /// <summary>
        /// 执行查询语句,返回结果,失败返回null,可通过getLastError获取失败原因
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="columns">结果应包含的列数</param>
        /// <returns></returns>
        public List<List<string>> ExecuteReader(string sql, int columns)
        {
            using (var mDbConnection = new SQLiteConnection(_mDbConnectionString))
            {
                try
                {
                    mDbConnection.Open();
                    using (var cmd = new SQLiteCommand(sql, mDbConnection))
                    {
                        using (var myReader = cmd.ExecuteReader())
                        {
                            var ret = new List<List<string>>();
                            while (myReader.Read())
                            {
                                var lst = new List<string>();
                                for (var i = 0; i < columns; i++)
                                {
                                    lst.Add(myReader[i].ToString());
                                }
                                ret.Add(lst);
                            }

                            return ret;
                        }
                    }

                }
                catch (SQLiteException e)
                {
                    _errorInfo = e.Message;
                    return null;
                }
            }

        }

        /// <summary>
        /// 获取最后一次失败原因
        /// </summary>
        /// <returns></returns>
        public string GetLastError() {
            return _errorInfo;
        }
    }
}
