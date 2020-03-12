namespace MisakaTranslator
{
    partial class ScreenCapForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ScreenCapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(654, 289);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScreenCapForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "截屏选择";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ScreenCapForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ScreenCapForm_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ScreenCapForm_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScreenCapForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScreenCapForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScreenCapForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}