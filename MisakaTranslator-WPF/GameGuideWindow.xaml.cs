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
using System.Windows.Shapes;

namespace MisakaTranslator_WPF
{
    /// <summary>
    /// GameGuideWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GameGuideWindow : Window
    {
        int GuideMode;

        public GameGuideWindow(int Mode)
        {
            InitializeComponent();

            this.AddHandler(GuidePages.PageChange.PageChangeRoutedEvent,new RoutedEventHandler(Next_Click));

            GuideMode = Mode;
            if (Mode == 1)
            {
                //Hook模式
                List<string> lstStep = new List<string>() {
                    Application.Current.Resources["GameGuideWin_Hook_Step_1"].ToString(),
                    Application.Current.Resources["GameGuideWin_Hook_Step_2"].ToString(),
                    Application.Current.Resources["GameGuideWin_Hook_Step_3"].ToString(),
                    Application.Current.Resources["GameGuideWin_Step_4"].ToString(),
                    Application.Current.Resources["GameGuideWin_Step_5"].ToString()
                };

                GuideStepBar.ItemsSource = lstStep;
                FuncHint.Text = Application.Current.Resources["GameGuideWin_FuncHint_Hook"].ToString();
                GuidePageFrame.Navigate(new Uri("GuidePages/Hook/ChooseGamePage.xaml", UriKind.Relative));
            }
            else if (Mode == 2)
            {
                //OCR模式
                List<string> lstStep = new List<string>() {
                    Application.Current.Resources["GameGuideWin_OCR_Step_1"].ToString(),
                    Application.Current.Resources["GameGuideWin_OCR_Step_2"].ToString(),
                    Application.Current.Resources["GameGuideWin_OCR_Step_3"].ToString(),
                    Application.Current.Resources["GameGuideWin_Step_4"].ToString(),
                    Application.Current.Resources["GameGuideWin_Step_5"].ToString()
                };

                GuideStepBar.ItemsSource = lstStep;
                FuncHint.Text = Application.Current.Resources["GameGuideWin_FuncHint_OCR"].ToString();
                GuidePageFrame.Navigate(new Uri("GuidePages/OCR/ChooseOCRAreaPage.xaml", UriKind.Relative));
            }
            else if (Mode == 3)
            {
                //重新选择Hook方法
                List<string> lstStep = new List<string>() {
                    Application.Current.Resources["GameGuideWin_ReHook_Step_1"].ToString(),
                    Application.Current.Resources["GameGuideWin_Step_5"].ToString()
                };

                GuideStepBar.ItemsSource = lstStep;
                FuncHint.Text = Application.Current.Resources["GameGuideWin_FuncHint_ReHook"].ToString();
                GuidePageFrame.Navigate(new Uri("GuidePages/Hook/ReChooseHookFuncPage.xaml", UriKind.Relative));
            }
            else if (Mode == 4) {
                //剪贴板监控
                List<string> lstStep = new List<string>() {
                    Application.Current.Resources["GameGuideWin_Hook_Step_3"].ToString(),
                    Application.Current.Resources["GameGuideWin_Step_4"].ToString(),
                    Application.Current.Resources["GameGuideWin_Step_5"].ToString()
                };

                GuideStepBar.ItemsSource = lstStep;
                FuncHint.Text = Application.Current.Resources["GameGuideWin_FuncHint_ClipBoard"].ToString();
                GuidePageFrame.Navigate(new Uri("GuidePages/Hook/ChooseTextRepairFuncPage.xaml", UriKind.Relative));
            }
        }

        /// <summary>
        /// 触发进入下一步的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_Click(object sender, RoutedEventArgs e) {
            GuidePages.PageChangeRoutedEventArgs args = e as GuidePages.PageChangeRoutedEventArgs;

            if (args.XamlPath == "1")
            {
                if (GuideMode == 1)
                {
                    //Hook方式设置 完成
                    Common.transMode = 1;
                    TranslateWindow translateWindow = new TranslateWindow();
                    translateWindow.Show();

                    this.Close();
                }
                else if (GuideMode == 2)
                {
                    //OCR方式设置 完成
                    Common.transMode = 2;
                    TranslateWindow translateWindow = new TranslateWindow();
                    translateWindow.Show();

                    this.Close();
                }
                else if (GuideMode == 3)
                {
                    //Hook方式设置 完成
                    Common.transMode = 1;
                    TranslateWindow translateWindow = new TranslateWindow();
                    translateWindow.Show();

                    this.Close();
                }
                else if (GuideMode == 4)
                {
                    //剪贴板监控方式设置 完成
                    Common.transMode = 1;
                    TranslateWindow translateWindow = new TranslateWindow();
                    translateWindow.Show();

                    this.Close();
                }
            }
            else {
                //其他情况就跳转指定页面
                GuidePageFrame.Navigate(new Uri(args.XamlPath, UriKind.Relative));
                GuideStepBar.Next();
            }

            
        }

    }
}
