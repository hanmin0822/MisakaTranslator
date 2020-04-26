//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TextHookLibrary
//{
//    /// <summary>
//    /// 该类使用TextractorCLI版本进行读写，由于bug多且不方便用户操作，故弃用
//    /// 使用开源项目https://github.com/lgztx96/texthost中的方法，直接使用dll操作
//    /// 新类的使用方法同旧类
//    /// </summary>
//    public class TextHookHandle_past
//    {
//        /// <summary>
//        /// Textractor进程
//        /// </summary>
//        public Process ProcessTextractor;

//        /// <summary>
//        /// Hook功能选择界面提供的数据收到事件
//        /// </summary>
//        public event HookFunSelectDataRecvEventHandler HFSevent;

//        /// <summary>
//        /// Hook功能重新选择界面提供的数据收到事件
//        /// </summary>
//        public event HookFunReSelectDataRecvEventHandler HFRSevent;

//        /// <summary>
//        /// 翻译界面或文本去重界面提供的数据收到事件
//        /// </summary>
//        public event SolvedDataRecvEventHandler Sevent;

//        /// <summary>
//        /// 暂停Hook标志,为真时暂停获取文本
//        /// </summary>
//        public bool Pause;

//        /// <summary>
//        /// 去除无关Hook标志，请先定义好需要用的Misaka码，设为真时，将在接受事件的时候边接收边删除
//        /// </summary>
//        public bool DetachUnrelatedHookWhenDataRecv;
        

//        /// <summary>
//        /// Misaka特殊码列表：一个Misaka特殊码能固定匹配一个入口函数
//        /// 此列表就表示当前进程要Hook的函数
//        /// </summary>
//        public List<string> MisakaCodeList;

//        /// <summary>
//        /// Hook特殊码列表：用于非首次设置好游戏时，已知特殊码但未知函数入口的情况（一个特殊码对应多个函数入口），这个就是Hook的函数的特殊码列表
//        /// </summary>
//        public List<string> HookCodeList;

//        public Queue<string> TextractorOutPutHistory;//Textractor的输出记录队列，用于查错

//        private int GamePID;//能够获取到文本的游戏进程ID
//        private Dictionary<Process, bool> PossibleGameProcessList;//与gamePID进程同名的进程列表
//        private int HandleMode;//处理的方式 1=已确定的单个进程 2=多个进程寻找能搜到文本的进程
//        private Process MaxMemoryProcess;//最大内存进程，用于智能处理时单独注入这个进程而不是PossibleGameProcessList中的每个进程都注入

//        private int listIndex;//用于Hook功能选择界面的方法序号
//        private Dictionary<string, int> TextractorFun_Index_List;//Misaka特殊码与列表索引一一对应

//        private int listIndex_Re;//用于Hook功能重新选择界面的方法序号
//        private Dictionary<string, int> TextractorFun_Re_Index_List;//Misaka特殊码与列表索引一一对应


//        public TextHookHandle_past(int gamePID)
//        {
//            MisakaCodeList = new List<string>();
//            HookCodeList = new List<string>();
//            ProcessTextractor = null;
//            MaxMemoryProcess = null;
//            GamePID = gamePID;
//            PossibleGameProcessList = null;
//            TextractorOutPutHistory = new Queue<string>(1000);
//            HandleMode = 1;
//            listIndex = 0;
//            listIndex_Re = 0;
//            TextractorFun_Index_List = new Dictionary<string, int>();
//            TextractorFun_Re_Index_List = new Dictionary<string, int>();
//        }

//        public TextHookHandle_past(List<Process> GameProcessList)
//        {
//            MisakaCodeList = new List<string>();
//            HookCodeList = new List<string>();
//            ProcessTextractor = null;
//            GamePID = -1;
//            TextractorOutPutHistory = new Queue<string>(1000);
//            PossibleGameProcessList = new Dictionary<Process, bool>();
//            MaxMemoryProcess = GameProcessList[0];
//            for (int i = 0; i < GameProcessList.Count; i++)
//            {
//                if (GameProcessList[i].WorkingSet64 > MaxMemoryProcess.WorkingSet64)
//                {
//                    MaxMemoryProcess = GameProcessList[i];
//                }
//                PossibleGameProcessList.Add(GameProcessList[i], false);
//            }

//            HandleMode = 2;
//            listIndex = 0;
//            listIndex_Re = 0;
//            TextractorFun_Index_List = new Dictionary<string, int>();
//            TextractorFun_Re_Index_List = new Dictionary<string, int>();
//        }

//        ~TextHookHandle_past()
//        {
//            if (ProcessTextractor != null)
//            {
//                CloseTextractor();
//            }
//        }

