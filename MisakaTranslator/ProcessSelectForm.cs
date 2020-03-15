/*
 *Namespace         MisakaTranslator
 *Class             ProcessSelectForm
 *Description       进程选择窗口的相关鼠标事件
 */

using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace MisakaTranslator
{
    public partial class ProcessSelectForm : MaterialForm
    {

        GlobalMouseHook hook;//全局鼠标钩子
        Process[] ps;//当前系统所有进程列表
        List<KeyValuePair<string, string>> processList;//显示在组合框中的进程列表
        int gamePID;//找到的游戏PID
        List<Process> SameNameGameProcessList;//与gamePID进程同名的进程列表


        public ProcessSelectForm()
        {
            InitializeComponent();
        }

        private void ProcessSelectForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            gamePID = -1;//初始化状态
            SameNameGameProcessList = null;
            processList = new List<KeyValuePair<string, string>>();

            ReNewSystemProcessCombox();


            //初始化钩子对象
            if (hook == null)
            {
                hook = new GlobalMouseHook();
                hook.OnMouseActivity += new MouseEventHandler(Hook_OnMouseActivity);
            }
        }

        private void SelectGameWindowBtn_BtnClick(object sender, EventArgs e)
        {
            if (SelectGameWindowBtn.BtnText == "选择窗口")
            {
                bool r = hook.Start();
                if (r)
                {
                    SelectGameWindowBtn.BtnText = "结束选择";
                    //MessageBox.Show("安装钩子成功!");
                }
                else
                {
                    MessageBox.Show("安装钩子失败!");
                }
            }
            else if (SelectGameWindowBtn.BtnText == "结束选择")
            {
                hook.Stop();
                SelectGameWindowBtn.BtnText = "选择窗口";
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        void Hook_OnMouseActivity(object sender, MouseEventArgs e)
        {
            int hwnd = FindWindowInfo.GetWindowHWND(e.X, e.Y);
            string gameName = FindWindowInfo.GetWindowName(hwnd);
            int pid = FindWindowInfo.GetProcessIDByHWND(hwnd);

            if (Process.GetCurrentProcess().Id != pid)
            {
                ProcessInfoLabel.Text = "[实时]" + gameName + "—" + pid;

                bool flag = false;
                for (int i = 0; i < processList.Count; i++)
                {
                    KeyValuePair<string, string> pkvp = processList[i];
                    if (pkvp.Key == pid.ToString())
                    {
                        SystemProcessCombox.SelectedIndex = i;
                        flag = true;
                        break;
                    }
                }

                if (flag == false)
                {
                    //打开这个窗口后再打开游戏的情况
                    ReNewSystemProcessCombox();
                    for (int i = 0; i < processList.Count; i++)
                    {
                        KeyValuePair<string, string> pkvp = processList[i];
                        if (pkvp.Key == pid.ToString())
                        {
                            SystemProcessCombox.SelectedIndex = i;
                            flag = true;
                            break;
                        }
                    }
                }

                gamePID = pid;

                hook.Stop();
                SelectGameWindowBtn.BtnText = "选择窗口";
            }


        }

        /// <summary>
        /// 刷新组合框中的进程列表
        /// </summary>
        private void ReNewSystemProcessCombox()
        {
            processList.Clear();
            //获取系统进程列表
            ps = Process.GetProcesses();
            for (int i = 0; i < ps.Length; i++)
            {
                Process p = ps[i];
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    string info = "";
                    info = p.ProcessName + "—" + p.Id;
                    KeyValuePair<string, string> pkvp = new KeyValuePair<string, string>(p.Id.ToString(), info);
                    processList.Add(pkvp);
                }
            }
            SystemProcessCombox.BoxStyle = ComboBoxStyle.DropDownList;
            SystemProcessCombox.Source = processList;
        }



        private void ProcessSelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SelectGameWindowBtn.BtnText == "结束选择")
            {
                hook.Stop();
            }
        }

        private void ConfirmBtn_BtnClick(object sender, EventArgs e)
        {
            if (SelectGameWindowBtn.BtnText == "结束选择")
            {
                hook.Stop();
            }

            if (gamePID != -1)
            {
                string game_ID = null;
                try
                {
                     game_ID = FindProcessPath(gamePID);
                }
                catch(System.ComponentModel.Win32Exception ex)
                {
                        MessageBox.Show("无法捕捉64位程序，请使用64位翻译器再次尝试","错误");
                    return;
                }
                Common.GameID = Common.GetGameID(game_ID);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\GameListInfo.ini", "Game" + Common.GameID, "gameName", System.IO.Path.GetFileName(game_ID));

                if (SameNameGameProcessList.Count == 1)
                {
                    Common.TextractorHandle = new TextHookHandle(gamePID);
                }
                else
                {
                    Common.TextractorHandle = new TextHookHandle(SameNameGameProcessList);
                }
                Common.TextractorHandle.Init();
                Common.TextractorHandle.StartHook();

                this.TopMost = false;

                TextractorFunSelectForm tfsf = new TextractorFunSelectForm();
                Common.TextractorHandle.SetSettingsOutPutform(tfsf);
                tfsf.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("请先选择一个进程再进行下一步操作！", "提示");
            }
        }

        private void SystemProcessCombox_SelectedChangedEvent(object sender, EventArgs e)
        {
            if (SystemProcessCombox.SelectedIndex != -1)
            {
                gamePID = int.Parse(SystemProcessCombox.SelectedValue);
            }

            SameNameGameProcessList = FindSameNameProcess(gamePID);
            ProcessNumLabel.Text = "【智能处理】共找到" + SameNameGameProcessList.Count + "个同名进程";
        }


        /// <summary>
        /// 查找同名进程并返回一个进程PID列表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private List<Process> FindSameNameProcess(int pid)
        {
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
        /// 根据进程PID找到程序所在路径
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private string FindProcessPath(int pid)
        {
            string filepath = "";
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Id == pid)
                {
                    try
                    {
                        filepath = ps[i].MainModule.FileName;
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        throw ex;
                    }
                    break;
                }
            }
            return filepath;
        }
    }
}
