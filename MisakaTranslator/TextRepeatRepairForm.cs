/*
 *Namespace         MisakaTranslator
 *Class             TextRepeatRepairForm
 *Description       文本去重方法选择窗口
 */

using MaterialSkin.Controls;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace MisakaTranslator
{
    public partial class TextRepeatRepairForm : MaterialForm
    {

        private bool isNormalClose;//判断窗口是否是正常关闭的，防止误杀Textractor进程，为假时说明用户未确认进入到下一窗口直接关闭，则需要杀死Textractor进程

        public TextRepeatRepairForm()
        {
            isNormalClose = false;
            InitializeComponent();
        }

        private void TextRepeatRepairForm_Load(object sender, EventArgs e)
        {

            FunctionSelectCombox.BoxStyle = ComboBoxStyle.DropDownList;
            FunctionSelectCombox.Source = TextRepeatRepair.GetAllFun();

            FunctionSelectCombox.SelectedIndex = 0;
        }

        public void TextractorHookContent(string[] Item)
        {
            SourceTextBox.BeginInvoke(new Action(() => { SourceTextBox.Text = Item[3]; }));

        }

        private void SourceTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateDes();
        }

        private void FunctionSelectCombox_SelectedChangedEvent(object sender, EventArgs e)
        {
            UpdateDes();
        }

        private void UpdateDes()
        {
            Console.WriteLine(FunctionSelectCombox.SelectedValue);

            Type t = typeof(TextRepeatRepair);//括号中的为所要使用的函数所在的类的类名
            MethodInfo mt = t.GetMethod(FunctionSelectCombox.SelectedValue);
            if (mt != null)
            {
                string str = (string)mt.Invoke(null, new object[] { SourceTextBox.Text });
                DesTextBox.Text = str;
            }
            else
            {
                DesTextBox.Text = "该方法处理错误！";
            }
        }

        private void ConfirmBtn_BtnClick(object sender, EventArgs e)
        {
            isNormalClose = true;

            Common.RepeatMethod = FunctionSelectCombox.SelectedValue;
            IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\GameListInfo.ini", "Game" + Common.GameID,
                    "RepeatMethod", FunctionSelectCombox.SelectedValue);//保存去重方法，之后可能会用到
            Common.TransMode = 1;
            Common.TextractorHandle.SetSettingsOutPutform(null);
            TransLangSettingForm tlsf = new TransLangSettingForm();
            tlsf.Show();
            this.Close();
        }

        private void TextRepeatRepairForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isNormalClose == false)
            {
                Common.TextractorHandle.CloseTextractor();
            }

        }
    }
}