//        /// <summary>
//        /// 初始化Textractor,建立CLI与本软件间的通信
//        /// </summary>
//        /// <returns>成功返回真，失败返回假</returns>
//        public bool Init(bool x86 = true)
//        {
//            string Path;
//            if (x86 == true)
//            {
//                //打开x86的Textractor
//                Path = Environment.CurrentDirectory + "\\lib\\TextHook\\x86";
//            }
//            else
//            {
//                //打开x64的Textractor
//                Path = Environment.CurrentDirectory + "\\lib\\TextHook\\x64";
//            }
            
//            string CurrentPath = Environment.CurrentDirectory;
//            try
//            {
//                Environment.CurrentDirectory = Path;//更改当前工作目录保证TextractorCLI正常运行
//            }
//            catch (System.IO.DirectoryNotFoundException ex)
//            {
//                return false;
//            }

//            ProcessTextractor = new Process();
//            ProcessTextractor.StartInfo.FileName = "TextractorCLI.exe";
//            ProcessTextractor.StartInfo.CreateNoWindow = true;
//            ProcessTextractor.StartInfo.UseShellExecute = false;
//            ProcessTextractor.StartInfo.StandardOutputEncoding = Encoding.Unicode;
//            ProcessTextractor.StartInfo.RedirectStandardInput = true;
//            ProcessTextractor.StartInfo.RedirectStandardOutput = true;
//            ProcessTextractor.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
//            try
//            {
//                bool res = ProcessTextractor.Start();
//                ProcessTextractor.BeginOutputReadLine();
//                Environment.CurrentDirectory = CurrentPath;//打开后即可恢复原目录
//                return res;
//            }
//            catch (System.ComponentModel.Win32Exception ex)
//            {
//                Environment.CurrentDirectory = CurrentPath;//恢复原目录
//                return false;
//            }
//        }

//        /// <summary>
//        /// 向Textractor CLI写入命令
//        /// 注入进程
//        /// </summary>
//        /// <param name="pid"></param>
//        public async Task AttachProcess(int pid)
//        {
//            await ProcessTextractor.StandardInput.WriteLineAsync("attach -P" + pid);
//            await ProcessTextractor.StandardInput.FlushAsync();
//        }

//        /// <summary>
//        /// 向Textractor CLI写入命令
//        /// 结束注入进程
//        /// </summary>
//        /// <param name="pid"></param>
//        public async Task DetachProcess(int pid)
//        {
//            await ProcessTextractor.StandardInput.WriteLineAsync("detach -P" + pid);
//            await ProcessTextractor.StandardInput.FlushAsync();
//        }

//        /// <summary>
//        /// 向Textractor CLI写入命令
//        /// 给定特殊码注入，由Textractor作者指导方法
//        /// </summary>
//        /// <param name="pid"></param>
//        public async Task AttachProcessByHookCode(int pid,string HookCode)
//        {
//            await ProcessTextractor.StandardInput.WriteLineAsync(HookCode + " -P" + pid);
//            await ProcessTextractor.StandardInput.FlushAsync();
//        }

//        /// <summary>
//        /// 向Textractor CLI写入命令
//        /// 根据Hook入口地址卸载一个Hook，由Textractor作者指导方法
//        /// </summary>
//        /// <param name="pid"></param>
//        public async Task DetachProcessByHookAddress(int pid, string HookAddress)
//        {
//            //这个方法的原理是注入一个用户给定的钩子，给定一个Hook地址，由于hook地址存在，Textractor会自动卸载掉之前的
//            //但是后续给定的模块并不存在，于是Textractor再卸载掉这个用户自定义钩子，达到卸载一个指定Hook办法
//            await ProcessTextractor.StandardInput.WriteLineAsync("HW0@"+ HookAddress + ":module_which_never_exists" + " -P" + pid);
//            await ProcessTextractor.StandardInput.FlushAsync();
//        }

//        /// <summary>
//        /// 关闭Textractor进程，关闭前Detach所有Hook
//        /// </summary>
//        public async void CloseTextractor()
//        {

//            if (ProcessTextractor != null && ProcessTextractor.HasExited == false)
//            {
//                if (HandleMode == 1)
//                {
//                    await DetachProcess(GamePID);
//                }
//                else if (HandleMode == 2)
//                {
//                    for (int i = 0; i < PossibleGameProcessList.Count; i++)
//                    {
//                        var item = PossibleGameProcessList.ElementAt(i);
//                        if (PossibleGameProcessList[item.Key] == true)
//                        {
//                            await DetachProcess(item.Key.Id);
//                            PossibleGameProcessList[item.Key] = false;
//                        }
//                    }
//                }
//                ProcessTextractor.Kill();
//            }

