using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextRepairLibrary
{
    public class TextRepir
    {
        public static int SingleWordRepeatTimes;
        public static int SentenceRepeatFindCharNum;
        public static string regexPattern;
        public static string regexReplacement;
        

        public static List<KeyValuePair<string, string>> GetAllTextRepirFun()
        {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();

            ret.Add(new KeyValuePair<string, string>("RepairFun_NoDeal", "不进行处理"));
            ret.Add(new KeyValuePair<string, string>("RepairFun_RemoveSingleWordRepeat", "单字重复处理"));
            ret.Add(new KeyValuePair<string, string>("RepairFun_RemoveSentenceRepeat", "句子重复处理"));
            ret.Add(new KeyValuePair<string, string>("RepairFun_RemoveLetterNumber", "去除字母和数字"));
            ret.Add(new KeyValuePair<string, string>("RepairFun_RegexReplace", "正则表达式替换(见说明)"));
            ret.Add(new KeyValuePair<string, string>("RepairFun_Custom", "用户自定义(见说明)"));

            return ret;
        }

        /// <summary>
        /// 无处理方式
        /// </summary>
        /// <returns></returns>
        public static string RepairFun_NoDeal(string source)
        {
            return source;
        }

        /// <summary>
        /// 处理单字重复
        /// 可以设置重复次数更准确的进行去重
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RepairFun_RemoveSingleWordRepeat(string source)
        {
            if (source == "")
            {
                return "";
            }

            int repeatTimes = SingleWordRepeatTimes;
            int flag = 0;
            string ret = "";

            if (repeatTimes <= 2)
            {
                //未设置固定重复次数时，逢重复就删
                for (int i = 1; i < source.Length; i++)
                {
                    if (source[i] != source[flag])
                    {
                        ret += source[flag];
                        flag = i;
                    }
                }
                ret += source[source.Length - 1];
            }
            else
            {
                //设置了固定重复次数且重复次数大于等于3的情况

                int r = 0;
                for (int i = 1; i < source.Length; i++)
                {
                    if (source[i] == source[flag])
                    {
                        r++;
                        if (r > repeatTimes)
                        {
                            ret += source[i];
                            r = 0;
                        }
                    }
                    else
                    {
                        ret += source[flag];
                        flag = i;
                        r = 0;
                    }
                }
                ret += source[source.Length - 1];
            }

            return ret;
        }

        /// <summary>
        /// 句子重复处理
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RepairFun_RemoveSentenceRepeat(string source)
        {
            int findNum = SentenceRepeatFindCharNum;

            if (source == "" || source.Length < findNum)
            {
                return "";
            }
            
            char[] arr = source.ToCharArray();
            Array.Reverse(arr);
            string text = new string(arr);

            string cmp = "";
            for (int i = 1; i <= findNum; i++)
            {
                cmp = cmp + text[i];
            }

            int pos = text.IndexOf(cmp, findNum);
            if (pos == -1)
            {
                return "句子去重出错";
            }

            string t1 = text.Remove(pos, text.Length - pos);

            char[] arr1 = t1.ToCharArray();
            Array.Reverse(arr1);
            string ret = new string(arr1);

            return ret;
        }

        /// <summary>
        /// 去字母和数字（包括大写和小写字母）
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RepairFun_RemoveLetterNumber(string source)
        {
            string strRemoved = Regex.Replace(source, "[a-z]", "", RegexOptions.IgnoreCase);
            strRemoved = Regex.Replace(strRemoved, "[0-9]", "", RegexOptions.IgnoreCase);
            return strRemoved;
        }


        /// <summary>
        /// 正则表达式替换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RepairFun_RegexReplace(string source) {
            return Regex.Replace(source, regexPattern, regexReplacement);
        }

        
        /// <summary>
        /// 用户自定义方法
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RepairFun_Custom(string source)
        {
            if (source == "")
            {
                return "";
            }

            Assembly asb =
                Assembly.LoadFrom(Environment.CurrentDirectory + "\\UserCustomRepairRepeat.dll");
            Type t = asb.GetType("UserCustomRepairRepeat.RepairRepeat");//获取类名 命名空间+类名
            object o = Activator.CreateInstance(t);
            MethodInfo method = t.GetMethod("UserCustomRepairRepeatFun");//functionname:方法名字
            object[] obj =
            {
                source
            };
            var ret = method.Invoke(o, obj);

            return (string)ret;
        }




    }
}
