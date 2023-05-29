using System;
using System.Drawing;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace OCRLibrary
{
    public class ScreenCapture
    {
        /// <summary>
        /// 根据窗口HWND截取窗口
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static Bitmap GetWindowCapture(IntPtr handle)
        {
            // get te hDC of the target window
            HDC hdcSrc = PInvoke.GetWindowDC((HWND)handle);
            PInvoke.ShowWindow((HWND)handle, SHOW_WINDOW_CMD.SW_SHOWNA);
            // get the size
            PInvoke.GetWindowRect((HWND)handle, out RECT windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            HDC hdcDest = PInvoke.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            HBITMAP hBitmap = PInvoke.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            HGDIOBJ hOld = PInvoke.SelectObject(hdcDest, hBitmap);
            // bitblt over
            PInvoke.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, ROP_CODE.SRCCOPY | ROP_CODE.CAPTUREBLT);
            // restore selection
            PInvoke.SelectObject(hdcDest, hOld);
            // clean up
            PInvoke.DeleteDC(hdcDest);
            PInvoke.ReleaseDC((HWND)handle, hdcSrc);
            // get a .NET image object for it
            Bitmap img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            PInvoke.DeleteObject(hBitmap);
            return img;
        }

        /// <summary>
        /// 得到截图中的某个区域
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="rec"></param>
        /// <param name="isAllWin">是否是全屏截屏</param>
        /// <returns></returns>
        public static Bitmap GetWindowRectCapture(IntPtr handle, Rectangle rec, bool isAllWin)
        {
            if (rec.Width == 0 || rec.Height == 0)
                return null;

            using (Bitmap img = isAllWin? GetAllWindow():GetWindowCapture(handle))
                return img.Clone(rec, img.PixelFormat);
        }

        /// <summary>
        /// 全屏截屏
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetAllWindow()
        {
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;

            Bitmap bitmap = new Bitmap(w, h);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(0, 0, 0, 0, new Size(w, h));
            graphics.Dispose();

            return bitmap;
        }
    }
}
