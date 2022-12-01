extern alias Tesseract;
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
        System.Drawing.Bitmap img;

        public GlobalOCRWindow(System.Drawing.Bitmap i)
        {
            InitializeComponent();
            img = i;
        }

        private async void dataInit()
        {
            OCREngine ocr;
            string res = null;
            if (Common.appSettings.OCRsource == "TesseractOCR")
            {
                ocr = new TesseractOCR();
                if (ocr.OCR_Init("", "") != false)
                {
                    ocr.SetOCRSourceLang(Common.appSettings.GlobalOCRLang);
                    res = await ocr.OCRProcessAsync(img);

                    if (res != null)
                    {
                        sourceText.Text = res;
                    }
                    else
                    {
                        HandyControl.Controls.Growl.WarningGlobal($"TesseractOCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                    }
                }
                else
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"TesseractOCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "Tesseract5")
            {
                ocr = new Tesseract5OCR();
                if (ocr.OCR_Init(Common.appSettings.Tesseract5OCR_Path, Common.appSettings.Tesseract5OCR_Args))
                {
                    ocr.SetOCRSourceLang(Common.appSettings.GlobalOCRLang);
                    res = await ocr.OCRProcessAsync(img);

                    if (res != null)
                    {
                        sourceText.Text = res;
                    }
                    else
                    {
                        HandyControl.Controls.Growl.WarningGlobal($"Tesseract5 {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                    }
                }
                else
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"Tesseract5 {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "BaiduOCR")
            {
                ocr = new BaiduGeneralOCR();
                if (ocr.OCR_Init(Common.appSettings.BDOCR_APIKEY, Common.appSettings.BDOCR_SecretKey))
                {
                    ocr.SetOCRSourceLang(Common.appSettings.GlobalOCRLang);
                    res = await ocr.OCRProcessAsync(img);

                    if (res != null)
                    {
                        sourceText.Text = res;
                    }
                    else
                    {
                        HandyControl.Controls.Growl.WarningGlobal($"百度智能云OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                    }
                }
                else
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"百度智能云OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "BaiduFanyiOCR")
            {
                ocr = new BaiduFanyiOCR();
                if (ocr.OCR_Init(Common.appSettings.BDappID, Common.appSettings.BDsecretKey))
                {
                    ocr.SetOCRSourceLang(Common.appSettings.GlobalOCRLang);
                    res = await ocr.OCRProcessAsync(img);

                    if (res != null)
                        FirstTransText.Text = res;
                    else
                        HandyControl.Controls.Growl.WarningGlobal($"百度翻译OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
                else
                    HandyControl.Controls.Growl.ErrorGlobal($"百度翻译OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
            }

            if (res == null)
            {
                FirstTransText.Text = "OCR ERROR";
            }
            else if (Common.appSettings.OCRsource != "BaiduFanyiOCR")
            {
                // 因为历史原因，OCR的源语言用的是三个字母的，如eng和jpn。而翻译的API即Common.UsingSrcLang用的两个字母，如en和jp
                string srclang;
                switch(Common.appSettings.GlobalOCRLang){
                    case "eng":
                        srclang = "en";
                        break;
                    case "jpn":
                        srclang = "jp";
                        break;
                    default:
                        srclang = Common.appSettings.GlobalOCRLang;
                        break;
                }

                if (!Common.appSettings.EachRowTrans)
                    if (srclang == "en")
                        res = res.Replace("\n", " ").Replace("\r", " ");
                    else
                        res = res.Replace("\n", "").Replace("\r", "");

                ITranslator translator1 = TranslateWindow.TranslatorAuto(Common.appSettings.FirstTranslator);
                ITranslator translator2 = TranslateWindow.TranslatorAuto(Common.appSettings.SecondTranslator);
                //5.提交翻译
                string transRes1 = "";
                string transRes2 = "";
                if (translator1 != null)
                {
                    transRes1 = await translator1.TranslateAsync(res, Common.UsingDstLang, srclang);
                }
                if (translator2 != null)
                {
                    transRes2 = await translator2.TranslateAsync(res, Common.UsingDstLang, srclang);
                }

                FirstTransText.Text = transRes1;
                SecondTransText.Text = transRes2;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataInit();
        }
        private void Window_Closing(object sender, object e)
        {
            img.Dispose();
        }
    }
}
