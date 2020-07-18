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

namespace MisakaTranslator_WPF.UserControls
{
    /// <summary>
    /// SelectTransLangDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTransLangDialog : UserControl
    {
        private readonly List<string> _langList;
        ComicTranslator.ComicTransMainWindow _win;

        public SelectTransLangDialog(ComicTranslator.ComicTransMainWindow win)
        {
            InitializeComponent();

            _win = win;

            _langList = CommonFunction.lstLanguage.Keys.ToList();
            SrcLangCombox.ItemsSource = _langList;
            DstLangCombox.ItemsSource = _langList;

            SrcLangCombox.SelectedIndex = 2;
            DstLangCombox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _win.SrcLang = CommonFunction.lstLanguage[_langList[SrcLangCombox.SelectedIndex]];
            _win.DstLang = CommonFunction.lstLanguage[_langList[DstLangCombox.SelectedIndex]];

            if (_win.SrcLang == "" || _win.DstLang == "" || _win.SrcLang == _win.DstLang)
            {
                HandyControl.Controls.Growl.ErrorGlobal(Application.Current.Resources["ChooseLanguagePage_NextErrorHint"].ToString());
                _win.Close();
            }
        }
    }
}
