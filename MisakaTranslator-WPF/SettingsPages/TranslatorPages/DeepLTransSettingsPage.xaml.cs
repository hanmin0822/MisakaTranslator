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
    /// DeepLTransSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class DeepLTransSettingsPage : Page
    {
        public DeepLTransSettingsPage()
        {
            InitializeComponent();
            DeepLTransSecretKeyBox.Text = Common.appSettings.DeepLsecretKey;
        }

        private async void AuthTestBtn_Click(object sender, RoutedEventArgs e)
        {
            Common.appSettings.DeepLsecretKey = DeepLTransSecretKeyBox.Text;
            ITranslator deepLTrans = new DeepLTranslator();
            deepLTrans.TranslatorInit(DeepLTransSecretKeyBox.Text, DeepLTransSecretKeyBox.Text);

            if (await deepLTrans.TranslateAsync("apple", "zh", "en") != null)
            {
                HandyControl.Controls.Growl.Success($"DeepL {Application.Current.Resources["APITest_Success_Hint"]}");
            }
            else
            {
                HandyControl.Controls.Growl.Error($"DeepL {Application.Current.Resources["APITest_Error_Hint"]}\n{deepLTrans.GetLastError()}");
            }
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(DeepLTranslator.SIGN_UP_URL);
        }

        private void DocBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(DeepLTranslator.DOCUMENT_URL);
        }

        private void BillBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(DeepLTranslator.BILL_URL);
        }

        private async void TransTestBtn_Click(object sender, RoutedEventArgs e)
        {
            ITranslator deepLTrans = new DeepLTranslator();
            deepLTrans.TranslatorInit(DeepLTransSecretKeyBox.Text, DeepLTransSecretKeyBox.Text);
            string res = await deepLTrans.TranslateAsync(TestSrcText.Text, TestDstLang.Text, TestSrcLang.Text);

            if (res != null)
            {
                HandyControl.Controls.MessageBox.Show(res, Application.Current.Resources["MessageBox_Result"].ToString());
            }
            else
            {
                HandyControl.Controls.Growl.Error(
                    $"DeepL {Application.Current.Resources["APITest_Error_Hint"]}\n{deepLTrans.GetLastError()}");
            }
        }
    }
}
