using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using DictionaryHelperLibrary;
using FontAwesome.WPF;
using HandyControl.Controls;
using KeyboardMouseHookLibrary;
using MecabHelperLibrary;
using TextHookLibrary;
using TextRepairLibrary;
using TranslatorLibrary;
using TransOptimizationLibrary;
using TTSHelperLibrary;

namespace MisakaTranslator_WPF
{
    /// <summary>
    /// TranslateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TranslateWindow
    {
        public System.Windows.Threading.DispatcherTimer dtimer;//定时器 定时刷新位置
        //winAPI 设置窗口位置
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);



        private MecabHelper _mecabHelper;
        private BeforeTransHandle _beforeTransHandle;
        private AfterTransHandle _afterTransHandle;
        private ITranslator _translator1; //第一翻译源
        private ITranslator _translator2; //第二翻译源

        private IDict _dict;

        private string _currentsrcText; //当前源文本内容

        public string SourceTextFont; //源文本区域字体
        public int SourceTextFontSize; //源文本区域字体大小

        private Queue<string> _gameTextHistory; //历史文本
        public static KeyboardMouseHook hook; //全局键盘鼠标钩子
        public bool IsOCRingFlag; //线程锁:判断是否正在OCR线程中，保证同时只有一组在跑OCR
        public bool IsPauseFlag; //是否处在暂停状态（专用于OCR）,为真可以翻译

        private bool _isShowSource; //是否显示原文
        private bool _isLocked;

        private TextSpeechHelper _textSpeechHelper;//TTS朗读对象

        private IntPtr winHandle;//窗口句柄，用于设置活动窗口，以达到全屏状态下总在最前的目的

        public TranslateWindow()
        {
            InitializeComponent();

            _isShowSource = true;
            _isLocked = false;

            _gameTextHistory = new Queue<string>();

            this.Topmost = true;
            UI_Init();
            IsOCRingFlag = false;
            

            _mecabHelper = new MecabHelper();

            _textSpeechHelper = new TextSpeechHelper();
            if (Common.appSettings.ttsVoice == "")
            {
                Growl.InfoGlobal(Application.Current.Resources["TranslateWin_NoTTS_Hint"].ToString());
            }
            else {
                _textSpeechHelper.SetTTSVoice(Common.appSettings.ttsVoice);
                _textSpeechHelper.SetVolume(Common.appSettings.ttsVolume);
                _textSpeechHelper.SetRate(Common.appSettings.ttsRate);
            }
            
            if (Common.appSettings.xxgPath != string.Empty)
            {
                _dict = new XxgJpzhDict();
                _dict.DictInit(Common.appSettings.xxgPath, string.Empty);
            }

            IsPauseFlag = true;
            _translator1 = TranslatorAuto(Common.appSettings.FirstTranslator);
            _translator2 = TranslatorAuto(Common.appSettings.SecondTranslator);

            _beforeTransHandle = new BeforeTransHandle(Convert.ToString(Common.GameID), Common.UsingSrcLang, Common.UsingDstLang);
            _afterTransHandle = new AfterTransHandle(_beforeTransHandle);

            if (Common.transMode == 1)
            {
                Common.textHooker.Sevent += DataRecvEventHandler;
            }
            else if (Common.transMode == 2)
            {
                MouseKeyboardHook_Init();
            }

        }

        /// <summary>
        /// 键盘鼠标钩子初始化
        /// </summary>
        private void MouseKeyboardHook_Init()
        {
            if (hook == null)
            {
                hook = new KeyboardMouseHook();
                bool r = false;

                if (Common.UsingHotKey.IsMouse)
                {
                    hook.OnMouseActivity += Hook_OnMouseActivity;
                    if (Common.UsingHotKey.MouseButton == System.Windows.Forms.MouseButtons.Left) {
                        r = hook.Start(true, 1);
                    } else if (Common.UsingHotKey.MouseButton == System.Windows.Forms.MouseButtons.Right) {
                        r = hook.Start(true, 2);
                    }
                }
                else
                {
                    hook.onKeyboardActivity += Hook_OnKeyBoardActivity;
                    int keycode = (int)Common.UsingHotKey.KeyCode;
                    r = hook.Start(false, keycode);
                }

                if (!r)
                {
                    Growl.ErrorGlobal(Application.Current.Resources["Hook_Error_Hint"].ToString());
                }
            }

            
        }

