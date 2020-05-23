
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
    /// TransWinSettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TransWinSettingsWindow : Window
    {
        TranslateWindow translateWin;

        List<string> FontList;

        public TransWinSettingsWindow(TranslateWindow Win)
        {
            translateWin = Win;
            
            InitializeComponent();

            FontList = new List<string>();

            System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily family in fonts.Families)
            {
                FontList.Add(family.Name);
            }

            sourceFont.ItemsSource = FontList;
            firstFont.ItemsSource = FontList;
            secondFont.ItemsSource = FontList;

            EventInit();

            UI_Init();


        }

        /// <summary>
        /// 事件初始化
        /// </summary>
        private void EventInit() {
            sourceFont.SelectionChanged += delegate {
                translateWin.SourceTextFont = FontList[sourceFont.SelectedIndex];
                Common.appSettings.TF_srcTextFont = FontList[sourceFont.SelectedIndex];
            };

            firstFont.SelectionChanged += delegate
            {
                translateWin.FirstTransText.FontFamily = new FontFamily(FontList[firstFont.SelectedIndex]);
                Common.appSettings.TF_firstTransTextFont = FontList[firstFont.SelectedIndex];
            };

            secondFont.SelectionChanged += delegate
            {
                translateWin.SecondTransText.FontFamily = new FontFamily(FontList[secondFont.SelectedIndex]);
                Common.appSettings.TF_secondTransTextFont = FontList[secondFont.SelectedIndex];
            };

            sourceFontSize.ValueChanged += delegate {
                translateWin.SourceTextFontSize = (int)sourceFontSize.Value;
                Common.appSettings.TF_srcTextSize = Convert.ToString(sourceFontSize.Value);
            };

            firstFontSize.ValueChanged += delegate {
                translateWin.FirstTransText.FontSize = firstFontSize.Value;
                Common.appSettings.TF_firstTransTextSize = Convert.ToString(firstFontSize.Value);
            };

            secondFontSize.ValueChanged += delegate {
                translateWin.SecondTransText.FontSize = secondFontSize.Value;
                Common.appSettings.TF_secondTransTextSize = Convert.ToString(secondFontSize.Value);
            };

            OpacityBar.ValueChanged += delegate {
                translateWin.BackWinChrome.Opacity = OpacityBar.Value / 100;
                Common.appSettings.TF_Opacity = Convert.ToString(OpacityBar.Value);
            };
        }

        /// <summary>
        /// UI初始化
        /// </summary>
        private void UI_Init() {
            BrushConverter brushConverter = new BrushConverter();
            BgColorBlock.Background = (Brush)brushConverter.ConvertFromString(Common.appSettings.TF_BackColor);
            firstColorBlock.Background = (Brush)brushConverter.ConvertFromString(Common.appSettings.TF_firstTransTextColor);
            secondColorBlock.Background = (Brush)brushConverter.ConvertFromString(Common.appSettings.TF_secondTransTextColor);

            for (int i = 0;i < FontList.Count;i++) {
                if (Common.appSettings.TF_srcTextFont == FontList[i]) {
                    sourceFont.SelectedIndex = i;
                }

                if (Common.appSettings.TF_firstTransTextFont == FontList[i])
                {
                    firstFont.SelectedIndex = i;
                }

                if (Common.appSettings.TF_secondTransTextFont == FontList[i])
                {
                    secondFont.SelectedIndex = i;
                }
            }

            sourceFontSize.Value = int.Parse(Common.appSettings.TF_srcTextSize);
            firstFontSize.Value = int.Parse(Common.appSettings.TF_firstTransTextSize);
            secondFontSize.Value = int.Parse(Common.appSettings.TF_secondTransTextSize);

            OpacityBar.Value = double.Parse(Common.appSettings.TF_Opacity);
        }

        private void ChooseColorBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = HandyControl.Tools.SingleOpenHelper.CreateControl<HandyControl.Controls.ColorPicker>();
            var window = new HandyControl.Controls.PopupWindow
            {
                PopupElement = picker,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                MinWidth = 0,
                MinHeight = 0,
                Title = "选择颜色"
            };
            picker.SelectedColorChanged += delegate {
                if (sender == BgColorBtn){
                    BgColorBlock.Background = picker.SelectedBrush;
                    translateWin.BackWinChrome.Background = picker.SelectedBrush;
                    Common.appSettings.TF_BackColor = picker.SelectedBrush.ToString();
                }
                else if (sender == firstColorBtn) {
                    firstColorBlock.Background = picker.SelectedBrush;
                    translateWin.FirstTransText.Foreground = picker.SelectedBrush;
                    Common.appSettings.TF_firstTransTextColor = picker.SelectedBrush.ToString();
                } else if (sender == secondColorBtn) {
                    secondColorBlock.Background = picker.SelectedBrush;
                    translateWin.SecondTransText.Foreground = picker.SelectedBrush;
                    Common.appSettings.TF_secondTransTextColor = picker.SelectedBrush.ToString();
                }
                window.Close();
            };
            picker.Canceled += delegate { window.Close(); };
            window.Show();
        }

        
    }
}
