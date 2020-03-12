/*
 *Namespace         MisakaTranslator
 *Class             OCRChooseForm
 *Description       OCR相关设置窗口，响应一些界面内按钮的点击事件
 *Author            Hanmin Qi
 *LastModifyTime    2020-03-12
 * ===============================================================
 * 以下是修改记录（任何一次修改都应被记录）
 * 日期   修改内容    作者
 * 2020-03-12       代码注释完成      果冻
 */

using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MisakaTranslator
{
    public partial class OCRChooseForm : MaterialForm
    {

        GlobalMouseHook hook;//全局鼠标钩子
        int SelectedHwnd;//选择的窗口句柄

        public OCRChooseForm()
        {
            InitializeComponent();
        }

        private void OCRChooseForm_Load(object sender, EventArgs e)
        {
            Common.isAllWindowCap = false;
            List<KeyValuePair<string, string>> srcLangls = new List<KeyValuePair<string, string>>();
            srcLangls.Add(new KeyValuePair<string, string>("JAP","日语"));
            srcLangls.Add(new KeyValuePair<string, string>("ENG", "英语"));

            srcLangCombox.BoxStyle = ComboBoxStyle.DropDownList;
            srcLangCombox.Source = srcLangls;
            srcLangCombox.SelectedIndex = 0;


            //初始化钩子对象
            if (hook == null)
            {
                hook = new GlobalMouseHook();
                hook.OnMouseActivity += new MouseEventHandler(Hook_OnMouseActivity);
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        void Hook_OnMouseActivity(object sender, MouseEventArgs e)
        {
            SelectedHwnd = FindWindowInfo.GetWindowHWND(e.X, e.Y);
            string gameName = FindWindowInfo.GetWindowName(SelectedHwnd);
            int pid = FindWindowInfo.GetProcessIDByHWND(SelectedHwnd);

            if (Process.GetCurrentProcess().Id != pid)
            {
                ProcessInfoLabel.Text = "[实时]" + gameName + "—" + pid;
            }
            hook.Stop();
            WindowChooseBtn.BtnText = "选择窗口";
        }


        private void ScreenCaptureBtn_BtnClick(object sender, EventArgs e)
        {
            Image img;
            if (Common.isAllWindowCap == true)
            {
                img = ScreenCapture.GetAllWindow();
            }
            else
            {
                img = ScreenCapture.GetWindowCapture((IntPtr)SelectedHwnd);
            }
            
            ScreenCapForm scf = new ScreenCapForm(img);
            scf.BackgroundImage = img;
            scf.Width = img.Width;
            scf.Height = img.Height;
            scf.Show();
        }

        private void WindowChooseBtn_BtnClick(object sender, EventArgs e)
        {
            if (WindowChooseBtn.BtnText == "选择窗口")
            {
                bool r = hook.Start();
                if (r)
                {
                    WindowChooseBtn.BtnText = "结束选择";
                    //MessageBox.Show("安装钩子成功!");
                }
                else
                {
                    MessageBox.Show("安装钩子失败!");
                }
            }
            else if (WindowChooseBtn.BtnText == "结束选择")
            {
                hook.Stop();
                WindowChooseBtn.BtnText = "选择窗口";
            }
        }

        private void OCRChooseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowChooseBtn.BtnText == "结束选择")
            {
                hook.Stop();
            }
        }

        private void renovateORCPicBtn_BtnClick(object sender, EventArgs e)
        {
            Image img = ScreenCapture.GetWindowRectCapture((IntPtr)SelectedHwnd,Common.OCRrec, Common.isAllWindowCap);
            PreviewBox.BackgroundImage = img;
        }

        private void OCRConfirmBtn_BtnClick(object sender, EventArgs e)
        {
            Common.OCRdelay = int.Parse(delaySetBox.InputText);
            if (Common.OCRdelay <= 0) {
                MessageBox.Show("延时不能小于等于0，将自动设置为1000", "提示");
                Common.OCRdelay = 1000;
            }
            Common.TransMode = 2;
            Common.OCRWinHwnd = (IntPtr)SelectedHwnd;
            Common.OCRsrcLangCode = srcLangCombox.SelectedValue;
            
            TransLangSettingForm tlsf = new TransLangSettingForm();
            tlsf.Show();
            this.Close();

        }

        private void TestOCRBtn_BtnClick(object sender, EventArgs e)
        {
            BaiduGeneralOCRBasic.BaiduGeneralOCRBasic_Init();
            Image img = ScreenCapture.GetWindowRectCapture((IntPtr)SelectedHwnd, Common.OCRrec, Common.isAllWindowCap);
            string ret = BaiduGeneralOCRBasic.BaiduGeneralBasicOCR(img,srcLangCombox.SelectedValue);
            
            MessageBox.Show(ret,"百度OCR结果");
        }

        private void AllWinCheckBox_CheckedChangeEvent(object sender, EventArgs e)
        {
            if (AllWinCheckBox.Checked == true)
            {
                WindowChooseBtn.Visible = false;
                ProcessInfoLabel.Visible = false;
            }
            else
            {
                WindowChooseBtn.Visible = true;
                ProcessInfoLabel.Visible = true;
            }
            Common.isAllWindowCap = AllWinCheckBox.Checked;
        }

        
    }
}