        /// <summary>
        /// UI方面的初始化
        /// </summary>
        private void UI_Init()
        {
            SourceTextFontSize = (int)Common.appSettings.TF_srcTextSize;
            FirstTransText.FontSize = Common.appSettings.TF_firstTransTextSize;
            SecondTransText.FontSize = Common.appSettings.TF_secondTransTextSize;

            SourceTextFont = Common.appSettings.TF_srcTextFont;
            FirstTransText.FontFamily = new FontFamily(Common.appSettings.TF_firstTransTextFont);
            SecondTransText.FontFamily = new FontFamily(Common.appSettings.TF_secondTransTextFont);

            BrushConverter brushConverter = new BrushConverter();
            FirstTransText.Fill = (Brush)brushConverter.ConvertFromString(Common.appSettings.TF_firstTransTextColor);
            SecondTransText.Fill = (Brush)brushConverter.ConvertFromString(Common.appSettings.TF_secondTransTextColor);

            BackWinChrome.Background = (Brush)brushConverter.ConvertFromString(Common.appSettings.TF_BackColor);
            BackWinChrome.Opacity = Common.appSettings.TF_Opacity / 100;

            if (int.Parse(Common.appSettings.TF_LocX) != -1 && int.Parse(Common.appSettings.TF_SizeW) != 0)
            {
                this.Left = int.Parse(Common.appSettings.TF_LocX);
                this.Top = int.Parse(Common.appSettings.TF_LocY);
                this.Width = int.Parse(Common.appSettings.TF_SizeW);
                this.Height = int.Parse(Common.appSettings.TF_SizeH);
            }
        }

