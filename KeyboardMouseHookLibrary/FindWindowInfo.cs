using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardMouseHookLibrary
{
    public class FindWindowInfo
    {
        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄
        public static extern int WindowFromPoint(
            int xPoint,
            int yPoint
        );

        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(
            int hWnd,
            StringBuilder lpString,
            int nMaxCount
        );

        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(
            int hWnd,
            StringBuilder lpString,
            int nMaxCont
        );

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(
            IntPtr hwnd,
            out int ID
        );

        /// <summary>
        /// 获取指定坐标处窗口的句柄
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <returns></returns>
        public static int GetWindowHWND(int mouseX, int mouseY)
        {

            int hwnd = WindowFromPoint(mouseX, mouseY);
            return hwnd;
        }

        /// <summary>
        /// 根据HWND获得窗口标题
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static string GetWindowName(int hwnd)
        {
            StringBuilder name = new StringBuilder(256);
            GetWindowText(hwnd, name, 256);
            return name.ToString();
        }

        /// <summary>
        /// 根据HWND获得类名
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static string GetWindowClassName(int hwnd)
        {
            StringBuilder name = new StringBuilder(256);
            GetClassName(hwnd, name, 256);
            return name.ToString();
        }

        /// <summary>
        /// 根据HWND获得进程ID
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static int GetProcessIDByHWND(int hwnd)
        {
            int oo;
            IntPtr ip = new IntPtr(hwnd);
            GetWindowThreadProcessId(ip, out oo);
            return oo;
        }

    }
}
