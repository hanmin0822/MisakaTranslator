/*
 *Namespace         MisakaTranslator
 *Class             TextHookHandle
 *Description       操作Textractor CLI进程，与其交互，获取结果
 */

using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakaTranslator
{

    class TextHookHandle
    {
        public Process ProcessTextractor;//Textractor进程
        private int GamePID;//能够获取到文本的游戏进程ID
        private Dictionary<Process, bool> PossibleGameProcessList;//与gamePID进程同名的进程列表
        private int HandleMode;//处理的方式 1=已确定的单个进程 2=多个进程寻找能搜到文本的进程
        private Process MaxMemoryProcess;//最大内存进程，用于智能处理时单独注入这个进程而不是PossibleGameProcessList中的每个进程都注入
        private MaterialForm OutputSettingsform;//设置类窗口的输出处理
        private GameTranslateForm GameTransForm;//游戏的实时翻译窗口
        private int listViewIndex;
        private Dictionary<string, int> TextractorFun_Index_List;//特殊码与列表索引一一对应

        private int listViewIndex_plus;
        private Dictionary<string, int> TextractorFunPlus_Index_List;//特殊码附加值与列表索引一一对应

        public TextHookHandle(int gamePID) {
            ProcessTextractor = null;
            MaxMemoryProcess = null;
            GamePID = gamePID;
            PossibleGameProcessList = null;
            HandleMode = 1;
            OutputSettingsform = null;
            GameTransForm = null;
            listViewIndex = 0;
            listViewIndex_plus = 0;
            TextractorFun_Index_List = new Dictionary<string, int>();
            TextractorFunPlus_Index_List = new Dictionary<string, int>();
        }

        public TextHookHandle(List<Process> GameProcessList)
        {
            ProcessTextractor = null;
            GamePID = -1;
            PossibleGameProcessList = new Dictionary<Process, bool>();
            MaxMemoryProcess = GameProcessList[0];
            for (int i = 0; i < GameProcessList.Count; i++) {
                if (GameProcessList[i].WorkingSet64 > MaxMemoryProcess.WorkingSet64) {
                    MaxMemoryProcess = GameProcessList[i];
                }
                PossibleGameProcessList.Add(GameProcessList[i], false);
            }

            HandleMode = 2;
            OutputSettingsform = null;
            GameTransForm = null;
            listViewIndex = 0;
            listViewIndex_plus = 0;
            TextractorFun_Index_List = new Dictionary<string, int>();
            TextractorFunPlus_Index_List = new Dictionary<string, int>();
        }

        ~TextHookHandle() {
            if (ProcessTextractor != null) {
                CloseTextractor();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public bool Init() {
            string Path = Environment.CurrentDirectory + "\\lib\\TextHook";

            string CurrentPath = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Path;//更改当前工作目录保证TextractorCLI正常运行

            ProcessTextractor = new Process();
            ProcessTextractor.StartInfo.FileName = "TextractorCLI.exe";
            ProcessTextractor.StartInfo.CreateNoWindow = true;
            ProcessTextractor.StartInfo.UseShellExecute = false;
            ProcessTextractor.StartInfo.StandardOutputEncoding = Encoding.Unicode;
            ProcessTextractor.StartInfo.RedirectStandardInput = true;
            ProcessTextractor.StartInfo.RedirectStandardOutput = true;
            ProcessTextractor.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            bool res = ProcessTextractor.Start();
            ProcessTextractor.BeginOutputReadLine();

            Environment.CurrentDirectory = CurrentPath;//打开后即可恢复原目录
            return res;
        }


        /// <summary>
        /// 开始注入，会判断是否自动注入
        /// </summary>
        public async void StartHook() {
            if (HandleMode == 1)
            {
                await AttachProcess(GamePID);
            }
            else if (HandleMode == 2)
            {
                if (Convert.ToBoolean(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "Textractor", "AutoHook", "false")))
                {
                    //不进行智能注入
                    for (int i = 0; i < PossibleGameProcessList.Count; i++)
                    {

                        var item = PossibleGameProcessList.ElementAt(i);
                        await AttachProcess(item.Key.Id);
                        PossibleGameProcessList[item.Key] = true;
                    }
                }
                else {
                    await AttachProcess(MaxMemoryProcess.Id);
                }
            }
        }


        /// <summary>
        /// 输出事件
        /// </summary>
        /// <param name="sendingProcess"></param>
        /// <param name="outLine"></param>
        void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //Console.WriteLine(outLine.Data);
            Common.AddTextractorHistory(outLine.Data);

            string[] dealRes = DealTextratorOutput(outLine.Data);

            if (dealRes != null) {

                if (dealRes[1] != "Console" && dealRes[1] != "") {

                    //Hook入口选择窗口处理
                    if (OutputSettingsform != null && OutputSettingsform is TextractorFunSelectForm)
                    {
                        if (TextractorFun_Index_List.ContainsKey(dealRes[2] + dealRes[4]) == true)
                        {
                            TextractorFunSelectForm frm = (TextractorFunSelectForm)OutputSettingsform;
                            frm.TextractorFunDealItem(TextractorFun_Index_List[dealRes[2] + dealRes[4]], dealRes, true);
                        }
                        else
                        {
                            TextractorFun_Index_List.Add(dealRes[2] + dealRes[4], listViewIndex);
                            TextractorFunSelectForm frm = (TextractorFunSelectForm)OutputSettingsform;
                            frm.TextractorFunDealItem(listViewIndex, dealRes, false);
                            listViewIndex++;
                        }
                    }

                    //文本去重窗口处理
                    if (OutputSettingsform != null && OutputSettingsform is TextRepeatRepairForm) {
                        TextRepeatRepairForm frm = (TextRepeatRepairForm)OutputSettingsform;
                        if (Common.HookCode != "" && dealRes[2] == Common.HookCode && dealRes[4] == Common.HookCodePlus) {
                            frm.TextractorHookContent(dealRes);
                        }
                    }

                    //Hook入口重复确认窗口处理
                    if (OutputSettingsform != null && OutputSettingsform is TextractorFunReConfirmForm)
                    {
                        TextractorFunReConfirmForm frm = (TextractorFunReConfirmForm)OutputSettingsform;
                        if (Common.HookCode != "" && dealRes[2] == Common.HookCode)
                        {
                            if (TextractorFunPlus_Index_List.ContainsKey(dealRes[4]) == true)
                            {

                                frm.TextractorFunDealItem(TextractorFunPlus_Index_List[dealRes[4]], dealRes, true);
                            }
                            else
                            {
                                TextractorFunPlus_Index_List.Add(dealRes[4], listViewIndex_plus);
                                frm.TextractorFunDealItem(listViewIndex_plus, dealRes, false);
                                listViewIndex_plus++;
                            }
                        }
                    }

                    //游戏翻译窗口处理
                    //如果Common.HookCodePlus = "NoMulti"则说明没有多重处理，不用再对比HookCodePlus
                    if (GameTransForm != null) {
                        if (Common.HookCode != "" && dealRes[2] == Common.HookCode && (Common.HookCodePlus == "NoMulti" || dealRes[4] == Common.HookCodePlus))
                        {
                            GameTransForm.TextractorHookContent(dealRes);
                        }
                    }
                }
            }




        }


        /// <summary>
        /// 向Textractor CLI写入命令
        /// 注入进程
        /// </summary>
        /// <param name="pid"></param>
        public async Task AttachProcess(int pid) {
            await ProcessTextractor.StandardInput.WriteLineAsync("attach -P" + pid);
            await ProcessTextractor.StandardInput.FlushAsync();
        }

        /// <summary>
        /// 向Textractor CLI写入命令
        /// 结束注入进程
        /// </summary>
        /// <param name="pid"></param>
        public async Task DetachProcess(int pid)
        {
            await ProcessTextractor.StandardInput.WriteLineAsync("detach -P" + pid);
            await ProcessTextractor.StandardInput.FlushAsync();
        }

        /// <summary>
        /// 关闭Textractor进程
        /// </summary>
        public async void CloseTextractor() {

            if (ProcessTextractor != null && ProcessTextractor.HasExited == false)
            {
                if (HandleMode == 1)
                {
                    await DetachProcess(GamePID);
                }
                else if (HandleMode == 2)
                {
                    for (int i = 0; i < PossibleGameProcessList.Count; i++)
                    {
                        var item = PossibleGameProcessList.ElementAt(i);
                        if (PossibleGameProcessList[item.Key] == true)
                        {
                            await DetachProcess(item.Key.Id);
                            PossibleGameProcessList[item.Key] = false;
                        }
                    }
                }
                ProcessTextractor.Kill();
            }

            ProcessTextractor = null;
        }

        /// <summary>
        /// 取出文本中间一部分
        /// </summary>
        /// <param name="Text">整个文本</param>
        /// <param name="front">前面的文本</param>
        /// <param name="back">后面的文本</param>
        /// <param name="location">起始搜寻位置</param>
        /// <returns></returns>
        private string GetMiddleString(string Text, string front, string back, int location)
        {

            if (front == "" || back == "") {
                return null;
            }

            int locA = Text.IndexOf(front, location);
            int locB = Text.IndexOf(back, locA + 1);
            if (locA < 0 || locB < 0)
            {
                return null;
            }
            else
            {
                locA = locA + front.Length;
                locB = locB - locA;
                if (locA < 0 || locB < 0)
                {
                    return null;
                }
                return Text.Substring(locA, locB);
            }
        }

        /// <summary>
        /// 智能处理来自Textrator的输出并获取一个固定长度的返回string数组用于下一步处理
        /// 具体的数组含义参见方法定义
        /// </summary>
        /// <param name="OutputText">来自Textrator的输出</param>
        /// <returns></returns>
        private string[] DealTextratorOutput(string OutputText) {

            if (OutputText == "" || OutputText == null) {
                return null;
            }

            string Info = GetMiddleString(OutputText, "[", "]", 0);
            if (Info == null) {
                return null;
            }

            string[] Infores = Info.Split(':');

            if (Infores.Length >= 7)
            {
                string content = OutputText.Replace("[" + Info + "] ", "");//删除信息头部分

                string[] ret = new string[5];

                ret[0] = Infores[1]; //游戏/本体进程ID（为0一般代表Textrator本体进程ID）

                ret[1] = Infores[5]; //方法名：Textrator注入游戏进程获得文本时的方法名（为 Console 时代表Textrator本体控制台输出；为 Clipboard 时代表从剪贴板获取的文本）

                //注意 本软件单独处理一套特殊码规则：即Textrator输出的从进程到特殊码之间的三组数字做记录
                //格式如下：特殊码【值1:值2:值3】
                //冒号是半角，中括号全角
                ret[2] = Infores[6]; //特殊码：Textrator注入游戏进程获得文本时的方法的特殊码，是一个唯一值，可用于判断

                ret[3] = content; //实际获取到的内容

                ret[4] = "【" + Infores[2] + ":" + Infores[3] + ":" + Infores[4] + "】"; //【值1:值2:值3】见上方格式说明

                return ret;
            }
            else {
                return null;
            }

        }

        public void SetSettingsOutPutform(MaterialForm form) {
            OutputSettingsform = form;
        }

        public void SetGameTransForm(GameTranslateForm frm) {
            GameTransForm = frm;
        }


        /// <summary>
        /// 处理本游戏专用特殊码的方法
        /// 分离  特殊码【值1:值2:值3】
        /// </summary>
        /// <returns>返回数组 [0]为特殊码，[1]为【值1:值2:值3】</returns>
        public static string[] DealCode(string code){
            if (code == "")
            {
                return null;
            }
            string[] ret = new string[2];
            
            string[] res = code.Split('【');
            ret[0] = res[0];
            ret[1] = "【" + res[1];
            return ret;
        }

    }
}
