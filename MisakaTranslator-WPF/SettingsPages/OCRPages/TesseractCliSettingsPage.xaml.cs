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
using OCRLibrary;

namespace MisakaTranslator_WPF.SettingsPages.OCRPages
{
    public partial class TesseractCliSettingsPage : Page
    {
        static Dictionary<string, string> modeLst = new Dictionary<string, string>()
        {
            { "日语（横向）", "jpn" },
            { "日语（纵向）", "jpn_vert" },
            { "英语", "eng" },
            { "自定义", "custom" }
        };
        static List<string> itemList = modeLst.Keys.ToList();
        static List<string> valueList = new List<string>();
        static TesseractCliSettingsPage()
        {
            foreach (var k in itemList)
            {
                valueList.Add(modeLst[k]);
            }
        }
        public TesseractCliSettingsPage()
        {
            InitializeComponent();
            PathBox.Text = Common.appSettings.TesseractCli_Path;
            ArgsBox.Text = Common.appSettings.TesseractCli_Args;
            SelectBox.ItemsSource = itemList;
            SelectBox.SelectedIndex = valueList.IndexOf(Common.appSettings.TesseractCli_Mode);
            SyncModeAndArgs();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "可执行文件|*.exe";
            if (dialog.ShowDialog() == true)
            {
                if (string.IsNullOrEmpty(dialog.FileName))
                {
                    HandyControl.Controls.Growl.Error(Application.Current.Resources["FilePath_Null_Hint"].ToString());
                }
                else
                {
                    PathBox.Text = dialog.FileName;
                    Common.appSettings.TesseractCli_Path = dialog.FileName;
                }
            }
        }

        private void SelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectBox.SelectedValue != null)
            {
                Common.appSettings.TesseractCli_Mode = modeLst[(string)SelectBox.SelectedValue];
                SyncModeAndArgs();
            }
        }

        private void SyncModeAndArgs()
        {
            switch (Common.appSettings.TesseractCli_Mode)
            {
                case "jpn":
                    Common.appSettings.TesseractCli_Args = "-l jpn --psm 6";
                    ArgsBox.Text = Common.appSettings.TesseractCli_Args;
                    ArgsBox.IsEnabled = false;
                    break;
                case "jpn_vert":
                    Common.appSettings.TesseractCli_Args = "-l jpn_vert --psm 5";
                    ArgsBox.Text = Common.appSettings.TesseractCli_Args;
                    ArgsBox.IsEnabled = false;
                    break;
                case "eng":
                    Common.appSettings.TesseractCli_Args = ArgsBox.Text = "--psm 6";
                    ArgsBox.IsEnabled = false;
                    break;
                default:
                    Common.appSettings.TesseractCli_Args = ArgsBox.Text;
                    ArgsBox.IsEnabled = true;
                    break;
            }
        }

        private void ArgsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Common.appSettings.TesseractCli_Args = ArgsBox.Text;
        }

        private void PathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Common.appSettings.TesseractCli_Path = PathBox.Text;
        }
    }
}
