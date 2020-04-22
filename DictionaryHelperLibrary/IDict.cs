using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryHelperLibrary
{
    public interface IDict
    {
        /// <summary>
        /// 字典API初始化
        /// </summary>
        /// <param name="param1">参数一 一般是appID或者路径（为路径时参数二无效）</param>
        /// <param name="param2">参数二 一般是密钥</param>
        void DictInit(string param1, string param2);

        /// <summary>
        /// 查询一次辞典
        /// </summary>
        /// <param name="sourceText">源单词</param>
        /// <returns>查询结果,如果查询有错误会返回空，可以通过GetLastError来获取错误</returns>
        string SearchInDict(string sourceWord);

        /// <summary>
        /// 返回最后一次错误的ID或原因
        /// </summary>
        /// <returns></returns>
        string GetLastError();
        
    }
}
