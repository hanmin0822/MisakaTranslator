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

namespace MisakaTranslator_WPF.SettingsPages
{
    /// <summary>
    /// TranslatorGeneralSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class TranslatorGeneralSettingsPage : Page
    {
        private List<string> TranslatorList;
            
        public TranslatorGeneralSettingsPage()
        {
            InitializeComponent();
            TranslatorList = CommonFunction.GetTranslatorList();
            FirstTransCombox.ItemsSource = TranslatorList;
            SecondTransCombox.ItemsSource = TranslatorList;

            FirstTransCombox.SelectedIndex = CommonFunction.GetTranslatorIndex(Common.appSettings.FirstTranslator);
            SecondTransCombox.SelectedIndex = CommonFunction.GetTranslatorIndex(Common.appSettings.SecondTranslator);

            EachRowTransCheckBox.IsChecked = Convert.ToBoolean(Common.appSettings.EachRowTrans);
        }

        private void FirstTransCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Common.appSettings.FirstTranslator = CommonFunction.lstTranslator[(string)FirstTransCombox.SelectedValue];
        }

        private void SecondTransCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Common.appSettings.SecondTranslator = CommonFunction.lstTranslator[(string)SecondTransCombox.SelectedValue];
        }
        
        private void EachRowTransCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Common.appSettings.EachRowTrans = Convert.ToString(EachRowTransCheckBox.IsChecked);
        }
    }
}
