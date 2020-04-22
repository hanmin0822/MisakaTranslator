using SQLHelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DictionaryHelperLibrary
{
    /// <summary>
    /// 小学馆日中词典
    /// </summary>
    public class XxgJpzhDict : IDict
    {
        SQLHelper sqlite;
        string errorInfo;

        public string GetLastError()
        {
            return errorInfo;
        }

        public string SearchInDict(string sourceWord)
        {
            List<List<string>> lst = sqlite.ExecuteReader("SELECT explanation FROM xiaoxueguanrizhong WHERE word LIKE '%" + sourceWord + "%';", 1);

            if (lst == null) {
                errorInfo = "DB Error:" + sqlite.getLastError();
                return null;
            }

            string ret = "";
            for (int i = 0;i < lst.Count;i++) {
                ret = ret + lst[i][0] + "\n";
            }

            return ret;
        }

        public void DictInit(string param1, string param2)
        {
            sqlite = new SQLHelper(param1);
        }

        /// <summary>
        /// 去掉HTML标记，改为换行
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string RemoveHTML(string src) {
            if (src == null) {
                return null;
            }

            string tmp = Regex.Replace(src, "<[^>]+>", "");
            tmp = Regex.Replace(tmp, "&[^;]+;", "");

            string[] arr = tmp.Split(new string[] { "\\n" }, StringSplitOptions.None);

            string ret = "";
            foreach (string s in arr)
            {
                ret = ret + s + "\n";
            }

            return ret;
        }
    }
}
