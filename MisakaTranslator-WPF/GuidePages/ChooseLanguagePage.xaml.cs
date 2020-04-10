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
using TranslatorLibrary;

namespace MisakaTranslator_WPF.GuidePages
{
    /// <summary>
    /// ChooseLanguagePage.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseLanguagePage : Page
    {
        private List<string> langlist;

        public ChooseLanguagePage()
        {
            InitializeComponent();

            langlist = CommonFunction.lstLanguage.Keys.ToList();
            SrcLangCombox.ItemsSource = langlist;
            DstLangCombox.ItemsSource = langlist;

            SrcLangCombox.SelectedIndex = 2;
            DstLangCombox.SelectedIndex = 0;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SrcLangCombox.SelectedIndex == DstLangCombox.SelectedIndex)
            {
                HandyControl.Controls.Growl.Error("两种语言不能相同！");
            }
            else {
                Common.UsingSrcLang = CommonFunction.lstLanguage[langlist[SrcLangCombox.SelectedIndex]];
                Common.UsingDstLang = CommonFunction.lstLanguage[langlist[DstLangCombox.SelectedIndex]];

                //写数据库信息
                if(Common.GameID != -1)
                {
                    SQLHelper sqliteH = new SQLHelper();
                    sqliteH.ExecuteSql(string.Format("UPDATE game_library SET src_lang = '{0}' WHERE gameid = {1};",
                            Common.UsingSrcLang, Common.GameID));
                    sqliteH.ExecuteSql(string.Format("UPDATE game_library SET dst_lang = '{0}' WHERE gameid = {1};",
                            Common.UsingDstLang, Common.GameID));
                }

                //使用路由事件机制通知窗口来完成下一步操作
                PageChangeRoutedEventArgs args = new PageChangeRoutedEventArgs(PageChange.PageChangeRoutedEvent, this);
                args.XamlPath = "GuidePages/CompletationPage.xaml";
                this.RaiseEvent(args);
            }
        }
    }
}
