using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace KeyboardMouseHookLibrary
{
    public class HotKeyInfo {
        /// <summary>
        /// 是否使用鼠标
        /// </summary>
        public bool IsMouse { get; set; }

        /// <summary>
        /// 键盘键值
        /// </summary>
        public Keys KeyCode { get; set;}

        /// <summary>
        /// 鼠标键位
        /// </summary>
        public MouseButtons MouseButton { get; set; }
    }



    //来源：https://www.cnblogs.com/CJSTONE/p/4961865.html
    

    /*
    注意：
        如果运行中出现SetWindowsHookEx的返回值为0，这是因为.net 调试模式的问题，具体的做法是禁用宿主进程，在 Visual Studio 中打开项目。
        在“项目”菜单上单击“属性”。
        单击“调试”选项卡。
        清除“启用 Visual Studio 宿主进程(启用windows承载进程)”复选框 或 勾选启用非托管代码调试
    */

    public class GlobalHook
    {
        public string moduleName;//设置的模块名用于确认句柄

        public GlobalHook()
        {
            //Start();
        }

        ~GlobalHook()
        {
            Stop();
        }

        public event MouseEventHandler OnMouseActivity;
        public event KeyEventHandler KeyDown;
        public event KeyPressEventHandler KeyPress;
        public event KeyEventHandler KeyUp;

        /// <summary>
        /// 定义鼠标钩子句柄.
        /// </summary>
        static HHOOK _hMouseHook = HHOOK.Null;
        /// <summary>
        /// 定义键盘钩子句柄
        /// </summary>
        static HHOOK _hKeyboardHook = HHOOK.Null;

        /// <summary>
        /// 定义鼠标处理过程的委托对象
        /// </summary>
        HOOKPROC MouseHookProcedure;
        /// <summary>
        /// 键盘处理过程的委托对象
        /// </summary>
        HOOKPROC KeyboardHookProcedure;

        public bool Start(string _moduleName)
        {
            moduleName = _moduleName;

            // install Mouse hook 
            if (_hMouseHook.IsNull)
            {
                // Create an instance of HookProc.
                MouseHookProcedure = new(MouseHookProc);
                try
                {
                    _hMouseHook = PInvoke.SetWindowsHookEx(WINDOWS_HOOK_ID.WH_MOUSE_LL,
                        MouseHookProcedure,
                        PInvoke.GetModuleHandle(moduleName),
                        0);
                }
                catch (Exception err)
                { return false; }
                //如果安装鼠标钩子失败
                if (_hMouseHook.IsNull)
                {
                    Stop();
                    return false;
                    //throw new Exception("SetWindowsHookEx failed.");
                }
            }
            //安装键盘钩子
            if (_hKeyboardHook.IsNull)
            {
                KeyboardHookProcedure = new(KeyboardHookProc);
                try
                {
                    _hKeyboardHook = PInvoke.SetWindowsHookEx(WINDOWS_HOOK_ID.WH_KEYBOARD_LL,
                        KeyboardHookProcedure,
                        PInvoke.GetModuleHandle(moduleName),
                        0);
                }
                catch (Exception err2)
                { return false; }
                //如果安装键盘钩子失败
                if (_hKeyboardHook.IsNull)
                {
                    Stop();
                    return false;
                    //throw new Exception("SetWindowsHookEx ist failed.");
                }
            }
            return true;
        }

        public void Stop()
        {
            bool retMouse = true;
            bool retKeyboard = true;
            if (!_hMouseHook.IsNull)
            {
                retMouse = PInvoke.UnhookWindowsHookEx(_hMouseHook);
                _hMouseHook = HHOOK.Null;
            }
            if (!_hKeyboardHook.IsNull)
            {
                retKeyboard = PInvoke.UnhookWindowsHookEx(_hKeyboardHook);
                _hKeyboardHook = HHOOK.Null;
            }
            //If UnhookWindowsHookEx fails.
            if (!(retMouse && retKeyboard))
            {
                //throw new Exception("UnhookWindowsHookEx ist failed.");
            }

        }
        /// <summary>
        /// 卸载hook,如果进程强制结束,记录上次钩子id,并把根据钩子id来卸载它
        /// </summary>
        public void Stop(IntPtr hMouseHook, IntPtr hKeyboardHook)
        {
            if (hMouseHook != IntPtr.Zero)
            {
                PInvoke.UnhookWindowsHookEx((HHOOK)hMouseHook);
            }
            if (hKeyboardHook != IntPtr.Zero)
            {
                PInvoke.UnhookWindowsHookEx((HHOOK)hKeyboardHook);
            }
        }

        private LRESULT MouseHookProc(int nCode, WPARAM wParam, LPARAM lParam)
        {
            if ((nCode >= 0) && (OnMouseActivity != null))
            {
                MouseButtons button = MouseButtons.None;
                switch (wParam.Value)
                {
                    case PInvoke.WM_LBUTTONDOWN:    //左键按下
                        //case WM_LBUTTONUP:    //右键按下
                        //case WM_LBUTTONDBLCLK:   //同时按下
                        button = MouseButtons.Left;
                        break;
                    case PInvoke.WM_RBUTTONDOWN:
                        //case WM_RBUTTONUP: 
                        //case WM_RBUTTONDBLCLK: 
                        button = MouseButtons.Right;
                        break;
                }
                int clickCount = 0;
                if (button != MouseButtons.None)
                    if (wParam == PInvoke.WM_LBUTTONDBLCLK || wParam == PInvoke.WM_RBUTTONDBLCLK)
                        clickCount = 2;
                    else clickCount = 1;

                //Marshall the data from callback.
                MOUSEHOOKSTRUCT MyMouseHookStruct =
                    (MOUSEHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MOUSEHOOKSTRUCT));
                MouseEventArgs e = new MouseEventArgs(
                    button,
                    clickCount,
                    MyMouseHookStruct.pt.X,
                    MyMouseHookStruct.pt.Y,
                    0);
                OnMouseActivity(this, e);
            }
            return PInvoke.CallNextHookEx((HHOOK)(IntPtr)_hMouseHook, nCode, wParam, lParam);
             
        }
        

        private LRESULT KeyboardHookProc(int nCode, WPARAM wParam, LPARAM lParam)
        {
            // it was ok and someone listens to events
            if ((nCode >= 0) && (KeyDown != null || KeyUp != null || KeyPress != null))
            {
                KBDLLHOOKSTRUCT MyKeyboardHookStruct =
                    (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam,
                    typeof(KBDLLHOOKSTRUCT));
                // raise KeyDown
                if (KeyDown != null && (wParam == PInvoke.WM_KEYDOWN || wParam == PInvoke.WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyDown(this, e);
                }
                // raise KeyPress
                if (KeyPress != null && wParam == PInvoke.WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    PInvoke.GetKeyboardState(keyState);
                    if (PInvoke.ToAscii(MyKeyboardHookStruct.vkCode,
                        MyKeyboardHookStruct.scanCode,
                        keyState,
                        out ushort inBuffer,
                        (uint)MyKeyboardHookStruct.flags) == 1)
                    {
                        KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer);
                        KeyPress(this, e);
                    }
                }
                // raise KeyUp
                if (KeyUp != null && (wParam == PInvoke.WM_KEYUP || wParam == PInvoke.WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyUp(this, e);
                }
            }


            return PInvoke.CallNextHookEx((HHOOK)(IntPtr)_hKeyboardHook, nCode, wParam, lParam);
            
        }
    }
}