//            ProcessTextractor = null;
//        }
        
//        /// <summary>
//        /// 开始注入，会判断是否智能注入
//        /// </summary>
//        public async void StartHook(bool AutoHook = false)
//        {
//            if (HandleMode == 1)
//            {
//                await AttachProcess(GamePID);
//            }
//            else if (HandleMode == 2)
//            {
//                if (AutoHook == false)
//                {
//                    //不进行智能注入
//                    for (int i = 0; i < PossibleGameProcessList.Count; i++)
//                    {

//                        var item = PossibleGameProcessList.ElementAt(i);
//                        await AttachProcess(item.Key.Id);
//                        PossibleGameProcessList[item.Key] = true;
//                    }
//                }
//                else
//                {
//                    await AttachProcess(MaxMemoryProcess.Id);
//                }
//            }
//        }

//        /// <summary>
//        /// 控制台输出事件，在这做内部消化处理
//        /// </summary>
//        /// <param name="sendingProcess"></param>
//        /// <param name="outLine"></param>
//        void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
//        {
//            AddTextractorHistory(outLine.Data);

//            if (Pause == false)
//            {
//                TextHookData thData = DealTextratorOutput(outLine.Data);
                
//                if (thData != null)
//                {
//                    TextHookData data = thData;
                    
//                    if (data.HookFunc != "Console" && data.HookFunc != "Clipboard" && data.HookFunc != "")
//                    {
//                        //Hook入口选择窗口处理
//                        if (TextractorFun_Index_List.ContainsKey(data.MisakaHookCode) == true)
//                        {
//                            HookSelectRecvEventArgs e = new HookSelectRecvEventArgs();
//                            e.Index = TextractorFun_Index_List[data.MisakaHookCode];
//                            e.Data = data;
//                            HFSevent?.Invoke(this, e);
//                        }
//                        else
//                        {
//                            TextractorFun_Index_List.Add(data.MisakaHookCode, listIndex);
//                            HookSelectRecvEventArgs e = new HookSelectRecvEventArgs();
//                            e.Index = TextractorFun_Index_List[data.MisakaHookCode];
//                            e.Data = data;
//                            HFSevent?.Invoke(this, e);
//                            listIndex++;
//                        }
                        
//                        //Hook入口重复确认窗口处理
//                        if (HookCodeList.Count != 0 && HookCodeList.Contains(data.HookCode))
//                        {
//                            if (TextractorFun_Re_Index_List.ContainsKey(data.MisakaHookCode) == true)
//                            {
//                                HookSelectRecvEventArgs e = new HookSelectRecvEventArgs
//                                {
//                                    Index = TextractorFun_Index_List[data.MisakaHookCode],
//                                    Data = data
//                                };
//                                HFRSevent?.Invoke(this, e);
//                            }
//                            else
//                            {
//                                TextractorFun_Re_Index_List.Add(data.MisakaHookCode, listIndex_Re);
//                                HookSelectRecvEventArgs e = new HookSelectRecvEventArgs
//                                {
//                                    Index = TextractorFun_Index_List[data.MisakaHookCode],
//                                    Data = data
//                                };
//                                HFRSevent?.Invoke(this, e);
//                                listIndex_Re++;
//                            }
//                        }

//                        //使用了边Hook边卸载的情况
//                        //纠错：注意不能删misakacode不同的，因为地址可能相同，仅根据Hook特殊码来删就行了
//                        if (DetachUnrelatedHookWhenDataRecv == true) {
//                            if (HookCodeList[0] != data.HookCode)
//                            {
//                                DetachUnrelatedHookAsync(int.Parse(data.GamePID, System.Globalization.NumberStyles.HexNumber), data.MisakaHookCode);
//                            }

//                        }


//                        //文本去重窗口处理&游戏翻译窗口处理
//                        //如果IsNeedReChooseHook=false则说明没有多重处理，不用再对比HookCodePlus
//                        if (HookCodeList.Count != 0 && HookCodeList.Contains(data.HookCode) && (MisakaCodeList == null || MisakaCodeList.Contains(data.MisakaHookCode)) )
//                        {
//                            SolvedDataRecvEventArgs e = new SolvedDataRecvEventArgs
//                            {
//                                Data = data
//                            };
//                            Sevent?.Invoke(this,e);
//                        }
                        
//                    }
//                }


//            }

//        }
        
