using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using TranslatorLibrary;

namespace MisakaTranslator_WPF.ComicTranslator
{
    /// <summary>
    /// ComicTransMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ComicTransMainWindow : Window
    {
        List<string> ComicImgList;//图片数组
        string DicPath;//文件夹路径
        int CurrentPos;//当前指针
        
        private ITranslator _translator1; //第一翻译源
        private ITranslator _translator2; //第二翻译源

        string transRes1;
        string transRes2;
        int CurrentTrans;//当前翻译源

        public string DstLang;
        public string SrcLang;

        public ComicTransMainWindow()
        {
            InitializeComponent();
            ComicImgList = new List<string>();
            CurrentPos = 0;

            transRes1 = "";
            transRes2 = "";
            CurrentTrans = 2;
            _translator1 = TranslateWindow.TranslatorAuto(Common.appSettings.FirstTranslator);
            _translator2 = TranslateWindow.TranslatorAuto(Common.appSettings.SecondTranslator);
        }

        /// <summary>
        /// 让图片框显示图片（按路径）
        /// </summary>
        /// <param name="path"></param>
        private void ShowPictrue(string path) {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(path);
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            ImageBrush imageBrush = new ImageBrush();
            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
            sourceComicImg.Source = (ImageSource)imageSourceConverter.ConvertFrom(stream);
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

        private void transBtn_Click(object sender, RoutedEventArgs e)
        {
            string sourceText = sourceTextBox.Text;

            if (transRes1 == "" && transRes2 == "") {
                if (_translator1 != null)
                {
                    transRes1 = _translator1.Translate(sourceText, DstLang, SrcLang);
                }
                else
                {
                    transRes1 = "None";
                }

                if (_translator2 != null)
                {
                    transRes2 = _translator2.Translate(sourceText, DstLang, SrcLang);
                }
                else
                {
                    transRes2 = "None";
                }
            }

            if (CurrentTrans == 1)
            {
                transTextBox.Text = transRes2;
                CurrentTrans = 2;
            }
            else {
                transTextBox.Text = transRes1;
                CurrentTrans = 1;
            }
        }

        private void AddOcrRectBtn_Click(object sender, RoutedEventArgs e)
        {

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
                HandyControl.Controls.Growl.ErrorGlobal(Application.Current.Resources["FilePath_Null_Hint"].ToString());
                this.Close();
            }
        }
    }

    public class FileNameSort : IComparer<string>
    {
        //调用windos 的 DLL
        [System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string param1, string param2);

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
            return StrCmpLogicalW(name1, name2);
        }
    }
}
