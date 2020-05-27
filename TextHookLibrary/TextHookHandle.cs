using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextHookLibrary
{
    //参考方法：https://www.lgztx.com/?p=170

    public class TextHookHandle
    {
        /// <summary>
        /// Hook功能选择界面提供的数据收到事件
        /// </summary>
        public event HookFunSelectDataRecvEventHandler HFSevent;

        /// <summary>
        /// Hook功能重新选择界面提供的数据收到事件
        /// </summary>
        public event HookFunReSelectDataRecvEventHandler HFRSevent;

        /// <summary>
        /// 翻译界面或文本去重界面提供的数据收到事件
        /// </summary>
        public event SolvedDataRecvEventHandler Sevent;

        /// <summary>
        /// 暂停Hook标志,为真时暂停获取文本
        /// </summary>
        public bool Pause;

        /// <summary>
        /// 去除无关Hook标志，请先定义好需要用的Misaka码，设为真时，将在接受事件的时候边接收边删除
        /// </summary>
        public bool DetachUnrelatedHookWhenDataRecv;


        /// <summary>
        /// Misaka特殊码列表：一个Misaka特殊码能固定匹配一个入口函数
        /// 此列表就表示当前进程要Hook的函数
        /// </summary>
        public List<string> MisakaCodeList;

        /// <summary>
        /// Hook特殊码列表：用于非首次设置好游戏时，已知特殊码但未知函数入口的情况（一个特殊码对应多个函数入口），这个就是Hook的函数的特殊码列表
        /// </summary>
        public List<string> HookCodeList;

        /// <summary>
        /// 用户自定义Hook特殊码：用于非首次设置好游戏时，能让系统自动注入一次
        /// </summary>
        public string HookCode_Custom;

        public Queue<string> TextractorOutPutHistory;//Textractor的输出记录队列，用于查错


        /// <summary>
        /// Textractor输出事件（根据线程判断输出）
        /// </summary>
        private TextHostLib.OnOutputFunc output;

        /// <summary>
        /// Textractor回调事件
        /// </summary>
        private TextHostLib.ProcessEvent callback;

        /// <summary>
        /// Textractor创建线程事件（从此线程获得具体信息）
        /// </summary>
        private TextHostLib.OnCreateThreadFunc createthread;

        /// <summary>
        /// Textractor销毁线程事件
        /// </summary>
        private TextHostLib.OnRemoveThreadFunc removethread;

        public int GamePID;//能够获取到文本的游戏进程ID


        private Dictionary<Process, bool> PossibleGameProcessList;//与gamePID进程同名的进程列表
        private int HandleMode;//处理的方式 1=已确定的单个进程 2=多个进程寻找能搜到文本的进程 3=剪贴板监控
        private Process MaxMemoryProcess;//最大内存进程，用于智能处理时单独注入这个进程而不是PossibleGameProcessList中的每个进程都注入

        private int listIndex;//用于Hook功能选择界面的方法序号
        private Dictionary<long, int> TextractorFun_Index_List;//Hook线程ID与列表索引一一对应

        private int listIndex_Re;//用于Hook功能重新选择界面的方法序号
        private Dictionary<long, int> TextractorFun_Re_Index_List;//Hook线程ID与列表索引一一对应

        private Dictionary<long, TextHookData> ThreadID_HookDataInfo_List;//Hook线程ID与Hook详细信息一一对应

        private Dictionary<long, int> ThreadID_RenewNum_List;//Hook线程ID与刷新次数一一对应，仅用于HookFunReSelectDataRecvEventHandler和HookFunSelectDataRecvEventHandler

        public TextHookHandle(int gamePID)
        {
            MisakaCodeList = new List<string>();
            HookCodeList = new List<string>();
            MaxMemoryProcess = null;
            GamePID = gamePID;
            PossibleGameProcessList = null;
            TextractorOutPutHistory = new Queue<string>(2000);
            HandleMode = 1;
            listIndex = 0;
            listIndex_Re = 0;
            TextractorFun_Index_List = new Dictionary<long, int>();
            TextractorFun_Re_Index_List = new Dictionary<long, int>();
            ThreadID_HookDataInfo_List = new Dictionary<long, TextHookData>();
            ThreadID_RenewNum_List = new Dictionary<long, int>();
        }

        public TextHookHandle(List<Process> GameProcessList)
        {
            MisakaCodeList = new List<string>();
            HookCodeList = new List<string>();
            GamePID = -1;
            TextractorOutPutHistory = new Queue<string>(2000);
            PossibleGameProcessList = new Dictionary<Process, bool>();
            MaxMemoryProcess = GameProcessList[0];
            for (int i = 0; i < GameProcessList.Count; i++)
            {
                if (GameProcessList[i].WorkingSet64 > MaxMemoryProcess.WorkingSet64)
                {
                    MaxMemoryProcess = GameProcessList[i];
                }
                PossibleGameProcessList.Add(GameProcessList[i], false);
            }

            HandleMode = 2;
            listIndex = 0;
            listIndex_Re = 0;
            TextractorFun_Index_List = new Dictionary<long, int>();
            TextractorFun_Re_Index_List = new Dictionary<long, int>();
            ThreadID_HookDataInfo_List = new Dictionary<long, TextHookData>();
            ThreadID_RenewNum_List = new Dictionary<long, int>();
        }

        public TextHookHandle() {
            //剪贴板方式读取专用
            MisakaCodeList = new List<string>();
            HookCodeList = new List<string>();
            MaxMemoryProcess = null;
            GamePID = -1;
            PossibleGameProcessList = null;
            TextractorOutPutHistory = new Queue<string>(2000);
            HandleMode = 3;
            listIndex = 0;
            listIndex_Re = 0;
            TextractorFun_Index_List = new Dictionary<long, int>();
            TextractorFun_Re_Index_List = new Dictionary<long, int>();
            ThreadID_HookDataInfo_List = new Dictionary<long, TextHookData>();
            ThreadID_RenewNum_List = new Dictionary<long, int>();
        }

        /// <summary>
        /// 结束时自动卸载所有Hook，调用方法即令此对象为null然后立即GC回收
        /// </summary>
        ~TextHookHandle()
        {
            if (HandleMode == 1)
            {
                DetachProcess(GamePID);
            }
            else if (HandleMode == 2)
            {
                for (int i = 0; i < PossibleGameProcessList.Count; i++)
                {
                    var item = PossibleGameProcessList.ElementAt(i);
                    if (PossibleGameProcessList[item.Key] == true)
                    {
                        DetachProcess(item.Key.Id);
                        PossibleGameProcessList[item.Key] = false;
                    }
                }
            }


            GC.Collect();
        }

        /// <summary>
        /// 初始化Textractor
        /// </summary>
        /// <returns>成功返回真，失败返回假</returns>
        public bool Init(bool x86 = true)
        {
            string DllPath = "";
            string ModulePath = "";
            if (x86 == true)
            {
                //复制x86的dll
                ModulePath = Environment.CurrentDirectory + "\\lib\\TextHook\\x86\\texthost.dll";
                DllPath = Environment.CurrentDirectory + "\\lib\\TextHook\\x86\\texthook.dll";
            }
            else
            {
                //复制x64的dll
                ModulePath = Environment.CurrentDirectory + "\\lib\\TextHook\\x64\\texthost.dll";
                DllPath = Environment.CurrentDirectory + "\\lib\\TextHook\\x64\\texthook.dll";
            }

            if (File.Exists(DllPath) && File.Exists(ModulePath))//判断要复制的文件是否存在
            {
                try {
                    File.Copy(DllPath, Environment.CurrentDirectory + "\\texthook.dll", true);
                    File.Copy(ModulePath, Environment.CurrentDirectory + "\\texthost.dll", true);
                }
                catch (System.IO.IOException) {
                    throw new Exception("无法复制texthook.dll或texthost.dll，请重启游戏和翻译器后再试！");
                }
            }
            else
            {
                return false;
            }

            createthread = CreateThreadHandle;
            output = OutputHandle;
            removethread = RemoveThreadHandle;
            callback = CallBackHandle;

            TextHostLib.TextHostInit(callback, callback, createthread, removethread, output);
            return true;
        }

        /// <summary>
        /// 注入进程
        /// </summary>
        /// <param name="pid"></param>
        public void AttachProcess(int pid)
        {
            TextHostLib.InjectProcess((uint)pid);
        }

        /// <summary>
        /// 结束注入进程
        /// </summary>
        /// <param name="pid"></param>
        public void DetachProcess(int pid)
        {
            TextHostLib.DetachProcess((uint)pid);
        }

        /// <summary>
        /// 给定特殊码注入
        /// </summary>
        /// <param name="pid"></param>
        public void AttachProcessByHookCode(int pid, string HookCode)
        {
            TextHostLib.InsertHook((uint)pid, HookCode);
        }

        /// <summary>
        /// 根据Hook入口地址卸载一个Hook
        /// </summary>
        /// <param name="pid"></param>
        public void DetachProcessByHookAddress(int pid, string HookAddress)
        {
            TextHostLib.RemoveHook((uint)pid, long.Parse(HookAddress));
        }


        /// <summary>
        /// 开始注入，会判断是否智能注入
        /// </summary>
        public void StartHook(bool AutoHook = false)
        {
            if (HandleMode == 1)
            {
                AttachProcess(GamePID);
            }
            else if (HandleMode == 2)
            {
                //不管是否进行智能注入，为了保证再次开启游戏时某些用户自定义特殊码能直接导入，这里强制让游戏ID为最大进程ID
                GamePID = MaxMemoryProcess.Id;

                if (AutoHook == false)
                {
                    //不进行智能注入
                    for (int i = 0; i < PossibleGameProcessList.Count; i++)
                    {
                        var item = PossibleGameProcessList.ElementAt(i);
                        AttachProcess(item.Key.Id);
                        PossibleGameProcessList[item.Key] = true;
                    }
                }
                else
                {
                    AttachProcess(MaxMemoryProcess.Id);
                }
            }


        }

        /// <summary>
        /// Textractor输出事件
        /// </summary>
        /// <param name="threadid"></param>
        /// <param name="opdata"></param>
        public void OutputHandle(long threadid, string opdata)
        {
            opdata = opdata.Replace("\r\n", "").Replace("\n", "");
            AddTextractorHistory(threadid + ":" + opdata);

            if (Pause == false)
            {
                TextHookData thData = ThreadID_HookDataInfo_List[threadid];
                
                if (thData != null)
                {
                    TextHookData data = thData;
                    data.Data = opdata;

                    if (data.HookFunc != "Console" && data.HookFunc != "Clipboard" && data.HookFunc != "")
                    {
                        //Hook入口选择窗口处理
                        //if (ThreadID_RenewNum_List[threadid] < 150)
                        //{
                        if (TextractorFun_Index_List.ContainsKey(threadid) == true)
                        {
                            HookSelectRecvEventArgs e = new HookSelectRecvEventArgs();
                            e.Index = TextractorFun_Index_List[threadid];
                            e.Data = data;
                            HFSevent?.Invoke(this, e);
                        }
                        else
                        {
                            TextractorFun_Index_List.Add(threadid, listIndex);
                            HookSelectRecvEventArgs e = new HookSelectRecvEventArgs();
                            e.Index = listIndex;
                            e.Data = data;
                            HFSevent?.Invoke(this, e);
                            listIndex++;
                        }
                        //ThreadID_RenewNum_List[threadid]++;
                        //}
                        //else {
                        //    DetachProcessByHookAddress(data.GamePID, data.HookAddress);
                        //}


                        //Hook入口重复确认窗口处理
                        if (HookCodeList.Count != 0 && HookCodeList.Contains(data.HookCode))
                        {
                            if (TextractorFun_Re_Index_List.ContainsKey(threadid) == true)
                            {
                                HookSelectRecvEventArgs hsre = new HookSelectRecvEventArgs
                                {
                                    Index = TextractorFun_Index_List[threadid],
                                    Data = data
                                };
                                HFRSevent?.Invoke(this, hsre);
                            }
                            else
                            {
                                TextractorFun_Re_Index_List.Add(threadid, listIndex_Re);
                                HookSelectRecvEventArgs hsre = new HookSelectRecvEventArgs
                                {
                                    Index = listIndex_Re,
                                    Data = data
                                };
                                HFRSevent?.Invoke(this, hsre);
                                listIndex_Re++;
                            }
                        }

                        //使用了边Hook边卸载的情况
                        //纠错：注意不能删misakacode不同的，因为地址可能相同，仅根据Hook特殊码来删就行了
                        if (DetachUnrelatedHookWhenDataRecv == true)
                        {
                            if (HookCodeList[0] != data.HookCode)
                            {
                                DetachUnrelatedHookAsync(data.GamePID, data.MisakaHookCode);
                            }

                        }


                        //文本去重窗口处理&游戏翻译窗口处理
                        //如果IsNeedReChooseHook=false则说明没有多重处理，不用再对比HookCodePlus
                        if (HookCodeList.Count != 0 && HookCodeList.Contains(data.HookCode) && (MisakaCodeList == null || MisakaCodeList.Contains(data.MisakaHookCode)))
                        {
                            SolvedDataRecvEventArgs sdre = new SolvedDataRecvEventArgs
                            {
                                Data = data
                            };
                            Sevent?.Invoke(this, sdre);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Textractor创建线程事件
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="processId"></param>
        /// <param name="address"></param>
        /// <param name="context"></param>
        /// <param name="subcontext"></param>
        /// <param name="name"></param>
        /// <param name="hookCode"></param>
        public void CreateThreadHandle(long threadId, uint processId, long address, long context, long subcontext, string name, string hookCode)
        {
            TextHookData data = new TextHookData()
            {
                GamePID = (int)processId,
                HookCode = hookCode,
                HookAddress = "" + address,
                MisakaHookCode = "【" + address + ":" + context + ":" + subcontext + "】",
                HookFunc = name
            };

            //ThreadID_RenewNum_List.Add(threadId,0);
            ThreadID_HookDataInfo_List.Add(threadId, data);
        }

        /// <summary>
        /// Textractor移除线程事件
        /// </summary>
        /// <param name="threadId"></param>
        public void RemoveThreadHandle(long threadId) {
            //无用
        }

        /// <summary>
        /// Textractor回调事件
        /// </summary>
        /// <param name="threadId"></param>
        public void CallBackHandle(int processId)
        {
            //无用
        }

        /// <summary>
        /// 卸载无关联Hook：缓解某些游戏同时进行很多Hook时造成的卡顿现象
        /// </summary>
        /// <param name="pid">欲操作的进程号（要求确保此进程号是游戏主进程且先对此进程号Attach）</param>
        /// <param name="UsedHookAddress">正在使用中的HookAddress列表，将卸载掉不存在于这个列表中的其他Hook</param>
        public void DetachUnrelatedHooks(int pid, List<string> UsedHookAddress)
        {
            var FunList = TextractorFun_Index_List.Keys.ToList();//这个得到的是MisakaCode列表
            for (int i = 0; i < TextractorFun_Index_List.Count; i++)
            {
                if (UsedHookAddress.Contains(ThreadID_HookDataInfo_List[FunList[i]].HookAddress) == false)
                {
                    DetachProcessByHookAddress(pid, ThreadID_HookDataInfo_List[FunList[i]].HookAddress);
                }
            }
        }

        /// <summary>
        /// 通过MisakaCode提取HookAddress
        /// </summary>
        /// <param name="MisakaCode"></param>
        /// <returns></returns>
        public string GetHookAddressByMisakaCode(string MisakaCode)
        {
            return GetMiddleString(MisakaCode, "【", ":", 0);
        }

        /// <summary>
        /// 记录Textractor的历史输出记录
        /// </summary>
        /// <param name="output"></param>
        public void AddTextractorHistory(string output)
        {
            if (TextractorOutPutHistory.Count >= 999)
            {
                TextractorOutPutHistory.Dequeue();
            }
            TextractorOutPutHistory.Enqueue(output);
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

            if (Text == null || front == "" || back == "")
            {
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
        /// 卸载无关Hook，仅用于处理事件中
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="misakacode"></param>
        /// <returns></returns>
        private void DetachUnrelatedHookAsync(int pid, string misakacode)
        {
            DetachProcessByHookAddress(pid, GetHookAddressByMisakaCode(misakacode));
        }

        /// <summary>
        /// 让系统自动注入用户设定好的特殊码，没有就不注入
        /// </summary>
        public void Auto_AddHookToGame() {
            if (HookCode_Custom != "NULL" || HookCode_Custom != "")
            {
                AttachProcessByHookCode(GamePID, HookCode_Custom);
            }
        }


        /// <summary>
        /// 添加剪切板监视线程
        /// </summary>
        /// <param name="winHandle"></param>
        public void AddClipBoardThread(IntPtr winHandle) {
            TextHostLib.AddClipboardThread(winHandle);
        }

    }
}
