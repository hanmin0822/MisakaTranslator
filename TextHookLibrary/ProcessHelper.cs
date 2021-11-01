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
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    string info = "";
                    info = p.ProcessName + "—" + p.Id;
                    ret.Add(info, p.Id);
                }
                p.Dispose();
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
            string DesProcessName = Process.GetProcessById(pid).ProcessName;

            List<Process> res = new List<Process>();
            foreach (Process p in Process.GetProcesses())
                if (p.ProcessName == DesProcessName)
                    res.Add(p);
                else
                    p.Dispose();
            return res;
        }

        /// <summary>
        /// 根据进程PID找到程序所在路径
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string FindProcessPath(int pid)
        {
            try
            {
                Process p = Process.GetProcessById(pid);
                return p.MainModule.FileName;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return "";
            }
        }

    }
}
