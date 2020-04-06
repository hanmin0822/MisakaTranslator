using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextHookLibrary
{
    public class ProcessHelper
    {

        /// <summary>
        /// 获得当前系统进程列表 形式：直接用于显示的字串和进程PID
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,int> GetProcessList_Name_PID()
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            
            //获取系统进程列表
            Process[] ps = Process.GetProcesses();
            for (int i = 0; i < ps.Length; i++)
            {
                Process p = ps[i];
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    string info = "";
                    info = p.ProcessName + "—" + p.Id;
                    ret.Add(info, p.Id);
                }
            }
            return ret;
        }

        /// <summary>
        /// 查找同名进程并返回一个进程PID列表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static List<Process> FindSameNameProcess(int pid)
        {
            List<Process> res = new List<Process>();
            Process[] ps = Process.GetProcesses();
            string DesProcessName = "";

            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Id == pid)
                {
                    DesProcessName = ps[i].ProcessName;
                    break;
                }
            }

            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].ProcessName == DesProcessName)
                {
                    res.Add(ps[i]);
                }
            }

            return res;
        }

        /// <summary>
        /// 根据进程PID找到程序所在路径
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string FindProcessPath(int pid)
        {
            string filepath = "";
            Process[] ps = Process.GetProcesses();
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Id == pid)
                {
                    try
                    {
                        filepath = ps[i].MainModule.FileName;
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        continue;
                        //这个地方直接跳过，是因为32位程序确实会读到64位的系统进程，而系统进程是不能被访问的
                        //throw ex;
                    }
                    break;
                }
            }
            return filepath;
        }

    }
}
