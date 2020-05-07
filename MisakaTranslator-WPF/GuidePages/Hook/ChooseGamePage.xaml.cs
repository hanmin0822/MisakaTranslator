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

namespace MisakaTranslator_WPF.GuidePages.Hook
{
    /// <summary>
    /// ChooseGamePage.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseGamePage : Page
    {
        private Dictionary<string, int> lstProcess;
        private int GamePid;
        private List<System.Diagnostics.Process> SameNameGameProcessList;

        public ChooseGamePage()
        {
            InitializeComponent();
            GamePid = -1;
            lstProcess = ProcessHelper.GetProcessList_Name_PID();
            GameProcessCombox.ItemsSource = lstProcess.Keys;
        }

        private void GameProcessCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GamePid = lstProcess[(string)GameProcessCombox.SelectedValue];
            SameNameGameProcessList = ProcessHelper.FindSameNameProcess(GamePid);
            AutoHookTag.Text = App.Current.Resources["ChooseGamePage_AutoHookTag_Begin"].ToString() + SameNameGameProcessList.Count + App.Current.Resources["ChooseGamePage_AutoHookTag_End"].ToString();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (GamePid != -1)
            {
                if (SameNameGameProcessList.Count == 1)
                {
                    Common.textHooker = new TextHookHandle(lstProcess[(string)GameProcessCombox.SelectedValue]);
                }
                else
                {
                    Common.textHooker = new TextHookHandle(SameNameGameProcessList);
                }

                Common.textHooker.Init(!(bool)x64GameCheckBox.IsChecked);

                
                Common.GameID = -1;
                string filepath = Common.FindProcessPath(GamePid);
                if (filepath != "") {
                    Common.GameID = GameLibraryHelper.GetGameID(filepath);
                }
                SQLHelper sqliteH = new SQLHelper();
                sqliteH.ExecuteSql(string.Format("UPDATE game_library SET isx64 = '{0}' WHERE gameid = {1};", x64GameCheckBox.IsChecked, Common.GameID));


                //使用路由事件机制通知窗口来完成下一步操作
                PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
                args.XamlPath = "GuidePages/Hook/ChooseHookFuncPage.xaml";
                this.RaiseEvent(args);
            }
            else {
                HandyControl.Controls.Growl.Error(App.Current.Resources["ChooseGamePage_NextErrorHint"].ToString());
            }
            
        }
    }
}
