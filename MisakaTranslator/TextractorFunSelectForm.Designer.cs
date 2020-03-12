namespace MisakaTranslator
{
    partial class TextractorFunSelectForm
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
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlwaysTopCheckBox = new HZH_Controls.Controls.UCCheckBox();
            this.InfoLabel = new MaterialSkin.Controls.MaterialLabel();
            this.ConfirmHookBtn = new HZH_Controls.Controls.UCBtnExt();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // TextractorFunListView
            // 
            this.TextractorFunListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextractorFunListView.CheckBoxes = true;
            this.TextractorFunListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
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
            this.TextractorFunListView.Size = new System.Drawing.Size(820, 365);
            this.TextractorFunListView.TabIndex = 0;
            this.TextractorFunListView.UseCompatibleStateImageBehavior = false;
            this.TextractorFunListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "进程号";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "方法";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "特殊码";
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "获取内容";
            this.columnHeader4.Width = 500;
            // 
            // AlwaysTopCheckBox
            // 
            this.AlwaysTopCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.AlwaysTopCheckBox.Checked = true;
            this.AlwaysTopCheckBox.ForeColor = System.Drawing.Color.Black;
            this.AlwaysTopCheckBox.Location = new System.Drawing.Point(680, 73);
            this.AlwaysTopCheckBox.Name = "AlwaysTopCheckBox";
            this.AlwaysTopCheckBox.Padding = new System.Windows.Forms.Padding(1);
            this.AlwaysTopCheckBox.Size = new System.Drawing.Size(152, 32);
            this.AlwaysTopCheckBox.TabIndex = 1;
            this.AlwaysTopCheckBox.TextValue = "本窗口总在最前";
            this.AlwaysTopCheckBox.CheckedChangeEvent += new System.EventHandler(this.AlwaysTopCheckBox_CheckedChangeEvent);
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.BackColor = System.Drawing.Color.White;
            this.InfoLabel.Depth = 0;
            this.InfoLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.InfoLabel.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.InfoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.InfoLabel.Location = new System.Drawing.Point(17, 78);
            this.InfoLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(564, 20);
            this.InfoLabel.TabIndex = 4;
            this.InfoLabel.Text = "进行游戏并使游戏中文本发生变化，在下方找到与游戏文本内容一致的对应项并确定";
            this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConfirmHookBtn
            // 
            this.ConfirmHookBtn.BackColor = System.Drawing.Color.White;
            this.ConfirmHookBtn.BtnBackColor = System.Drawing.Color.White;
            this.ConfirmHookBtn.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ConfirmHookBtn.BtnForeColor = System.Drawing.Color.White;
            this.ConfirmHookBtn.BtnText = "确定该Hook入口";
            this.ConfirmHookBtn.ConerRadius = 5;
            this.ConfirmHookBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConfirmHookBtn.EnabledMouseEffect = true;
            this.ConfirmHookBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ConfirmHookBtn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ConfirmHookBtn.IsRadius = true;
            this.ConfirmHookBtn.IsShowRect = true;
            this.ConfirmHookBtn.IsShowTips = false;
            this.ConfirmHookBtn.Location = new System.Drawing.Point(633, 485);
            this.ConfirmHookBtn.Margin = new System.Windows.Forms.Padding(0);
            this.ConfirmHookBtn.Name = "ConfirmHookBtn";
            this.ConfirmHookBtn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ConfirmHookBtn.RectWidth = 1;
            this.ConfirmHookBtn.Size = new System.Drawing.Size(198, 60);
            this.ConfirmHookBtn.TabIndex = 5;
            this.ConfirmHookBtn.TabStop = false;
            this.ConfirmHookBtn.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ConfirmHookBtn.TipsText = "";
            this.ConfirmHookBtn.BtnClick += new System.EventHandler(this.ConfirmHookBtn_BtnClick);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.BackColor = System.Drawing.Color.White;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.materialLabel1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(17, 485);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(594, 60);
            this.materialLabel1.TabIndex = 6;
            this.materialLabel1.Text = "提示：部分游戏如果出现文本重复或文本前后包含乱码字串属正常现象（可以将获取内容\r\n一栏拉长一点看），将在下一步进行优化；游戏时本窗口内容被高频刷新，如果选中项没\r" +
    "\n显示出来，只需要点击要选中的项以后按\"确定该Hook入口\"即可。";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextractorFunSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 557);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.ConfirmHookBtn);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.AlwaysTopCheckBox);
            this.Controls.Add(this.TextractorFunListView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextractorFunSelectForm";
            this.Text = "选择Hook入口方法";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextractorFunSelectForm_FormClosing);
            this.Load += new System.EventHandler(this.TextractorFunSelectForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialListView TextractorFunListView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private HZH_Controls.Controls.UCCheckBox AlwaysTopCheckBox;
        private MaterialSkin.Controls.MaterialLabel InfoLabel;
        private HZH_Controls.Controls.UCBtnExt ConfirmHookBtn;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
    }
}