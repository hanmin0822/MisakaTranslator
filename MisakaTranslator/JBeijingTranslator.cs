/*
 *Namespace         MisakaTranslator
 *Class             JBeijingTranslator
 *Description       JBeijing翻译的调用方法
 */

using System;
using System.Runtime.InteropServices;

namespace MisakaTranslator
{
    class JBeijingTranslator
    {
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

        /// <summary>
        /// 调用JBeijing离线翻译 日语转中文
        /// </summary>
        /// <param name="sourceString">源语句</param>
        /// <param name="issimplified">为真则翻译结果为简体中文</param>
        /// <returns></returns>
        public static string Translate_JapanesetoChinese(string sourceString, bool issimplified = true)
        {
            string JBeijingTranslatorPath = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "JBeijing", "JBJCTDllPath");

            if (JBeijingTranslatorPath == "")
            {
                return null;
            }

            /*
            CP932   SAP Shift-JIS
            CP950   SAP 繁体中文
            CP936   SAP 简体中文
            */
            int desCP;
            if (issimplified == true)
            {
                desCP = 936;
            }
            else
            {
                desCP = 950;
            }

            string path = Environment.CurrentDirectory;
            Environment.CurrentDirectory = JBeijingTranslatorPath;

            IntPtr jp = Marshal.StringToHGlobalUni(sourceString);

            IntPtr jp2 = Marshal.AllocHGlobal(3000);
            IntPtr jp3 = Marshal.AllocHGlobal(3000);

            int p1 = 1500;
            int p2 = 1500;

            try
            {
                int a = JC_Transfer_Unicode(0, 932, (uint)desCP, 1, 1, jp, jp2, ref p1, jp3, ref p2);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Environment.CurrentDirectory = path;

            string ret = Marshal.PtrToStringAuto(jp2);

            Marshal.FreeHGlobal(jp);
            Marshal.FreeHGlobal(jp2);
            Marshal.FreeHGlobal(jp3);

            return ret;
        }

    }
}
