namespace MisakaTranslator
{
    partial class TransLangSettingForm
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
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.srcLangCombox = new HZH_Controls.Controls.UCCombox();
            this.dstLangCombox = new HZH_Controls.Controls.UCCombox();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            this.ConfirmLangBtn = new HZH_Controls.Controls.UCBtnExt();
            this.SuspendLayout();
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.BackColor = System.Drawing.Color.White;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel2.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(26, 90);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(339, 20);
            this.materialLabel2.TabIndex = 10;
            this.materialLabel2.Text = "最后一步设置！选择游戏源语言和目标翻译语言：";
            this.materialLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.BackColor = System.Drawing.Color.White;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(26, 149);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(69, 20);
            this.materialLabel1.TabIndex = 11;
            this.materialLabel1.Text = "源语言：";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // srcLangCombox
            // 
            this.srcLangCombox.BackColor = System.Drawing.Color.Transparent;
            this.srcLangCombox.BackColorExt = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.srcLangCombox.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.srcLangCombox.ConerRadius = 5;
            this.srcLangCombox.DropPanelHeight = -1;
            this.srcLangCombox.FillColor = System.Drawing.Color.White;
            this.srcLangCombox.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.srcLangCombox.IsRadius = true;
            this.srcLangCombox.IsShowRect = true;
            this.srcLangCombox.ItemWidth = 70;
            this.srcLangCombox.Location = new System.Drawing.Point(110, 140);
            this.srcLangCombox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.srcLangCombox.Name = "srcLangCombox";
            this.srcLangCombox.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.srcLangCombox.RectWidth = 1;
            this.srcLangCombox.SelectedIndex = -1;
            this.srcLangCombox.SelectedValue = "";
            this.srcLangCombox.Size = new System.Drawing.Size(254, 41);
            this.srcLangCombox.Source = null;
            this.srcLangCombox.TabIndex = 12;
            this.srcLangCombox.TextValue = null;
            this.srcLangCombox.TriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            // 
            // dstLangCombox
            // 
            this.dstLangCombox.BackColor = System.Drawing.Color.Transparent;
            this.dstLangCombox.BackColorExt = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dstLangCombox.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.dstLangCombox.ConerRadius = 5;
            this.dstLangCombox.DropPanelHeight = -1;
            this.dstLangCombox.FillColor = System.Drawing.Color.White;
            this.dstLangCombox.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dstLangCombox.IsRadius = true;
            this.dstLangCombox.IsShowRect = true;
            this.dstLangCombox.ItemWidth = 70;
            this.dstLangCombox.Location = new System.Drawing.Point(110, 202);
            this.dstLangCombox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dstLangCombox.Name = "dstLangCombox";
            this.dstLangCombox.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dstLangCombox.RectWidth = 1;
            this.dstLangCombox.SelectedIndex = -1;
            this.dstLangCombox.SelectedValue = "";
            this.dstLangCombox.Size = new System.Drawing.Size(254, 41);
            this.dstLangCombox.Source = null;
            this.dstLangCombox.TabIndex = 14;
            this.dstLangCombox.TextValue = null;
            this.dstLangCombox.TriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.BackColor = System.Drawing.Color.White;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel3.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.Location = new System.Drawing.Point(26, 211);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(84, 20);
            this.materialLabel3.TabIndex = 13;
            this.materialLabel3.Text = "目标语言：";
            this.materialLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialLabel4
            // 
            this.materialLabel4.AutoSize = true;
            this.materialLabel4.BackColor = System.Drawing.Color.White;
            this.materialLabel4.Depth = 0;
            this.materialLabel4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel4.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel4.Location = new System.Drawing.Point(26, 271);
            this.materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(348, 20);
            this.materialLabel4.TabIndex = 15;
            this.materialLabel4.Text = "注意：某些翻译API可能仅支持一对一语言翻译哦！";
            this.materialLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConfirmLangBtn
            // 
            this.ConfirmLangBtn.BackColor = System.Drawing.Color.Transparent;
            this.ConfirmLangBtn.BtnBackColor = System.Drawing.Color.White;
            this.ConfirmLangBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ConfirmLangBtn.BtnForeColor = System.Drawing.Color.White;
            this.ConfirmLangBtn.BtnText = "确认";
            this.ConfirmLangBtn.ConerRadius = 5;
            this.ConfirmLangBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConfirmLangBtn.EnabledMouseEffect = true;
            this.ConfirmLangBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ConfirmLangBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ConfirmLangBtn.IsRadius = true;
            this.ConfirmLangBtn.IsShowRect = true;
            this.ConfirmLangBtn.IsShowTips = false;
            this.ConfirmLangBtn.Location = new System.Drawing.Point(220, 315);
            this.ConfirmLangBtn.Margin = new System.Windows.Forms.Padding(0);
            this.ConfirmLangBtn.Name = "ConfirmLangBtn";
            this.ConfirmLangBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ConfirmLangBtn.RectWidth = 1;
            this.ConfirmLangBtn.Size = new System.Drawing.Size(144, 48);
            this.ConfirmLangBtn.TabIndex = 16;
            this.ConfirmLangBtn.TabStop = false;
            this.ConfirmLangBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ConfirmLangBtn.TipsText = "";
            this.ConfirmLangBtn.BtnClick += new System.EventHandler(this.ConfirmLangBtn_BtnClick);
            // 
            // TransLangSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 384);
            this.Controls.Add(this.ConfirmLangBtn);
            this.Controls.Add(this.materialLabel4);
            this.Controls.Add(this.dstLangCombox);
            this.Controls.Add(this.materialLabel3);
            this.Controls.Add(this.srcLangCombox);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.materialLabel2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransLangSettingForm";
            this.Text = "游戏翻译语言设置";
            this.Load += new System.EventHandler(this.TransLangSettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private HZH_Controls.Controls.UCCombox srcLangCombox;
        private HZH_Controls.Controls.UCCombox dstLangCombox;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialLabel materialLabel4;
        private HZH_Controls.Controls.UCBtnExt ConfirmLangBtn;
    }
}