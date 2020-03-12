namespace MisakaTranslator
{
    partial class TranslateFrmSettingsForm
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
            this.OpacityTrackBar = new HZH_Controls.Controls.UCTrackBar();
            this.BtnHinrLabel = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.TextSizeBox = new HZH_Controls.Controls.UCNumTextBox();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.FontsCombox = new MaterialSkin.Controls.MaterialComboBox();
            this.ColorRTrackBar = new HZH_Controls.Controls.UCTrackBar();
            this.ColorGTrackBar = new HZH_Controls.Controls.UCTrackBar();
            this.ColorBTrackBar = new HZH_Controls.Controls.UCTrackBar();
            this.materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel5 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel6 = new MaterialSkin.Controls.MaterialLabel();
            this.PartCombox = new MaterialSkin.Controls.MaterialComboBox();
            this.materialLabel7 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel8 = new MaterialSkin.Controls.MaterialLabel();
            this.ChooseColorBtn = new HZH_Controls.Controls.UCBtnExt();
            this.WinColorDialog = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // OpacityTrackBar
            // 
            this.OpacityTrackBar.BackColor = System.Drawing.Color.White;
            this.OpacityTrackBar.DcimalDigits = 0;
            this.OpacityTrackBar.IsShowTips = false;
            this.OpacityTrackBar.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.OpacityTrackBar.LineWidth = 10F;
            this.OpacityTrackBar.Location = new System.Drawing.Point(12, 108);
            this.OpacityTrackBar.MaxValue = 100F;
            this.OpacityTrackBar.MinValue = 1F;
            this.OpacityTrackBar.Name = "OpacityTrackBar";
            this.OpacityTrackBar.Size = new System.Drawing.Size(272, 30);
            this.OpacityTrackBar.TabIndex = 0;
            this.OpacityTrackBar.Text = "ucTrackBar1";
            this.OpacityTrackBar.TipsFormat = null;
            this.OpacityTrackBar.Value = 0F;
            this.OpacityTrackBar.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.OpacityTrackBar.ValueChanged += new System.EventHandler(this.OpacityTrackBar_ValueChanged);
            // 
            // BtnHinrLabel
            // 
            this.BtnHinrLabel.AutoSize = true;
            this.BtnHinrLabel.BackColor = System.Drawing.Color.White;
            this.BtnHinrLabel.Depth = 0;
            this.BtnHinrLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnHinrLabel.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.BtnHinrLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnHinrLabel.Location = new System.Drawing.Point(12, 85);
            this.BtnHinrLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.BtnHinrLabel.Name = "BtnHinrLabel";
            this.BtnHinrLabel.Size = new System.Drawing.Size(114, 20);
            this.BtnHinrLabel.TabIndex = 8;
            this.BtnHinrLabel.Text = "翻译窗口透明度";
            this.BtnHinrLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.BackColor = System.Drawing.Color.White;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(12, 268);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(69, 20);
            this.materialLabel1.TabIndex = 9;
            this.materialLabel1.Text = "文字字体";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.BackColor = System.Drawing.Color.White;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel2.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(12, 335);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(69, 20);
            this.materialLabel2.TabIndex = 11;
            this.materialLabel2.Text = "文字大小";
            this.materialLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextSizeBox
            // 
            this.TextSizeBox.BackColor = System.Drawing.Color.White;
            this.TextSizeBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TextSizeBox.InputType = HZH_Controls.TextInputType.Number;
            this.TextSizeBox.IsNumCanInput = true;
            this.TextSizeBox.KeyBoardType = HZH_Controls.Controls.KeyBoardType.UCKeyBorderNum;
            this.TextSizeBox.Location = new System.Drawing.Point(12, 358);
            this.TextSizeBox.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TextSizeBox.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TextSizeBox.Name = "TextSizeBox";
            this.TextSizeBox.Num = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.TextSizeBox.Padding = new System.Windows.Forms.Padding(2);
            this.TextSizeBox.Size = new System.Drawing.Size(272, 31);
            this.TextSizeBox.TabIndex = 13;
            this.TextSizeBox.NumChanged += new System.EventHandler(this.TextSizeBox_NumChanged);
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.BackColor = System.Drawing.Color.White;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel3.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.Location = new System.Drawing.Point(12, 408);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(69, 20);
            this.materialLabel3.TabIndex = 14;
            this.materialLabel3.Text = "文字颜色";
            this.materialLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FontsCombox
            // 
            this.FontsCombox.Depth = 0;
            this.FontsCombox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.FontsCombox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FontsCombox.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.FontsCombox.FormattingEnabled = true;
            this.FontsCombox.Location = new System.Drawing.Point(12, 291);
            this.FontsCombox.MouseState = MaterialSkin.MouseState.HOVER;
            this.FontsCombox.Name = "FontsCombox";
            this.FontsCombox.Size = new System.Drawing.Size(272, 26);
            this.FontsCombox.TabIndex = 15;
            this.FontsCombox.SelectedIndexChanged += new System.EventHandler(this.FontsCombox_SelectedIndexChanged);
            // 
            // ColorRTrackBar
            // 
            this.ColorRTrackBar.BackColor = System.Drawing.Color.White;
            this.ColorRTrackBar.DcimalDigits = 0;
            this.ColorRTrackBar.IsShowTips = true;
            this.ColorRTrackBar.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.ColorRTrackBar.LineWidth = 10F;
            this.ColorRTrackBar.Location = new System.Drawing.Point(38, 431);
            this.ColorRTrackBar.MaxValue = 255F;
            this.ColorRTrackBar.MinValue = 0F;
            this.ColorRTrackBar.Name = "ColorRTrackBar";
            this.ColorRTrackBar.Size = new System.Drawing.Size(246, 30);
            this.ColorRTrackBar.TabIndex = 16;
            this.ColorRTrackBar.Text = "ColorRTrackBar";
            this.ColorRTrackBar.TipsFormat = null;
            this.ColorRTrackBar.Value = 0F;
            this.ColorRTrackBar.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ColorRTrackBar.ValueChanged += new System.EventHandler(this.ColorRTrackBar_ValueChanged);
            // 
            // ColorGTrackBar
            // 
            this.ColorGTrackBar.BackColor = System.Drawing.Color.White;
            this.ColorGTrackBar.DcimalDigits = 0;
            this.ColorGTrackBar.IsShowTips = true;
            this.ColorGTrackBar.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.ColorGTrackBar.LineWidth = 10F;
            this.ColorGTrackBar.Location = new System.Drawing.Point(38, 467);
            this.ColorGTrackBar.MaxValue = 255F;
            this.ColorGTrackBar.MinValue = 0F;
            this.ColorGTrackBar.Name = "ColorGTrackBar";
            this.ColorGTrackBar.Size = new System.Drawing.Size(246, 30);
            this.ColorGTrackBar.TabIndex = 17;
            this.ColorGTrackBar.Text = "ColorGTrackBar";
            this.ColorGTrackBar.TipsFormat = null;
            this.ColorGTrackBar.Value = 0F;
            this.ColorGTrackBar.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ColorGTrackBar.ValueChanged += new System.EventHandler(this.ColorGTrackBar_ValueChanged);
            // 
            // ColorBTrackBar
            // 
            this.ColorBTrackBar.BackColor = System.Drawing.Color.White;
            this.ColorBTrackBar.DcimalDigits = 0;
            this.ColorBTrackBar.IsShowTips = true;
            this.ColorBTrackBar.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.ColorBTrackBar.LineWidth = 10F;
            this.ColorBTrackBar.Location = new System.Drawing.Point(38, 503);
            this.ColorBTrackBar.MaxValue = 255F;
            this.ColorBTrackBar.MinValue = 0F;
            this.ColorBTrackBar.Name = "ColorBTrackBar";
            this.ColorBTrackBar.Size = new System.Drawing.Size(246, 30);
            this.ColorBTrackBar.TabIndex = 18;
            this.ColorBTrackBar.Text = "ColorBTrackBar";
            this.ColorBTrackBar.TipsFormat = null;
            this.ColorBTrackBar.Value = 0F;
            this.ColorBTrackBar.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ColorBTrackBar.ValueChanged += new System.EventHandler(this.ColorBTrackBar_ValueChanged);
            // 
            // materialLabel4
            // 
            this.materialLabel4.AutoSize = true;
            this.materialLabel4.BackColor = System.Drawing.Color.White;
            this.materialLabel4.Depth = 0;
            this.materialLabel4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel4.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel4.Location = new System.Drawing.Point(17, 436);
            this.materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(19, 20);
            this.materialLabel4.TabIndex = 19;
            this.materialLabel4.Text = "R";
            this.materialLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialLabel5
            // 
            this.materialLabel5.AutoSize = true;
            this.materialLabel5.BackColor = System.Drawing.Color.White;
            this.materialLabel5.Depth = 0;
            this.materialLabel5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel5.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel5.Location = new System.Drawing.Point(17, 472);
            this.materialLabel5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel5.Name = "materialLabel5";
            this.materialLabel5.Size = new System.Drawing.Size(20, 20);
            this.materialLabel5.TabIndex = 20;
            this.materialLabel5.Text = "G";
            this.materialLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialLabel6
            // 
            this.materialLabel6.AutoSize = true;
            this.materialLabel6.BackColor = System.Drawing.Color.White;
            this.materialLabel6.Depth = 0;
            this.materialLabel6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel6.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel6.Location = new System.Drawing.Point(18, 508);
            this.materialLabel6.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel6.Name = "materialLabel6";
            this.materialLabel6.Size = new System.Drawing.Size(18, 20);
            this.materialLabel6.TabIndex = 21;
            this.materialLabel6.Text = "B";
            this.materialLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PartCombox
            // 
            this.PartCombox.Depth = 0;
            this.PartCombox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.PartCombox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PartCombox.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.PartCombox.FormattingEnabled = true;
            this.PartCombox.Location = new System.Drawing.Point(12, 230);
            this.PartCombox.MouseState = MaterialSkin.MouseState.HOVER;
            this.PartCombox.Name = "PartCombox";
            this.PartCombox.Size = new System.Drawing.Size(272, 26);
            this.PartCombox.TabIndex = 23;
            this.PartCombox.SelectedIndexChanged += new System.EventHandler(this.PartCombox_SelectedIndexChanged);
            // 
            // materialLabel7
            // 
            this.materialLabel7.AutoSize = true;
            this.materialLabel7.BackColor = System.Drawing.Color.White;
            this.materialLabel7.Depth = 0;
            this.materialLabel7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel7.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel7.Location = new System.Drawing.Point(12, 207);
            this.materialLabel7.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel7.Name = "materialLabel7";
            this.materialLabel7.Size = new System.Drawing.Size(69, 20);
            this.materialLabel7.TabIndex = 22;
            this.materialLabel7.Text = "设置部分";
            this.materialLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialLabel8
            // 
            this.materialLabel8.AutoSize = true;
            this.materialLabel8.BackColor = System.Drawing.Color.White;
            this.materialLabel8.Depth = 0;
            this.materialLabel8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel8.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel8.Location = new System.Drawing.Point(12, 145);
            this.materialLabel8.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel8.Name = "materialLabel8";
            this.materialLabel8.Size = new System.Drawing.Size(114, 20);
            this.materialLabel8.TabIndex = 24;
            this.materialLabel8.Text = "翻译窗口背景色";
            this.materialLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChooseColorBtn
            // 
            this.ChooseColorBtn.BackColor = System.Drawing.Color.Transparent;
            this.ChooseColorBtn.BtnBackColor = System.Drawing.Color.White;
            this.ChooseColorBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChooseColorBtn.BtnForeColor = System.Drawing.Color.White;
            this.ChooseColorBtn.BtnText = "选择颜色";
            this.ChooseColorBtn.ConerRadius = 5;
            this.ChooseColorBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ChooseColorBtn.EnabledMouseEffect = true;
            this.ChooseColorBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ChooseColorBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ChooseColorBtn.IsRadius = true;
            this.ChooseColorBtn.IsShowRect = true;
            this.ChooseColorBtn.IsShowTips = false;
            this.ChooseColorBtn.Location = new System.Drawing.Point(62, 169);
            this.ChooseColorBtn.Margin = new System.Windows.Forms.Padding(0);
            this.ChooseColorBtn.Name = "ChooseColorBtn";
            this.ChooseColorBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ChooseColorBtn.RectWidth = 1;
            this.ChooseColorBtn.Size = new System.Drawing.Size(168, 29);
            this.ChooseColorBtn.TabIndex = 25;
            this.ChooseColorBtn.TabStop = false;
            this.ChooseColorBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ChooseColorBtn.TipsText = "";
            this.ChooseColorBtn.BtnClick += new System.EventHandler(this.ChooseColorBtn_BtnClick);
            // 
            // TranslateFrmSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 548);
            this.Controls.Add(this.ChooseColorBtn);
            this.Controls.Add(this.materialLabel8);
            this.Controls.Add(this.PartCombox);
            this.Controls.Add(this.materialLabel7);
            this.Controls.Add(this.materialLabel6);
            this.Controls.Add(this.materialLabel5);
            this.Controls.Add(this.materialLabel4);
            this.Controls.Add(this.ColorBTrackBar);
            this.Controls.Add(this.ColorGTrackBar);
            this.Controls.Add(this.ColorRTrackBar);
            this.Controls.Add(this.FontsCombox);
            this.Controls.Add(this.materialLabel3);
            this.Controls.Add(this.TextSizeBox);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.BtnHinrLabel);
            this.Controls.Add(this.OpacityTrackBar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TranslateFrmSettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Sizable = false;
            this.Text = "翻译窗口设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TranslateFrmSettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.TranslateFrmSettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HZH_Controls.Controls.UCTrackBar OpacityTrackBar;
        private MaterialSkin.Controls.MaterialLabel BtnHinrLabel;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private HZH_Controls.Controls.UCNumTextBox TextSizeBox;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialComboBox FontsCombox;
        private HZH_Controls.Controls.UCTrackBar ColorRTrackBar;
        private HZH_Controls.Controls.UCTrackBar ColorGTrackBar;
        private HZH_Controls.Controls.UCTrackBar ColorBTrackBar;
        private MaterialSkin.Controls.MaterialLabel materialLabel4;
        private MaterialSkin.Controls.MaterialLabel materialLabel5;
        private MaterialSkin.Controls.MaterialLabel materialLabel6;
        private MaterialSkin.Controls.MaterialComboBox PartCombox;
        private MaterialSkin.Controls.MaterialLabel materialLabel7;
        private MaterialSkin.Controls.MaterialLabel materialLabel8;
        private HZH_Controls.Controls.UCBtnExt ChooseColorBtn;
        private System.Windows.Forms.ColorDialog WinColorDialog;
    }
}