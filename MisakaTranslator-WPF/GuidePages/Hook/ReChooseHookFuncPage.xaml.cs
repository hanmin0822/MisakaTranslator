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
    /// ReChooseHookFuncPage.xaml 的交互逻辑
    /// </summary>
    public partial class ReChooseHookFuncPage : Page
    {
        BindingList<TextHookData> lstData = new BindingList<TextHookData>();
        int sum = 0;

        public ReChooseHookFuncPage()
        {
            InitializeComponent();
            HookFunListView.ItemsSource = lstData;
            sum = 0;
            Common.textHooker.HFRSevent += DataRecvEventHandler;
            Common.textHooker.StartHook(Convert.ToBoolean(Common.appSettings.AutoHook));
        }

        public void DataRecvEventHandler(object sender, HookSelectRecvEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (e.Index < sum)
                {
                    lstData[e.Index] = e.Data;
                }
                else
                {
                    lstData.Add(e.Data);
                    sum++;
                }
            }));
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (HookFunListView.SelectedIndex != -1)
            {
                //先关闭对本窗口的输出
                Common.textHooker.HFRSevent -= DataRecvEventHandler;
                
                Common.textHooker.MisakaCodeList.Add(lstData[HookFunListView.SelectedIndex].MisakaHookCode);

                Common.textHooker.DetachUnrelatedHookWhenDataRecv = Convert.ToBoolean(Common.appSettings.AutoDetach);

                //使用路由事件机制通知窗口来完成下一步操作
                PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
                args.XamlPath = "GuidePages/CompletationPage.xaml";
                this.RaiseEvent(args);
            }
                
        }
    }
}
