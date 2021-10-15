using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorLibrary
{
    //参考方法来源：https://www.lgztx.com/?p=220

    public class KingsoftFastAITTranslator : ITranslator
    {
        const string DEFAULT_DIC = "DCT";

        int buffersize = 0x4f4;
        int key = 0x4f4;

        //=====================================日译汉===================================

        [DllImport("JPNSCHSDK.dll", EntryPoint = "StartSession")]
        internal static extern int StartSession_JPNSCH(
            [MarshalAs(UnmanagedType.LPWStr)]  string dicpath,
            IntPtr bufferStart,
            IntPtr bufferStop,
            [MarshalAs(UnmanagedType.LPWStr)] string app
        );

        [DllImport("JPNSCHSDK.dll", EntryPoint = "EndSession")]
        internal static extern int EndSession_JPNSCH();

        [DllImport("JPNSCHSDK.dll", EntryPoint = "OpenEngine")]
        internal static extern int OpenEngine_JPNSCH(int key);

        [DllImport("JPNSCHSDK.dll", EntryPoint = "CloseEngineM")]
        internal static extern int CloseEngineM_JPNSCH(int key);

        [DllImport("JPNSCHSDK.dll", EntryPoint = "SimpleTransSentM")]
        internal static extern int SimpleTransSentM_JPNSCH(
            int key,
            [MarshalAs(UnmanagedType.LPWStr)] string fr,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder t,
            int unknown,
            int unknown2
            );

        [DllImport("JPNSCHSDK.dll", EntryPoint = "SetBasicDictPathW")]
        internal static extern int SetBasicDictPathW_JPNSCH(
            int key,
            [MarshalAs(UnmanagedType.LPWStr)]  string fr
            );

        [DllImport("JPNSCHSDK.dll", EntryPoint = "SetProfDictPathW")]
        internal static extern int SetProfDictPathW_JPNSCH(
            int key,
            [MarshalAs(UnmanagedType.LPWStr)] string path,
            int priority
            );


        //=====================================英译汉===================================

        [DllImport("EngSChSDK.dll", EntryPoint = "StartSession")]
        internal static extern int StartSession_EngSCh(
            [MarshalAs(UnmanagedType.LPWStr)]  string dicpath,
            IntPtr bufferStart,
            IntPtr bufferStop,
            [MarshalAs(UnmanagedType.LPWStr)] string app
        );

        [DllImport("EngSChSDK.dll", EntryPoint = "EndSession")]
        internal static extern int EndSession_EngSCh();

        [DllImport("EngSChSDK.dll", EntryPoint = "OpenEngine")]
        internal static extern int OpenEngine_EngSCh(int key);

        [DllImport("EngSChSDK.dll", EntryPoint = "CloseEngineM")]
        internal static extern int CloseEngineM_EngSCh(int key);

        [DllImport("EngSChSDK.dll", EntryPoint = "SimpleTransSentM")]
        internal static extern int SimpleTransSentM_EngSCh(
            int key,
            [MarshalAs(UnmanagedType.LPWStr)] string fr,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder t,
            int unknown,
            int unknown2
            );

        [DllImport("EngSChSDK.dll", EntryPoint = "SetBasicDictPathW")]
        internal static extern int SetBasicDictPathW_EngSCh(
            int key,
            [MarshalAs(UnmanagedType.LPWStr)]  string fr
            );

        [DllImport("EngSChSDK.dll", EntryPoint = "SetProfDictPathW")]
        internal static extern int SetProfDictPathW_EngSCh(
            int key,
            [MarshalAs(UnmanagedType.LPWStr)] string path,
            int priority
            );


        public string FilePath;//文件路径
        private string errorInfo;//错误信息

        public string GetLastError()
        {
            return errorInfo;
        }

        public async Task<string> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            if (FilePath == "" || desLang != "zh")
            {
                return null;
            }

            
            IntPtr buffer = Marshal.AllocHGlobal(buffersize);
            StringBuilder to = new StringBuilder(0x400);
            string path = Environment.CurrentDirectory;

            if (srcLang == "en")
            {
                
                Environment.CurrentDirectory = FilePath + "\\GTS\\EnglishSChinese\\";

                string dicPath = FilePath + "\\GTS\\EnglishSChinese\\" + DEFAULT_DIC;
                try
                {
                    StartSession_EngSCh(dicPath, buffer, buffer + buffersize, "DCT");//return 0 成功
                    OpenEngine_EngSCh(key); //return 0 成功
                    SetBasicDictPathW_EngSCh(key, dicPath);//return 0 成功
                    SimpleTransSentM_EngSCh(key, sourceText, to, 0x28, 0x4);//return 0 成功
                }
                catch (Exception ex)
                {
                    Environment.CurrentDirectory = path;
                    errorInfo = ex.Message;
                    return null;
                }
                finally
                {
                    CloseEngineM_EngSCh(key);
                    EndSession_EngSCh();
                    Marshal.FreeHGlobal(buffer);
                }
            }
            else if (srcLang == "jp")
            {
                Environment.CurrentDirectory = FilePath + "\\GTS\\JapaneseSChinese\\";
                string dicPath = FilePath + "\\GTS\\JapaneseSChinese\\" + DEFAULT_DIC;
                try
                {
                    StartSession_JPNSCH(dicPath, buffer, buffer + buffersize, "DCT");//return 0 成功
                    OpenEngine_JPNSCH(key); //return 0 成功
                    SetBasicDictPathW_JPNSCH(key, dicPath);//return 0 成功
                    SimpleTransSentM_JPNSCH(key, sourceText, to, 0x28, 0x4);//return 0 成功
                }
                catch (Exception ex)
                {
                    Environment.CurrentDirectory = path;
                    errorInfo = ex.Message;
                    return null;
                }
                finally
                {
                    CloseEngineM_JPNSCH(key);
                    EndSession_JPNSCH();
                    Marshal.FreeHGlobal(buffer);
                }
            }
            else
            {
                return null;
            }
            Environment.CurrentDirectory = path;
            return to.ToString();
        }

        public void TranslatorInit(string param1, string param2 = "")
        {
            FilePath = param1;
        }
    }
}
