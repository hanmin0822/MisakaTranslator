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
            Common.repairSettings = new ConfigurationBuilder<IRepeatRepairSettings>().UseIniFile(Environment.CurrentDirectory + "\\settings\\RepairSettings.ini").Build();


            ISettings settings = new ConfigurationBuilder<ISettings>().UseJsonFile("settings/settings.json").Build();
            this.Resources["Foreground"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(settings.ForegroundHex));
        }

        private static SettingsWindow _settingsWindow;

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_settingsWindow == null || _settingsWindow.IsVisible == false)
            {
                _settingsWindow = new SettingsWindow();
                _settingsWindow.Show();
            }
            else
            {
                _settingsWindow.WindowState = WindowState.Normal;
                _settingsWindow.Activate();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameGuideWindow ggw = new GameGuideWindow(2);
            ggw.Show();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TranslateWindow translateWindow = new TranslateWindow();
            translateWindow.Show();
            
        }
    }
}
