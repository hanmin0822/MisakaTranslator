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
        public GameGuideWindow(int Mode)
        {
            InitializeComponent();

            this.AddHandler(GuidePages.PageChange.PageChangeRoutedEvent,new RoutedEventHandler(Next_Click));

            if (Mode == 1)
            {
                //Hook模式
                List<string> lstStep = new List<string>() { "选择游戏", "选择Hook方法", "选择去重方法", "选择翻译语言", "完成" };
                GuideStepBar.ItemsSource = lstStep;
                FuncHint.Text = "使用TextHook方式提取文本";
                GuidePageFrame.Navigate(new Uri("GuidePages/Hook/ChooseGamePage.xaml", UriKind.Relative));
            }
            else if (Mode == 2)
            {
                //OCR模式
                List<string> lstStep = new List<string>() { "选择截图区域", "OCR预处理", "设置触发键", "选择翻译语言", "完成" };
                GuideStepBar.ItemsSource = lstStep;
                FuncHint.Text = "使用OCR方式提取文本";
                GuidePageFrame.Navigate(new Uri("GuidePages/OCR/ChooseOCRAreaPage.xaml", UriKind.Relative));
            }
            else if(Mode == 3)
            {
                //重新选择Hook方法
                List<string> lstStep = new List<string>() { "重新选择Hook方法", "完成" };
                GuideStepBar.ItemsSource = lstStep;
                FuncHint.Text = "重新选择Hook方法以适配上次的设置";
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
                //Hook方式设置 完成
                this.Close();
            }
            else if (args.XamlPath == "2") {
                //OCR方式设置 完成
                this.Close();
            }
            else {
                //其他情况就跳转指定页面
                GuidePageFrame.Navigate(new Uri(args.XamlPath, UriKind.Relative));
                GuideStepBar.Next();
            }
        }

    }
}