//        /// <summary>
//        /// 卸载无关联Hook：缓解某些游戏同时进行很多Hook时造成的卡顿现象
//        /// </summary>
//        /// <param name="pid">欲操作的进程号（要求确保此进程号是游戏主进程且先对此进程号Attach）</param>
//        /// <param name="UsedHookAddress">正在使用中的HookAddress列表，将卸载掉不存在于这个列表中的其他Hook</param>
//        public async void DetachUnrelatedHooks(int pid,List<string> UsedHookAddress) {

//            var FunList = TextractorFun_Index_List.Keys.ToList();//这个得到的是MisakaCode列表
//            for (int i = 0;i < TextractorFun_Index_List.Count;i++) {
//                if (UsedHookAddress.Contains(GetHookAddressByMisakaCode(FunList[i])) == false) {
//                    await DetachProcessByHookAddress(pid, GetHookAddressByMisakaCode(FunList[i]));
//                }
//            }
//        }

//        /// <summary>
//        /// 通过MisakaCode提取HookAddress
//        /// </summary>
//        /// <param name="MisakaCode"></param>
//        /// <returns></returns>
//        public string GetHookAddressByMisakaCode(string MisakaCode) {
//            return GetMiddleString(MisakaCode, "【", ":", 0);
//        }

//        /// <summary>
//        /// 记录Textractor的历史输出记录
//        /// </summary>
//        /// <param name="output"></param>
//        public void AddTextractorHistory(string output)
//        {
//            if (TextractorOutPutHistory.Count >= 1000)
//            {
//                TextractorOutPutHistory.Dequeue();
//            }
//            TextractorOutPutHistory.Enqueue(output);
//        }

//        /// <summary>
//        /// 取出文本中间一部分
//        /// </summary>
//        /// <param name="Text">整个文本</param>
//        /// <param name="front">前面的文本</param>
//        /// <param name="back">后面的文本</param>
//        /// <param name="location">起始搜寻位置</param>
//        /// <returns></returns>
//        private string GetMiddleString(string Text, string front, string back, int location)
//        {

//            if (front == "" || back == "")
//            {
//                return null;
//            }

//            int locA = Text.IndexOf(front, location);
//            int locB = Text.IndexOf(back, locA + 1);
//            if (locA < 0 || locB < 0)
//            {
//                return null;
//            }
//            else
//            {
//                locA = locA + front.Length;
//                locB = locB - locA;
//                if (locA < 0 || locB < 0)
//                {
//                    return null;
//                }
//                return Text.Substring(locA, locB);
//            }
//        }

//        /// <summary>
//        /// 智能处理来自Textrator的输出并返回一个TextHookData用于下一步处理(TextHookData可为空)
//        /// 具体的含义参见TextHookData定义
//        /// </summary>
//        /// <param name="OutputText">来自Textrator的输出</param>
//        /// <returns></returns>
//        private TextHookData DealTextratorOutput(string OutputText)
//        {
//            if (OutputText == "" || OutputText == null)
//            {
//                return null;
//            }

//            string Info = GetMiddleString(OutputText, "[", "]", 0);
//            if (Info == null)
//            {
//                return null;
//            }

//            string[] Infores = Info.Split(':');

//            if (Infores.Length >= 7)
//            {
//                TextHookData thd = new TextHookData();
                
//                string content = OutputText.Replace("[" + Info + "] ", "");//删除信息头部分
                
//                thd.GamePID = Infores[1]; //游戏/本体进程ID（为0一般代表Textrator本体进程ID）

//                thd.HookFunc = Infores[5]; //方法名：Textrator注入游戏进程获得文本时的方法名（为 Console 时代表Textrator本体控制台输出；为 Clipboard 时代表从剪贴板获取的文本）
                
//                thd.HookCode = Infores[6]; //特殊码：Textrator注入游戏进程获得文本时的方法的特殊码，是一个唯一值，可用于判断

//                thd.Data = content; //实际获取到的内容

//                thd.HookAddress = Infores[2]; //Hook入口地址：可用于以后卸载Hook

//                thd.MisakaHookCode = "【" + Infores[2] + ":" + Infores[3] + ":" + Infores[4] + "】"; //【值1:值2:值3】见上方格式说明


//                return thd;
//            }
//            else
//            {
//                return null;
//            }

//        }

//        /// <summary>
//        /// 卸载无关Hook，仅用于处理事件中
//        /// </summary>
//        /// <param name="pid"></param>
//        /// <param name="misakacode"></param>
//        /// <returns></returns>
//        private async Task DetachUnrelatedHookAsync(int pid,string misakacode) {
//            await DetachProcessByHookAddress(pid, GetHookAddressByMisakaCode(misakacode));
//        }
//    }
//}
