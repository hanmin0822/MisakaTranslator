using Config.Net;
using HandyControl.Controls;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MisakaTranslator_WPF
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeAppearance();
        }

        private void InitializeAppearance()
        {
            Common.appSettings = new ConfigurationBuilder<IAppSettings>().UseIniFile(Environment.CurrentDirectory + "\\settings\\settings.ini").Build();

            ISettings settings = new ConfigurationBuilder<ISettings>().UseJsonFile("settings/settings.json").Build();
            this.Resources["Foreground"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(settings.ForegroundHex));
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameGuideWindow ggw = new GameGuideWindow(1);
            ggw.Show();

        }
    }
}
