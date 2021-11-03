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
    /// SoftwareSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SoftwareSettingsPage : Page
    {
        public SoftwareSettingsPage()
        {
            InitializeComponent();

            var appSettingsOnClickCloseButton = Common.appSettings.OnClickCloseButton;
            switch (appSettingsOnClickCloseButton)
            {
                case "Minimization":
                    MinimizationRadioButton.IsChecked = true;
                    break;
                case "Exit":
                    ExitRadioButton.IsChecked = true;
                    break;
            }

            GrowlEnabledCheckBox.IsChecked = Common.appSettings.GrowlEnabled;
        }

        private void RadioButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            switch (radioButton.Name)
            {
                case "MinimizationRadioButton":
                    Common.appSettings.OnClickCloseButton = "Minimization";
                    break;
                case "ExitRadioButton":
                    Common.appSettings.OnClickCloseButton = "Exit";
                    break;
            }
        }

        private void GrowlEnabledCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Common.appSettings.GrowlEnabled = GrowlEnabledCheckBox.IsChecked.Value;
        }
    }
}