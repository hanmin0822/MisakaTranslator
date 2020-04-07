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

namespace MisakaTranslator_WPF.SettingsPages
{
    /// <summary>
    /// OCRGeneralSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class OCRGeneralSettingsPage : Page
    {
        private List<string> lstOCR = new List<string>() {
            "BaiduOCR",
            "TesseractOCR"
        };

        public OCRGeneralSettingsPage()
        {
            InitializeComponent();
            OCRsourceCombox.ItemsSource = lstOCR;

            OCRsourceCombox.SelectedValue = Common.appSettings.OCRsource;
        }

        private void OCRsourceCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Common.appSettings.OCRsource = (string)OCRsourceCombox.SelectedValue;
        }
    }
}
