using SQLHelperLibrary;
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
using TextHookLibrary;
using TextRepairLibrary;

namespace MisakaTranslator_WPF.GuidePages.Hook
{
    /// <summary>
    /// ChooseTextRepairFuncPage.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseTextRepairFuncPage : Page
    {
        private List<string> lstRepairFun = TextRepair.lstRepairFun.Keys.ToList();

        public ChooseTextRepairFuncPage()
        {
            InitializeComponent();

            RepairFuncCombox.ItemsSource = lstRepairFun;
            RepairFuncCombox.SelectedIndex = 0;

            Common.textHooker.Sevent += DataRecvEventHandler;
        }

        public void DataRecvEventHandler(object sender, SolvedDataRecvEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                sourceTextBox.Text = e.Data.Data;
                repairedTextBox.Text = TextRepair.RepairFun_Auto(TextRepair.lstRepairFun[lstRepairFun[RepairFuncCombox.SelectedIndex]], sourceTextBox.Text);

            }));
        }

        private void RepairFuncCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (TextRepair.lstRepairFun[lstRepairFun[RepairFuncCombox.SelectedIndex]])
            {
                case "RepairFun_RemoveSingleWordRepeat":
                    Single_InputDrawer.IsOpen = true;
                    break;
                case "RepairFun_RemoveSentenceRepeat":
                    Sentence_InputDrawer.IsOpen = true;
                    break;
                case "RepairFun_RegexReplace":
                    Regex_InputDrawer.IsOpen = true;
                    break;
            }

            repairedTextBox.Text = TextRepair.RepairFun_Auto(TextRepair.lstRepairFun[lstRepairFun[RepairFuncCombox.SelectedIndex]],sourceTextBox.Text);
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            Common.textHooker.Sevent -= DataRecvEventHandler;

            Common.UsingRepairFunc = TextRepair.lstRepairFun[lstRepairFun[RepairFuncCombox.SelectedIndex]];

            //写入数据库的去重方法
            if (Common.GameID != -1)
            {
                switch (TextRepair.lstRepairFun[lstRepairFun[RepairFuncCombox.SelectedIndex]])
                {
                    case "RepairFun_RemoveSingleWordRepeat":
                        GameLibraryHelper.sqlHelper.ExecuteSql(
                            $"UPDATE game_library SET repair_func = '{Common.UsingRepairFunc}',repair_param_a = '{Common.repairSettings.SingleWordRepeatTimes}' WHERE gameid = {Common.GameID};");
                        break;
                    case "RepairFun_RemoveSentenceRepeat":
                        GameLibraryHelper.sqlHelper.ExecuteSql(
                            $"UPDATE game_library SET repair_func = '{Common.UsingRepairFunc}',repair_param_a = '{Common.repairSettings.SentenceRepeatFindCharNum}' WHERE gameid = {Common.GameID};");
                        break;
                    case "RepairFun_RegexReplace":
                        GameLibraryHelper.sqlHelper.ExecuteSql(
                            $"UPDATE game_library SET repair_func = '{Common.UsingRepairFunc}',repair_param_a = '{Common.repairSettings.Regex}',repair_param_b = '{Common.repairSettings.Regex_Replace}' WHERE gameid = {Common.GameID};");
                        break;
                    default:
                        GameLibraryHelper.sqlHelper.ExecuteSql(
                            $"UPDATE game_library SET repair_func = '{Common.UsingRepairFunc}' WHERE gameid = {Common.GameID};");
                        break;
                }

            }

            //使用路由事件机制通知窗口来完成下一步操作
            PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
            args.XamlPath = "GuidePages/ChooseLanguagePage.xaml";
            this.RaiseEvent(args);
        }

        private void SingleConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Single_TextBox.Text, out int times))
                return;
            Common.repairSettings.SingleWordRepeatTimes = times;
            Common.RepairFuncInit();
            repairedTextBox.Text = TextRepair.RepairFun_RemoveSingleWordRepeat(sourceTextBox.Text);
            Single_InputDrawer.IsOpen = false;
        }

        private void SentenceConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Sentence_TextBox.Text, out int num))
                return;
            Common.repairSettings.SentenceRepeatFindCharNum = num;
            Common.RepairFuncInit();
            repairedTextBox.Text = TextRepair.RepairFun_RemoveSentenceRepeat(sourceTextBox.Text);
            Sentence_InputDrawer.IsOpen = false;
        }

        private void RegexConfirm_Click(object sender, RoutedEventArgs e)
        {
            Common.repairSettings.Regex = Regex_TextBox.Text;
            Common.repairSettings.Regex_Replace = Replace_TextBox.Text;
            Common.RepairFuncInit();
            repairedTextBox.Text = TextRepair.RepairFun_RegexReplace(sourceTextBox.Text);
            Regex_InputDrawer.IsOpen = false;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Single_InputDrawer.IsOpen = false;
            Sentence_InputDrawer.IsOpen = false;
            Regex_InputDrawer.IsOpen = false;
        }
    }
}
