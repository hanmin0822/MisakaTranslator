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
    /// LESettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class LESettingsPage : Page
    {
        public LESettingsPage()
        {
            InitializeComponent();

            PathBox.Text = Common.appSettings.LEPath;
        }

        private void ChoosePathBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = Application.Current.Resources["LESettingsPage_ChooseFilePathHint"].ToString()
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    HandyControl.Controls.Growl.Error(Application.Current.Resources["FilePath_Null_Hint"].ToString());
                }
                else
                {
                    PathBox.Text = dialog.SelectedPath;
                    Common.appSettings.LEPath = PathBox.Text;
                }
            }
        }
    }
}