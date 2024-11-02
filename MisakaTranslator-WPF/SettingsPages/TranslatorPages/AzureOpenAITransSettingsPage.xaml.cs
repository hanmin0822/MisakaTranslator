using System.Windows;
using System.Windows.Controls;
using TranslatorLibrary;

namespace MisakaTranslator_WPF.SettingsPages.TranslatorPages
{
    /// <summary>
    /// AzureOpenAITransSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class AzureOpenAITransSettingsPage : Page
    {
        public AzureOpenAITransSettingsPage()
        {
            InitializeComponent();
            AzureOpenAITransSecretKeyBox.Text = Common.appSettings.AzureOpenAIApiKey;
            AzureOpenAITransUrlBox.Text = Common.appSettings.AzureOpenAIApiUrl;
        }

        private async void AuthTestBtn_Click(object sender, RoutedEventArgs e)
        {
            Common.appSettings.AzureOpenAIApiKey = AzureOpenAITransSecretKeyBox.Text;
            Common.appSettings.AzureOpenAIApiUrl = AzureOpenAITransUrlBox.Text;

            ITranslator trans = new AzureOpenAITranslator();
            trans.TranslatorInit(AzureOpenAITransSecretKeyBox.Text, AzureOpenAITransUrlBox.Text);

            if (await trans.TranslateAsync("apple", "zh", "en") != null)
            {
                HandyControl.Controls.Growl.Success($"Azure OpenAI {Application.Current.Resources["APITest_Success_Hint"]}");
            }
            else
            {
                HandyControl.Controls.Growl.Error($"Azure OpenAI {Application.Current.Resources["APITest_Error_Hint"]}\n{trans.GetLastError()}");
            }
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://azure.microsoft.com/en-us/solutions/ai/");
        }

        private void DocBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://learn.microsoft.com/en-us/azure/ai-services/cognitive-services-support-options");
        }

        private void BillBtn_Click(object sender, RoutedEventArgs e)
        {
            // System.Diagnostics.Process.Start(ChatGPTTranslator.BILL_URL);
        }

        private async void TransTestBtn_Click(object sender, RoutedEventArgs e)
        {
            ITranslator trans = new AzureOpenAITranslator();
            trans.TranslatorInit(AzureOpenAITransSecretKeyBox.Text, AzureOpenAITransUrlBox.Text);
            string res = await trans.TranslateAsync(TestSrcText.Text, TestDstLang.Text, TestSrcLang.Text);

            if (res != null)
            {
                HandyControl.Controls.MessageBox.Show(res, Application.Current.Resources["MessageBox_Result"].ToString());
            }
            else
            {
                HandyControl.Controls.Growl.Error(
                    $"Azure OpenAI {Application.Current.Resources["APITest_Error_Hint"]}\n{trans.GetLastError()}");
            }
        }
    }
}