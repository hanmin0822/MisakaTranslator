using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLHelperLibrary
{
    public class SQLHelper
    {
        private string m_ConnectionString;
        private SQLiteConnection m_dbConnection;//数据库连接
        private string errorInfo;//最后一次错误信息

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="DataSource">数据库文件路径</param>
        public SQLHelper(string DataSource)
        {
            try
            {
                SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder
                {
                    Version = 3,
                    Pooling = true,
                    FailIfMissing = false,
                    DataSource = DataSource
                };
                m_ConnectionString = connectionStringBuilder.ConnectionString;
                m_dbConnection = new SQLiteConnection(m_ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 专用于游戏库的初始化
        /// </summary>
        public SQLHelper()
        {
            string DataSource = Environment.CurrentDirectory + "\\MisakaGameLibrary.sqlite";
            try
            {
                SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder
                {
                    Version = 3,
                    Pooling = true,
                    FailIfMissing = false,
                    DataSource = DataSource
                };
                m_ConnectionString = connectionStringBuilder.ConnectionString;
                m_dbConnection = new SQLiteConnection(m_ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


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
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            try
            {
                m_dbConnection.Open();
                int res = command.ExecuteNonQuery();
                m_dbConnection.Close();
                return res;
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                m_dbConnection.Close();
                errorInfo = ex.Message;
                return -1;
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
            SQLiteCommand cmd = new SQLiteCommand(sql, m_dbConnection);
            try
            {
                m_dbConnection.Open();
                SQLiteDataReader myReader = cmd.ExecuteReader();

                if (myReader.HasRows == false)
                {
                    m_dbConnection.Close();
                    return new List<string>();
                }
                
                List<string> ret = new List<string>();
                while (myReader.Read())
                {
                    for (int i = 0; i < columns; i++)
                    {
                        ret.Add(myReader[i].ToString());
                    }
                }

                m_dbConnection.Close();
                return ret;
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                m_dbConnection.Close();
                errorInfo = e.Message;
                return null;
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
            SQLiteCommand cmd = new SQLiteCommand(sql, m_dbConnection);
            try
            {
                m_dbConnection.Open();
                SQLiteDataReader myReader = cmd.ExecuteReader();

                if (myReader.HasRows == false)
                {
                    m_dbConnection.Close();
                    return new List<List<string>>();
                }

                List<string> lst = new List<string>();
                List<List<string>> ret = new List<List<string>>();
                while (myReader.Read())
                {
                    for (int i = 0; i < columns; i++)
                    {
                        lst.Add(myReader[i].ToString());
                    }

                    ret.Add(lst);
                }

                m_dbConnection.Close();
                return ret;
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                m_dbConnection.Close();
                errorInfo = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取最后一次失败原因
        /// </summary>
        /// <returns></returns>
        public string getLastError() {
            return errorInfo;
        }
    }
}
