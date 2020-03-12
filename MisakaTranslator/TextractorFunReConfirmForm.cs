/*
 *Namespace         MisakaTranslator
 *Class             TextractorFunReConfirmForm
 *Description       Textractor Hook方法的重新选择窗口
 *Author            Hanmin Qi
 *LastModifyTime    2020-03-12
 * ===============================================================
 * 以下是修改记录（任何一次修改都应被记录）
 * 日期   修改内容    作者
 * 2020-03-12       代码注释完成      果冻
 */

using MaterialSkin.Controls;
using System;
using System.Windows.Forms;

namespace MisakaTranslator
{
    public partial class TextractorFunReConfirmForm : MaterialForm
    {

        private bool isNormalClose;//判断窗口是否是正常关闭的，防止误杀Textractor进程，为假时说明用户未确认进入到下一窗口直接关闭，则需要杀死Textractor进程

        public TextractorFunReConfirmForm()
        {
            isNormalClose = false;
            InitializeComponent();
        }

        private void TextractorFunReConfirmForm_Load(object sender, EventArgs e)
        {
            TextractorFunListView.HideSelection = true;
        }

        private void AlwaysTopCheckBox_CheckedChangeEvent(object sender, EventArgs e)
        {
            this.TopMost = AlwaysTopCheckBox.Checked;
        }

        private void FunConfirmBtn_BtnClick(object sender, EventArgs e)
        {
            isNormalClose = true;
            Common.HookCodePlus = TextractorFunListView.SelectedItems[0].SubItems[2].Text;
            GameTranslateForm gtf = new GameTranslateForm();
            Common.TextractorHandle.SetSettingsOutPutform(null);
            Common.TextractorHandle.SetGameTransForm(gtf);
            GameTranslateBackForm.Show(gtf);
            this.Close();
        }

        public void TextractorFunDealItem(int index, string[] Item, bool isExist)
        {

            if (isExist == true)
            {//表项已存在，更新
                TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.BeginUpdate(); }));
                TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.Items[index].SubItems[3].Text = Item[3]; }));
                TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.EndUpdate(); }));
            }
            else
            {
                TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.BeginUpdate(); }));
                ListViewItem lvi = new ListViewItem();
                lvi.Text = Item[1];
                lvi.SubItems.Add(Item[2]);
                lvi.SubItems.Add(Item[4]);
                lvi.SubItems.Add(Item[3]);
                TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.Items.Insert(index, lvi); }));
                TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.EndUpdate(); }));
            }

        }

        private void TextractorFunReConfirmForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isNormalClose == false)
            {
                Common.TextractorHandle.CloseTextractor();
            }
        }
    }
}
