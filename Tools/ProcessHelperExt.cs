// 无参使用时输出所有进程的 pid|绝对路径，有参使用时输出指定pid进程的路径
// 编译：csc ProcessHelperExt.cs /o /platform:x64
using System;
using System.Diagnostics;

if (args.Length > 1)
    return 1;

try
{
    if (args.Length == 1)
    {
        int pid = int.Parse(args[0]);
        Process p;
        try { p = Process.GetProcessById(pid); }
        catch (System.ArgumentException) { return 3; } // 不存在pid对应的进程
        Console.WriteLine(p.MainModule.FileName);
    }
    else
        foreach (var p in Process.GetProcesses())
            try { Console.WriteLine("{0}|{1}", p.Id, p.MainModule.FileName); }
            catch (System.ComponentModel.Win32Exception) { } // 跳过权限不够的进程

    return 0;
}
catch { return 2; }
