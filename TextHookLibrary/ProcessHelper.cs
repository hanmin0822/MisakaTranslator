using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextHookLibrary
{
    public static class ProcessHelper
    {
        const string ExtPath = "lib\\ProcessHelperExt.exe";

        /// <summary>
        /// 获得当前系统进程列表 形式：直接用于显示的字串和进程PID
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetProcessList_Name_PID()
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
            foreach (Process p in Process.GetProcessesByName(DesProcessName))
                res.Add(p);
            return res;
        }

        /// <summary>
        /// 根据进程PID找到程序所在路径
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string FindProcessPath(int pid, bool isx64game = false)
        {
            try
            {
                Process p = Process.GetProcessById(pid);
                return p.MainModule.FileName;
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                if (!(isx64game && e.NativeErrorCode == 299 && System.IO.File.Exists(ExtPath)))
                    return "";

                // Win32Exception:“A 32 bit processes cannot access modules of a 64 bit process.”
                // 通过调用外部64位程序，使主程序在32位下获取其它64位程序的路径。外部程序不存在或不是此错误时保持原有逻辑返回""
                var p = Process.Start(new ProcessStartInfo(ExtPath, pid.ToString())
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                });
                string path = p.StandardOutput.ReadToEnd().TrimEnd();
                if (p.ExitCode == 3) // 不存在此pid对应的进程
                    return "";
                else if (p.ExitCode != 0)
                    throw new InvalidOperationException("Failed to execute ProcessHelper.exe");
                return path;
            }
        }

        /// <summary>
        /// 返回 pid,绝对路径 的列表
        /// </summary>
        public static List<(int, string)> GetProcessesData(bool isx64game = false)
        {
            var l = new List<(int, string)>();
            // 在32位主程序、64位游戏（或想获取全部进程）、存在外部程序时调用
            if (isx64game && !Environment.Is64BitProcess && System.IO.File.Exists(ExtPath))
            {
                var p = Process.Start(new ProcessStartInfo(ExtPath)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                });
                string output = p.StandardOutput.ReadToEnd();
                if (p.ExitCode != 0)
                    throw new InvalidOperationException("Failed to execute ProcessHelperExt.exe\n" + p.StandardError.ReadToEnd());

                string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    var parts = line.Split('|');
                    l.Add((int.Parse(parts[0]), parts[1]));
                }
            }
            else
                foreach (var p in Process.GetProcesses())
                    using (p)
                        try { l.Add((p.Id, p.MainModule.FileName)); }
                        catch (System.ComponentModel.Win32Exception) { } // 无权限
                        catch (InvalidOperationException) { } // 进程已退出
            return l;
        }

        /// <summary>
        /// internal bool System.Diagnostics.ProcessManager.IsProcessRunning(int pid)
        /// </summary>
        public static Func<int, bool> IsProcessRunning = (Func<int, bool>)typeof(Process).Assembly.GetType("System.Diagnostics.ProcessManager").GetMethod("IsProcessRunning", new[] { typeof(int) }).CreateDelegate(typeof(Func<int, bool>));
    }
}
