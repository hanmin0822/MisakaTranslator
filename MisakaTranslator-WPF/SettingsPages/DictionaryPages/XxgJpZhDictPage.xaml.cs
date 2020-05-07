using DictionaryHelperLibrary;
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

namespace MisakaTranslator_WPF.SettingsPages.DictionaryPages
{
    /// <summary>
    /// XxgJpZhDictPage.xaml 的交互逻辑
    /// </summary>
    public partial class XxgJpZhDictPage : Page
    {
        public XxgJpZhDictPage()
        {
            InitializeComponent();
            PathBox.Text = Common.appSettings.xxgPath;
        }

        private void ChoosePathBtn_Click(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = App.Current.Resources["XxgJpZhDictPage_ChoosePathHint"].ToString();
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.FileName))
                {
                    HandyControl.Controls.Growl.Error(App.Current.Resources["FilePath_Null_Hint"].ToString());
                }
                else
                {
                    PathBox.Text = dialog.FileName;
                    Common.appSettings.xxgPath = PathBox.Text;
                }
            }
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            IDict dict = new XxgJpzhDict();

            dict.DictInit(Common.appSettings.xxgPath, "");

            string ret = dict.SearchInDict(TestSrcText.Text);
            if (ret != null)
            {
                ret = XxgJpzhDict.RemoveHTML(ret);

                var textbox = new HandyControl.Controls.TextBox();
                textbox.Text = ret;
                textbox.FontSize = 15;
                textbox.TextWrapping = TextWrapping.Wrap;
                textbox.TextAlignment = TextAlignment.Left;
                textbox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                var window = new HandyControl.Controls.PopupWindow
                {
                    PopupElement = textbox,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    BorderThickness = new Thickness(0, 0, 0, 0),
                    MaxWidth = 600,
                    MaxHeight = 300,
                    MinWidth = 600,
                    MinHeight = 300,
                    Title = "字典结果"
                };
                window.Show();
            }
            else {
                HandyControl.Controls.Growl.Error("查询错误！" + dict.GetLastError());
            }

            
        }
    }
}
