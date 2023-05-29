using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace TextHookLibrary
{
    public class ClipboardNotification
    {
        HWND winHandle;
        HWND ClipboardViewerNext;

        public ClipboardNotification(IntPtr winH)
        {
            winHandle = (HWND)winH;
        }

        public void RegisterClipboardViewer()
        {
            ClipboardViewerNext = PInvoke.SetClipboardViewer(winHandle);
        }

        public void UnregisterClipboardViewer()
        {
            PInvoke.ChangeClipboardChain(winHandle, ClipboardViewerNext);
        }
    }


    /// <summary>
    /// 剪贴板更新事件
    /// </summary>
    /// <param name="ClipboardText">更新的文本</param>
    public delegate void ClipboardUpdateEventHandler(string ClipboardText);

    /// <summary>
    /// 使用一个隐藏窗口来接受窗口消息,对外就是剪贴板监视类
    /// </summary>
    public class ClipboardMonitor : Form
    {
        public event ClipboardUpdateEventHandler onClipboardUpdate;
        private IntPtr hWnd;
        public ClipboardNotification cn;

        public ClipboardMonitor(ClipboardUpdateEventHandler onClipboardUpdate)
        {
            this.onClipboardUpdate = onClipboardUpdate;
            this.hWnd = this.Handle;
            cn = new ClipboardNotification(hWnd);
            cn.RegisterClipboardViewer();
        }

        ~ClipboardMonitor()
        {
            cn.UnregisterClipboardViewer();
        }

        protected override void WndProc(ref Message m)
        {
            switch ((int)m.Msg)
            {
                case 0x308: //WM_DRAWCLIPBOARD
                    {
                        IDataObject iData = Clipboard.GetDataObject();
                        if (iData != null)
                        {
                            string str = (string)iData.GetData(DataFormats.UnicodeText);
                            this.onClipboardUpdate(str);
                        }
                        else {
                            this.onClipboardUpdate("剪贴板更新失败 ClipBoard Update Failed");
                        }
                        break;
                    }
                default:
                    {
                        base.WndProc(ref m);
                        break;
                    }
            }
        }
    }
}
