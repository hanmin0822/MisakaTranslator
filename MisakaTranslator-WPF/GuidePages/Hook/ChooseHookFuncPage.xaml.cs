using SQLHelperLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MisakaTranslator_WPF.GuidePages.Hook
{
    /// <summary>
    /// ChooseHookFuncPage.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseHookFuncPage : Page
    {
        BindingList<TextHookData> lstData = new BindingList<TextHookData>();

        string LastCustomHookCode;

        int sum = 0;

        public ChooseHookFuncPage()
        {
            InitializeComponent();

            LastCustomHookCode = "NULL";

            HookFunListView.ItemsSource = lstData;
            sum = 0;
            Common.textHooker.HFSevent += DataRecvEventHandler;
            Common.textHooker.StartHook(Convert.ToBoolean(Common.appSettings.AutoHook));
        }

        public void DataRecvEventHandler(object sender, HookSelectRecvEventArgs e) {

            //加一步判断防止卡顿，部分不可能使用的方法刷新速度过快，在几秒之内就能刷新超过100个，这时候就停止对他们的刷新,直接卸载这个方法
            
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (e.Index < sum)
                {
                    lstData[e.Index] = e.Data;
                }
                else {
                    lstData.Add(e.Data);
                    sum++;
                }
            }), System.Windows.Threading.DispatcherPriority.DataBind);
            
            
        }

        private void AddHookBtn_Click(object sender, RoutedEventArgs e)
        {
            InputDrawer.IsOpen = true;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (HookFunListView.SelectedIndex != -1)
            {
                string hookAdd = lstData[HookFunListView.SelectedIndex].HookAddress;
                int pid = lstData[HookFunListView.SelectedIndex].GamePID;

                //先关闭对本窗口的输出
                Common.textHooker.HFSevent -= DataRecvEventHandler;

                //先要将需要用到的方法注明，再进行后续卸载操作
                Common.textHooker.HookCodeList.Add(lstData[HookFunListView.SelectedIndex].HookCode);
                Common.textHooker.MisakaCodeList.Add(lstData[HookFunListView.SelectedIndex].MisakaHookCode);

                List<string> usedHook = new List<string>();
                usedHook.Add(hookAdd);

                //用户开启了自动卸载
                if (Convert.ToBoolean(Common.appSettings.AutoDetach) == true) {
                    Common.textHooker.DetachUnrelatedHooks(pid, usedHook);
                }

                int sum = 0;
                for (int i = 0; i < lstData.Count; i++)
                {
                    if (lstData[i].HookCode == lstData[HookFunListView.SelectedIndex].HookCode)
                    {
                        sum++;
                    }

                    if (sum >= 2)
                    {
                        //往数据库写信息，下一次游戏启动需要重新选方法
                        if (Common.GameID != -1)
                        {
                            GameLibraryHelper.sqlHelper.ExecuteSql(
                                $"UPDATE game_library SET isMultiHook = '{"True"}' WHERE gameid = {Common.GameID};");
                        }

                        break;
                    }
                }

                //不满足的游戏也应该记录一下
                if (sum <= 1)
                {
                    if (Common.GameID != -1)
                    {
                        GameLibraryHelper.sqlHelper.ExecuteSql(
                            $"UPDATE game_library SET isMultiHook = '{"False"}' WHERE gameid = {Common.GameID};");
                    }
                }

                if (Common.GameID != -1)
                {
                    GameLibraryHelper.sqlHelper.ExecuteSql($"UPDATE game_library SET transmode = {"1"} WHERE gameid = {Common.GameID};");
                    GameLibraryHelper.sqlHelper.ExecuteSql(
                        $"UPDATE game_library SET hookcode = '{lstData[HookFunListView.SelectedIndex].HookCode}' WHERE gameid = {Common.GameID};");

                    if (LastCustomHookCode != "NULL")
                    {
                        MessageBoxResult result = HandyControl.Controls.MessageBox.Show(
                            Application.Current.Resources["ChooseHookFuncPage_MBOX_hookcodeConfirm_left"] + "\n" + LastCustomHookCode + "\n" + Application.Current.Resources["ChooseHookFuncPage_MBOX_hookcodeConfirm_right"],
                            Application.Current.Resources["MessageBox_Ask"].ToString(),
                            MessageBoxButton.YesNoCancel,
                            MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            //记录这个特殊码到数据库
                            GameLibraryHelper.sqlHelper.ExecuteSql(
                                $"UPDATE game_library SET hookcode_custom = '{LastCustomHookCode}' WHERE gameid = {Common.GameID};");
                        }
                        else if (result == MessageBoxResult.No)
                        {
                            //返回界面，否则会自动进入下一个界面
                            return;
                        } else{
                            //不记录特殊码，但也要写NULL
                            GameLibraryHelper.sqlHelper.ExecuteSql(
                                $"UPDATE game_library SET hookcode_custom = '{"NULL"}' WHERE gameid = {Common.GameID};");

                        }
                    }
                    else {
                        GameLibraryHelper.sqlHelper.ExecuteSql(
                            $"UPDATE game_library SET hookcode_custom = '{"NULL"}' WHERE gameid = {Common.GameID};");
                    }

                }

                //使用路由事件机制通知窗口来完成下一步操作
                PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this)
                {
                    XamlPath = "GuidePages/Hook/ChooseTextRepairFuncPage.xaml"
                };
                this.RaiseEvent(args);
            }
            else {
                HandyControl.Controls.Growl.Error(Application.Current.Resources["ChooseHookFuncPage_NextErrorHint"].ToString());
            }


        }

        private void HookCodeConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PIDTextBox.Text != "" && HookCodeTextBox.Text != "" && int.TryParse(PIDTextBox.Text, out int pid))
            {
                Common.textHooker.AttachProcessByHookCode(pid, HookCodeTextBox.Text);
                LastCustomHookCode = HookCodeTextBox.Text;
                InputDrawer.IsOpen = false;
                HandyControl.Controls.Growl.Info(Application.Current.Resources["ChooseHookFuncPage_HookApplyHint"].ToString());
            }
            else {
                HandyControl.Controls.MessageBox.Show(Application.Current.Resources["ChooseHookFuncPage_HookApplyErrorHint"].ToString(), Application.Current.Resources["MessageBox_Error"].ToString());
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            InputDrawer.IsOpen = false;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) {
                Common.textHooker.HFSevent -= DataRecvEventHandler;
                HandyControl.Controls.Growl.Warning(Application.Current.Resources["ChooseHookFuncPage_PauseHint"].ToString());
            }
            
        }

        private void CannotfindHookBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/hanmin0822/MisakaHookFinder");
        }
    }
}
