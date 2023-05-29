using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TranslatorLibrary;
using OCRLibrary;
using System.ComponentModel;
using Windows.Win32;

namespace MisakaTranslator_WPF.ComicTranslator
{
    /// <summary>
    /// ComicTransMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ComicTransMainWindow : Window
    {
        BindingList<ComicTransData> lstData = new BindingList<ComicTransData>();


        List<string> ComicImgList;//图片数组
        string DicPath;//文件夹路径
        int CurrentPos;//当前指针

        private ITranslator _translator1; //第一翻译源
        private ITranslator _translator2; //第二翻译源

        private OCREngine ocr;//OCR对象

        string transRes1;
        string transRes2;

        public string DstLang;
        public string SrcLang;

        System.Windows.Point iniP;
        private ViewModel viewModel;
        System.Drawing.Rectangle selectRect;
        double scale;

        public ComicTransMainWindow()
        {
            InitializeComponent();

            TransResListView.ItemsSource = lstData;

            ComicImgList = new List<string>();
            CurrentPos = 0;

            if (Common.appSettings.HttpProxy != "")
            {
                CommonFunction.SetHttpProxiedClient(Common.appSettings.HttpProxy);
            }
            transRes1 = "";
            transRes2 = "";
            _translator1 = TranslateWindow.TranslatorAuto(Common.appSettings.FirstTranslator);
            _translator2 = TranslateWindow.TranslatorAuto(Common.appSettings.SecondTranslator);

            ocr = OCRCommon.OCRAuto(Common.appSettings.OCRsource);
            ocr.SetOCRSourceLang("jpn");
            if (Common.appSettings.OCRsource == "BaiduOCR")
            {
                if (ocr.OCR_Init(Common.appSettings.BDOCR_APIKEY, Common.appSettings.BDOCR_SecretKey) == false)
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"百度智能云OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "BaiduFanyiOCR")
            {
                if (ocr.OCR_Init(Common.appSettings.BDappID, Common.appSettings.BDsecretKey) == false)
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"百度翻译OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "TencentOCR")
            {
                if (ocr.OCR_Init(Common.appSettings.TXOSecretId, Common.appSettings.TXOSecretKey) == false)
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"腾讯云图片翻译 {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "TesseractCli")
            {
                if (ocr.OCR_Init(Common.appSettings.TesseractCli_Path, Common.appSettings.TesseractCli_Args) == false)
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"TesseractCli {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "TesseractOCR")
            {
                if (ocr.OCR_Init("", "") == false)
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"TesseractOCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }
            else if (Common.appSettings.OCRsource == "WindowsOCR")
            {
                if (ocr.OCR_Init("", "") == false)
                {
                    HandyControl.Controls.Growl.ErrorGlobal($"Windows OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{ocr.GetLastError()}");
                }
            }


            scale = Common.GetScale();
            DrawingAttributes drawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
                Width = 2,
                Height = 2,
                StylusTip = StylusTip.Rectangle,
                //FitToCurve = true,
                IsHighlighter = false,
                IgnorePressure = true,
            };
            inkCanvasMeasure.DefaultDrawingAttributes = drawingAttributes;

            viewModel = new ViewModel
            {
                MeaInfo = "",
                InkStrokes = new StrokeCollection(),
            };

            DataContext = viewModel;
        }

        private async void InkCanvasMeasure_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                iniP = e.GetPosition(inkCanvasMeasure);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                // 当从右下往左上选择时，宽高会为负，导致出现OutOfMemoryException。因为Width为负，X+Width实际上是减少X
                if(selectRect.Width < 0)
                {
                    selectRect.Location = new System.Drawing.Point(selectRect.X + selectRect.Width, selectRect.Y);
                    selectRect.Size = new System.Drawing.Size(-selectRect.Width, selectRect.Height);
                }
                if(selectRect.Height < 0)
                {
                    selectRect.Location = new System.Drawing.Point(selectRect.X, selectRect.Y + selectRect.Height);
                    selectRect.Size = new System.Drawing.Size(selectRect.Width, -selectRect.Height);
                }

                Bitmap bmpImage = new Bitmap(DicPath + "\\" + ComicImgList[CurrentPos]);
                Bitmap bmp = bmpImage.Clone(selectRect, bmpImage.PixelFormat);
                bmpImage.Dispose();

                ImageProcWindow ipw = new ImageProcWindow(bmp);
                ipw.ShowDialog();
                bmp.Dispose();

                if (File.Exists(Environment.CurrentDirectory + "\\comicTemp.png"))
                {
                    Bitmap bm = new Bitmap(Environment.CurrentDirectory + "\\comicTemp.png");
                    bm = ImageProcFunc.ColorToGrayscale(bm);
                    if (!(Common.appSettings.OCRsource == "BaiduFanyiOCR" || Common.appSettings.OCRsource == "TencentOCR"))
                        sourceTextBox.Text = (await ocr.OCRProcessAsync(bm))?.Replace("\f", "");
                    else
                        transTextBox.Text = await ocr.OCRProcessAsync(bm);
                    bm.Dispose();
                }
                else {
                    sourceTextBox.Text = "OCR error";
                }
                e.Handled = true;
            }
        }





        private void InkCanvasMeasure_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Draw square
                System.Windows.Point endP = e.GetPosition(inkCanvasMeasure);
                System.Windows.Point[] pointList = new System.Windows.Point[]
                    {
                        new System.Windows.Point(iniP.X, iniP.Y),
                        new System.Windows.Point(iniP.X, endP.Y),
                        new System.Windows.Point(endP.X, endP.Y),
                        new System.Windows.Point(endP.X, iniP.Y),
                        new System.Windows.Point(iniP.X, iniP.Y),
                    };
                StylusPointCollection point = new StylusPointCollection(pointList);
                Stroke stroke = new Stroke(point)
                {
                    DrawingAttributes = inkCanvasMeasure.DefaultDrawingAttributes.Clone()
                };

                viewModel.InkStrokes.Clear();
                viewModel.InkStrokes.Add(stroke);

                selectRect = new System.Drawing.Rectangle((int)iniP.X, (int)iniP.Y, (int)endP.X - (int)iniP.X, (int)endP.Y - (int)iniP.Y);
                //selectRect = new Rect(new System.Windows.Point(iniP.X * scale, iniP.Y * scale), new System.Windows.Point(endP.X * scale, endP.Y * scale));
            }
        }

        /// <summary>
        /// 让图片框显示图片（按路径）
        /// </summary>
        /// <param name="path"></param>
        private void ShowPictrue(string path) {
            System.Drawing.Bitmap bitmap;
            try
            {
                bitmap = new System.Drawing.Bitmap(path);
            }
            catch(ArgumentException)
            {
                HandyControl.Controls.Growl.Warning("Failed to open " + path);
                return;
            }

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            ImageBrush imageBrush = new ImageBrush();
            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
            sourceComicImg.Source = (ImageSource)imageSourceConverter.ConvertFrom(stream);
            if (RealsizeTBtn.IsChecked == true)
            {
                sourceComicImg.Width = bitmap.Width;
                sourceComicImg.Height = bitmap.Height;
            }
            else {
                sourceComicImg.Width = this.Width * 0.7;
                sourceComicImg.Height = this.Height;
            }
        }

        private void PreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPos > 0) {
                CurrentPos--;
                ShowPictrue(DicPath + "\\" + ComicImgList[CurrentPos]);
            }
            else
            {
                HandyControl.Controls.Growl.InfoGlobal(Application.Current.Resources["ComicTransMainWindow_Hint_FirstofAll"].ToString());
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPos < ComicImgList.Count - 1)
            {
                CurrentPos++;
                ShowPictrue(DicPath + "\\" + ComicImgList[CurrentPos]);
            }
            else {
                HandyControl.Controls.Growl.InfoGlobal(Application.Current.Resources["ComicTransMainWindow_Hint_LastofAll"].ToString());
            }
        }

        private void InputJpnBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Environment.CurrentDirectory + "\\lib\\BaiduJpnInput.exe");
            sourceTextBox.Focus();
        }

        private async void transBtn_Click(object sender, RoutedEventArgs e)
        {
            string sourceText = sourceTextBox.Text;

            if (_translator1 != null)
            {
                transRes1 = await _translator1.TranslateAsync(sourceText, DstLang, SrcLang);
            }
            else
            {
                transRes1 = "None";
            }

            if (_translator2 != null)
            {
                transRes2 = await _translator2.TranslateAsync(sourceText, DstLang, SrcLang);
            }
            else
            {
                transRes2 = "None";
            }

            transTextBox.Text = "翻译一：" + transRes1 + "\n翻译二：" + transRes2;

        }

        private void AddOcrRectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectRect != null && sourceTextBox.Text != "" && transTextBox.Text != "") {
                lstData.Add(new ComicTransData()
                {
                    Pos = CurrentPos + "," + selectRect.Left + "," + selectRect.Top + "," + selectRect.Width + "," + selectRect.Height,
                    SourceText = sourceTextBox.Text,
                    TransText = transTextBox.Text
                });
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = Application.Current.Resources["ComicTransMainWindow_ChoosePathHint"].ToString();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    HandyControl.Controls.Growl.ErrorGlobal(Application.Current.Resources["FilePath_Null_Hint"].ToString());
                    this.Close();
                }
                else
                {
                    //选择语言，该页面所用语言独立于Common
                    HandyControl.Controls.Dialog.Show(new UserControls.SelectTransLangDialog(this));

                    DicPath = dialog.SelectedPath;
                    DirectoryInfo TheFolder = new DirectoryInfo(DicPath);
                    foreach (FileInfo NextFile in TheFolder.GetFiles())
                        ComicImgList.Add(NextFile.Name);

                    ComicImgList.Sort(new FileNameSort());

                    ShowPictrue(DicPath + "\\" + ComicImgList[CurrentPos]);
                }
            }
            else {
                this.Close();
            }
        }

        private void RealsizeTBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(DicPath + "\\" + ComicImgList[CurrentPos]);
            sourceComicImg.Width = bitmap.Width;
            sourceComicImg.Height = bitmap.Height;

            viewModel.InkStrokes.Clear();
        }

        private void FitWinTBtn_Click(object sender, RoutedEventArgs e)
        {
            sourceComicImg.Width = this.Width * 0.7;
            sourceComicImg.Height = this.Height;

            viewModel.InkStrokes.Clear();
        }

        private void RemoveBlankBtn_Click(object sender, RoutedEventArgs e)
        {
            sourceTextBox.Text = sourceTextBox.Text.Replace(" ","").Replace("\r", "").Replace("\n", "");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (lstData.Count != 0) {
                if (MessageBox.Show(Application.Current.Resources["ComicTransMainWindow_IsSaveRes"].ToString(), Application.Current.Resources["MessageBox_Ask"].ToString(), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Windows.Forms.SaveFileDialog saveImageDialog = new System.Windows.Forms.SaveFileDialog();
                    saveImageDialog.Filter = "Txt Files(*.txt)|*.txt";

                    if (saveImageDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SaveResult(saveImageDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show("Save Error");
                        e.Cancel = true;
                    }
                }
            }
        }

        private void SaveResult(string filePath) {
            for (int i = 0;i < lstData.Count;i++) {

            }

            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < lstData.Count; i++)
            {
                sw.WriteLine(lstData[i].Pos);
                sw.WriteLine(lstData[i].SourceText);
                sw.WriteLine(lstData[i].TransText);
                sw.WriteLine("====================");
            }
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }

    public class FileNameSort : IComparer<string>
    {
        //前后文件名进行比较。
        public int Compare(string name1, string name2)
        {
            if (null == name1 && null == name2)
            {
                return 0;
            }
            if (null == name1)
            {
                return -1;
            }
            if (null == name2)
            {
                return 1;
            }
            return PInvoke.StrCmpLogical(name1, name2);
        }
    }

    public class ComicTransData {
        /// <summary>
        /// 文字位置
        /// </summary>
        public string Pos { set; get; }

        /// <summary>
        /// 原文
        /// </summary>
        public string SourceText { set; get; }

        /// <summary>
        /// 译文
        /// </summary>
        public string TransText { set; get; }

    }
}
