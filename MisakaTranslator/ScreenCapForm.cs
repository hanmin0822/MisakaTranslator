/*
 *Namespace         MisakaTranslator
 *Class             ScreenCapForm
 *Description       截屏显示窗口
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MisakaTranslator
{
    public partial class ScreenCapForm : Form
    {
        Image back;

        bool isDown;
        Point startPoint;
        Rectangle Drawrc;

        public ScreenCapForm(Image setback)
        {
            InitializeComponent();
            back = setback;
        }

        private void ScreenCapForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Drawrc = new Rectangle(0, 0, 0, 0);
                this.Invalidate();
                isDown = true;
                startPoint = new Point(e.X, e.Y);
            }
        }

        private void ScreenCapForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
            {
                
                Size sz = new Size(Math.Abs(e.X - startPoint.X), Math.Abs(e.Y - startPoint.Y));
                Drawrc = new Rectangle(startPoint,sz);

                this.Invalidate();
            }
        }

        private void ScreenCapForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDown = false;
                this.Invalidate();
            }
        }

        private void ScreenCapForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            g.DrawImage(back,0,0);
            g.DrawString("按住鼠标左键并拖动鼠标绘制出要识别的区域，确认完成后单击右键退出",new Font("微软雅黑",12), new SolidBrush(Color.Red), 10,10);

            if (Drawrc.X == 0 && Drawrc.Y == 0 && Drawrc.Width == 0 && Drawrc.Height == 0)
            {

            }
            else {
                g.DrawRectangle(new Pen(Color.Red, 3), Drawrc);
            }
            
        }

        private void ScreenCapForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                Common.OCRrec = Drawrc;
                this.Close();
            }
        }

        private void ScreenCapForm_Load(object sender, EventArgs e)
        {
            //加上这一句代码变成双缓冲绘图，修复闪屏
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
