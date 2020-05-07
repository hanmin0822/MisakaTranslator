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
using TranslatorLibrary;

namespace MisakaTranslator_WPF.SettingsPages.TranslatorPages
{
    /// <summary>
    /// JbeijingTransSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class JbeijingTransSettingsPage : Page
    {
        public JbeijingTransSettingsPage()
        {
            InitializeComponent();
            PathBox.Text = Common.appSettings.JBJCTDllPath;
        }

        private void TransTestBtn_Click(object sender, RoutedEventArgs e)
        {
            ITranslator Trans = new JBeijingTranslator();
            Trans.TranslatorInit(Common.appSettings.JBJCTDllPath, "");
            string res = Trans.Translate(TestSrcText.Text, "", "");
            if (res != null)
            {
                HandyControl.Controls.MessageBox.Show(res, App.Current.Resources["MessageBox_Result"].ToString());
            }
            else
            {
                HandyControl.Controls.Growl.Error("JBeijing翻译" + App.Current.Resources["APITest_Error_Hint"].ToString() + "\n" + Trans.GetLastError());
            }
        }

        private void ChoosePathBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = App.Current.Resources["JbeijingTransSettingsPage_ChoosePathHint"].ToString();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    HandyControl.Controls.Growl.Error(App.Current.Resources["FilePath_Null_Hint"].ToString());
                }
                else
                {
                    PathBox.Text = dialog.SelectedPath;
                    Common.appSettings.JBJCTDllPath = PathBox.Text;
                }
            }
        }
    }
}
