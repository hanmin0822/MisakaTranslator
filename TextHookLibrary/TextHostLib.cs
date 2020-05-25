using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TextHookLibrary
{
    public class TextHostLib
    {
        public delegate void ProcessEvent(Int32 processId);
        public delegate void OnCreateThreadFunc(Int64 threadId,
            UInt32 processId,
            Int64 address,
            Int64 context,
            Int64 subcontext,
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            [MarshalAs(UnmanagedType.LPWStr)] string hookCode
            );
        public delegate void OnRemoveThreadFunc(Int64 threadId);
        public delegate void OnOutputFunc(Int64 threadId, [MarshalAs(UnmanagedType.LPWStr)] string data);

        [DllImport("texthost.dll")]
        public static extern Int32 TextHostInit(ProcessEvent OnConnect,
            ProcessEvent OnDisconnect,
            OnCreateThreadFunc OnCreateThread,
            OnRemoveThreadFunc OnRemoveThread,
            OnOutputFunc OnOutput
            );

        [DllImport("texthost.dll")]
        public static extern Int32 InsertHook(UInt32 processId, [MarshalAs(UnmanagedType.LPWStr)] string hookCode);

        [DllImport("texthost.dll")]
        public static extern Int32 RemoveHook(UInt32 processId, Int64 address);

        [DllImport("texthost.dll")]
        public extern static Int32 InjectProcess(UInt32 processId);

        [DllImport("texthost.dll")]
        public extern static Int32 DetachProcess(UInt32 processId);

        [DllImport("texthost.dll")]
        public extern static Int32 AddClipboardThread(IntPtr windowHandle);
    }
}
