using KeyboardMouseHookLibrary;
using OCRLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MisakaTranslator_WPF.GuidePages.OCR
{
    /// <summary>
    /// ChooseOCRAreaPage.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseOCRAreaPage : Page
    {
        bool isAllWin;
        int SelectedHwnd;
        System.Drawing.Rectangle OCRArea;
        GlobalHook hook;
        bool IsChoosingWin;

        public ChooseOCRAreaPage()
        {
            InitializeComponent();

            if (Common.appSettings.OCRsource == "TesseractOCR")
            {
                Common.ocr = new TesseractOCR();
            }
            else if (Common.appSettings.OCRsource == "BaiduOCR")
            {
                Common.ocr = new BaiduGeneralOCR();
            }

            //初始化钩子对象
            if (hook == null)
            {
                hook = new GlobalHook();
                hook.OnMouseActivity += new System.Windows.Forms.MouseEventHandler(Hook_OnMouseActivity);
            }

        }

        private void AllWinCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)AllWinCheckBox.IsChecked)
            {
                ChooseWinBtn.Visibility = Visibility.Hidden;
                WinNameTag.Visibility = Visibility.Hidden;
            }
            else {
                ChooseWinBtn.Visibility = Visibility.Visible;
                WinNameTag.Visibility = Visibility.Visible;
            }
            isAllWin = (bool)AllWinCheckBox.IsChecked;
        }

        private void ChooseWinBtn_Click(object sender, RoutedEventArgs e)
        {

            if (IsChoosingWin == false)
            {
                bool r = hook.Start();
                if (r)
                {
                    IsChoosingWin = true;
                }
                else
                {
                    MessageBox.Show("安装钩子失败!");
                }
            }
            else if (IsChoosingWin == true)
            {
                hook.Stop();
                IsChoosingWin = false;
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        void Hook_OnMouseActivity(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                SelectedHwnd = FindWindowInfo.GetWindowHWND(e.X, e.Y);
                string gameName = FindWindowInfo.GetWindowName(SelectedHwnd);
                int pid = FindWindowInfo.GetProcessIDByHWND(SelectedHwnd);

                if (Process.GetCurrentProcess().Id != pid)
                {
                    WinNameTag.Text = "[实时]" + gameName + "—" + pid;
                }
                hook.Stop();
                IsChoosingWin = false;
            }
        }

        private void RenewAreaBtn_Click(object sender, RoutedEventArgs e)
        {
            OCRArea = ScreenCaptureWindow.OCRArea;
            Common.ocr.SetOCRArea((IntPtr)SelectedHwnd, OCRArea, isAllWin);
            OCRAreaPicBox.Source = ImageProcFunc.ImageToBitmapImage(
                Common.ocr.GetOCRAreaCap());

            GC.Collect();
        }
        
        private void ChooseAreaBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isAllWin == false && SelectedHwnd == 0)
            {
                HandyControl.Controls.Growl.Error("请先选择窗口!");
                return;
            }
            BitmapImage img;
            
            if (isAllWin == true)
            {
                img = ImageProcFunc.ImageToBitmapImage(ScreenCapture.GetAllWindow());
            }
            else
            {
                img = ImageProcFunc.ImageToBitmapImage(ScreenCapture.GetWindowCapture((IntPtr)SelectedHwnd));
            }

            ScreenCaptureWindow scw = new ScreenCaptureWindow(img);
            scw.Width = img.PixelWidth;
            scw.Height = img.PixelHeight;
            
            scw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            scw.Show();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            Common.isAllWindowCap = isAllWin;
            Common.OCRWinHwnd = (IntPtr)SelectedHwnd;
            Common.ocr.SetOCRArea((IntPtr)SelectedHwnd, OCRArea, isAllWin);

            //使用路由事件机制通知窗口来完成下一步操作
            PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
            args.XamlPath = "GuidePages/OCR/ChooseHandleFuncPage.xaml";
            this.RaiseEvent(args);
        }
    }
}
