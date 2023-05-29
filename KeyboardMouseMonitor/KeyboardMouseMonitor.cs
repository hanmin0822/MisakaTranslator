/*************************************************
*   由于使用全局键盘鼠标钩子总是会出现回调事件
*   没有反应的情况，故使用单写一个进程调用
*   这个EXE并进行通信，获取键鼠钩子事件。
*   启动传参为： 钩子类型（1=鼠标 2=键盘） 指定键值（见代码）
*   当指定键动作时输出一行返回值
*   使用时将这个项目单独编译，生成的EXE文件放入lib中即可
*************************************************/

using System;
using System.Drawing;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace KeyboardMouseMonitor
{
    internal static class Program
    {
        private static int keyCode;

        private static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                return -1;
            }
            HHOOK hook = default;
            int actID = Convert.ToInt32(args[0]);
            keyCode = Convert.ToInt32(args[1]);
            if (actID == 1)
            {
                //鼠标 keyCode=1代表左键 keyCode=2代表右键
                hook = PInvoke.SetWindowsHookEx(WINDOWS_HOOK_ID.WH_MOUSE_LL, mymouse, HMODULE.Null, 0);
            }
            else if (actID == 2)
            {
                //键盘 keyCode代表对应键的ASCII码
                hook = PInvoke.SetWindowsHookEx(WINDOWS_HOOK_ID.WH_KEYBOARD_LL, mykeyboard, HMODULE.Null, 0);
            }
            if (hook.IsNull)
            {
                Console.WriteLine("hookFailed");
            }
            while (PInvoke.GetMessage(out MSG _, HWND.Null, 0, 0)) { }
            PInvoke.UnhookWindowsHookEx(hook);
            return 0;
        }

        private unsafe static LRESULT mymouse(int nCode, WPARAM wParam, LPARAM lParam)
        {
            MOUSEHOOKSTRUCT* mhookstruct = (MOUSEHOOKSTRUCT*)lParam.Value;
            Point pt = mhookstruct->pt;
            if (keyCode == 1)
            {
                if (wParam.Value == PInvoke.WM_LBUTTONUP)
                {
                    Console.WriteLine($"MouseAction {pt.X} {pt.Y}");
                }
            }
            else if (keyCode == 2)
            {
                if (wParam.Value == PInvoke.WM_RBUTTONUP)
                {
                    Console.WriteLine($"MouseAction {pt.X} {pt.Y}");
                }
            }
            return PInvoke.CallNextHookEx(HHOOK.Null, nCode, wParam, lParam);
        }

        private unsafe static LRESULT mykeyboard(int nCode, WPARAM wParam, LPARAM lParam)
        {
            KBDLLHOOKSTRUCT* pKeyboardHookStruct = (KBDLLHOOKSTRUCT*)lParam.Value;

            if (wParam.Value == PInvoke.WM_KEYUP)
            {
                if (pKeyboardHookStruct->vkCode == keyCode)
                {
                    Console.WriteLine("KeyboardAction");
                }
            }
            return PInvoke.CallNextHookEx(HHOOK.Null, nCode, wParam, lParam);
        }
    }
}
