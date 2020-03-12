/*
 *Namespace         MisakaTranslator
 *Class             GameTranslateForm
 *Description       翻译窗口的前部窗口，通过固定颜色去色达到完全透明，与翻译窗口后部窗口叠加
 *Author            Hanmin Qi
 *LastModifyTime    2020-03-12
 * ===============================================================
 * 以下是修改记录（任何一次修改都应被记录）
 * 日期   修改内容    作者
 * 2020-03-12       代码注释完成      果冻
 */

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MisakaTranslator
{
    public partial class GameTranslateForm : Form
    {
        #region win32 api
        private const uint WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int GWL_STYLE = (-16);
        private const int GWL_EXSTYLE = (-20);
        private const int LWA_ALPHA = 0;

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(
            IntPtr hwnd,
            int nIndex,
            uint dwNewLong
        );
        #endregion

        public GameTranslateForm()
        {
            string color = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "BackColor", "Noset");
            if (color == "Noset")
            {
                TransparencyColor = Color.AliceBlue;
            }
            else {
                TransparencyColor = Color.FromArgb(int.Parse(color));
            }
            
            InitUI();
            this.Load += GameTranslateForm_Load;
        }

        /// <summary>
        /// 翻译窗口加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTranslateForm_Load(object sender, EventArgs e) {
            // 设置背景全透明
            this.BackColor = TransparencyColor;
            this.TransparencyKey = TransparencyColor;
            // 设置窗体鼠标穿透
            SetPenetrate();
        }

        /// <summary>
        /// 设置窗体具有鼠标穿透效果
        /// </summary>
        /// <param name="flag">true穿透，false不穿透</param>
        public void SetPenetrate(bool flag = true)
        {
            if (flag)
                SetWindowLong(this.Handle, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_LAYERED);
            else
                this.FormBorderStyle = this.FormBorderStyle;
        }

        public GameTranslateBackForm back;//翻译窗口后部窗口
        public Label srcTextLabel;//源文本标签
        public Label firstTransTextLabel;//第一翻译源标签
        public Label secondTransTextLabel;//第二翻译源标签

        public bool IsOCRingFlag;//线程锁:判断是否正在OCR线程中，保证同时只有一组在跑OCR

        public Color TransparencyColor;//透明色

        GlobalMouseHook hook;//全局鼠标钩子

        private string firstTransAPI;//第一翻译源
        private string secondTransAPI;//第二翻译源

        private string srcText;//源文本
        private string firstTransText;//第一翻译源结果
        private string secondTransText;//第二翻译源结果

        Font srcTextFont;//源文本字体
        Font firstTransTextFont;//第一翻译源结果字体
        Font secondTransTextFont;//第二翻译源结果字体

        Color srcTextColor;//源文本颜色
        Color firstTransTextColor;//第一翻译源结果颜色
        Color secondTransTextColor;//第二翻译源结果颜色

        /// <summary>
        /// UI初始化、翻译API初始化、如果是OCR模式则Hook鼠标点击事件
        /// </summary>
        private void InitUI()
        {
            srcTextLabel = new Label();
            srcTextLabel.AutoSize = false;
            srcTextLabel.Text = "等待源文本";
            srcTextLabel.BackColor = TransparencyColor; //背景色透明
            srcTextLabel.TextAlign = ContentAlignment.TopLeft;

            firstTransTextLabel = new Label();
            firstTransTextLabel.AutoSize = false;
            firstTransTextLabel.Text = "等待源文本";
            firstTransTextLabel.BackColor = TransparencyColor; //背景色透明
            firstTransTextLabel.TextAlign = ContentAlignment.TopLeft;

            secondTransTextLabel = new Label();
            secondTransTextLabel.AutoSize = false;
            secondTransTextLabel.Text = "等待源文本";
            secondTransTextLabel.BackColor = TransparencyColor; //背景色透明
            secondTransTextLabel.TextAlign = ContentAlignment.TopLeft;

            LabelInit();
            this.Controls.Add(srcTextLabel);
            this.Controls.Add(firstTransTextLabel);
            this.Controls.Add(secondTransTextLabel);

            this.FormBorderStyle = FormBorderStyle.None;
            this.SizeChanged += GameTranslateForm_SizeChanged;
            this.FormClosing += GameTranslateForm_FormClosing;

            BaiduTranslator.BaiduTrans_Init();
            TencentTranslator.TencentTrans_Init();
            TencentOldTranslator.TencentOldTrans_Init();

            if (Common.TransMode == 2)
            {
                BaiduGeneralOCRBasic.BaiduGeneralOCRBasic_Init();

                //初始化钩子对象
                if (hook == null)
                {
                    hook = new GlobalMouseHook();
                    hook.OnMouseActivity += new MouseEventHandler(Hook_OnMouseActivity);
                }

                IsOCRingFlag = false;
                bool r = hook.Start();
                if (r == false)
                {
                    MessageBox.Show("安装钩子失败!");
                }

            }
            

            TextFontColorInit();

            firstTransAPI = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", 
                "Translate_All", "FirstTranslator", "NoTranslate");
            secondTransAPI = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", 
                "Translate_All", "SecondTranslator", "NoTranslate");
            
        }

        /// <summary>
        /// 设置背景窗口
        /// </summary>
        /// <param name="bk"></param>
        public void SetBackForm(GameTranslateBackForm bk) {
            back = bk;
        }

        /// <summary>
        /// 窗口加载完成或大小改变时，自绘所有标签的大小、位置等信息
        /// </summary>
        private void LabelInit()
        {
            if (firstTransAPI == "NoTranslate")
            {
                srcTextLabel.Left = 10;
                srcTextLabel.Top = 10;
                srcTextLabel.Width = this.Width - 20;
                srcTextLabel.Height = this.Height - 20;

                firstTransTextLabel.Visible = false;
                secondTransTextLabel.Visible = false;
            }
            else if (firstTransAPI != "NoTranslate" && secondTransAPI == "NoTranslate")
            {
                srcTextLabel.Left = 10;
                srcTextLabel.Top = 10;
                srcTextLabel.Width = this.Width - 20;
                srcTextLabel.Height = (this.Height - 20) / 2;

                firstTransTextLabel.Left = srcTextLabel.Left;
                firstTransTextLabel.Top = srcTextLabel.Top + srcTextLabel.Height;
                firstTransTextLabel.Width = srcTextLabel.Width;
                firstTransTextLabel.Height = srcTextLabel.Height;

                secondTransTextLabel.Visible = false;
            }
            else
            {
                srcTextLabel.Left = 10;
                srcTextLabel.Top = 10;
                srcTextLabel.Width = this.Width - 20;
                srcTextLabel.Height = (this.Height - 20) / 3;

                firstTransTextLabel.Left = srcTextLabel.Left;
                firstTransTextLabel.Top = srcTextLabel.Top + srcTextLabel.Height;
                firstTransTextLabel.Width = srcTextLabel.Width;
                firstTransTextLabel.Height = srcTextLabel.Height;

                secondTransTextLabel.Left = srcTextLabel.Left;
                secondTransTextLabel.Top = srcTextLabel.Top + srcTextLabel.Height + firstTransTextLabel.Height;
                secondTransTextLabel.Width = srcTextLabel.Width;
                secondTransTextLabel.Height = srcTextLabel.Height;
            }
        }

        /// <summary>
        /// 翻译窗口大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTranslateForm_SizeChanged(object sender, EventArgs e) {
            LabelInit();
            //SetWindowRegion();
        }

        /// <summary>
        /// 翻译窗口关闭事件
        /// Hook:关闭进程
        /// OCR:关闭鼠标钩子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTranslateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.TransMode = 0;
            if (Common.TextractorHandle != null)
            {
                Common.TextractorHandle.SetGameTransForm(null);
                Common.TextractorHandle.CloseTextractor();
            }

            if (hook != null)
            {
                hook.Stop();
                hook = null;
            }

        }

        /// <summary>
        /// 加载窗口时初始化所有标签的字体颜色信息，从INI文件读取
        /// </summary>
        public void TextFontColorInit()
        {
            srcTextFont = new Font(
                IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextFont", "微软雅黑"),
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextsize", "12"))
                );

            firstTransTextFont = new Font(
                IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextFont", "微软雅黑"),
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextsize", "12"))
                );

            secondTransTextFont = new Font(
                IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextFont", "微软雅黑"),
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextsize", "12"))
                );


            srcTextColor = Color.FromArgb(
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorR", "0")),
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorG", "0")),
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorB", "0"))
                );

            firstTransTextColor = Color.FromArgb(
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorR", "0")),
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorG", "0")),
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorB", "0"))
                );

            secondTransTextColor = Color.FromArgb(
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorR", "0")),
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorG", "0")),
                int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorB", "0"))
                );


            srcTextLabel.Font = srcTextFont;
            srcTextLabel.ForeColor = srcTextColor;

            firstTransTextLabel.Font = firstTransTextFont;
            firstTransTextLabel.ForeColor = firstTransTextColor;

            secondTransTextLabel.Font = secondTransTextFont;
            secondTransTextLabel.ForeColor = secondTransTextColor;
        }

        /// <summary>
        /// 设置文字字体
        /// </summary>
        /// <param name="Fontname"></param>
        /// <param name="size"></param>
        /// <param name="setPartIndex"></param>
        public void SetTextFont(string Fontname, int size, int setPartIndex)
        {
            if (size > 0)
            {
                if (setPartIndex == 0)
                {
                    srcTextLabel.Font = new Font(Fontname, size);
                }
                else if (setPartIndex == 1)
                {
                    firstTransTextLabel.Font = new Font(Fontname, size);
                }
                else if (setPartIndex == 2)
                {
                    secondTransTextLabel.Font = new Font(Fontname, size);
                }
            }
        }

        /// <summary>
        /// 设置文字颜色
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <param name="setPartIndex"></param>
        public void SetTextColor(int R, int G, int B, int setPartIndex)
        {
            if (setPartIndex == 0)
            {
                srcTextLabel.ForeColor = Color.FromArgb(R, G, B);
            }
            else if (setPartIndex == 1)
            {
                firstTransTextLabel.ForeColor = Color.FromArgb(R, G, B);
            }
            else if (setPartIndex == 2)
            {
                secondTransTextLabel.ForeColor = Color.FromArgb(R, G, B);
            }
        }


        /// <summary>
        /// OCR:鼠标点击的挂载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Hook_OnMouseActivity(object sender, MouseEventArgs e)
        {
            if (Common.TransMode == 2)
            {
                //OCR方式下单击鼠标进行更新
                //先判断是单窗口还是全屏幕，如果是单窗口再判断是不是在游戏窗口中点的
                
                if ( (Common.isAllWindowCap == true && Process.GetCurrentProcess().Id != FindWindowInfo.GetProcessIDByHWND(FindWindowInfo.GetWindowHWND(e.X, e.Y)) ) 
                    || Common.OCRWinHwnd == (IntPtr)FindWindowInfo.GetWindowHWND(e.X, e.Y))
                {
                    if (IsOCRingFlag == false)
                    {
                        IsOCRingFlag = true;
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            int j = 0;

                            for (; j < 3; j++)
                            {

                                Thread.Sleep(Common.OCRdelay);

                                Image img = ScreenCapture.GetWindowRectCapture(Common.OCRWinHwnd, Common.OCRrec, Common.isAllWindowCap);
                                string ret = BaiduGeneralOCRBasic.BaiduGeneralBasicOCR(img, Common.OCRsrcLangCode);

                                string srcText = "";
                                BaiduOCRresOutInfo oinfo = JsonConvert.DeserializeObject<BaiduOCRresOutInfo>(ret);
                                if (oinfo.words_result != null)
                                {
                                    for (int i = 0; i < oinfo.words_result_num; i++)
                                    {
                                        srcText = srcText + oinfo.words_result[i].words + "\n";
                                    }
                                }

                                if (srcText != "")
                                {
                                    GameTranslateAuto(srcText, Common.srcLang, Common.desLang);

                                    srcTextLabel.BeginInvoke(new Action(() =>
                                    {
                                        srcTextLabel.Text = srcText;
                                    }));

                                    firstTransTextLabel.BeginInvoke(new Action(() =>
                                    {
                                        firstTransTextLabel.Text = firstTransText;
                                    }));

                                    secondTransTextLabel.BeginInvoke(new Action(() =>
                                    {
                                        secondTransTextLabel.Text = secondTransText;
                                    }));

                                    Common.AddHistoryText(srcText, firstTransText, secondTransText);

                                    IsOCRingFlag = false;
                                    break;
                                }
                            }

                            if (j == 3)
                            {
                                srcTextLabel.BeginInvoke(new Action(() =>
                                {
                                    srcTextLabel.Text = "[OCR]识别三次均为空，请自行刷新！";
                                }));
                                IsOCRingFlag = false;
                            }

                        });
                    }

                }



            }
        }

        /// <summary>
        /// TextHook:收到文本更新后发生的事件
        /// </summary>
        /// <param name="Item"></param>
        public void TextractorHookContent(string[] Item)
        {

            Type t = typeof(TextRepeatRepair);//括号中的为所要使用的函数所在的类的类名
            MethodInfo mt = t.GetMethod(Common.RepeatMethod);
            if (mt != null)
            {
                string str = (string)mt.Invoke(null, new object[] { Item[3] });

                srcTextLabel.BeginInvoke(new Action(() => {
                    srcTextLabel.Text = str;
                    GameTranslateAuto(str, Common.srcLang, Common.desLang);
                    firstTransTextLabel.Text = firstTransText;
                    secondTransTextLabel.Text = secondTransText;
                    Common.AddHistoryText(str, firstTransText, secondTransText);
                }));
                
            }
            else
            {
                srcTextLabel.BeginInvoke(new Action(() => {
                    srcTextLabel.Text = "Hook去重处理出现错误";
                    firstTransTextLabel.Text = "";
                    secondTransTextLabel.Text = "";
                }));
            }
        }

        
        /*
         * 

        const int WM_NCHITTEST = 0x0084;
        const int HTLEFT = 10;
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOM = 15;
        const int HTBOTTOMLEFT = 0x10;
        const int HTBOTTOMRIGHT = 17;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)HTBOTTOMLEFT;
                        else m.Result = (IntPtr)HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)HTBOTTOM;
                    break;
            }
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
         */
        /*
        //之前的窗口改变大小部分代码，已弃用
        private void GameTranslateForm_Move(object sender, EventArgs e)
        {
            ReleaseCapture();
            
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            
        }
        */
        /*
         * //之前的窗口拖动部分代码，已弃用
        private void TransparentPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void TransparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                Location = mousePos;
            }
            
        }
        */

        
        /// <summary>
        /// 自动进行游戏翻译
        /// 检查是否设置了分行翻译、去掉一些乱码类型的符号、检查是否空文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="srcLang"></param>
        /// <param name="desLang"></param>
        public void GameTranslateAuto(string text,string srcLang,string desLang) {

            //先检查玩家是否设置了分行翻译
            bool eachRowTrans = Convert.ToBoolean(IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "Translate_All", "EachRowTrans", "True"));
            if (eachRowTrans == false) {
                text = text.Replace("<br>","").Replace("</br>","").Replace("\n", "").Replace("\t", "").Replace("\r", "");
            }

            //处理：去掉一些乱码类型的符号
            text = text.Replace("_", "").Replace("-", "").Replace("+", "");

            string currentAPI;
            string ret = "";
            
            for (int i = 1; i <= 2; i++)
            {
                if (i == 1)
                {
                    currentAPI = firstTransAPI;
                }
                else
                {
                    currentAPI = secondTransAPI;
                }

                if (text == "")
                {
                    ret = "";
                }
                else {
                    if (currentAPI == "BaiduTranslator")
                    {
                        ret = BaiduTranslator.Baidu_Translate(text, desLang, srcLang);
                        if (ret == "Request Timeout")
                        {
                            ret = "[百度]翻译超时";
                        }
                        else
                        {
                            BaiduTransOutInfo oinfo = JsonConvert.DeserializeObject<BaiduTransOutInfo>(ret);
                            if (oinfo.trans_result != null)
                            {
                                ret = "";
                                for (int k = 0; k < oinfo.trans_result.Count; k++)
                                {
                                    ret = ret + oinfo.trans_result[k].dst + "\n";
                                }
                            }
                            else
                            {
                                ret = "[百度]翻译错误";
                            }
                        }
                    }
                    else if (currentAPI == "TencentTranslator")
                    {
                        ret = TencentTranslator.Fanyijun_Translate(text, desLang, srcLang);
                        if (ret == "Request Timeout")
                        {
                            ret = "[腾讯]翻译超时";
                        }
                        else
                        {
                            TencentTransOutInfo oinfo = JsonConvert.DeserializeObject<TencentTransOutInfo>(ret);
                            if (oinfo.data != null)
                            {
                                ret = oinfo.data.target_text;
                            }
                            else
                            {
                                ret = "[腾讯]翻译错误";
                            }
                        }
                    }
                    else if (currentAPI == "JBjTranslator")
                    {
                        ret = JBeijingTranslator.Translate_JapanesetoChinese(text);
                        if (ret == null || ret == "")
                        {
                            ret = "";
                        }
                    }
                    else if (currentAPI == "TencentOldTranslator")
                    {
                        ret = TencentOldTranslator.TencentOld_Translate(text, desLang, srcLang);
                        if (ret == "Request Timeout")
                        {
                            ret = "[腾讯旧]翻译超时";
                        }
                        else
                        {
                            TencentOldTransOutInfo oinfo = JsonConvert.DeserializeObject<TencentOldTransOutInfo>(ret);
                            if (oinfo.Response.TargetText != null)
                            {
                                ret = oinfo.Response.TargetText;
                            }
                            else
                            {
                                ret = "[腾讯旧]翻译错误";
                            }
                        }
                    }
                    else
                    {
                        ret = "";
                    }
                }
                
                if (i == 1)
                {
                    firstTransText = ret;
                }
                else
                {
                    secondTransText = ret;
                }
                
            }
        }

        /// <summary>
        /// 刷新OCR识别，重置窗口结果,使用多线程解决卡顿
        /// </summary>
        public void ReNewOCR() {
            if (Common.TransMode == 2)
            {
                if (IsOCRingFlag == false)
                {
                    IsOCRingFlag = true;

                    ThreadPool.QueueUserWorkItem(state => {
                        Image img = ScreenCapture.GetWindowRectCapture(Common.OCRWinHwnd, Common.OCRrec, Common.isAllWindowCap);
                        string ret = BaiduGeneralOCRBasic.BaiduGeneralBasicOCR(img, Common.OCRsrcLangCode);

                        string srcText = "";
                        BaiduOCRresOutInfo oinfo = JsonConvert.DeserializeObject<BaiduOCRresOutInfo>(ret);
                        if (oinfo.words_result != null)
                        {
                            for (int i = 0; i < oinfo.words_result_num; i++)
                            {
                                srcText = srcText + oinfo.words_result[i].words + "\n";
                            }
                        }

                        srcTextLabel.BeginInvoke(new Action(() =>
                        {
                            srcTextLabel.Text = srcText;
                        }));

                        GameTranslateAuto(srcText, Common.srcLang, Common.desLang);
                        firstTransTextLabel.BeginInvoke(new Action(() =>
                        {
                            firstTransTextLabel.Text = firstTransText;
                        }));

                        secondTransTextLabel.BeginInvoke(new Action(() =>
                        {
                            secondTransTextLabel.Text = secondTransText;
                        }));
                        Common.AddHistoryText(srcText, firstTransText, secondTransText);
                    });

                    IsOCRingFlag = false;
                }
                
            }
            else {
                MessageBox.Show("当前处于非OCR翻译模式，无法刷新！","错误");
            }
        }

        /// <summary>
        /// 设置原文标签是否可视
        /// </summary>
        /// <param name="flag"></param>
        public void SetSrcTextLabelVisible(bool flag) {
            srcTextLabel.BeginInvoke(new Action(() => {
                srcTextLabel.Visible = flag;
            }));
        }
       
    }
}
