namespace MisakaTranslator
{
    partial class HistoryTextForm
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
            this.HistoryTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // HistoryTextBox
            // 
            this.HistoryTextBox.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HistoryTextBox.Location = new System.Drawing.Point(12, 77);
            this.HistoryTextBox.Multiline = true;
            this.HistoryTextBox.Name = "HistoryTextBox";
            this.HistoryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.HistoryTextBox.Size = new System.Drawing.Size(721, 288);
            this.HistoryTextBox.TabIndex = 1;
            // 
            // HistoryTextForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 377);
            this.Controls.Add(this.HistoryTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HistoryTextForm";
            this.Sizable = false;
            this.Text = "历史文本记录";
            this.Load += new System.EventHandler(this.HistoryTextForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox HistoryTextBox;
    }
}