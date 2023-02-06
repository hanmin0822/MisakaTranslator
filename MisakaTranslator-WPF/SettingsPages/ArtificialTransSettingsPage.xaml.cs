using System;
using System.Collections.Generic;
using System.IO;
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
    /// ArtificialTransSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class ArtificialTransSettingsPage : Page
    {
        string[] strNames;

        public ArtificialTransSettingsPage()
        {
            InitializeComponent();

            ATonCheckBox.IsChecked = Common.appSettings.ATon;
            PathBox.Text = Common.appSettings.ArtificialPatchPath;

            if (Directory.Exists(Environment.CurrentDirectory + "\\ArtificialTranslation")) {
                strNames = Directory.GetFiles(Environment.CurrentDirectory + "\\ArtificialTranslation");

                List<string> fileList = new List<string>();

                for (int i = 0; i < strNames.Length; i++)
                {
                    fileList.Add(System.IO.Path.GetFileName(strNames[i]));
                }

                PatchFileCombox.ItemsSource = fileList;

                if (fileList.Count > 0)
                {
                    PatchFileCombox.SelectedIndex = 0;
                }
            }

            
        }

        private void ChoosePathBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "MisakaTranslator人工翻译文件|*.txt",
            };

            if (dialog.ShowDialog() == true)
            {
                PathBox.Text = dialog.FileName;
                Common.appSettings.ArtificialPatchPath = PathBox.Text;
            }
            else
            {
                HandyControl.Controls.Growl.Error(Application.Current.Resources["FilePath_Null_Hint"].ToString());
            }
        }

        private void ATonCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Common.appSettings.ATon = (bool)ATonCheckBox.IsChecked;
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "MisakaTranslator人工翻译文件|*.txt",
            };

            if (dialog.ShowDialog().GetValueOrDefault())
            {
                string savePath = dialog.FileName;
                bool res = ArtificialTransHelperLibrary.ArtificialTransHelper.ExportDBtoFile(savePath, strNames[PatchFileCombox.SelectedIndex]);

                if (res)
                {
                    HandyControl.Controls.Growl.Success(Application.Current.Resources["ArtificialTransSettingsPage_Export_Success"].ToString());
                }
                else {
                    HandyControl.Controls.Growl.Error(Application.Current.Resources["ArtificialTransSettingsPage_Export_Error"].ToString());
                }
                
            }
            else
            {
                HandyControl.Controls.Growl.Error(Application.Current.Resources["ArtificialTransSettingsPage_Export_Error"].ToString());
            }

            
        }
    }
}
