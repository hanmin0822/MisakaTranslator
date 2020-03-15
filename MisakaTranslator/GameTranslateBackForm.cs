/*
 *Namespace         MisakaTranslator
 *Class             GameTranslateBackForm
 *Description       翻译窗口的后部窗口，可调节透明度、背景颜色，与翻译窗口前部窗口叠加
 */


using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MisakaTranslator
{
    public partial class GameTranslateBackForm : Form
    {
        public GameTranslateBackForm()
        {
            InitializeComponent();
        }


        public static GameTranslateBackForm back;
        public static GameTranslateForm top;
        public static int offY = 30;
        public static bool isLock;
        public static bool isShowSrcText;

        /// <summary>
        /// 设置背景窗口透明度
        /// </summary>
        /// <param name="frmBackOpacity"></param>
        public static void SetBackFormOpacity(double frmBackOpacity)
        {
            back.Opacity = frmBackOpacity;
        }

        /// <summary>
        /// 背景窗口的静态方法，用于显示一个背景窗口
        /// </summary>
        /// <param name="frmTop">与其叠加的前景窗口</param>
        public static void Show(GameTranslateForm frmTop)
        {

            //弹出菜单设置
            MaterialSkin.Controls.MaterialContextMenuStrip FunMenuStrip = new MaterialSkin.Controls.MaterialContextMenuStrip();
            ToolStripMenuItem LockItem = new ToolStripMenuItem();
            ToolStripMenuItem SettingsItem = new ToolStripMenuItem();
            ToolStripMenuItem ReNewOCRItem = new ToolStripMenuItem();
            ToolStripMenuItem HistoryTextItem = new ToolStripMenuItem();
            ToolStripMenuItem ExitTransFrmItem = new ToolStripMenuItem();
            ToolStripMenuItem ShowsrcTextFrmItem = new ToolStripMenuItem();

            FunMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            FunMenuStrip.Depth = 0;
            FunMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                LockItem,
                SettingsItem,
                ReNewOCRItem,
                HistoryTextItem,
                ShowsrcTextFrmItem,
                ExitTransFrmItem});
            FunMenuStrip.MouseState = MaterialSkin.MouseState.HOVER;
            FunMenuStrip.ShowImageMargin = false;
            FunMenuStrip.Size = new System.Drawing.Size(156, 136);
            // 
            // 加锁解锁
            // 
            LockItem.Size = new System.Drawing.Size(155, 22);
            LockItem.Text = "加锁/解锁";
            LockItem.Click += LockItem_Click;
            // 
            // 设置
            // 
            SettingsItem.Size = new System.Drawing.Size(155, 22);
            SettingsItem.Text = "设置";
            SettingsItem.Click += SettingsItem_Click;
            // 
            // 刷新OCRToolStripMenuItem
            // 
            ReNewOCRItem.Size = new System.Drawing.Size(155, 22);
            ReNewOCRItem.Text = "刷新OCR";
            ReNewOCRItem.Click += ReNewOCRItem_Click;
            // 
            // 历史文本ToolStripMenuItem
            // 
            HistoryTextItem.Size = new System.Drawing.Size(155, 22);
            HistoryTextItem.Text = "历史文本";
            HistoryTextItem.Click += HistoryTextItem_Click;
            // 
            // 退出翻译ToolStripMenuItem
            // 
            ExitTransFrmItem.Size = new System.Drawing.Size(155, 22);
            ExitTransFrmItem.Text = "退出翻译";
            ExitTransFrmItem.Click += ExitTransFrmItem_Click;
            // 
            // 显示原文ToolStripMenuItem
            // 
            ShowsrcTextFrmItem.Size = new System.Drawing.Size(155, 22);
            ShowsrcTextFrmItem.Text = "显示/隐藏原文";
            ShowsrcTextFrmItem.Click += ShowSrcTextItem_Click;


            top = frmTop;
            // 背景窗体设置
            GameTranslateBackForm frmBack = new GameTranslateBackForm();
            top.SetBackForm(frmBack);
            back = frmBack;

            double frmBackOpacity = (double)(double.Parse(
                IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings",
                "opacity", "50")) / 100);

            frmBack.Text = "MisakaTranslator游戏翻译窗口";
            frmBack.FormBorderStyle = FormBorderStyle.None;
            frmBack.MaximizeBox = false;

            int LocX = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "LocX", "-1"));
            int LocY = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "LocY", "-1"));
            int SizeW = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "SizeW", "-1"));
            int SizeH = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "SizeH", "-1"));
            if (LocX == -1 && LocY == -1)
            {
                frmBack.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                frmBack.Location = new Point(LocX, LocY);
                frmBack.StartPosition = FormStartPosition.Manual;
                frmBack.Width = SizeW;
                frmBack.Height = SizeH;
            }

            frmBack.ShowIcon = false;
            frmBack.ShowInTaskbar = false;
            frmBack.Opacity = frmBackOpacity;
            string color = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "BackColor", "Noset");
            if (color == "Noset")
            {
                frmBack.BackColor = Color.LightGray;
            }
            else
            {
                frmBack.BackColor = Color.FromArgb(int.Parse(color));
            }

            frmBack.ContextMenuStrip = FunMenuStrip;

            // 顶部窗体设置
            frmTop.Owner = frmBack;

            frmBack.SizeChanged += GameTranslateBackForm_SizeLocationChanged;
            frmBack.LocationChanged += GameTranslateBackForm_SizeLocationChanged;
            frmBack.MouseDoubleClick += GameTranslateBackForm_DoubleClick;
            frmBack.FormClosing += GameTranslateBackForm_FormClosing;

            frmBack.TopMost = true;
            frmTop.TopMost = true;

            isLock = true;
            isShowSrcText = true;

            // 显示窗体
            frmTop.Show();
            frmBack.Show();
        }

        /// <summary>
        /// 背景窗口关闭时保存位置和大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void GameTranslateBackForm_FormClosing(object sender, EventArgs e)
        {
            IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "LocX", back.Left.ToString());
            IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "LocY", back.Top.ToString());
            IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "SizeW", back.Width.ToString());
            IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "SizeH", back.Height.ToString());
        }

        /// <summary>
        /// 背景窗口大小位置改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void GameTranslateBackForm_SizeLocationChanged(object sender, EventArgs e)
        {

            offY = back.Height - back.ClientRectangle.Height;

            top.Height = back.Height - offY;
            top.Width = back.Width;
            Point Lp = back.Location;
            Lp.Y = Lp.Y + offY;
            top.Location = Lp;
        }

        /// <summary>
        /// 背景窗口双击事件，用于解锁/加锁
        /// 由于前景框穿透，故前景框无法响应鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void GameTranslateBackForm_DoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isLock == true)
                {
                    back.FormBorderStyle = FormBorderStyle.Sizable;
                    isLock = false;
                }
                else
                {
                    back.FormBorderStyle = FormBorderStyle.None;
                    SetWindowRegion();
                    isLock = true;
                }
            }
        }

        /// <summary>
        /// 设置窗体的Region
        /// </summary>
        public static void SetWindowRegion()
        {
            GraphicsPath FormPath;
            Rectangle rect = new Rectangle(0, 0, back.Width, back.Height);
            FormPath = GetRoundedRectPath(rect, 10);
            back.Region = new Region(FormPath);

        }

        /// <summary>
        /// 绘制圆角路径
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            // 左上角
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();//闭合曲线
            return path;
        }

        //以下是各菜单项的点击事件
        private static void LockItem_Click(object sender, EventArgs e)
        {
            if (isLock == true)
            {
                back.FormBorderStyle = FormBorderStyle.Sizable;
                isLock = false;
            }
            else
            {
                back.FormBorderStyle = FormBorderStyle.None;
                SetWindowRegion();
                isLock = true;
            }
        }

        private static void SettingsItem_Click(object sender, EventArgs e)
        {
            TranslateFrmSettingsForm tfsf = new TranslateFrmSettingsForm(top, back);
            tfsf.Show();
        }

        private static void ReNewOCRItem_Click(object sender, EventArgs e)
        {
            top.ReNewOCR();
        }

        private static void HistoryTextItem_Click(object sender, EventArgs e)
        {
            HistoryTextForm htf = new HistoryTextForm();
            htf.Show();
        }

        private static void ExitTransFrmItem_Click(object sender, EventArgs e)
        {
            back.Close();
        }

        private static void ShowSrcTextItem_Click(object sender, EventArgs e)
        {
            if (isShowSrcText == true)
            {
                isShowSrcText = false;
            }
            else
            {
                isShowSrcText = true;
            }

            top.SetSrcTextLabelVisible(isShowSrcText);
        }
    }
}
