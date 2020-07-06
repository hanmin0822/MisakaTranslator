using ArtificialTransHelperLibrary;
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
    /// ArtificialTransAddWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ArtificialTransAddWindow : Window
    {
        private string secondTransRes;

        public ArtificialTransAddWindow(string src,string trans,string secondTrans)
        {
            InitializeComponent();

            srcText.Text = src;
            transText.Text = trans;

            secondTransRes = secondTrans;

            this.Topmost = true;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            ArtificialTransHelper ath = new ArtificialTransHelper(Convert.ToString(Common.GameID));

            bool res = ath.UpdateTrans(srcText.Text,transText.Text);
            if (res)
            {
                HandyControl.Controls.Growl.InfoGlobal(Application.Current.Resources["ArtificialTransWin_Success_Hint"].ToString());
            }
            else
            {
                HandyControl.Controls.Growl.ErrorGlobal(Application.Current.Resources["ArtificialTransWin_Error_Hint"].ToString());
            }
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            transText.Text = "";
        }

        private void SecondTransBtn_Click(object sender, RoutedEventArgs e)
        {
            transText.Text = secondTransRes;
        }
    }
}
