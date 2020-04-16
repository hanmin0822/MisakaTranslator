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

namespace MisakaTranslator_WPF.SettingsPages
{
    /// <summary>
    /// OCRGeneralSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class OCRGeneralSettingsPage : Page
    {

        public List<string> Langlist;


        private List<string> lstOCR = new List<string>() {
            "BaiduOCR",
            "TesseractOCR"
        };

        public OCRGeneralSettingsPage()
        {
            InitializeComponent();
            OCRsourceCombox.ItemsSource = lstOCR;

            OCRsourceCombox.SelectedValue = Common.appSettings.OCRsource;
            OCRHotKeyBox.Text = Common.appSettings.GlobalOCRHotkey;

            Langlist = ImageProcFunc.lstOCRLang.Keys.ToList();
            OCRLangCombox.ItemsSource = Langlist;
            for (int i = 0;i < Langlist.Count;i++) {
                if (ImageProcFunc.lstOCRLang[Langlist[i]] == Common.appSettings.GlobalOCRLang) {
                    OCRLangCombox.SelectedIndex = i;
                }
            }
            
        }

        private void OCRsourceCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Common.appSettings.OCRsource = (string)OCRsourceCombox.SelectedValue;
        }

        
        private void OCRHotKeyBox_KeyDown(object sender, KeyEventArgs e1)
        {
            System.Windows.Forms.KeyEventArgs e = ToWinforms(e1);
            StringBuilder keyValue = new StringBuilder();
            keyValue.Length = 0;
            keyValue.Append("");
            if (e.Modifiers != 0)
            {
                if (e.Control)
                    keyValue.Append("Ctrl + ");
                if (e.Alt)
                    keyValue.Append("Alt + ");
                if (e.Shift)
                    keyValue.Append("Shift + ");
            }
            if ((e.KeyValue >= 33 && e.KeyValue <= 40) ||
                (e.KeyValue >= 65 && e.KeyValue <= 90) ||   //a-z/A-Z
                (e.KeyValue >= 112 && e.KeyValue <= 123))   //F1-F12
            {
                keyValue.Append(e.KeyCode);
            }
            else if ((e.KeyValue >= 48 && e.KeyValue <= 57))    //0-9
            {
                keyValue.Append(e.KeyCode.ToString().Substring(1));
            }
            ((TextBox)sender).Text = keyValue.ToString();

            Common.appSettings.GlobalOCRHotkey = OCRHotKeyBox.Text;
        }


        /// <summary>
        /// 键盘事件转换
        /// </summary>
        /// <param name="keyEventArgs"></param>
        /// <returns></returns>
        public static System.Windows.Forms.KeyEventArgs ToWinforms(System.Windows.Input.KeyEventArgs keyEventArgs)
        {
            var wpfKey = keyEventArgs.Key == System.Windows.Input.Key.System ? keyEventArgs.SystemKey : keyEventArgs.Key;
            var winformModifiers = ToWinforms(keyEventArgs.KeyboardDevice.Modifiers);
            var winformKeys = (System.Windows.Forms.Keys)System.Windows.Input.KeyInterop.VirtualKeyFromKey(wpfKey);
            return new System.Windows.Forms.KeyEventArgs(winformKeys | winformModifiers);
        }

        public static System.Windows.Forms.Keys ToWinforms(System.Windows.Input.ModifierKeys modifier)
        {
            var retVal = System.Windows.Forms.Keys.None;
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.Alt))
            {
                retVal |= System.Windows.Forms.Keys.Alt;
            }
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.Control))
            {
                retVal |= System.Windows.Forms.Keys.Control;
            }
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.None))
            {
                // Pointless I know
                retVal |= System.Windows.Forms.Keys.None;
            }
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.Shift))
            {
                retVal |= System.Windows.Forms.Keys.Shift;
            }
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.Windows))
            {
                // Not supported lel
            }
            return retVal;
        }

        private void OCRLangCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Common.appSettings.GlobalOCRLang = ImageProcFunc.lstOCRLang[Langlist[OCRLangCombox.SelectedIndex]];
        }
    }
}
