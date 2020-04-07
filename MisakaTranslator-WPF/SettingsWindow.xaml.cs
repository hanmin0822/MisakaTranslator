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
using System.Windows.Shapes;

namespace MisakaTranslator_WPF
{
    /// <summary>
    /// SettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

        }

        private void Item_About_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("res/SettingsPages/AboutPage.xaml", UriKind.Relative));
        }

        private void Item_TransGeneral_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("SettingsPages/TranslatorPages/TranslatorGeneralSettingsPage.xaml", UriKind.Relative));
        }

        private void Item_BaiduTrans_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("res/SettingsPages/TranslatorPages/BaiduTransSettingsPage.xaml", UriKind.Relative));
        }

        private void Item_FYJTrans_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("res/SettingsPages/TranslatorPages/TencentFYJTransSettingsPage.xaml", UriKind.Relative));
        }

        private void Item_TXOTrans_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("res/SettingsPages/TranslatorPages/TencentOldTransSettingsPage.xaml", UriKind.Relative));
        }

        private void Item_Caiyun_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("res/SettingsPages/TranslatorPages/CaiyunTransSettingsPage.xaml", UriKind.Relative));
        }

        private void Item_JBeijing_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("res/SettingsPages/TranslatorPages/JbeijingTransSettingsPage.xaml", UriKind.Relative));
        }

        private void Item_BaiduOCR_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("res/SettingsPages/OCRPages/BaiduOCRSettingsPage.xaml", UriKind.Relative));
        }
        
        private void Item_OCRGeneral_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("SettingsPages/OCRPages/OCRGeneralSettingsPage.xaml", UriKind.Relative));
        }

        private void Item_HookSettings_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("res/SettingsPages/HookSettingsPage.xaml", UriKind.Relative));
        }
        
    }
}
