using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IronPython.Hosting;

namespace TextRepairLibrary
{
    public class TextRepair
    {
        public static int SingleWordRepeatTimes;
        public static int SentenceRepeatFindCharNum;
        public static string regexPattern;
        public static string regexReplacement;

        public static Dictionary<string, string> lstRepairFun = new Dictionary<string, string>() {
            { "不进行处理" , "RepairFun_NoDeal" },
            { "单字重复处理" , "RepairFun_RemoveSingleWordRepeat" },
            { "句子重复处理" , "RepairFun_RemoveSentenceRepeat" },
            { "去除字母和数字" , "RepairFun_RemoveLetterNumber" },
            { "去除HTML标签" , "RepairFun_RemoveHTML" },
            { "正则表达式替换(见说明)" , "RepairFun_RegexReplace" },
            { "用户自定义DLL(见说明)" , "RepairFun_Custom" }
        };

        static TextRepair()
        {
            try
            {
                string[] handlers = Directory.GetFiles("textRepairPlugins");
                foreach(var handler in handlers)
                {
                    string stem = Path.GetFileNameWithoutExtension(handler);
                    string ext = Path.GetExtension(handler);
                    if (ext != ".py" || stem == "__init__")
                    {
                        continue;
                    }
                    lstRepairFun.Add("用户自定义Python2脚本: " + stem, "#" + stem);
                }
            }
            catch { }
        }


        /// <summary>
        /// 采用反射方式调用方法
        /// </summary>
        /// <param name="functionName">提供方法函数名</param>
        /// <param name="sourceText">源文本</param>
        /// <returns></returns>
        public static string RepairFun_Auto(string functionName,string sourceText)
        {
            if (functionName == null) return sourceText;
            if (functionName.StartsWith("#"))
            {
                return RepairFun_PythonScript(functionName.Substring(1), sourceText);
            }
            Type t = typeof(TextRepair);//括号中的为所要使用的函数所在的类的类名
            MethodInfo mt = t.GetMethod(functionName);
            if (mt != null)
            {
                string str = (string)mt.Invoke(null, new object[] { sourceText });
                return str;
            }
            else
            {
                return "该方法处理错误！";
            }
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
        /// 去除HTML标签
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RepairFun_RemoveHTML(string source) {
            string strRemoved = Regex.Replace(source, "<[^>]+>", "");
            strRemoved = Regex.Replace(strRemoved, "&[^;]+;", "");
            return strRemoved;
        }

        /// <summary>
        /// 正则表达式替换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RepairFun_RegexReplace(string source) {
            if (regexPattern == null || regexReplacement == null || source == "") {
                return "";
            }
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
            try
            {
                Assembly asb = Assembly.LoadFrom(Environment.CurrentDirectory + "\\UserCustomRepairRepeat.dll");
                Type t = asb.GetType("UserCustomRepairRepeat.RepairRepeat");//获取类名 命名空间+类名
                object o = Activator.CreateInstance(t);
                MethodInfo method = t.GetMethod("UserCustomRepairRepeatFun");//functionname:方法名字
                object[] obj =
                {
                    source
                };
                var ret = method.Invoke(o, obj);
                return (string)ret;
            } catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 用户自定义Python脚本
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RepairFun_PythonScript(string handler, string source)
        {
            if (source == "")
            {
                return "";
            }

            Microsoft.Scripting.Hosting.ScriptEngine pythonEngine = Python.CreateEngine();
            Microsoft.Scripting.Hosting.ScriptSource pythonScript = pythonEngine.CreateScriptSourceFromString(
                $"import textRepairPlugins.{handler} as customHandler\n" +
                "ResultStr = customHandler.process(SourceStr)\n"
                );
            Microsoft.Scripting.Hosting.ScriptScope scope = pythonEngine.CreateScope();
            scope.SetVariable("SourceStr", source);

            try
            {
                pythonScript.Execute(scope);
            } 
            catch (Exception e)
            {
                return e.Message;
            }
            return (string)scope.GetVariable("ResultStr");
        }
    }
}
