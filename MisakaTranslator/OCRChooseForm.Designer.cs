namespace MisakaTranslator
{
    partial class OCRChooseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OCRChooseForm));
            this.ScreenCaptureBtn = new HZH_Controls.Controls.UCBtnExt();
            this.WindowChooseBtn = new HZH_Controls.Controls.UCBtnExt();
            this.ProcessInfoLabel = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.renovateORCPicBtn = new HZH_Controls.Controls.UCBtnExt();
            this.OCRConfirmBtn = new HZH_Controls.Controls.UCBtnExt();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.srcLangCombox = new HZH_Controls.Controls.UCCombox();
            this.TestOCRBtn = new HZH_Controls.Controls.UCBtnExt();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.delaySetBox = new HZH_Controls.Controls.UCTextBoxEx();
            this.AllWinCheckBox = new HZH_Controls.Controls.UCCheckBox();
            this.PreviewBox = new System.Windows.Forms.PictureBox();
            this.PreHandleCheckBox = new HZH_Controls.Controls.UCCheckBox();
            this.threshTrackBar = new HZH_Controls.Controls.UCTrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ScreenCaptureBtn
            // 
            this.ScreenCaptureBtn.BackColor = System.Drawing.Color.White;
            this.ScreenCaptureBtn.BtnBackColor = System.Drawing.Color.White;
            this.ScreenCaptureBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ScreenCaptureBtn.BtnForeColor = System.Drawing.Color.White;
            this.ScreenCaptureBtn.BtnText = "选择识别区域";
            this.ScreenCaptureBtn.ConerRadius = 5;
            this.ScreenCaptureBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ScreenCaptureBtn.EnabledMouseEffect = true;
            this.ScreenCaptureBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ScreenCaptureBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ScreenCaptureBtn.IsRadius = true;
            this.ScreenCaptureBtn.IsShowRect = true;
            this.ScreenCaptureBtn.IsShowTips = false;
            this.ScreenCaptureBtn.Location = new System.Drawing.Point(23, 269);
            this.ScreenCaptureBtn.Margin = new System.Windows.Forms.Padding(0);
            this.ScreenCaptureBtn.Name = "ScreenCaptureBtn";
            this.ScreenCaptureBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ScreenCaptureBtn.RectWidth = 1;
            this.ScreenCaptureBtn.Size = new System.Drawing.Size(171, 41);
            this.ScreenCaptureBtn.TabIndex = 0;
            this.ScreenCaptureBtn.TabStop = false;
            this.ScreenCaptureBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ScreenCaptureBtn.TipsText = "";
            this.ScreenCaptureBtn.BtnClick += new System.EventHandler(this.ScreenCaptureBtn_BtnClick);
            // 
            // WindowChooseBtn
            // 
            this.WindowChooseBtn.BackColor = System.Drawing.Color.White;
            this.WindowChooseBtn.BtnBackColor = System.Drawing.Color.White;
            this.WindowChooseBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WindowChooseBtn.BtnForeColor = System.Drawing.Color.White;
            this.WindowChooseBtn.BtnText = "选择窗口";
            this.WindowChooseBtn.ConerRadius = 5;
            this.WindowChooseBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WindowChooseBtn.EnabledMouseEffect = true;
            this.WindowChooseBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.WindowChooseBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.WindowChooseBtn.IsRadius = true;
            this.WindowChooseBtn.IsShowRect = true;
            this.WindowChooseBtn.IsShowTips = false;
            this.WindowChooseBtn.Location = new System.Drawing.Point(152, 164);
            this.WindowChooseBtn.Margin = new System.Windows.Forms.Padding(0);
            this.WindowChooseBtn.Name = "WindowChooseBtn";
            this.WindowChooseBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.WindowChooseBtn.RectWidth = 1;
            this.WindowChooseBtn.Size = new System.Drawing.Size(158, 43);
            this.WindowChooseBtn.TabIndex = 1;
            this.WindowChooseBtn.TabStop = false;
            this.WindowChooseBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.WindowChooseBtn.TipsText = "";
            this.WindowChooseBtn.BtnClick += new System.EventHandler(this.WindowChooseBtn_BtnClick);
            // 
            // ProcessInfoLabel
            // 
            this.ProcessInfoLabel.BackColor = System.Drawing.Color.White;
            this.ProcessInfoLabel.Depth = 0;
            this.ProcessInfoLabel.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.ProcessInfoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ProcessInfoLabel.Location = new System.Drawing.Point(323, 164);
            this.ProcessInfoLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.ProcessInfoLabel.Name = "ProcessInfoLabel";
            this.ProcessInfoLabel.Size = new System.Drawing.Size(441, 43);
            this.ProcessInfoLabel.TabIndex = 2;
            this.ProcessInfoLabel.Text = "等待选择窗口";
            this.ProcessInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.BackColor = System.Drawing.Color.White;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(23, 84);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(503, 60);
            this.materialLabel1.TabIndex = 3;
            this.materialLabel1.Text = "您可以在这个界面进行OCR的相关设置\r\n首先选择到游戏界面(建议点击游戏画面)或使用全屏选择,再框选出识别区域\r\n之后进行设置，在下方预览框中查看效果，确认无误后" +
    "点击完成设置";
            // 
            // renovateORCPicBtn
            // 
            this.renovateORCPicBtn.BackColor = System.Drawing.Color.White;
            this.renovateORCPicBtn.BtnBackColor = System.Drawing.Color.White;
            this.renovateORCPicBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.renovateORCPicBtn.BtnForeColor = System.Drawing.Color.White;
            this.renovateORCPicBtn.BtnText = "刷新显示OCR区域";
            this.renovateORCPicBtn.ConerRadius = 5;
            this.renovateORCPicBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.renovateORCPicBtn.EnabledMouseEffect = true;
            this.renovateORCPicBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.renovateORCPicBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.renovateORCPicBtn.IsRadius = true;
            this.renovateORCPicBtn.IsShowRect = true;
            this.renovateORCPicBtn.IsShowTips = false;
            this.renovateORCPicBtn.Location = new System.Drawing.Point(304, 269);
            this.renovateORCPicBtn.Margin = new System.Windows.Forms.Padding(0);
            this.renovateORCPicBtn.Name = "renovateORCPicBtn";
            this.renovateORCPicBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.renovateORCPicBtn.RectWidth = 1;
            this.renovateORCPicBtn.Size = new System.Drawing.Size(172, 41);
            this.renovateORCPicBtn.TabIndex = 5;
            this.renovateORCPicBtn.TabStop = false;
            this.renovateORCPicBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.renovateORCPicBtn.TipsText = "";
            this.renovateORCPicBtn.BtnClick += new System.EventHandler(this.renovateORCPicBtn_BtnClick);
            // 
            // OCRConfirmBtn
            // 
            this.OCRConfirmBtn.BackColor = System.Drawing.Color.White;
            this.OCRConfirmBtn.BtnBackColor = System.Drawing.Color.White;
            this.OCRConfirmBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OCRConfirmBtn.BtnForeColor = System.Drawing.Color.White;
            this.OCRConfirmBtn.BtnText = "完成设置";
            this.OCRConfirmBtn.ConerRadius = 5;
            this.OCRConfirmBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OCRConfirmBtn.EnabledMouseEffect = true;
            this.OCRConfirmBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.OCRConfirmBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.OCRConfirmBtn.IsRadius = true;
            this.OCRConfirmBtn.IsShowRect = true;
            this.OCRConfirmBtn.IsShowTips = false;
            this.OCRConfirmBtn.Location = new System.Drawing.Point(606, 507);
            this.OCRConfirmBtn.Margin = new System.Windows.Forms.Padding(0);
            this.OCRConfirmBtn.Name = "OCRConfirmBtn";
            this.OCRConfirmBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.OCRConfirmBtn.RectWidth = 1;
            this.OCRConfirmBtn.Size = new System.Drawing.Size(158, 51);
            this.OCRConfirmBtn.TabIndex = 6;
            this.OCRConfirmBtn.TabStop = false;
            this.OCRConfirmBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.OCRConfirmBtn.TipsText = "";
            this.OCRConfirmBtn.BtnClick += new System.EventHandler(this.OCRConfirmBtn_BtnClick);
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.BackColor = System.Drawing.Color.White;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.materialLabel2.Location = new System.Drawing.Point(568, 227);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(69, 20);
            this.materialLabel2.TabIndex = 7;
            this.materialLabel2.Text = "源语言：";
            this.materialLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.srcLangCombox.Location = new System.Drawing.Point(644, 221);
            this.srcLangCombox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.srcLangCombox.Name = "srcLangCombox";
            this.srcLangCombox.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.srcLangCombox.RectWidth = 1;
            this.srcLangCombox.SelectedIndex = -1;
            this.srcLangCombox.SelectedValue = "";
            this.srcLangCombox.Size = new System.Drawing.Size(120, 34);
            this.srcLangCombox.Source = null;
            this.srcLangCombox.TabIndex = 8;
            this.srcLangCombox.TextValue = null;
            this.srcLangCombox.TriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            // 
            // TestOCRBtn
            // 
            this.TestOCRBtn.BackColor = System.Drawing.Color.White;
            this.TestOCRBtn.BtnBackColor = System.Drawing.Color.White;
            this.TestOCRBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestOCRBtn.BtnForeColor = System.Drawing.Color.White;
            this.TestOCRBtn.BtnText = "测试OCR工作情况";
            this.TestOCRBtn.ConerRadius = 5;
            this.TestOCRBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TestOCRBtn.EnabledMouseEffect = true;
            this.TestOCRBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.TestOCRBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.TestOCRBtn.IsRadius = true;
            this.TestOCRBtn.IsShowRect = true;
            this.TestOCRBtn.IsShowTips = false;
            this.TestOCRBtn.Location = new System.Drawing.Point(592, 269);
            this.TestOCRBtn.Margin = new System.Windows.Forms.Padding(0);
            this.TestOCRBtn.Name = "TestOCRBtn";
            this.TestOCRBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.TestOCRBtn.RectWidth = 1;
            this.TestOCRBtn.Size = new System.Drawing.Size(172, 41);
            this.TestOCRBtn.TabIndex = 9;
            this.TestOCRBtn.TabStop = false;
            this.TestOCRBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.TestOCRBtn.TipsText = "";
            this.TestOCRBtn.BtnClick += new System.EventHandler(this.TestOCRBtn_BtnClick);
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.BackColor = System.Drawing.Color.White;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.materialLabel3.Location = new System.Drawing.Point(23, 227);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(370, 20);
            this.materialLabel3.TabIndex = 10;
            this.materialLabel3.Text = "刷新延迟(鼠标左键单击后隔多久进行OCR，单位:毫秒)";
            this.materialLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // delaySetBox
            // 
            this.delaySetBox.BackColor = System.Drawing.Color.Transparent;
            this.delaySetBox.ConerRadius = 5;
            this.delaySetBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.delaySetBox.DecLength = 2;
            this.delaySetBox.FillColor = System.Drawing.Color.Empty;
            this.delaySetBox.FocusBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.delaySetBox.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.delaySetBox.InputText = "2000";
            this.delaySetBox.InputType = HZH_Controls.TextInputType.Number;
            this.delaySetBox.IsFocusColor = true;
            this.delaySetBox.IsRadius = true;
            this.delaySetBox.IsShowClearBtn = true;
            this.delaySetBox.IsShowKeyboard = false;
            this.delaySetBox.IsShowRect = true;
            this.delaySetBox.IsShowSearchBtn = false;
            this.delaySetBox.KeyBoardType = HZH_Controls.Controls.KeyBoardType.UCKeyBorderAll_EN;
            this.delaySetBox.Location = new System.Drawing.Point(399, 221);
            this.delaySetBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.delaySetBox.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.delaySetBox.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.delaySetBox.Name = "delaySetBox";
            this.delaySetBox.Padding = new System.Windows.Forms.Padding(5);
            this.delaySetBox.PromptColor = System.Drawing.Color.Gray;
            this.delaySetBox.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.delaySetBox.PromptText = "";
            this.delaySetBox.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.delaySetBox.RectWidth = 1;
            this.delaySetBox.RegexPattern = "";
            this.delaySetBox.Size = new System.Drawing.Size(97, 34);
            this.delaySetBox.TabIndex = 11;
            // 
            // AllWinCheckBox
            // 
            this.AllWinCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.AllWinCheckBox.Checked = false;
            this.AllWinCheckBox.Location = new System.Drawing.Point(22, 164);
            this.AllWinCheckBox.Name = "AllWinCheckBox";
            this.AllWinCheckBox.Padding = new System.Windows.Forms.Padding(1);
            this.AllWinCheckBox.Size = new System.Drawing.Size(125, 43);
            this.AllWinCheckBox.TabIndex = 12;
            this.AllWinCheckBox.TextValue = "全屏截取";
            this.AllWinCheckBox.CheckedChangeEvent += new System.EventHandler(this.AllWinCheckBox_CheckedChangeEvent);
            // 
            // PreviewBox
            // 
            this.PreviewBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PreviewBox.Location = new System.Drawing.Point(23, 323);
            this.PreviewBox.Name = "PreviewBox";
            this.PreviewBox.Size = new System.Drawing.Size(741, 138);
            this.PreviewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PreviewBox.TabIndex = 4;
            this.PreviewBox.TabStop = false;
            // 
            // PreHandleCheckBox
            // 
            this.PreHandleCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.PreHandleCheckBox.Checked = false;
            this.PreHandleCheckBox.Location = new System.Drawing.Point(22, 471);
            this.PreHandleCheckBox.Name = "PreHandleCheckBox";
            this.PreHandleCheckBox.Padding = new System.Windows.Forms.Padding(1);
            this.PreHandleCheckBox.Size = new System.Drawing.Size(125, 26);
            this.PreHandleCheckBox.TabIndex = 13;
            this.PreHandleCheckBox.TextValue = "二值化处理";
            this.PreHandleCheckBox.CheckedChangeEvent += new System.EventHandler(this.PreHandleCheckBox_CheckedChangeEvent);
            // 
            // threshTrackBar
            // 
            this.threshTrackBar.BackColor = System.Drawing.Color.White;
            this.threshTrackBar.DcimalDigits = 0;
            this.threshTrackBar.IsShowTips = true;
            this.threshTrackBar.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.threshTrackBar.LineWidth = 10F;
            this.threshTrackBar.Location = new System.Drawing.Point(139, 471);
            this.threshTrackBar.MaxValue = 255F;
            this.threshTrackBar.MinValue = 0F;
            this.threshTrackBar.Name = "threshTrackBar";
            this.threshTrackBar.Size = new System.Drawing.Size(624, 26);
            this.threshTrackBar.TabIndex = 14;
            this.threshTrackBar.Text = "ucTrackBar1";
            this.threshTrackBar.TipsFormat = null;
            this.threshTrackBar.Value = 120F;
            this.threshTrackBar.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.threshTrackBar.ValueChanged += new System.EventHandler(this.threshTrackBar_ValueChanged);
            // 
            // OCRChooseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 576);
            this.Controls.Add(this.threshTrackBar);
            this.Controls.Add(this.PreHandleCheckBox);
            this.Controls.Add(this.AllWinCheckBox);
            this.Controls.Add(this.delaySetBox);
            this.Controls.Add(this.materialLabel3);
            this.Controls.Add(this.TestOCRBtn);
            this.Controls.Add(this.srcLangCombox);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.OCRConfirmBtn);
            this.Controls.Add(this.renovateORCPicBtn);
            this.Controls.Add(this.PreviewBox);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.ProcessInfoLabel);
            this.Controls.Add(this.WindowChooseBtn);
            this.Controls.Add(this.ScreenCaptureBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OCRChooseForm";
            this.Text = "OCR相关设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OCRChooseForm_FormClosing);
            this.Load += new System.EventHandler(this.OCRChooseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HZH_Controls.Controls.UCBtnExt ScreenCaptureBtn;
        private HZH_Controls.Controls.UCBtnExt WindowChooseBtn;
        private MaterialSkin.Controls.MaterialLabel ProcessInfoLabel;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private System.Windows.Forms.PictureBox PreviewBox;
        private HZH_Controls.Controls.UCBtnExt renovateORCPicBtn;
        private HZH_Controls.Controls.UCBtnExt OCRConfirmBtn;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private HZH_Controls.Controls.UCCombox srcLangCombox;
        private HZH_Controls.Controls.UCBtnExt TestOCRBtn;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private HZH_Controls.Controls.UCTextBoxEx delaySetBox;
        private HZH_Controls.Controls.UCCheckBox AllWinCheckBox;
        private HZH_Controls.Controls.UCCheckBox PreHandleCheckBox;
        private HZH_Controls.Controls.UCTrackBar threshTrackBar;
    }
}