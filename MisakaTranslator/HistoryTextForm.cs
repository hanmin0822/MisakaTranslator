/*
 *Namespace         MisakaTranslator
 *Class             HistoryTextForm
 *Description       显示历史记录的窗口
 *Author            Hanmin Qi
 *LastModifyTime    2020-03-12
 * ===============================================================
 * 以下是修改记录（任何一次修改都应被记录）
 * 日期   修改内容    作者
 * 2020-03-12       代码注释完成      果冻
 */


using MaterialSkin.Controls;
using System;
using System.Linq;

namespace MisakaTranslator
{
    public partial class HistoryTextForm : MaterialForm
    {
        public HistoryTextForm()
        {
            InitializeComponent();
        }

        private void HistoryTextForm_Load(object sender, EventArgs e)
        {
            TextInfo[] ti = Common.HistoryTextInfos.ToArray();
            for (int i = ti.Count()-1; i >=0 ;i--) {
                HistoryTextBox.Text += ti[i].TIsrcText + "\r\n";
                HistoryTextBox.Text += ti[i].TIfirstTransText + "\r\n";
                HistoryTextBox.Text += ti[i].TIsecondTransText + "\r\n";
                HistoryTextBox.Text += "=================================\r\n";
            }
        }
    }
}
