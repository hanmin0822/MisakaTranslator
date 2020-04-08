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

namespace MisakaTranslator_WPF.GuidePages
{
    /// <summary>
    /// CompletationPage.xaml 的交互逻辑
    /// </summary>
    public partial class CompletationPage : Page
    {
        public CompletationPage()
        {
            InitializeComponent();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            //使用路由事件机制通知窗口来完成下一步操作
            PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
            args.XamlPath = "1";
            this.RaiseEvent(args);
        }
    }
}
