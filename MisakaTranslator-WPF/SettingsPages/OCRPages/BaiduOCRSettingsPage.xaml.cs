﻿using System;
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
using OCRLibrary;

namespace MisakaTranslator_WPF.SettingsPages.OCRPages
{
    /// <summary>
    /// BaiduOCRPage.xaml 的交互逻辑
    /// </summary>
    public partial class BaiduOCRPage : Page
    {
        public BaiduOCRPage()
        {
            InitializeComponent();
            APIKEYBox.Text = Common.appSettings.BDOCR_APIKEY;
            SecretKeyBox.Text = Common.appSettings.BDOCR_SecretKey;
        }

        private void AuthTestBtn_Click(object sender, RoutedEventArgs e)
        {
            Common.appSettings.BDOCR_APIKEY = APIKEYBox.Text;
            Common.appSettings.BDOCR_SecretKey = SecretKeyBox.Text;

            if(APIKEYBox.Text.Length == 17)
            {
                HandyControl.Controls.Growl.Error($"百度OCR {Application.Current.Resources["APITest_Error_Hint"]}\nDo not use fanyi.baidu.com endpoint.");
                return;
            }

            BaiduGeneralOCR bgocr = new BaiduGeneralOCR();

            bool ret = bgocr.OCR_Init(APIKEYBox.Text, SecretKeyBox.Text);

            if (ret == true)
            {
                HandyControl.Controls.Growl.Success($"百度OCR {Application.Current.Resources["APITest_Success_Hint"]}");
            }
            else
            {
                HandyControl.Controls.Growl.Error($"百度OCR {Application.Current.Resources["APITest_Error_Hint"]}\n{bgocr.GetLastError()}");
            }
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(BaiduGeneralOCR.GetUrl_allpyAPI());
        }

        private void DocBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(BaiduGeneralOCR.GetUrl_Doc());
        }

        private void BillBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(BaiduGeneralOCR.GetUrl_bill());
        }
    }
}