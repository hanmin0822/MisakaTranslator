namespace MisakaTranslator
{
    partial class TextRepeatRepairForm
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
            this.SourceTextBox = new HZH_Controls.Controls.TextBoxEx();
            this.DesTextBox = new HZH_Controls.Controls.TextBoxEx();
            this.BtnHinrLabel = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.FunctionSelectCombox = new HZH_Controls.Controls.UCCombox();
            this.ConfirmBtn = new HZH_Controls.Controls.UCBtnExt();
            this.SuspendLayout();
            // 
            // SourceTextBox
            // 
            this.SourceTextBox.DecLength = 2;
            this.SourceTextBox.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SourceTextBox.InputType = HZH_Controls.TextInputType.NotControl;
            this.SourceTextBox.Location = new System.Drawing.Point(19, 211);
            this.SourceTextBox.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.SourceTextBox.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.SourceTextBox.Multiline = true;
            this.SourceTextBox.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.SourceTextBox.Name = "SourceTextBox";
            this.SourceTextBox.OldText = null;
            this.SourceTextBox.PromptColor = System.Drawing.Color.Gray;
            this.SourceTextBox.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.SourceTextBox.PromptText = "";
            this.SourceTextBox.RegexPattern = "";
            this.SourceTextBox.Size = new System.Drawing.Size(675, 110);
            this.SourceTextBox.TabIndex = 1;
            this.SourceTextBox.TextChanged += new System.EventHandler(this.SourceTextBox_TextChanged);
            // 
            // DesTextBox
            // 
            this.DesTextBox.DecLength = 2;
            this.DesTextBox.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DesTextBox.InputType = HZH_Controls.TextInputType.NotControl;
            this.DesTextBox.Location = new System.Drawing.Point(19, 363);
            this.DesTextBox.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.DesTextBox.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.DesTextBox.Multiline = true;
            this.DesTextBox.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.DesTextBox.Name = "DesTextBox";
            this.DesTextBox.OldText = null;
            this.DesTextBox.PromptColor = System.Drawing.Color.Gray;
            this.DesTextBox.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.DesTextBox.PromptText = "";
            this.DesTextBox.RegexPattern = "";
            this.DesTextBox.Size = new System.Drawing.Size(675, 110);
            this.DesTextBox.TabIndex = 2;
            // 
            // BtnHinrLabel
            // 
            this.BtnHinrLabel.AutoSize = true;
            this.BtnHinrLabel.BackColor = System.Drawing.Color.White;
            this.BtnHinrLabel.Depth = 0;
            this.BtnHinrLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnHinrLabel.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.BtnHinrLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnHinrLabel.Location = new System.Drawing.Point(23, 188);
            this.BtnHinrLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.BtnHinrLabel.Name = "BtnHinrLabel";
            this.BtnHinrLabel.Size = new System.Drawing.Size(69, 20);
            this.BtnHinrLabel.TabIndex = 7;
            this.BtnHinrLabel.Text = "原文本：";
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
            this.materialLabel1.Location = new System.Drawing.Point(23, 340);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(99, 20);
            this.materialLabel1.TabIndex = 8;
            this.materialLabel1.Text = "处理后文本：";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialLabel2
            // 
            this.materialLabel2.BackColor = System.Drawing.Color.White;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel2.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(23, 83);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(671, 44);
            this.materialLabel2.TabIndex = 9;
            this.materialLabel2.Text = "提示：选择一种去重处理方式后，点击右下方\"完成处理\"按钮即可开始正常游戏翻译，如果所有的去重方式都无法匹配您的游戏，您可以联系作者或等待后续版本更新。";
            this.materialLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FunctionSelectCombox
            // 
            this.FunctionSelectCombox.BackColor = System.Drawing.Color.Transparent;
            this.FunctionSelectCombox.BackColorExt = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.FunctionSelectCombox.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.FunctionSelectCombox.ConerRadius = 5;
            this.FunctionSelectCombox.DropPanelHeight = -1;
            this.FunctionSelectCombox.FillColor = System.Drawing.Color.White;
            this.FunctionSelectCombox.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.FunctionSelectCombox.IsRadius = true;
            this.FunctionSelectCombox.IsShowRect = true;
            this.FunctionSelectCombox.ItemWidth = 70;
            this.FunctionSelectCombox.Location = new System.Drawing.Point(19, 140);
            this.FunctionSelectCombox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FunctionSelectCombox.Name = "FunctionSelectCombox";
            this.FunctionSelectCombox.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.FunctionSelectCombox.RectWidth = 1;
            this.FunctionSelectCombox.SelectedIndex = -1;
            this.FunctionSelectCombox.SelectedValue = "";
            this.FunctionSelectCombox.Size = new System.Drawing.Size(675, 33);
            this.FunctionSelectCombox.Source = null;
            this.FunctionSelectCombox.TabIndex = 10;
            this.FunctionSelectCombox.TextValue = null;
            this.FunctionSelectCombox.TriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.FunctionSelectCombox.SelectedChangedEvent += new System.EventHandler(this.FunctionSelectCombox_SelectedChangedEvent);
            // 
            // ConfirmBtn
            // 
            this.ConfirmBtn.BackColor = System.Drawing.Color.White;
            this.ConfirmBtn.BtnBackColor = System.Drawing.Color.White;
            this.ConfirmBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ConfirmBtn.BtnForeColor = System.Drawing.Color.White;
            this.ConfirmBtn.BtnText = "完成处理";
            this.ConfirmBtn.ConerRadius = 5;
            this.ConfirmBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConfirmBtn.EnabledMouseEffect = true;
            this.ConfirmBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ConfirmBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ConfirmBtn.IsRadius = true;
            this.ConfirmBtn.IsShowRect = true;
            this.ConfirmBtn.IsShowTips = false;
            this.ConfirmBtn.Location = new System.Drawing.Point(495, 486);
            this.ConfirmBtn.Margin = new System.Windows.Forms.Padding(0);
            this.ConfirmBtn.Name = "ConfirmBtn";
            this.ConfirmBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ConfirmBtn.RectWidth = 1;
            this.ConfirmBtn.Size = new System.Drawing.Size(198, 41);
            this.ConfirmBtn.TabIndex = 11;
            this.ConfirmBtn.TabStop = false;
            this.ConfirmBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ConfirmBtn.TipsText = "";
            this.ConfirmBtn.BtnClick += new System.EventHandler(this.ConfirmBtn_BtnClick);
            // 
            // TextRepeatRepairForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 544);
            this.Controls.Add(this.ConfirmBtn);
            this.Controls.Add(this.FunctionSelectCombox);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.BtnHinrLabel);
            this.Controls.Add(this.DesTextBox);
            this.Controls.Add(this.SourceTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextRepeatRepairForm";
            this.Text = "文本去重处理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextRepeatRepairForm_FormClosing);
            this.Load += new System.EventHandler(this.TextRepeatRepairForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HZH_Controls.Controls.TextBoxEx SourceTextBox;
        private HZH_Controls.Controls.TextBoxEx DesTextBox;
        private MaterialSkin.Controls.MaterialLabel BtnHinrLabel;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private HZH_Controls.Controls.UCCombox FunctionSelectCombox;
        private HZH_Controls.Controls.UCBtnExt ConfirmBtn;
    }
}