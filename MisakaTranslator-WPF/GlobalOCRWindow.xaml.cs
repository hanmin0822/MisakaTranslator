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
using System.Windows.Shapes;
using OCRLibrary;
using TranslatorLibrary;

namespace MisakaTranslator_WPF
{
    /// <summary>
    /// GlobalOCRWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GlobalOCRWindow : Window
    {
        System.Drawing.Image img;

        public GlobalOCRWindow(System.Drawing.Image i)
        {
            InitializeComponent();
            img = i;
        }

        private void dataInit()
        {
            OCREngine ocr;
            string res = null;
            if (Common.appSettings.OCRsource == "TesseractOCR")
            {
                ocr = new TesseractOCR();
                if (ocr.OCR_Init("", "") != false)
                {
                    ocr.SetOCRSourceLang(Common.appSettings.GlobalOCRLang);
                    res = ocr.OCRProcess(new System.Drawing.Bitmap(img));

                    if (res != null)
                    {
                        sourceText.Text = res;
                    }
                    else
                    {
                        HandyControl.Controls.Growl.ErrorGlobal($"TesseractOCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                    }
                }
                else
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"TesseractOCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "BaiduOCR")
            {
                ocr = new BaiduGeneralOCR();
                if (ocr.OCR_Init(Common.appSettings.BDOCR_APIKEY, Common.appSettings.BDOCR_SecretKey))
                {
                    ocr.SetOCRSourceLang(Common.appSettings.GlobalOCRLang);
                    res = ocr.OCRProcess(new System.Drawing.Bitmap(img));

                    if (res != null)
                    {
                        sourceText.Text = res;
                    }
                    else
                    {
                        HandyControl.Controls.Growl.ErrorGlobal($"百度OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                    }
                }
                else
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"百度OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }

            if (res == null)
            {
                FirstTransText.Text = "OCR ERROR";
            }
            else
            {
                ITranslator translator1 = TranslateWindow.TranslatorAuto(Common.appSettings.FirstTranslator);
                ITranslator translator2 = TranslateWindow.TranslatorAuto(Common.appSettings.SecondTranslator);
                //5.提交翻译
                string transRes1 = "";
                string transRes2 = "";
                if (translator1 != null)
                {
                    transRes1 = translator1.Translate(res, Common.UsingDstLang, Common.UsingSrcLang);
                }
                if (translator2 != null)
                {
                    transRes2 = translator2.Translate(res, Common.UsingDstLang, Common.UsingSrcLang);
                }

                FirstTransText.Text = transRes1;
                SecondTransText.Text = transRes2;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataInit();
        }
    }
}