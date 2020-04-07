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
            }));
        }

        private void AddHookBtn_Click(object sender, RoutedEventArgs e)
        {
            InputDrawer.IsOpen = true;
        }

        private async void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (HookFunListView.SelectedIndex != -1)
            {
                string hookAdd = lstData[HookFunListView.SelectedIndex].HookAddress;
                int pid = int.Parse(lstData[HookFunListView.SelectedIndex].GamePID, System.Globalization.NumberStyles.HexNumber);

                //先关闭对本窗口的输出
                Common.textHooker.HFSevent -= DataRecvEventHandler;

                //用户开启了自动卸载
                if (Convert.ToBoolean(Common.appSettings.AutoDetach) == true) {
                    await Common.textHooker.DetachProcessByHookAddress(pid,hookAdd);
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

                        break;
                    }
                }

                //不满足的游戏也应该记录一下
                if (sum <= 1)
                {
                    
                }
                
                Common.textHooker.HookCodeList.Add(lstData[HookFunListView.SelectedIndex].HookCode);
                Common.textHooker.MisakaCodeList.Add(lstData[HookFunListView.SelectedIndex].MisakaHookCode);

                //使用路由事件机制通知窗口来完成下一步操作
                PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
                args.XamlPath = "GuidePages/Hook/ChooseTextRepairFuncPage.xaml";
                this.RaiseEvent(args);
            }
            else {
                HandyControl.Controls.Growl.Error("请选择一项Hook方法后进入下一步！");
            }


        }

        private async void HookCodeConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PIDTextBox.Text != "" && HookCodeTextBox.Text != "")
            {
                await Common.textHooker.AttachProcessByHookCode(int.Parse(PIDTextBox.Text), HookCodeTextBox.Text);
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
    }
}
