using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TranslatorLibrary;

namespace MisakaTranslator_WPF.SettingsPages.TranslatorPages
{
    /// <summary>
    /// YandexTransSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class YandexTransSettingsPage : Page
    {
        public YandexTransSettingsPage()
        {
            InitializeComponent();
            YandexTransApiKeyBox.Text = Common.appSettings.YandexApiKey;
        }

        private async void AuthTestBtn_Click(object sender, RoutedEventArgs e)
        {
            Common.appSettings.YandexApiKey = YandexTransApiKeyBox.Text;
            ITranslator YandexTrans = new YandexTranslator();
            YandexTrans.TranslatorInit(YandexTransApiKeyBox.Text, "");

            if (await YandexTrans.TranslateAsync("apple", "zh", "en") != null)
            {
                HandyControl.Controls.Growl.Success($"Yandex {Application.Current.Resources["APITest_Success_Hint"]}");
            }
            else
            {
                HandyControl.Controls.Growl.Error($"Yandex {Application.Current.Resources["APITest_Error_Hint"]}\n{YandexTrans.GetLastError()}");
            }
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(YandexTranslator.GetUrl_allpyAPI());
        }

        private void DocBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(YandexTranslator.GetUrl_Doc());
        }

        private void BillBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(YandexTranslator.GetUrl_bill());
        }

        private async void TransTestBtn_Click(object sender, RoutedEventArgs e)
        {
            ITranslator YandexTrans = new YandexTranslator();
            YandexTrans.TranslatorInit(YandexTransApiKeyBox.Text, "");
            string res = await YandexTrans.TranslateAsync(TestSrcText.Text, TestDstLang.Text, TestSrcLang.Text);

            if (res != null)
            {
                HandyControl.Controls.MessageBox.Show(res, Application.Current.Resources["MessageBox_Result"].ToString());
            }
            else
            {
                HandyControl.Controls.Growl.Error(
                    $"Yandex {Application.Current.Resources["APITest_Error_Hint"]}\n{YandexTrans.GetLastError()}");
            }
        }
    }
}
