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

            EachRowTransCheckBox.IsChecked = Common.appSettings.EachRowTrans;
            HttpProxyBox.Text = Common.appSettings.HttpProxy;

            TransLimitBox.Value = Common.appSettings.TransLimitNums;
            // 给TransLimitBox添加Minimum后，初始化它时就会触发一次ValueChanged，导致Settings被设为1，因此只能从设置中读取数据后再添加事件处理函数
            TransLimitBox.ValueChanged += TransLimitBox_ValueChanged;
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
            Common.appSettings.EachRowTrans = EachRowTransCheckBox.IsChecked ?? false;
        }

        private void HttpProxyBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = HttpProxyBox.Text.Trim();
            try { new Uri(text); }
            catch (UriFormatException) { HandyControl.Controls.Growl.Error("Proxy url unsupported."); return; };
            Common.appSettings.HttpProxy = text;
        }

        private void TransLimitBox_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            Common.appSettings.TransLimitNums = (int)TransLimitBox.Value;
        }
    }
}
