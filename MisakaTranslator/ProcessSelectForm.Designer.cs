namespace MisakaTranslator
{
    partial class ProcessSelectForm
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
            this.SystemProcessCombox = new HZH_Controls.Controls.UCCombox();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.SelectGameWindowBtn = new HZH_Controls.Controls.UCBtnExt();
            this.ProcessInfoLabel = new MaterialSkin.Controls.MaterialLabel();
            this.ConfirmBtn = new HZH_Controls.Controls.UCBtnExt();
            this.ProcessNumLabel = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // SystemProcessCombox
            // 
            this.SystemProcessCombox.AutoScroll = true;
            this.SystemProcessCombox.BackColor = System.Drawing.Color.Transparent;
            this.SystemProcessCombox.BackColorExt = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SystemProcessCombox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SystemProcessCombox.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SystemProcessCombox.ConerRadius = 5;
            this.SystemProcessCombox.DropPanelHeight = -1;
            this.SystemProcessCombox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SystemProcessCombox.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.SystemProcessCombox.IsRadius = true;
            this.SystemProcessCombox.IsShowRect = true;
            this.SystemProcessCombox.ItemWidth = 100;
            this.SystemProcessCombox.Location = new System.Drawing.Point(25, 155);
            this.SystemProcessCombox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SystemProcessCombox.Name = "SystemProcessCombox";
            this.SystemProcessCombox.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SystemProcessCombox.RectWidth = 3;
            this.SystemProcessCombox.SelectedIndex = -1;
            this.SystemProcessCombox.SelectedValue = "";
            this.SystemProcessCombox.Size = new System.Drawing.Size(571, 44);
            this.SystemProcessCombox.Source = null;
            this.SystemProcessCombox.TabIndex = 0;
            this.SystemProcessCombox.TextValue = null;
            this.SystemProcessCombox.TriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.SystemProcessCombox.SelectedChangedEvent += new System.EventHandler(this.SystemProcessCombox_SelectedChangedEvent);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoEllipsis = true;
            this.materialLabel1.BackColor = System.Drawing.Color.White;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(25, 87);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(571, 48);
            this.materialLabel1.TabIndex = 1;
            this.materialLabel1.Text = "说明：请先运行游戏，之后通过下拉列表选择游戏进程名或点选“选择窗口”按钮并单击游戏窗口以获得游戏进程。最终结果以下拉列表显示项为准。\r\n\r\n\r\n\r\n";
            // 
            // SelectGameWindowBtn
            // 
            this.SelectGameWindowBtn.BackColor = System.Drawing.Color.White;
            this.SelectGameWindowBtn.BtnBackColor = System.Drawing.Color.White;
            this.SelectGameWindowBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SelectGameWindowBtn.BtnForeColor = System.Drawing.Color.White;
            this.SelectGameWindowBtn.BtnText = "选择窗口";
            this.SelectGameWindowBtn.ConerRadius = 5;
            this.SelectGameWindowBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SelectGameWindowBtn.EnabledMouseEffect = true;
            this.SelectGameWindowBtn.FillColor = System.Drawing.SystemColors.WindowFrame;
            this.SelectGameWindowBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.SelectGameWindowBtn.IsRadius = true;
            this.SelectGameWindowBtn.IsShowRect = true;
            this.SelectGameWindowBtn.IsShowTips = false;
            this.SelectGameWindowBtn.Location = new System.Drawing.Point(25, 235);
            this.SelectGameWindowBtn.Margin = new System.Windows.Forms.Padding(0);
            this.SelectGameWindowBtn.Name = "SelectGameWindowBtn";
            this.SelectGameWindowBtn.RectColor = System.Drawing.Color.Gray;
            this.SelectGameWindowBtn.RectWidth = 1;
            this.SelectGameWindowBtn.Size = new System.Drawing.Size(200, 53);
            this.SelectGameWindowBtn.TabIndex = 2;
            this.SelectGameWindowBtn.TabStop = false;
            this.SelectGameWindowBtn.TipsColor = System.Drawing.Color.Black;
            this.SelectGameWindowBtn.TipsText = "";
            this.SelectGameWindowBtn.BtnClick += new System.EventHandler(this.SelectGameWindowBtn_BtnClick);
            // 
            // ProcessInfoLabel
            // 
            this.ProcessInfoLabel.BackColor = System.Drawing.Color.White;
            this.ProcessInfoLabel.Depth = 0;
            this.ProcessInfoLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ProcessInfoLabel.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.ProcessInfoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ProcessInfoLabel.Location = new System.Drawing.Point(228, 249);
            this.ProcessInfoLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.ProcessInfoLabel.Name = "ProcessInfoLabel";
            this.ProcessInfoLabel.Size = new System.Drawing.Size(368, 29);
            this.ProcessInfoLabel.TabIndex = 3;
            this.ProcessInfoLabel.Text = "等待选择\r\n\r\n\r\n\r\n";
            this.ProcessInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConfirmBtn
            // 
            this.ConfirmBtn.BackColor = System.Drawing.Color.White;
            this.ConfirmBtn.BtnBackColor = System.Drawing.Color.White;
            this.ConfirmBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ConfirmBtn.BtnForeColor = System.Drawing.Color.White;
            this.ConfirmBtn.BtnText = "确认进程";
            this.ConfirmBtn.ConerRadius = 5;
            this.ConfirmBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConfirmBtn.EnabledMouseEffect = true;
            this.ConfirmBtn.FillColor = System.Drawing.SystemColors.WindowFrame;
            this.ConfirmBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ConfirmBtn.IsRadius = true;
            this.ConfirmBtn.IsShowRect = true;
            this.ConfirmBtn.IsShowTips = false;
            this.ConfirmBtn.Location = new System.Drawing.Point(347, 321);
            this.ConfirmBtn.Margin = new System.Windows.Forms.Padding(0);
            this.ConfirmBtn.Name = "ConfirmBtn";
            this.ConfirmBtn.RectColor = System.Drawing.Color.Gray;
            this.ConfirmBtn.RectWidth = 1;
            this.ConfirmBtn.Size = new System.Drawing.Size(249, 53);
            this.ConfirmBtn.TabIndex = 4;
            this.ConfirmBtn.TabStop = false;
            this.ConfirmBtn.TipsColor = System.Drawing.Color.Black;
            this.ConfirmBtn.TipsText = "";
            this.ConfirmBtn.BtnClick += new System.EventHandler(this.ConfirmBtn_BtnClick);
            // 
            // ProcessNumLabel
            // 
            this.ProcessNumLabel.BackColor = System.Drawing.Color.White;
            this.ProcessNumLabel.Depth = 0;
            this.ProcessNumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ProcessNumLabel.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.ProcessNumLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ProcessNumLabel.Location = new System.Drawing.Point(25, 333);
            this.ProcessNumLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.ProcessNumLabel.Name = "ProcessNumLabel";
            this.ProcessNumLabel.Size = new System.Drawing.Size(305, 29);
            this.ProcessNumLabel.TabIndex = 5;
            this.ProcessNumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProcessSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 401);
            this.Controls.Add(this.ProcessNumLabel);
            this.Controls.Add(this.ConfirmBtn);
            this.Controls.Add(this.ProcessInfoLabel);
            this.Controls.Add(this.SelectGameWindowBtn);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.SystemProcessCombox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProcessSelectForm";
            this.ShowInTaskbar = false;
            this.Sizable = false;
            this.Text = "选择游戏/软件进程";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProcessSelectForm_FormClosing);
            this.Load += new System.EventHandler(this.ProcessSelectForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private HZH_Controls.Controls.UCCombox SystemProcessCombox;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private HZH_Controls.Controls.UCBtnExt SelectGameWindowBtn;
        private MaterialSkin.Controls.MaterialLabel ProcessInfoLabel;
        private HZH_Controls.Controls.UCBtnExt ConfirmBtn;
        private MaterialSkin.Controls.MaterialLabel ProcessNumLabel;
    }
}