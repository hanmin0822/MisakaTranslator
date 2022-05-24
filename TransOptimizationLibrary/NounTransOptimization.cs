using AppEnvironmentLibrary;
using SQLHelperLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TransOptimizationLibrary
{
    public class NounTransOptimization
    {
        public SQLHelper sqlite;
        private string srcLangCode;
        private string dstLangCode;
        public string PeopleChatName;//显示在结果中的对话人名 以 人名：对话 的形式展示

        public NounTransOptimization(string gameName,string srcL,string dstL) {
            if (File.Exists(AppEnvironment.LocalFolder + "\\TransOptimization\\Misaka_" + gameName + ".sqlite") == false)
            {
                CreateNewNounTransDB(gameName);
            }
            else {
                sqlite = new SQLHelper(AppEnvironment.LocalFolder + "\\TransOptimization\\Misaka_" + gameName + ".sqlite");
            }
            srcLangCode = srcL;
            dstLangCode = dstL;
        }



        /// <summary>
        /// 处理句子中间出现的人名地名，进行直接替换
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ReplacePeoPleLocNameInSentence(string text) {
            //先从库中查找已经定义的人名地名列表
            List<List<string>> lst = sqlite.ExecuteReader("SELECT source,userTrans,type FROM NounTransOpt WHERE type = 1 OR type = 2;", 3);

            PeopleChatName = "";

            if (lst == null) {
                return text;
            }

            //直接替换
            for (int i = 0;i < lst.Count;i++) {
                List<string> l = lst[i];

                if (l[2] == "1") {
                    //如果是人名

                    if (text.StartsWith(l[0])) {
                        //出现在首部
                        text = text.Remove(0,l[0].Length);
                        PeopleChatName = l[1];
                    }
                    else if (text.EndsWith(l[0]))
                    {
                        //出现在尾部
                        int pos = text.LastIndexOf(l[0]);
                        if (pos > 0) {
                            text = text.Remove(pos, l[0].Length);
                            PeopleChatName = l[1];
                        }
                    }
                }

                text = text.Replace(l[0],l[1]);
            }
            return text;
        }

        /// <summary>
        /// 添加一条用户自定义名词翻译
        /// </summary>
        /// <param name="source">源单词</param>
        /// <param name="type">类型：1=人名 2=地名 3=专有名词（暂不支持）</param>
        /// <param name="userTrans">用户定义的翻译结果</param>
        /// <param name="machineTrans">机器翻译的结果（可空）</param>
        /// <returns>添加结果，如果已存在则失败</returns>
        public bool AddNounTrans(string source,int type,string userTrans,string machineTrans = "") {
            string sql =
                $"INSERT INTO NounTransOpt VALUES('{source}','{srcLangCode}',{type},'{userTrans}','{dstLangCode}','{machineTrans}');";
            if (sqlite.ExecuteSql(sql) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// (未实现)替换通过Mecab分词得到的名词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ReplaceNounInSentence(string text) {
            return text;

        }

        /// <summary>
        /// 新建一个名词翻译数据库（一个游戏一个库）
        /// </summary>
        /// <param name="gameName"></param>
        private void CreateNewNounTransDB(string gameName) {
            sqlite = new SQLHelper(AppEnvironment.LocalFolder + "\\TransOptimization\\Misaka_" + gameName + ".sqlite");
            sqlite.ExecuteSql("CREATE TABLE NounTransOpt(source TEXT PRIMARY KEY,src_lang TEXT,type INT,userTrans TEXT,dst_lang TEXT,machineTrans TEXT);");
        }
    }
}
