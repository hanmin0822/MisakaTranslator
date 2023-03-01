extern alias Tesseract;
using HandyControl.Controls;
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
            OCRLangCombox.SelectedIndex = 1;

        }

        private void RenewAreaBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Bitmap img = Common.ocr.GetOCRAreaCap();
            
            SrcImg.Source = ImageProcFunc.ImageToBitmapImage(img);

            DstImg.Source = ImageProcFunc.ImageToBitmapImage(ImageProcFunc.Auto_Thresholding(img, 
                ImageProcFunc.lstHandleFun[ImageProcFunclist[HandleFuncCombox.SelectedIndex]]));

            GC.Collect();
        }

        private async void OCRTestBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (Common.appSettings.OCRsource == "TesseractOCR")
            {
                if (Common.ocr.OCR_Init("", "") != false)
                {
                    string res = await Common.ocr.OCRProcessAsync();

                    if (res != null)
                    {
                        HandyControl.Controls.MessageBox.Show(res, Application.Current.Resources["MessageBox_Result"].ToString());
                    }
                    else {
                        HandyControl.Controls.Growl.Error($"TesseractOCR {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
                    }
                }
                else
                {
                    HandyControl.Controls.Growl.Error($"TesseractOCR {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "BaiduOCR")
            {
                if (Common.ocr.OCR_Init(Common.appSettings.BDOCR_APIKEY, Common.appSettings.BDOCR_SecretKey))
                {
                    string res = await Common.ocr.OCRProcessAsync();

                    if (res != null)
                    {
                        HandyControl.Controls.MessageBox.Show(res, Application.Current.Resources["MessageBox_Result"].ToString());
                    }
                    else
                    {
                        HandyControl.Controls.Growl.Error($"百度智能云OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
                    }
                }
                else {
                    HandyControl.Controls.Growl.Error($"百度智能云OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "BaiduFanyiOCR")
            {
                if (Common.ocr.OCR_Init(Common.appSettings.BDappID, Common.appSettings.BDsecretKey))
                {
                    string res = await Common.ocr.OCRProcessAsync();

                    if (res != null)
                        HandyControl.Controls.MessageBox.Show(res, Application.Current.Resources["MessageBox_Result"].ToString());
                    else
                        HandyControl.Controls.Growl.Error($"百度翻译OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
                }
                else
                    HandyControl.Controls.Growl.Error($"百度翻译OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
            }
            else if (Common.appSettings.OCRsource == "TencentOCR")
            {
                if (Common.ocr.OCR_Init(Common.appSettings.TXOSecretId, Common.appSettings.TXOSecretKey))
                {
                    string res = await Common.ocr.OCRProcessAsync();

                    if (res != null)
                        HandyControl.Controls.MessageBox.Show(res, Application.Current.Resources["MessageBox_Result"].ToString());
                    else
                        HandyControl.Controls.Growl.Error($"腾讯图片翻译 {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
                }
                else
                    HandyControl.Controls.Growl.Error($"腾讯图片翻译 {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
            }
            else if (Common.appSettings.OCRsource == "TesseractCli")
            {
                if (Common.ocr.OCR_Init(Common.appSettings.TesseractCli_Path, Common.appSettings.TesseractCli_Args))
                {
                    string res = await Common.ocr.OCRProcessAsync();

                    if (res != null)
                    {
                        HandyControl.Controls.MessageBox.Show(res, Application.Current.Resources["MessageBox_Result"].ToString());
                    }
                    else
                    {
                        HandyControl.Controls.Growl.Error($"TesseractCli {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
                    }
                }
                else
                {
                    HandyControl.Controls.Growl.Error($"TesseractCli {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
                }
            }
            else if(Common.appSettings.OCRsource == "WindowsOCR")
            {
                if (Common.ocr.OCR_Init("", "") != false)
                {
                    string res = await Common.ocr.OCRProcessAsync();

                    if (res != null)
                    {
                        HandyControl.Controls.MessageBox.Show(res, Application.Current.Resources["MessageBox_Result"].ToString());
                    }
                    else
                    {
                        HandyControl.Controls.Growl.Error($"Windows OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
                    }
                }
                else
                {
                    HandyControl.Controls.Growl.Error($"Windows OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{Common.ocr.GetLastError()}");
                }
            }

            ConfirmBtn.IsEnabled = true;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            Common.GameID = -1;//OCR暂时还不支持保存,因此强制给值-1

            //使用路由事件机制通知窗口来完成下一步操作
            PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
            args.XamlPath = "GuidePages/OCR/ChooseHotKeyPage.xaml";
            this.RaiseEvent(args);
        }

        private void OCRLangCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Common.ocr.SetOCRSourceLang(ImageProcFunc.lstOCRLang[Langlist[OCRLangCombox.SelectedIndex]]);
        }

        private void HandleFuncCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Drawing.Image img = Common.ocr.GetOCRAreaCap();

            SrcImg.Source = ImageProcFunc.ImageToBitmapImage(img);
            try
            {
                string imgProc = ImageProcFunc.lstHandleFun[ImageProcFunclist[HandleFuncCombox.SelectedIndex]];
                DstImg.Source = ImageProcFunc.ImageToBitmapImage(ImageProcFunc.Auto_Thresholding(new System.Drawing.Bitmap(img), imgProc));
                Common.ocr.SetOCRSourceImgProc(imgProc);
            }
            catch(NullReferenceException ex)
            {
                Growl.Error(Application.Current.Resources["ChooseOCRAreaPage_RenewErrorHint"].ToString());
                return;
            }

            GC.Collect();
        }
    }
}
