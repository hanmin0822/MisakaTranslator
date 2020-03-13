/*
 *Namespace         MisakaTranslator
 *Class             MainForm
 *Description       主窗口按钮点击的相关事件以及两个功能函数
 */

using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MisakaTranslator
    
{
    public partial class MainForm : MaterialForm
    {
        public MainForm()
        {
            InitializeComponent();

            //以下是UI美化
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        /// <summary>
        /// 【Hook】选择游戏按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectGameProcessFunBtn_BtnClick(object sender, EventArgs e)
        {
            ProcessSelectForm psf = new ProcessSelectForm();
            psf.Show();
        }

        /// <summary>
        /// 【Hook】选择游戏按钮鼠标退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectGameProcessFunBtn_MouseEffected(object sender, EventArgs e)
        {
            BtnHinrLabel.Text = "";
        }

        /// <summary>
        /// 【Hook】选择游戏按钮鼠标进入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectGameProcessFunBtn_MouseEffecting(object sender, EventArgs e)
        {
            BtnHinrLabel.Text = "通过窗口/列表选取一个进程并使用Textrator方法注入以获取翻译";
        }
        
        /// <summary>
        /// 设置按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsBtn_BtnClick(object sender, EventArgs e)
        {
            ApplicationSettingsForm asf = new ApplicationSettingsForm();
            asf.Show();
        }

        /// <summary>
        /// 自动开始游戏按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoStartBtn_BtnClick(object sender, EventArgs e)
        {
            List<int> res = GetGameListHasProcessGame_PID_ID();
            if (res == null)
            {
                MessageBox.Show("未找到任何正在运行中的已保存游戏", "提示");
            }
            else {
                Common.TransMode = 1;
                Common.GameID = res[1];
                Common.HookCode = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\GameListInfo.ini", "Game" + Common.GameID, "hookCode");
                Common.RepeatMethod = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\GameListInfo.ini", "Game" + Common.GameID, "RepeatMethod");
                Common.srcLang = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\GameListInfo.ini", "Game" + Common.GameID, "srcLang");
                Common.desLang = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\GameListInfo.ini", "Game" + Common.GameID, "dstLang");

                List<Process> proList = FindSameNameProcess(res[0]);
                if (proList.Count == 1)
                {
                    Common.TextractorHandle = new TextHookHandle(proList[0].Id);
                }
                else {
                    Common.TextractorHandle = new TextHookHandle(proList);
                }
                
                Common.TextractorHandle.Init();

                bool isFunReSelect = Convert.ToBoolean(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\GameListInfo.ini", "Game" + Common.GameID, "isHookFunMulti","False"));

                if (isFunReSelect == true)
                {
                    TextractorFunReConfirmForm tfcf = new TextractorFunReConfirmForm();
                    Common.TextractorHandle.StartHook();
                    Common.TextractorHandle.SetSettingsOutPutform(tfcf);
                    tfcf.Show();
                }
                else {
                    Common.HookCodePlus = "NoMulti";//提示无重复码。直接进游戏
                    GameTranslateForm gtf = new GameTranslateForm();
                    Common.TextractorHandle.StartHook();
                    Common.TextractorHandle.SetGameTransForm(gtf);
                    GameTranslateBackForm.Show(gtf);
                    
                }
            }
        }

        /// <summary>
        /// 查找一个进程的同名进程
        /// </summary>
        /// <param name="pid">进程PID</param>
        /// <returns>同名进程列表（包括自己）</returns>
        private List<Process> FindSameNameProcess(int pid)
        {
            Process[] ps = Process.GetProcesses();
            List<Process> res = new List<Process>();
            string DesProcessName = "";

            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Id == pid)
                {
                    DesProcessName = ps[i].ProcessName;
                    break;
                }
            }

            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].ProcessName == DesProcessName)
                {
                    res.Add(ps[i]);
                }
            }

            return res;
        }

        /// <summary>
        /// 寻找任何正在运行中的之前已保存过的游戏
        /// </summary>
        /// <returns>进程ID列表</returns>
        private List<int> GetGameListHasProcessGame_PID_ID() {
            Process[] ps = Process.GetProcesses();
            List<int> ret = new List<int>();
            if (File.Exists("GameList.txt") == true)
            {
                string[] ReadText = File.ReadAllLines("GameList.txt");

                for (int i = 0; i < ps.Length; i++)
                {
                    for (int j = 0; j < ReadText.Length; j++)
                    {
                        string filepath = "";
                        try
                        {
                            filepath = ps[i].MainModule.FileName;
                        }
                        catch (Win32Exception ex)
                        {
                            continue;
                        }
                        Console.WriteLine(filepath);
                        if (filepath == ReadText[j])
                        {
                            ret.Add(ps[i].Id);
                            ret.Add(int.Parse(ReadText[j + 1]));
                            return ret;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 自动开始游戏按钮鼠标退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoStartBtn_MouseEffected(object sender, EventArgs e)
        {
            BtnHinrLabel.Text = "";
        }

        /// <summary>
        /// 自动开始游戏按钮鼠标进入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoStartBtn_MouseEffecting(object sender, EventArgs e)
        {
            BtnHinrLabel.Text = "自动注入之前已经保存过相关设置的游戏,搜寻过程卡顿请耐心等待";
        }

        /// <summary>
        /// 设置按钮鼠标进入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsBtn_MouseEffecting(object sender, EventArgs e)
        {
            BtnHinrLabel.Text = "MisakaTranslator软件本体设置和关于";
        }

        /// <summary>
        /// 设置按钮鼠标退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsBtn_MouseEffected(object sender, EventArgs e)
        {
            BtnHinrLabel.Text = "";
        }

        /// <summary>
        /// 【OCR】文字扫描按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OCRFunBtn_BtnClick(object sender, EventArgs e)
        {
            OCRChooseForm frm = new OCRChooseForm();
            frm.Show();
        }

        /// <summary>
        /// 【OCR】文字扫描按钮鼠标退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OCRFunBtn_MouseEffected(object sender, EventArgs e)
        {
            BtnHinrLabel.Text = "";

        }

        /// <summary>
        /// 【OCR】文字扫描按钮鼠标进入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OCRFunBtn_MouseEffecting(object sender, EventArgs e)
        {
            BtnHinrLabel.Text = "通过OCR方式翻译游戏，适用于TextHook方式不支持的情况";
        }
    }
}
