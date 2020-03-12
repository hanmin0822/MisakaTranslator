/*
 *Namespace         MisakaTranslator
 *Class             TextractorFunSelectForm
 *Description       Textractor Hook方法的选择窗口
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
    public partial class TextractorFunSelectForm : MaterialForm
    {

        private bool isNormalClose;//判断窗口是否是正常关闭的，防止误杀Textractor进程，为假时说明用户未确认进入到下一窗口直接关闭，则需要杀死Textractor进程

        public TextractorFunSelectForm()
        {
            isNormalClose = false;
            InitializeComponent();
        }

        

        public void TextractorFunDealItem(int index, string[] Item,bool isExist) {
            
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
                lvi.Text = Item[0];
                lvi.SubItems.Add(Item[1]);
                lvi.SubItems.Add(Item[2] + Item[4]);
                lvi.SubItems.Add(Item[3]);
                TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.Items.Insert(index,lvi); }));
                TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.EndUpdate(); }));
            }

        }

        private void AlwaysTopCheckBox_CheckedChangeEvent(object sender, EventArgs e)
        {
            this.TopMost = AlwaysTopCheckBox.Checked;
        }

        private void ConfirmHookBtn_BtnClick(object sender, EventArgs e)
        {
            if (TextractorFunListView.SelectedItems.Count != 0)
            {
                Common.TextractorHandle.SetSettingsOutPutform(null);//先关闭对本窗口的输出

                string[] res = TextHookHandle.DealCode(TextractorFunListView.SelectedItems[0].SubItems[2].Text);

                int sum = 0;
                for (int i = 0;i < TextractorFunListView.Items.Count;i++) {
                    if (TextractorFunListView.Items[i].SubItems[2].Text.Contains(res[0]))
                    {
                        sum++;
                    }
                    if (sum >= 2) {
                        IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\GameListInfo.ini", 
                            "Game" + Common.GameID,"isHookFunMulti", "True");
                        break;
                    }
                }

                //不满足的游戏也应该记录一下
                if (sum <= 1) {
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\GameListInfo.ini",
                            "Game" + Common.GameID, "isHookFunMulti", "False");
                }
                
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\GameListInfo.ini", "Game" + Common.GameID, 
                    "hookCode", res[0]);//保存特殊码，以后可以自动匹配这个游戏，但需要重设Plus部分

                Common.HookCode = res[0];
                Common.HookCodePlus = res[1];

                isNormalClose = true;

                TextRepeatRepairForm trrf = new TextRepeatRepairForm();
                Common.TextractorHandle.SetSettingsOutPutform(trrf);
                trrf.Show();

                this.Close();

            }
            else {
                MessageBox.Show("请先选择一个Hook方法再进行下一步操作！", "提示");
            }
        }

        private void TextractorFunSelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isNormalClose == false)
            {
                Common.TextractorHandle.CloseTextractor();
            }
        }

        private void TextractorFunSelectForm_Load(object sender, EventArgs e)
        {
            TextractorFunListView.HideSelection = true;
        }
    }
}
