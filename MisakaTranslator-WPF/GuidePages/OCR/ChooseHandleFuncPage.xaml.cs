using OCRLibrary;
using System;
using System.Collections.Generic;
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
    /// ChooseHandleFuncPage.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseHandleFuncPage : Page
    {
        public List<string> ImageProcFunclist;
        public List<string> Langlist;

        public ChooseHandleFuncPage()
        {
            InitializeComponent();

            ImageProcFunclist = ImageProcFunc.lstHandleFun.Keys.ToList();
            HandleFuncCombox.ItemsSource = ImageProcFunclist;
            HandleFuncCombox.SelectedIndex = 0;

            Langlist = ImageProcFunc.lstOCRLang.Keys.ToList();
            OCRLangCombox.ItemsSource = Langlist;
            OCRLangCombox.SelectedIndex = 0;

        }

        private void RenewAreaBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image img = ScreenCapture.GetWindowRectCapture(Common.UsingOCRWinHwnd, Common.UsingOCRArea, Common.UsingIsAllWin);
            
            SrcImg.Source = ImageProcFunc.ImageToBitmapImage(img);

            DstImg.Source = ImageProcFunc.ImageToBitmapImage(ImageProcFunc.Auto_Thresholding(new System.Drawing.Bitmap(img), 
                ImageProcFunc.lstHandleFun[ImageProcFunclist[HandleFuncCombox.SelectedIndex]]));

            GC.Collect();
        }

        private void OCRTestBtn_Click(object sender, RoutedEventArgs e)
        {
            IOptChaRec ocr = new BaiduGeneralOCR();

            
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            //使用路由事件机制通知窗口来完成下一步操作
            PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
            args.XamlPath = "GuidePages/OCR/ChooseHotKeyPage.xaml";
            this.RaiseEvent(args);
        }

        private void OCRLangCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void HandleFuncCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Drawing.Image img = ScreenCapture.GetWindowRectCapture(Common.UsingOCRWinHwnd, Common.UsingOCRArea, Common.UsingIsAllWin);

            SrcImg.Source = ImageProcFunc.ImageToBitmapImage(img);

            DstImg.Source = ImageProcFunc.ImageToBitmapImage(ImageProcFunc.Auto_Thresholding(new System.Drawing.Bitmap(img),
                ImageProcFunc.lstHandleFun[ImageProcFunclist[HandleFuncCombox.SelectedIndex]]));

            GC.Collect();
        }
    }
}
