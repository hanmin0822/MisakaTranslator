/*
 *Namespace         MisakaTranslator
 *Class             GlobalMouseHook
 *Description       全局鼠标Hook方法，其余几个类为辅助处理类
 */


using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MisakaTranslator
{

    //Declare wrapper managed POINT class.
    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        public int x;
        public int y;
    }

    //Declare wrapper managed MouseHookStruct class.
    [StructLayout(LayoutKind.Sequential)]
    public class MouseHookStruct
    {
        public POINT pt;
        public int hwnd;
        public int wHitTestCode;
        public int dwExtraInfo;
    }

    public class GlobalMouseHook
    {
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        public delegate int GlobalHookProc(int nCode, Int32 wParam, IntPtr lParam);

        public GlobalMouseHook()
        {
            //Start();
        }
        ~GlobalMouseHook()
        {
            Stop();
        }
        public event MouseEventHandler OnMouseActivity;

        // 定义鼠标钩子句柄.
        static int _hMouseHook = 0;
        
        public int HMouseHook
        {
            get { return _hMouseHook; }
        }
        

        // 鼠标钩子常量(from Microsoft SDK  Winuser.h )
        public const int WH_MOUSE_LL = 14;
        
        //定义鼠标处理过程的委托对象
        GlobalHookProc MouseHookProcedure;
        

        //导入window 钩子扩展方法导入

        /// <summary>
        /// 安装钩子方法
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, GlobalHookProc lpfn, IntPtr hInstance, int threadId);

        /// <summary>
        /// 卸载钩子方法
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //Import for CallNextHookEx.
        /// <summary>
        /// 使用这个函数钩信息传递给链中的下一个钩子过程。
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        public bool Start()
        {
            // install Mouse hook
            if (_hMouseHook == 0)
            {
                // Create an instance of HookProc.
                MouseHookProcedure = new GlobalHookProc(MouseHookProc);
                try
                {
                    _hMouseHook = SetWindowsHookEx(WH_MOUSE_LL,
                        MouseHookProcedure,
                        Marshal.GetHINSTANCE(
                        Assembly.GetExecutingAssembly().GetModules()[0]),
                        0);
                }
                catch (Exception err)
                { }
                //如果安装鼠标钩子失败
                if (_hMouseHook == 0)
                {
                    Stop();
                    return false;
                    //throw new Exception("SetWindowsHookEx failed.");
                }
            }
            
            return true;
        }

        public void Stop()
        {
            bool retMouse = true;
            if (_hMouseHook != 0)
            {
                retMouse = UnhookWindowsHookEx(_hMouseHook);
                _hMouseHook = 0;
            }
            
            //If UnhookWindowsHookEx fails.
            if (!(retMouse))
            {
                //throw new Exception("UnhookWindowsHookEx ist failed.");
            }

        }
        /// <summary>
        /// 卸载hook,如果进程强制结束,记录上次钩子id,并把根据钩子id来卸载它
        /// </summary>
        public void Stop(int hMouseHook)
        {
            if (hMouseHook != 0)
            {
                UnhookWindowsHookEx(hMouseHook);
            }
            
        }

        private const int WM_MOUSEMOVE = 0x200;

        private const int WM_LBUTTONDOWN = 0x201;

        private const int WM_RBUTTONDOWN = 0x204;

        private const int WM_MBUTTONDOWN = 0x207;

        private const int WM_LBUTTONUP = 0x202;

        private const int WM_RBUTTONUP = 0x205;

        private const int WM_MBUTTONUP = 0x208;

        private const int WM_LBUTTONDBLCLK = 0x203;

        private const int WM_RBUTTONDBLCLK = 0x206;

        private const int WM_MBUTTONDBLCLK = 0x209;

        private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if ((nCode >= 0) && (OnMouseActivity != null))
            {
                MouseButtons button = MouseButtons.None;
                switch (wParam)
                {
                    case WM_LBUTTONDOWN:    //左键按下
                        button = MouseButtons.Left;
                        int clickCount = 0;
                        if (button != MouseButtons.None)
                            if (wParam == WM_LBUTTONDBLCLK || wParam == WM_RBUTTONDBLCLK)
                                clickCount = 2;
                            else clickCount = 1;

                        //Marshall the data from callback.
                        MouseHookStruct MyMouseHookStruct =
                            (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                        MouseEventArgs e = new MouseEventArgs(
                            button,
                            clickCount,
                            MyMouseHookStruct.pt.x,
                            MyMouseHookStruct.pt.y,
                            0);
                        OnMouseActivity(this, e);

                        break;
                    case WM_RBUTTONDOWN:
                        
                        button = MouseButtons.Right;
                        break;
                }
            }
            return CallNextHookEx(_hMouseHook, nCode, wParam, lParam);
        }
    }
}
