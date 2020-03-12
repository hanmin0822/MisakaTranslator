namespace MisakaTranslator
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SelectGameProcessFunBtn = new HZH_Controls.Controls.UCBtnExt();
            this.BtnHinrLabel = new MaterialSkin.Controls.MaterialLabel();
            this.OCRFunBtn = new HZH_Controls.Controls.UCBtnExt();
            this.SettingsBtn = new HZH_Controls.Controls.UCBtnExt();
            this.AutoStartBtn = new HZH_Controls.Controls.UCBtnExt();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // SelectGameProcessFunBtn
            // 
            this.SelectGameProcessFunBtn.BackColor = System.Drawing.Color.White;
            this.SelectGameProcessFunBtn.BtnBackColor = System.Drawing.Color.White;
            this.SelectGameProcessFunBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SelectGameProcessFunBtn.BtnForeColor = System.Drawing.Color.White;
            this.SelectGameProcessFunBtn.BtnText = "【Hook】选择游戏";
            this.SelectGameProcessFunBtn.ConerRadius = 5;
            this.SelectGameProcessFunBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SelectGameProcessFunBtn.EnabledMouseEffect = true;
            this.SelectGameProcessFunBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.SelectGameProcessFunBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.SelectGameProcessFunBtn.IsRadius = true;
            this.SelectGameProcessFunBtn.IsShowRect = true;
            this.SelectGameProcessFunBtn.IsShowTips = false;
            this.SelectGameProcessFunBtn.Location = new System.Drawing.Point(264, 441);
            this.SelectGameProcessFunBtn.Margin = new System.Windows.Forms.Padding(0);
            this.SelectGameProcessFunBtn.Name = "SelectGameProcessFunBtn";
            this.SelectGameProcessFunBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.SelectGameProcessFunBtn.RectWidth = 1;
            this.SelectGameProcessFunBtn.Size = new System.Drawing.Size(177, 72);
            this.SelectGameProcessFunBtn.TabIndex = 0;
            this.SelectGameProcessFunBtn.TabStop = false;
            this.SelectGameProcessFunBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.SelectGameProcessFunBtn.TipsText = "";
            this.SelectGameProcessFunBtn.BtnClick += new System.EventHandler(this.SelectGameProcessFunBtn_BtnClick);
            this.SelectGameProcessFunBtn.MouseEffecting += new System.EventHandler(this.SelectGameProcessFunBtn_MouseEffecting);
            this.SelectGameProcessFunBtn.MouseEffected += new System.EventHandler(this.SelectGameProcessFunBtn_MouseEffected);
            // 
            // BtnHinrLabel
            // 
            this.BtnHinrLabel.BackColor = System.Drawing.Color.White;
            this.BtnHinrLabel.Depth = 0;
            this.BtnHinrLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnHinrLabel.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.BtnHinrLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnHinrLabel.Location = new System.Drawing.Point(35, 530);
            this.BtnHinrLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.BtnHinrLabel.Name = "BtnHinrLabel";
            this.BtnHinrLabel.Size = new System.Drawing.Size(860, 29);
            this.BtnHinrLabel.TabIndex = 6;
            this.BtnHinrLabel.Text = "MisakaTranslator 致力于为您提供更好的翻译";
            this.BtnHinrLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OCRFunBtn
            // 
            this.OCRFunBtn.BackColor = System.Drawing.Color.White;
            this.OCRFunBtn.BtnBackColor = System.Drawing.Color.White;
            this.OCRFunBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OCRFunBtn.BtnForeColor = System.Drawing.Color.White;
            this.OCRFunBtn.BtnText = "【OCR】文字扫描";
            this.OCRFunBtn.ConerRadius = 5;
            this.OCRFunBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OCRFunBtn.EnabledMouseEffect = true;
            this.OCRFunBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.OCRFunBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.OCRFunBtn.IsRadius = true;
            this.OCRFunBtn.IsShowRect = true;
            this.OCRFunBtn.IsShowTips = false;
            this.OCRFunBtn.Location = new System.Drawing.Point(491, 441);
            this.OCRFunBtn.Margin = new System.Windows.Forms.Padding(0);
            this.OCRFunBtn.Name = "OCRFunBtn";
            this.OCRFunBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.OCRFunBtn.RectWidth = 1;
            this.OCRFunBtn.Size = new System.Drawing.Size(177, 72);
            this.OCRFunBtn.TabIndex = 7;
            this.OCRFunBtn.TabStop = false;
            this.OCRFunBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.OCRFunBtn.TipsText = "";
            this.OCRFunBtn.BtnClick += new System.EventHandler(this.OCRFunBtn_BtnClick);
            this.OCRFunBtn.MouseEffecting += new System.EventHandler(this.OCRFunBtn_MouseEffecting);
            this.OCRFunBtn.MouseEffected += new System.EventHandler(this.OCRFunBtn_MouseEffected);
            // 
            // SettingsBtn
            // 
            this.SettingsBtn.BackColor = System.Drawing.Color.White;
            this.SettingsBtn.BtnBackColor = System.Drawing.Color.White;
            this.SettingsBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SettingsBtn.BtnForeColor = System.Drawing.Color.White;
            this.SettingsBtn.BtnText = "本体设置＆关于";
            this.SettingsBtn.ConerRadius = 5;
            this.SettingsBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingsBtn.EnabledMouseEffect = true;
            this.SettingsBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.SettingsBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.SettingsBtn.IsRadius = true;
            this.SettingsBtn.IsShowRect = true;
            this.SettingsBtn.IsShowTips = false;
            this.SettingsBtn.Location = new System.Drawing.Point(718, 441);
            this.SettingsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsBtn.Name = "SettingsBtn";
            this.SettingsBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.SettingsBtn.RectWidth = 1;
            this.SettingsBtn.Size = new System.Drawing.Size(177, 72);
            this.SettingsBtn.TabIndex = 8;
            this.SettingsBtn.TabStop = false;
            this.SettingsBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.SettingsBtn.TipsText = "";
            this.SettingsBtn.BtnClick += new System.EventHandler(this.SettingsBtn_BtnClick);
            this.SettingsBtn.MouseEffecting += new System.EventHandler(this.SettingsBtn_MouseEffecting);
            this.SettingsBtn.MouseEffected += new System.EventHandler(this.SettingsBtn_MouseEffected);
            // 
            // AutoStartBtn
            // 
            this.AutoStartBtn.BackColor = System.Drawing.Color.White;
            this.AutoStartBtn.BtnBackColor = System.Drawing.Color.White;
            this.AutoStartBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AutoStartBtn.BtnForeColor = System.Drawing.Color.White;
            this.AutoStartBtn.BtnText = "自动开始游戏";
            this.AutoStartBtn.ConerRadius = 5;
            this.AutoStartBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AutoStartBtn.EnabledMouseEffect = true;
            this.AutoStartBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.AutoStartBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.AutoStartBtn.IsRadius = true;
            this.AutoStartBtn.IsShowRect = true;
            this.AutoStartBtn.IsShowTips = false;
            this.AutoStartBtn.Location = new System.Drawing.Point(39, 441);
            this.AutoStartBtn.Margin = new System.Windows.Forms.Padding(0);
            this.AutoStartBtn.Name = "AutoStartBtn";
            this.AutoStartBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.AutoStartBtn.RectWidth = 1;
            this.AutoStartBtn.Size = new System.Drawing.Size(177, 72);
            this.AutoStartBtn.TabIndex = 9;
            this.AutoStartBtn.TabStop = false;
            this.AutoStartBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.AutoStartBtn.TipsText = "";
            this.AutoStartBtn.BtnClick += new System.EventHandler(this.AutoStartBtn_BtnClick);
            this.AutoStartBtn.MouseEffecting += new System.EventHandler(this.AutoStartBtn_MouseEffecting);
            this.AutoStartBtn.MouseEffected += new System.EventHandler(this.AutoStartBtn_MouseEffected);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::MisakaTranslator.Properties.Resources.Background;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(39, 82);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(856, 344);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(935, 568);
            this.Controls.Add(this.AutoStartBtn);
            this.Controls.Add(this.SettingsBtn);
            this.Controls.Add(this.OCRFunBtn);
            this.Controls.Add(this.BtnHinrLabel);
            this.Controls.Add(this.SelectGameProcessFunBtn);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Sizable = false;
            this.Text = "御坂翻译器 MisakaTranslator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private HZH_Controls.Controls.UCBtnExt SelectGameProcessFunBtn;
        private MaterialSkin.Controls.MaterialLabel BtnHinrLabel;
        private HZH_Controls.Controls.UCBtnExt OCRFunBtn;
        private HZH_Controls.Controls.UCBtnExt SettingsBtn;
        private HZH_Controls.Controls.UCBtnExt AutoStartBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}