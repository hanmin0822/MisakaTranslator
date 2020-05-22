using SQLHelperLibrary;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DictionaryHelperLibrary
{
    /// <summary>
    /// 小学馆日中词典
    /// </summary>
    public class XxgJpzhDict : IDict
    {
        private SQLHelper _sqlHelper;
        private string _errorInfo;

        public string GetLastError()
        {
            return _errorInfo;
        }

        public string SearchInDict(string sourceWord)
        {
            var lst = _sqlHelper.ExecuteReader($"SELECT explanation FROM xiaoxueguanrizhong WHERE word LIKE '%{sourceWord}%';", 1);

            if (lst != null) return lst.Aggregate(string.Empty, (current, t) => current + t[0] + "\n");
            _errorInfo = "DB Error:" + _sqlHelper.GetLastError();
            return null;

        }

        public void DictInit(string param1, string param2)
        {
            _sqlHelper = new SQLHelper(param1);
        }

        /// <summary>
        /// 去掉HTML标记，改为换行
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string RemoveHTML(string src)
        {
            if (src == null)
            {
                return null;
            }

            var tmp = Regex.Replace(src, "<[^>]+>", string.Empty);
            tmp = Regex.Replace(tmp, "&[^;]+;", string.Empty);

            var arr = tmp.Split(new[] { "\\n" }, StringSplitOptions.None);

            return arr.Aggregate(string.Empty, (current, s) => current + s + "\n");
        }
    }
}
