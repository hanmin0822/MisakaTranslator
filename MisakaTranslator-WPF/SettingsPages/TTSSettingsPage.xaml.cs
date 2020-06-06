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
using TTSHelperLibrary;

namespace MisakaTranslator_WPF.SettingsPages
{
    /// <summary>
    /// TTSSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class TTSSettingsPage : Page
    {
        TextSpeechHelper tsh;

        public TTSSettingsPage()
        {
            tsh = new TextSpeechHelper();
            InitializeComponent();

            List<string> lst = tsh.GetAllTTSEngine();
            TTSSourceCombox.ItemsSource = lst;

            for (int i = 0;i < lst.Count;i++) {
                if (lst[i] == Common.appSettings.ttsVoice) {
                    TTSSourceCombox.SelectedIndex = i;
                    break;
                }
            }
            VolumeBar.Value = int.Parse(Common.appSettings.ttsVolume);
            RateBar.Value = int.Parse(Common.appSettings.ttsRate);
        }

        private void TTSSourceCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tsh.SetTTSVoice((string)TTSSourceCombox.SelectedValue);
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            tsh.SpeakAsync(TestSrcText.Text);
            Common.appSettings.ttsVoice = (string)TTSSourceCombox.SelectedValue;
            Common.appSettings.ttsVolume = ((int)VolumeBar.Value).ToString();
            Common.appSettings.ttsRate = ((int)RateBar.Value).ToString();
        }

        private void VolumeBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tsh.SetVolume((int)VolumeBar.Value);
        }

        private void RateBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tsh.SetRate((int)RateBar.Value);
        }
    }
}