        /// <summary>
        /// 根据翻译器名称自动返回翻译器类实例(包括初始化)
        /// </summary>
        /// <param name="translator"></param>
        /// <returns></returns>
        public static ITranslator TranslatorAuto(string translator)
        {
            switch (translator)
            {
                case "BaiduTranslator":
                    BaiduTranslator bd = new BaiduTranslator();
                    bd.TranslatorInit(Common.appSettings.BDappID, Common.appSettings.BDsecretKey);
                    return bd;
                case "TencentFYJTranslator":
                    TencentFYJTranslator tx = new TencentFYJTranslator();
                    tx.TranslatorInit(Common.appSettings.TXappID, Common.appSettings.TXappKey);
                    return tx;
                case "TencentOldTranslator":
                    TencentOldTranslator txo = new TencentOldTranslator();
                    txo.TranslatorInit(Common.appSettings.TXOSecretId, Common.appSettings.TXOSecretKey);
                    return txo;
                case "CaiyunTranslator":
                    CaiyunTranslator cy = new CaiyunTranslator();
                    cy.TranslatorInit(Common.appSettings.CaiyunToken);
                    return cy;
                case "YoudaoTranslator":
                    YoudaoTranslator yd = new YoudaoTranslator();
                    yd.TranslatorInit();
                    return yd;
                case "AlapiTranslator":
                    AlapiTranslator al = new AlapiTranslator();
                    al.TranslatorInit();
                    return al;
                case "GoogleCNTranslator":
                    GoogleCNTranslator gct = new GoogleCNTranslator();
                    gct.TranslatorInit();
                    return gct;
                case "JBeijingTranslator":
                    JBeijingTranslator bj = new JBeijingTranslator();
                    bj.TranslatorInit(Common.appSettings.JBJCTDllPath);
                    return bj;
                case "KingsoftFastAITTranslator":
                    KingsoftFastAITTranslator kfat = new KingsoftFastAITTranslator();
                    kfat.TranslatorInit(Common.appSettings.KingsoftFastAITPath);
                    return kfat;
                case "Dreye":
                    DreyeTranslator drt = new DreyeTranslator();
                    drt.TranslatorInit(Common.appSettings.DreyePath);
                    return drt;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 键盘点击事件
        /// </summary>
        void Hook_OnKeyBoardActivity(object sender)
        {
            OCR();
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hook_OnMouseActivity(object sender, POINT e)
        {
            if (Common.isAllWindowCap && Process.GetCurrentProcess().Id != FindWindowInfo.GetProcessIDByHWND(FindWindowInfo.GetWindowHWND(e.x, e.y))
                || Common.OCRWinHwnd == (IntPtr)FindWindowInfo.GetWindowHWND(e.x, e.y))
            {
                OCR();
            }
        }

        private void OCR()
        {
            if (IsPauseFlag)
            {
                if (IsOCRingFlag == false)
                {
                    IsOCRingFlag = true;

                    int j = 0;

                    for (; j < 3; j++)
                    {

                        Thread.Sleep(Common.UsingOCRDelay);

                        string srcText = Common.ocr.OCRProcess();
                        GC.Collect();

                        if (!string.IsNullOrEmpty(srcText))
                        {
                            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                //0.清除面板
                                SourceTextPanel.Children.Clear();

                                //1.得到原句
                                string source = srcText;

                                _currentsrcText = source;

                                if (_isShowSource)
                                {
                                    //3.分词
                                    List<MecabWordInfo> mwi = _mecabHelper.SentenceHandle(source);
                                    //分词后结果显示
                                    for (int i = 0; i < mwi.Count; i++)
                                    {
                                        StackPanel stackPanel = new StackPanel();
                                        stackPanel.Orientation = Orientation.Vertical;
                                        stackPanel.Margin = new Thickness(10, 0, 0, 10);

                                        TextBlock textBlock = new TextBlock();
                                        if (!string.IsNullOrEmpty(SourceTextFont))
                                        {
                                            FontFamily fontFamily = new FontFamily(SourceTextFont);
                                            textBlock.FontFamily = fontFamily;
                                        }
                                        textBlock.Text = mwi[i].Word;
                                        textBlock.Tag = mwi[i].Kana;
                                        textBlock.Margin = new Thickness(0, 0, 0, 0);
                                        textBlock.FontSize = SourceTextFontSize;
                                        textBlock.Background = Brushes.Transparent;
                                        textBlock.MouseLeftButtonDown += DictArea_MouseLeftButtonDown;
                                        //根据不同词性跟字体上色
                                        switch (mwi[i].PartOfSpeech)
                                        {
                                            case "名詞":
                                                textBlock.Foreground = Brushes.AliceBlue;
                                                break;
                                            case "助詞":
                                                textBlock.Foreground = Brushes.LightGreen;
                                                break;
                                            case "動詞":
                                                textBlock.Foreground = Brushes.Red;
                                                break;
                                            case "連体詞":
                                                textBlock.Foreground = Brushes.Orange;
                                                break;
                                            default:
                                                textBlock.Foreground = Brushes.White;
                                                break;
                                        }


                                        TextBlock superScript = new TextBlock();//假名或注释等的上标标签
                                        if (!string.IsNullOrEmpty(SourceTextFont))
                                        {
                                            FontFamily fontFamily = new FontFamily(SourceTextFont);
                                            superScript.FontFamily = fontFamily;
                                        }
                                        superScript.Text = mwi[i].Kana;
                                        superScript.Margin = new Thickness(0, 0, 0, 2);
                                        superScript.HorizontalAlignment = HorizontalAlignment.Center;
                                        if ((double)SourceTextFontSize - 6.5 > 0)
                                        {
                                            superScript.FontSize = (double)SourceTextFontSize - 6.5;
                                        }
                                        else
                                        {
                                            superScript.FontSize = 1;
                                        }
                                        superScript.Background = Brushes.Transparent;
                                        superScript.Foreground = Brushes.White;
                                        stackPanel.Children.Add(superScript);


                                        //是否打开假名标注
                                        if (Common.appSettings.TF_isKanaShow)
                                        {
                                            stackPanel.Children.Add(textBlock);
                                            SourceTextPanel.Children.Add(stackPanel);
                                        }
                                        else
                                        {
                                            textBlock.Margin = new Thickness(10, 0, 0, 10);
                                            SourceTextPanel.Children.Add(textBlock);
                                        }
                                    }
                                }

                                if (Convert.ToBoolean(Common.appSettings.EachRowTrans) == false)
                                {
                                    //不需要分行翻译
                                    source = source.Replace("<br>", "").Replace("</br>", "").Replace("\n", "").Replace("\t", "").Replace("\r", "");
                                }
                                //去乱码
                                source = source.Replace("_", "").Replace("-", "").Replace("+", "");

                                //4.翻译前预处理
                                string beforeString = _beforeTransHandle.AutoHandle(source);

                                //5.提交翻译
                                string transRes1 = string.Empty;
                                string transRes2 = string.Empty;
                                if (_translator1 != null)
                                {
                                    transRes1 = _translator1.Translate(beforeString, Common.UsingDstLang, Common.UsingSrcLang);
                                }
                                if (_translator2 != null)
                                {
                                    transRes2 = _translator2.Translate(beforeString, Common.UsingDstLang, Common.UsingSrcLang);
                                }

                                //6.翻译后处理
                                string afterString1 = _afterTransHandle.AutoHandle(transRes1);
                                string afterString2 = _afterTransHandle.AutoHandle(transRes2);

                                //7.翻译结果显示到窗口上
                                FirstTransText.Text = afterString1;
                                SecondTransText.Text = afterString2;

                                //8.翻译结果记录到队列
                                if (_gameTextHistory.Count > 5)
                                {
                                    _gameTextHistory.Dequeue();
                                }
                                _gameTextHistory.Enqueue(source + "\n" + afterString1 + "\n" + afterString2);
                            }));

                            IsOCRingFlag = false;
                            break;
                        }
                    }

                    if (j == 3)
                    {
                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            FirstTransText.Text = "[OCR]自动识别三次均为空，请自行刷新！";
                        }));

                        IsOCRingFlag = false;
                    }
                }
            }

            
        }

        private void DictArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_dict != null)
            {
                if (e.ClickCount == 2)
                {
                    //双击事件
                    TextBlock textBlock = sender as TextBlock;

                    string ret = _dict.SearchInDict(textBlock.Text);
                    if (ret != null)
                    {
                        if (ret == string.Empty)
                        {
                            Growl.ErrorGlobal(Application.Current.Resources["TranslateWin_DictError_Hint"] + _dict.GetLastError());
                        }
                        else
                        {
                            dtimer.Stop();
                            DictResWindow _dictResWindow = new DictResWindow(textBlock.Text,(string)textBlock.Tag,_textSpeechHelper);
                            _dictResWindow.ShowDialog();
                            dtimer.Start();
                        }
                    }
                    else
                    {
                        Growl.ErrorGlobal(Application.Current.Resources["TranslateWin_DictError_Hint"] + _dict.GetLastError());
                    }
                }
            }

        }

        /// <summary>
        /// Hook模式下调用的事件
        /// </summary>
        public void DataRecvEventHandler(object sender, SolvedDataRecvEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {

                //1.得到原句
                string source = e.Data.Data;

                //2.进行去重
                string repairedText = TextRepair.RepairFun_Auto(Common.UsingRepairFunc, source);

                if (Convert.ToBoolean(Common.appSettings.EachRowTrans) == false)
                {
                    //不需要分行翻译
                    repairedText = repairedText.Replace("<br>", "").Replace("</br>", "").Replace("\n", "").Replace("\t", "").Replace("\r", "");
                }
                //去乱码
                repairedText = repairedText.Replace("_", "").Replace("-", "").Replace("+", "");

                //补充:如果去重之后的文本长度超过100，直接不翻译、不显示
                if (repairedText.Length <= 100)
                {
                    //2.5 清除面板
                    SourceTextPanel.Children.Clear();

                    _currentsrcText = repairedText;

                    if (_isShowSource)
                    {
                        //3.分词
                        var mwi = _mecabHelper.SentenceHandle(repairedText);
                        //分词后结果显示
                        for (int i = 0; i < mwi.Count; i++)
                        {
                            StackPanel stackPanel = new StackPanel();
                            stackPanel.Orientation = Orientation.Vertical;
                            stackPanel.Margin = new Thickness(10, 0, 0, 10);

                            TextBlock textBlock = new TextBlock();
                            if (!string.IsNullOrEmpty(SourceTextFont))
                            {
                                FontFamily fontFamily = new FontFamily(SourceTextFont);
                                textBlock.FontFamily = fontFamily;
                            }
                            textBlock.Text = mwi[i].Word;
                            textBlock.Tag = mwi[i].Kana;
                            textBlock.Margin = new Thickness(0, 0, 0, 0);
                            textBlock.FontSize = SourceTextFontSize;
                            textBlock.Background = Brushes.Transparent;
                            textBlock.MouseLeftButtonDown += DictArea_MouseLeftButtonDown;
                            //根据不同词性跟字体上色
                            switch (mwi[i].PartOfSpeech)
                            {
                                case "名詞":
                                    textBlock.Foreground = Brushes.AliceBlue;
                                    break;
                                case "助詞":
                                    textBlock.Foreground = Brushes.LightGreen;
                                    break;
                                case "動詞":
                                    textBlock.Foreground = Brushes.Red;
                                    break;
                                case "連体詞":
                                    textBlock.Foreground = Brushes.Orange;
                                    break;
                                default:
                                    textBlock.Foreground = Brushes.White;
                                    break;
                            }


                            TextBlock superScript = new TextBlock();//假名或注释等的上标标签
                            if (!string.IsNullOrEmpty(SourceTextFont))
                            {
                                FontFamily fontFamily = new FontFamily(SourceTextFont);
                                superScript.FontFamily = fontFamily;
                            }
                            superScript.Text = mwi[i].Kana;
                            superScript.Margin = new Thickness(0, 0, 0, 2);
                            superScript.HorizontalAlignment = HorizontalAlignment.Center;
                            if ((double)SourceTextFontSize - 6.5 > 0)
                            {
                                superScript.FontSize = (double)SourceTextFontSize - 6.5;
                            }
                            else {
                                superScript.FontSize = 1;
                            }
                            superScript.Background = Brushes.Transparent;
                            superScript.Foreground = Brushes.White;
                            stackPanel.Children.Add(superScript);
                            

                            //是否打开假名标注
                            if (Common.appSettings.TF_isKanaShow)
                            {
                                stackPanel.Children.Add(textBlock);
                                SourceTextPanel.Children.Add(stackPanel);
                            }
                            else {
                                textBlock.Margin = new Thickness(10, 0, 0, 10);
                                SourceTextPanel.Children.Add(textBlock);
                            }
                            
                        }
                    }

                    

                    //4.翻译前预处理
                    string beforeString = _beforeTransHandle.AutoHandle(repairedText);

                    //5.提交翻译
                    string transRes1 = string.Empty;
                    string transRes2 = string.Empty;
                    if (_translator1 != null)
                    {
                        transRes1 = _translator1.Translate(beforeString, Common.UsingDstLang, Common.UsingSrcLang);
                    }
                    if (_translator2 != null)
                    {
                        transRes2 = _translator2.Translate(beforeString, Common.UsingDstLang, Common.UsingSrcLang);
                    }

                    //6.翻译后处理
                    string afterString1 = _afterTransHandle.AutoHandle(transRes1);
                    string afterString2 = _afterTransHandle.AutoHandle(transRes2);

                    //7.翻译结果显示到窗口上
                    FirstTransText.Text = afterString1;
                    SecondTransText.Text = afterString2;

                    //8.翻译结果记录到队列
                    if (_gameTextHistory.Count > 5)
                    {
                        _gameTextHistory.Dequeue();
                    }
                    _gameTextHistory.Enqueue(repairedText + "\n" + afterString1 + "\n" + afterString2);
                }
                
            }));

            
        }

        private void ChangeSize_Item_Click(object sender, RoutedEventArgs e)
        {
            if (BackWinChrome.Opacity != 1)
            {
                BackWinChrome.Opacity = 1;
                ChangeSizeButton.Foreground = Brushes.Gray;
                Growl.InfoGlobal(Application.Current.Resources["TranslateWin_DragBox_Hint"].ToString());
            }
            else
            {
                BackWinChrome.Opacity = Common.appSettings.TF_Opacity / 100;
                ChangeSizeButton.Foreground = Brushes.PapayaWhip;
            }

        }

        private void Exit_Item_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Pause_Item_Click(object sender, RoutedEventArgs e)
        {
            if (Common.transMode == 1)
            {
                if (Common.textHooker.Pause)
                {
                    PauseButton.SetValue(FontAwesome.WPF.Awesome.ContentProperty, FontAwesomeIcon.Pause);
                }
                else
                {
                    PauseButton.SetValue(FontAwesome.WPF.Awesome.ContentProperty, FontAwesomeIcon.Play);
                }
                Common.textHooker.Pause = !Common.textHooker.Pause;
            }
            else
            {
                if(IsPauseFlag)
                {
                    PauseButton.SetValue(FontAwesome.WPF.Awesome.ContentProperty, FontAwesomeIcon.Play);
                }
                else
                {
                    PauseButton.SetValue(FontAwesome.WPF.Awesome.ContentProperty, FontAwesomeIcon.Pause);
                }
                IsPauseFlag = !IsPauseFlag;
            }

        }

        private void ShowSource_Item_Click(object sender, RoutedEventArgs e)
        {
            if(_isShowSource)
            {
                ShowSourceButton.SetValue(FontAwesome.WPF.Awesome.ContentProperty, FontAwesomeIcon.Eye);
            }
            else
            {
                ShowSourceButton.SetValue(FontAwesome.WPF.Awesome.ContentProperty, FontAwesomeIcon.EyeSlash);
            }
            _isShowSource = !_isShowSource;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Common.appSettings.TF_LocX = Convert.ToString((int)this.Left);
            Common.appSettings.TF_LocY = Convert.ToString((int)this.Top);
            Common.appSettings.TF_SizeW = Convert.ToString((int)this.Width);
            Common.appSettings.TF_SizeH = Convert.ToString((int)this.Height);

            if (hook != null)
            {
                hook.Stop();
                hook = null;
            }

            if (Common.textHooker != null)
            {
                Common.textHooker.Sevent -= DataRecvEventHandler;
                Common.textHooker.StopHook();
                Common.textHooker = null;
            }

            dtimer.Stop();
            dtimer = null;

            //立即清一次，否则重复打开翻译窗口会造成异常：Mecab处理类库异常
            _mecabHelper = null;
            GC.Collect();

            
        }


        private void Settings_Item_Click(object sender, RoutedEventArgs e)
        {
            dtimer.Stop();
            TransWinSettingsWindow twsw = new TransWinSettingsWindow(this);
            twsw.Show();
        }

        private void History_Item_Click(object sender, RoutedEventArgs e)
        {
            var textbox = new HandyControl.Controls.TextBox();
            string his = string.Empty;
            string[] history = _gameTextHistory.ToArray();
            for (int i = history.Length - 1; i > 0; i--)
            {
                his += history[i] + "\n";
                his += "==================================\n";
            }
            textbox.Text = his;
            textbox.FontSize = 15;
            textbox.TextWrapping = TextWrapping.Wrap;
            textbox.TextAlignment = TextAlignment.Left;
            textbox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            var window = new PopupWindow
            {
                PopupElement = textbox,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                BorderThickness = new Thickness(0, 0, 0, 0),
                MaxWidth = 600,
                MaxHeight = 300,
                MinWidth = 600,
                MinHeight = 300,
                Title = Application.Current.Resources["TranslateWin_History_Title"].ToString()
            };
            dtimer.Stop();
            window.Topmost = true;
            window.ShowDialog();
            dtimer.Start();
        }

        private void AddNoun_Item_Click(object sender, RoutedEventArgs e)
        {
            dtimer.Stop();
            AddOptWindow win = new AddOptWindow(_currentsrcText);
            win.ShowDialog();
            dtimer.Start();
        }

        private void RenewOCR_Item_Click(object sender, RoutedEventArgs e)
        {
            if (Common.transMode == 2)
            {
                OCR();
            }
            else
            {
                if (Convert.ToBoolean(Common.appSettings.EachRowTrans))
                {
                    //需要分行翻译
                    _currentsrcText = _currentsrcText.Replace("<br>", string.Empty).Replace("</br>", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty);
                }
                //去乱码
                _currentsrcText = _currentsrcText.Replace("_", string.Empty).Replace("-", string.Empty).Replace("+", string.Empty);

                //4.翻译前预处理
                string beforeString = _beforeTransHandle.AutoHandle(_currentsrcText);

                //5.提交翻译
                string transRes1 = string.Empty;
                string transRes2 = string.Empty;
                if (_translator1 != null)
                {
                    transRes1 = _translator1.Translate(beforeString, Common.UsingDstLang, Common.UsingSrcLang);
                }
                if (_translator2 != null)
                {
                    transRes2 = _translator2.Translate(beforeString, Common.UsingDstLang, Common.UsingSrcLang);
                }

                //6.翻译后处理
                string afterString1 = _afterTransHandle.AutoHandle(transRes1);
                string afterString2 = _afterTransHandle.AutoHandle(transRes2);

                //7.翻译结果显示到窗口上
                FirstTransText.Text = afterString1;
                SecondTransText.Text = afterString2;
            }
        }

        private void Min_Item_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Lock_Item_Click(object sender, RoutedEventArgs e)
        {
            if(!_isLocked)
            {
                BackWinChrome.Opacity = 0;
                _isLocked = true;
                LockButton.SetValue(FontAwesome.WPF.Awesome.ContentProperty, FontAwesomeIcon.UnlockAlt);
            }
            else
            {
                BackWinChrome.Opacity = Common.appSettings.TF_Opacity / 100;
                _isLocked = false;
                LockButton.SetValue(FontAwesome.WPF.Awesome.ContentProperty, FontAwesomeIcon.Lock);
            }
        }

        private void TTS_Item_Click(object sender, RoutedEventArgs e)
        {
            _textSpeechHelper.SpeakAsync(_currentsrcText);
        }

        private void TransWin_Loaded(object sender, RoutedEventArgs e)
        {
            winHandle = new WindowInteropHelper(this).Handle;//记录翻译窗口句柄

            dtimer = new System.Windows.Threading.DispatcherTimer();
            dtimer.Interval = TimeSpan.FromMilliseconds(10);
            dtimer.Tick += dtimer_Tick;
            dtimer.Start();
        }

        void dtimer_Tick(object sender, EventArgs e)
        {
            if (this.WindowState != WindowState.Minimized) {
                //定时刷新窗口到顶层
                SetWindowPos(winHandle, -1, 0, 0, 0, 0, 1 | 2);
            }
        }


    }
}