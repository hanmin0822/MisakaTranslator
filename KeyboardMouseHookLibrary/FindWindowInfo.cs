using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace KeyboardMouseHookLibrary
{
    public class FindWindowInfo
    {
        /// <summary>
        /// 获取指定坐标处窗口的句柄
        /// </summary>
        public static IntPtr GetWindowHWND(Point point)
        {
            return PInvoke.WindowFromPoint(point);
        }

        /// <summary>
        /// 根据HWND获得窗口标题
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static unsafe string GetWindowName(IntPtr hwnd)
        {
            Span<char> name = stackalloc char[PInvoke.GetWindowTextLength((HWND)hwnd) + 1];
            fixed(char* pName = name)
            {
                PInvoke.GetWindowText((HWND)hwnd, pName, name.Length);
            }
            return name.ToString();
        }

        /// <summary>
        /// 根据HWND获得类名
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static unsafe string GetWindowClassName(IntPtr hwnd)
        {
            Span<char> name = stackalloc char[256];
            fixed(char* pName = name)
            {
                name = name.Slice(0, PInvoke.GetClassName((HWND)hwnd, pName, 256) + 1);
            }
            return name.ToString();
        }

        /// <summary>
        /// 根据HWND获得进程ID
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static unsafe uint GetProcessIDByHWND(IntPtr hWnd)
        {
            uint result;
            PInvoke.GetWindowThreadProcessId((HWND)hWnd, &result);
            return result;
        }
    }
}
