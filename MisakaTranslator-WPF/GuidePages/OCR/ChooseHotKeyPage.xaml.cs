using KeyboardMouseHookLibrary;
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

namespace MisakaTranslator_WPF.GuidePages.OCR
{
    /// <summary>
    /// ChooseHotKeyPage.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseHotKeyPage : Page
    {
        GlobalHook hook;
        List<string> lstHotKeySource = new List<string>()
        {
            "键盘","鼠标左键","鼠标右键"
        };

        HotKeyInfo HotKey;

        public ChooseHotKeyPage()
        {
            InitializeComponent();

            HotKeySourceCombox.ItemsSource = lstHotKeySource;
            HotKeySourceCombox.SelectedIndex = 0;

            HotKey = new HotKeyInfo();
        }

        /// <summary>
        /// 键盘点击事件
        /// </summary>
        void Hook_OnKeyBoardActivity(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            HotKey.IsMouse = false;
            HotKey.KeyCode = e.KeyCode;
            HotKeyTag.Text = "已设置触发键 键值:" + HotKey.KeyCode;
            hook.Stop();
            hook = null;
            WaitHotKeyDrawer.IsOpen = false;
        }

        private void HotKeySetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (HotKeySourceCombox.SelectedIndex == 0)
            {
                //初始化钩子对象
                if (hook == null)
                {
                    hook = new GlobalHook();
                    hook.KeyDown += new System.Windows.Forms.KeyEventHandler(Hook_OnKeyBoardActivity);
                }
            }

            bool r = hook.Start();
            if (r)
            {
                WaitHotKeyDrawer.IsOpen = true;
            }
            else
            {
                HandyControl.Controls.Growl.Error("安装钩子失败");
            }
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (HotKeyTag.Text == "")
            {
                HandyControl.Controls.Growl.Error("请设置一个触发键");
            }
            else if (int.Parse(OCRDelayBox.Text) <= 0)
            {
                HandyControl.Controls.Growl.Error("延时时间必须大于0毫秒");
            }
            else {
                Common.UsingHotKey = HotKey;
                Common.UsingOCRDelay = int.Parse(OCRDelayBox.Text);

                //存入数据库

                //使用路由事件机制通知窗口来完成下一步操作
                PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
                args.XamlPath = "GuidePages/ChooseLanguagePage.xaml";
                this.RaiseEvent(args);
            }
        }

        private void HotKeySourceCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (HotKeySourceCombox.SelectedIndex == 0)
            {
                ChooseHotkeyBtn.Visibility = Visibility.Visible;
            }
            else if (HotKeySourceCombox.SelectedIndex == 1)
            {
                HotKey.IsMouse = true;
                HotKey.MouseButton = System.Windows.Forms.MouseButtons.Left;
                HotKeyTag.Text = "已设置触发键 键值:鼠标左键";
                ChooseHotkeyBtn.Visibility = Visibility.Hidden;
            }
            else if (HotKeySourceCombox.SelectedIndex == 2)
            {
                HotKey.IsMouse = true;
                HotKey.MouseButton = System.Windows.Forms.MouseButtons.Right;
                HotKeyTag.Text = "已设置触发键 键值:鼠标右键";
                ChooseHotkeyBtn.Visibility = Visibility.Hidden;
            }
        }
    }
}
