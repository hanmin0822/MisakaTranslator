using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorLibrary
{
    public class JBeijingTranslator : ITranslator
    {
        /*
         * 根据猜测得到的DLL函数 来源 https://github.com/Artikash/VNR-Core/blob/6ed038bda9dcd35696040bd45d31afa6a30e8978/py/libs/jbeijing/jbjct.py
        int __cdecl JC_Transfer_Unicode(
            HWND hwnd,
            UINT fromCodePage,
            UINT toCodePage,
            int unknown,
            int unknown,
            LPCWSTR from,
            LPWSTR to,
            int &toCapacity,
            LPWSTR buffer,
            int &bufferCapacity)
        */

        [DllImport("JBJCT.dll", EntryPoint = "JC_Transfer_Unicode", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern int JC_Transfer_Unicode(
            int hwnd,
            uint fromCodePage,
            uint toCodePage,
            int unknown,
            int unknown1,
            IntPtr from,
            IntPtr to,
            ref int toCapacity,
            IntPtr buffer,
            ref int bufferCapacity);

        public string JBJCTDllPath;//DLL路径
        private string errorInfo;//错误信息

        public string GetLastError()
        {
            return errorInfo;
        }

        public async Task<string> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            string JBeijingTranslatorPath = JBJCTDllPath;

            if (JBeijingTranslatorPath == "")
            {
                return null;
            }

            /*
            CP932   SAP Shift-JIS
            CP950   SAP 繁体中文
            CP936   SAP 简体中文
            */

            string path = Environment.CurrentDirectory;
            Environment.CurrentDirectory = JBeijingTranslatorPath;

            IntPtr jp = Marshal.StringToHGlobalUni(sourceText);

            IntPtr jp2 = Marshal.AllocHGlobal(3000);
            IntPtr jp3 = Marshal.AllocHGlobal(3000);

            int p1 = 1500;
            int p2 = 1500;

            try
            {
                int a = JC_Transfer_Unicode(0, 932, 936, 1, 1, jp, jp2, ref p1, jp3, ref p2);
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return null;
            }

            Environment.CurrentDirectory = path;

            string ret = Marshal.PtrToStringAuto(jp2);

            Marshal.FreeHGlobal(jp);
            Marshal.FreeHGlobal(jp2);
            Marshal.FreeHGlobal(jp3);

            return ret;
        }

        public void TranslatorInit(string param1, string param2 = "")
        {
            JBJCTDllPath = param1;
        }
    }
}
