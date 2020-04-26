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
        
        int sum = 0;

        public ChooseHookFuncPage()
        {
            InitializeComponent();

            

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
                SQLHelper sqliteH = new SQLHelper();

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
                            sqliteH.ExecuteSql(string.Format("UPDATE game_library SET isMultiHook = '{0}' WHERE gameid = {1};", "True", Common.GameID));
                        }

                        break;
                    }
                }

                //不满足的游戏也应该记录一下
                if (sum <= 1)
                {
                    if (Common.GameID != -1)
                    {
                        sqliteH.ExecuteSql(string.Format("UPDATE game_library SET isMultiHook = '{0}' WHERE gameid = {1};", "False", Common.GameID));
                    }
                }

                if (Common.GameID != -1)
                {
                    sqliteH.ExecuteSql(string.Format("UPDATE game_library SET transmode = {0} WHERE gameid = {1};", "1", Common.GameID));
                    sqliteH.ExecuteSql(string.Format("UPDATE game_library SET hookcode = '{0}' WHERE gameid = {1};", lstData[HookFunListView.SelectedIndex].HookCode, Common.GameID));
                }

                //使用路由事件机制通知窗口来完成下一步操作
                PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
                args.XamlPath = "GuidePages/Hook/ChooseTextRepairFuncPage.xaml";
                this.RaiseEvent(args);
            }
            else {
                HandyControl.Controls.Growl.Error("请选择一项Hook方法后进入下一步！");
            }


        }

        private void HookCodeConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PIDTextBox.Text != "" && HookCodeTextBox.Text != "")
            {
                Common.textHooker.AttachProcessByHookCode(int.Parse(PIDTextBox.Text), HookCodeTextBox.Text);
                InputDrawer.IsOpen = false;
                HandyControl.Controls.Growl.Info("已提交Hook申请，请重新确认！");
            }
            else {
                HandyControl.Controls.MessageBox.Show("请输入内容后再确认！","错误");
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
                HandyControl.Controls.Growl.Warning("已强制暂停刷新");
            }
        }
    }
}
