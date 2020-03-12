namespace MisakaTranslator
{
    partial class TextractorFunReConfirmForm
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
            this.components = new System.ComponentModel.Container();
            this.TextractorFunListView = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.InfoLabel = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.FunConfirmBtn = new HZH_Controls.Controls.UCBtnExt();
            this.AlwaysTopCheckBox = new HZH_Controls.Controls.UCCheckBox();
            this.SuspendLayout();
            // 
            // TextractorFunListView
            // 
            this.TextractorFunListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextractorFunListView.CheckBoxes = true;
            this.TextractorFunListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader5,
            this.columnHeader4});
            this.TextractorFunListView.Depth = 0;
            this.TextractorFunListView.FullRowSelect = true;
            this.TextractorFunListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.TextractorFunListView.Location = new System.Drawing.Point(12, 111);
            this.TextractorFunListView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.TextractorFunListView.MouseState = MaterialSkin.MouseState.OUT;
            this.TextractorFunListView.MultiSelect = false;
            this.TextractorFunListView.Name = "TextractorFunListView";
            this.TextractorFunListView.OwnerDraw = true;
            this.TextractorFunListView.Size = new System.Drawing.Size(820, 360);
            this.TextractorFunListView.TabIndex = 1;
            this.TextractorFunListView.UseCompatibleStateImageBehavior = false;
            this.TextractorFunListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "方法";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "特殊码";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "附加码";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "获取内容";
            this.columnHeader4.Width = 520;
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.BackColor = System.Drawing.Color.White;
            this.InfoLabel.Depth = 0;
            this.InfoLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.InfoLabel.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.InfoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.InfoLabel.Location = new System.Drawing.Point(12, 77);
            this.InfoLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(639, 20);
            this.InfoLabel.TabIndex = 5;
            this.InfoLabel.Text = "已经成功定位游戏，但由于本方法有多个入口，请刷新游戏文本，重新确认最佳内容并选择进入";
            this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.BackColor = System.Drawing.Color.White;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(12, 493);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(491, 20);
            this.materialLabel1.TabIndex = 6;
            this.materialLabel1.Text = "*该游戏每次自动开启都可能会重新设置Hook方法以确保最佳的翻译体验";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FunConfirmBtn
            // 
            this.FunConfirmBtn.BackColor = System.Drawing.Color.White;
            this.FunConfirmBtn.BtnBackColor = System.Drawing.Color.White;
            this.FunConfirmBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FunConfirmBtn.BtnForeColor = System.Drawing.Color.White;
            this.FunConfirmBtn.BtnText = "确认";
            this.FunConfirmBtn.ConerRadius = 5;
            this.FunConfirmBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FunConfirmBtn.EnabledMouseEffect = true;
            this.FunConfirmBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.FunConfirmBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FunConfirmBtn.IsRadius = true;
            this.FunConfirmBtn.IsShowRect = true;
            this.FunConfirmBtn.IsShowTips = false;
            this.FunConfirmBtn.Location = new System.Drawing.Point(614, 482);
            this.FunConfirmBtn.Margin = new System.Windows.Forms.Padding(0);
            this.FunConfirmBtn.Name = "FunConfirmBtn";
            this.FunConfirmBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.FunConfirmBtn.RectWidth = 1;
            this.FunConfirmBtn.Size = new System.Drawing.Size(217, 39);
            this.FunConfirmBtn.TabIndex = 7;
            this.FunConfirmBtn.TabStop = false;
            this.FunConfirmBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.FunConfirmBtn.TipsText = "";
            this.FunConfirmBtn.BtnClick += new System.EventHandler(this.FunConfirmBtn_BtnClick);
            // 
            // AlwaysTopCheckBox
            // 
            this.AlwaysTopCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.AlwaysTopCheckBox.Checked = true;
            this.AlwaysTopCheckBox.ForeColor = System.Drawing.Color.Black;
            this.AlwaysTopCheckBox.Location = new System.Drawing.Point(679, 69);
            this.AlwaysTopCheckBox.Name = "AlwaysTopCheckBox";
            this.AlwaysTopCheckBox.Padding = new System.Windows.Forms.Padding(1);
            this.AlwaysTopCheckBox.Size = new System.Drawing.Size(152, 32);
            this.AlwaysTopCheckBox.TabIndex = 8;
            this.AlwaysTopCheckBox.TextValue = "本窗口总在最前";
            this.AlwaysTopCheckBox.CheckedChangeEvent += new System.EventHandler(this.AlwaysTopCheckBox_CheckedChangeEvent);
            // 
            // TextractorFunReConfirmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 534);
            this.Controls.Add(this.AlwaysTopCheckBox);
            this.Controls.Add(this.FunConfirmBtn);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.TextractorFunListView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextractorFunReConfirmForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "重新确认方法入口";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextractorFunReConfirmForm_FormClosing);
            this.Load += new System.EventHandler(this.TextractorFunReConfirmForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialListView TextractorFunListView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private MaterialSkin.Controls.MaterialLabel InfoLabel;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private HZH_Controls.Controls.UCBtnExt FunConfirmBtn;
        private HZH_Controls.Controls.UCCheckBox AlwaysTopCheckBox;
    }
}