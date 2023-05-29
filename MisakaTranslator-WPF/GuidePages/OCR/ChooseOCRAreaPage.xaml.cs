using HandyControl.Controls;
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
        IntPtr SelectedHwnd;
        System.Drawing.Rectangle OCRArea;
        GlobalHook hook;
        bool IsChoosingWin;

        public ChooseOCRAreaPage()
        {
            InitializeComponent();

            Common.ocr = OCRCommon.OCRAuto(Common.appSettings.OCRsource);

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
                bool r = hook.Start(Process.GetCurrentProcess().MainModule.ModuleName);
                if (r)
                {
                    IsChoosingWin = true;
                }
                else
                {
                    Growl.Error(Application.Current.Resources["Hook_Error_Hint"].ToString());
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
                SelectedHwnd = FindWindowInfo.GetWindowHWND(new System.Drawing.Point(e.X, e.Y));
                string gameName = FindWindowInfo.GetWindowName(SelectedHwnd);
                uint pid = FindWindowInfo.GetProcessIDByHWND(SelectedHwnd);
                string className = FindWindowInfo.GetWindowClassName(SelectedHwnd);

                if (Process.GetCurrentProcess().Id != pid)
                {
                    WinNameTag.Text = $"[实时] {gameName} - {pid} - {className}";
                }
                hook.Stop();
                IsChoosingWin = false;
            }
        }

        private void RenewAreaBtn_Click(object sender, RoutedEventArgs e)
        {
            OCRArea = ScreenCaptureWindow.OCRArea;
            Common.ocr.SetOCRArea(SelectedHwnd, OCRArea, isAllWin);
            OCRAreaPicBox.Source = ImageProcFunc.ImageToBitmapImage(
                Common.ocr.GetOCRAreaCap());

            GC.Collect();
        }
        
        private void ChooseAreaBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isAllWin && SelectedHwnd == IntPtr.Zero)
            {
                HandyControl.Controls.Growl.Error(Application.Current.Resources["ChooseOCRAreaPage_NextErrorHint"].ToString());
                return;
            }
            BitmapImage img;
            
            if (isAllWin)
            {
                img = ImageProcFunc.ImageToBitmapImage(ScreenCapture.GetAllWindow());
            }
            else
            {
                img = ImageProcFunc.ImageToBitmapImage(ScreenCapture.GetWindowCapture(SelectedHwnd));
            }

            ScreenCaptureWindow scw = new ScreenCaptureWindow(img);
            scw.Width = img.Width;
            scw.Height = img.Height;
            scw.Topmost = true;
            scw.Top = 0;
            scw.Left = 0;
            scw.ShowDialog(); // 不用Show()因为需要阻塞等待结果

            RenewAreaBtn_Click(null, null); // 显示结果
            ConfirmBtn.IsEnabled = true;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            Common.isAllWindowCap = isAllWin;
            Common.OCRWinHwnd = SelectedHwnd;
            Common.ocr.SetOCRArea(SelectedHwnd, OCRArea, isAllWin);

            //使用路由事件机制通知窗口来完成下一步操作
            PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
            args.XamlPath = "GuidePages/OCR/ChooseHandleFuncPage.xaml";
            this.RaiseEvent(args);
        }
    }
}
