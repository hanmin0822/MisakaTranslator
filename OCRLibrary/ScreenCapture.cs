using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OCRLibrary
{
    public struct SC_RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public class ScreenCapture
    {
        public const int CAPTUREBLT = 1073741824;
        public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter  
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
            int nWidth, int nHeight, IntPtr hObjectSource,
            int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
          int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref SC_RECT rect);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        #region  窗口关联
        //            nCmdShow的含义
        //0 关闭窗口
        //1 正常大小显示窗口
        //2 最小化窗口
        //3 最大化窗口
        //使用实例: ShowWindow(myPtr, 0);
        #endregion

        /// <summary>
        /// 根据窗口HWND截取窗口
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static Image GetWindowCapture(IntPtr handle)
        {
            // get te hDC of the target window  
            IntPtr hdcSrc = GetWindowDC(handle);
            ShowWindow(hdcSrc, 1);
            // get the size  
            SC_RECT windowRect = new SC_RECT();
            GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to  
            IntPtr hdcDest = CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,  
            // using GetDeviceCaps to get the width/height  
            IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object  
            IntPtr hOld = SelectObject(hdcDest, hBitmap);
            // bitblt over  
            BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY | CAPTUREBLT);
            // restore selection  
            SelectObject(hdcDest, hOld);
            // clean up   
            DeleteDC(hdcDest);
            ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it  
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object  
            DeleteObject(hBitmap);
            return img;
        }

        /// <summary>
        /// 得到截图中的某个区域
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="rec"></param>
        /// <param name="isAllWin">是否是全屏截屏</param>
        /// <returns></returns>
        public static Image GetWindowRectCapture(IntPtr handle, Rectangle rec, bool isAllWin)
        {
            if (rec.Width == 0 || rec.Height == 0)
            {
                return null;
            }
            Image img;
            if (isAllWin == true)
            {
                img = GetAllWindow();
            }
            else
            {
                img = GetWindowCapture(handle);
            }
            Bitmap bmpImage = new Bitmap(img);
            try
            {
                return bmpImage.Clone(rec, bmpImage.PixelFormat);
            }
            catch (OutOfMemoryException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 全屏截屏
        /// </summary>
        /// <returns></returns>
        public static Image GetAllWindow()
        {

            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;

            Bitmap bitmap = new Bitmap(w, h);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(0, 0, 0, 0, new Size(w, h));

            return bitmap;
        }
    }
}
