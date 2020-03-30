/*
 *Namespace         MisakaTranslator
 *Class             TextractorFunSelectForm
 *Description       Textractor Hook方法的选择窗口
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
                //缺失texthook.dll时有时会抛出异常。尚未成功捕捉到异常
                //try
                //{
                //TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.BeginUpdate(); }));
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("未找到texthook.dll，请检查应用完整性。");
                //    return;
                //}
                TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.BeginUpdate(); }));
                ListViewItem lvi = new ListViewItem();
                lvi.Text = Item[0];
                lvi.SubItems.Add(Item[1]);
                lvi.SubItems.Add(Item[2] + Item[4]);
                lvi.SubItems.Add(Item[3]);
                TextractorFunListView.BeginInvoke(new Action(() => { TextractorFunListView.Items.Insert(index, lvi); }));
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
                for (int i = 0; i < TextractorFunListView.Items.Count; i++)
                {
                    if (TextractorFunListView.Items[i].SubItems[2].Text.Contains(res[0]))
                    {
                        sum++;
                    }
                    if (sum >= 2)
                    {
                        SQLiteHelper sqliteH1 = new SQLiteHelper(Environment.CurrentDirectory + "\\settings\\GameList.sqlite");
                        sqliteH1.ExecuteSql(string.Format("UPDATE gamelist SET isHookFunMulti = 'True' WHERE gameID = {0};", Common.GameID));
                        

                        break;
                    }
                }

                //不满足的游戏也应该记录一下
                if (sum <= 1)
                {
                    SQLiteHelper sqliteH1 = new SQLiteHelper(Environment.CurrentDirectory + "\\settings\\GameList.sqlite");
                    sqliteH1.ExecuteSql(string.Format("UPDATE gamelist SET isHookFunMulti = 'False' WHERE gameID = {0};", Common.GameID));
                }

                

                SQLiteHelper sqliteH = new SQLiteHelper(Environment.CurrentDirectory + "\\settings\\GameList.sqlite");
                sqliteH.ExecuteSql(string.Format("UPDATE gamelist SET hookCode = '{0}' WHERE gameID = {1};", res[0], Common.GameID));

                Common.HookCode = res[0];
                Common.HookCodePlus = res[1];

                isNormalClose = true;

                TextRepeatRepairForm trrf = new TextRepeatRepairForm();
                Common.TextractorHandle.SetSettingsOutPutform(trrf);
                trrf.Show();

                this.Close();

            }
            else
            {
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
