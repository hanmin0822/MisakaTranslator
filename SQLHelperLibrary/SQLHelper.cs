using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;

namespace SQLHelperLibrary
{
    public class SQLHelper
    {
        private readonly string _mDbConnectionString;
        private string _errorInfo;//最后一次错误信息

        static SQLHelper()
        {
            if (Environment.OSVersion.Version.Build >= 10586)
            {
                raw.SetProvider(new SQLite3Provider_winsqlite3());
            }
            else
            {
                raw.SetProvider(new SQLite3Provider_e_sqlite3());
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dataSource">数据库文件路径</param>
        public SQLHelper(string dataSource)
        {
            _mDbConnectionString = "Filename=" + dataSource;
        }

        /// <summary>
        /// 执行一条非查询语句,失败会返回-1，可通过getLastError获取失败原因
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回影响的结果数</returns>
        public int ExecuteSql(string sql)
        {
            using (var mDbConnection = new SqliteConnection(_mDbConnectionString))
            {
                try
                {
                    mDbConnection.Open();
                    using (var command = new SqliteCommand(sql, mDbConnection))
                    {
                        var res = command.ExecuteNonQuery();
                        return res;
                    }
                }
                catch (SqliteException ex)
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
            using (var mDbConnection = new SqliteConnection(_mDbConnectionString))
            {
                try
                {
                    mDbConnection.Open();
                    using (var cmd = new SqliteCommand(sql, mDbConnection))
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
                catch (SqliteException e)
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
            using (var mDbConnection = new SqliteConnection(_mDbConnectionString))
            {
                try
                {
                    mDbConnection.Open();
                    using (var cmd = new SqliteCommand(sql, mDbConnection))
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
                catch (SqliteException e)
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
        public string GetLastError()
        {
            return _errorInfo;
        }
    }
}
