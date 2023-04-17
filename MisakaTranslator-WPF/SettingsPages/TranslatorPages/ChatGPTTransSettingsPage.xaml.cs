using System.Windows;
using System.Windows.Controls;
using TranslatorLibrary;

namespace MisakaTranslator_WPF.SettingsPages.TranslatorPages
{
    /// <summary>
    /// ChatGPTTransSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class ChatGPTTransSettingsPage : Page
    {
        public ChatGPTTransSettingsPage()
        {
            InitializeComponent();
            ChatGPTTransSecretKeyBox.Text = Common.appSettings.ChatGPTapiKey;
            ChatGPTTransUrlBox.Text = Common.appSettings.ChatGPTapiUrl;
        }

        private async void AuthTestBtn_Click(object sender, RoutedEventArgs e)
        {
            Common.appSettings.ChatGPTapiKey = ChatGPTTransSecretKeyBox.Text;
            Common.appSettings.ChatGPTapiUrl = ChatGPTTransUrlBox.Text;

            ITranslator chatGPTTrans = new ChatGPTTranslator();
            chatGPTTrans.TranslatorInit(ChatGPTTransSecretKeyBox.Text, ChatGPTTransUrlBox.Text);

            if (await chatGPTTrans.TranslateAsync("apple", "zh", "en") != null)
            {
                HandyControl.Controls.Growl.Success($"ChatGPT {Application.Current.Resources["APITest_Success_Hint"]}");
            }
            else
            {
                HandyControl.Controls.Growl.Error($"ChatGPT {Application.Current.Resources["APITest_Error_Hint"]}\n{chatGPTTrans.GetLastError()}");
            }
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(ChatGPTTranslator.SIGN_UP_URL);
        }

        private void DocBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(ChatGPTTranslator.DOCUMENT_URL);
        }

        private void BillBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(ChatGPTTranslator.BILL_URL);
        }

        private async void TransTestBtn_Click(object sender, RoutedEventArgs e)
        {
            ITranslator chatGPTTrans = new ChatGPTTranslator();
            chatGPTTrans.TranslatorInit(ChatGPTTransSecretKeyBox.Text, ChatGPTTransUrlBox.Text);
            string res = await chatGPTTrans.TranslateAsync(TestSrcText.Text, TestDstLang.Text, TestSrcLang.Text);

            if (res != null)
            {
                HandyControl.Controls.MessageBox.Show(res, Application.Current.Resources["MessageBox_Result"].ToString());
            }
            else
            {
                HandyControl.Controls.Growl.Error(
                    $"ChatGPT {Application.Current.Resources["APITest_Error_Hint"]}\n{chatGPTTrans.GetLastError()}");
            }
        }
    }
}