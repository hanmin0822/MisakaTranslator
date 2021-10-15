using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorLibrary
{
    //参考方法：https://www.lgztx.com/?p=209

    public class DreyeTranslator : ITranslator
    {
        const int EC_DAT = 1;   //英中
        const int CE_DAT = 2;   //中英
        const int CJ_DAT = 3;   //中日
        const int JC_DAT = 10;  //日中

        [DllImport("TransCOM.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int MTInitCJ(int dat_index);

        [DllImport("TransCOM.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int MTEndCJ();

        [DllImport("TransCOM.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int TranTextFlowCJ(
            byte[] src,
            byte[] dest,
            int dest_size,
            int dat_index
            );


        [DllImport("TransCOMEC.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int MTInitEC(int dat_index);

        [DllImport("TransCOMEC.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int MTEndEC();

        [DllImport("TransCOMEC.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int TranTextFlowEC(
            byte[] src,
            byte[] dest,
            int dest_size,
            int dat_index
            );

        public string FilePath;//文件路径
        private string errorInfo;//错误信息

        public string GetLastError()
        {
            return errorInfo;
        }

        public async Task<string> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            if (FilePath == "")
            {
                return null;
            }

            Encoding shiftjis = Encoding.GetEncoding("shift-jis"); 
            Encoding gbk = Encoding.GetEncoding("gbk");
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            string currentpath = Environment.CurrentDirectory;
            string workingDirectory = FilePath + "\\DreyeMT\\SDK\\bin";
            string ret;

            if (desLang == "zh") {
                if (srcLang == "jp")
                {
                    try
                    {
                        Directory.SetCurrentDirectory(workingDirectory);
                        MTInitCJ(JC_DAT); //返回值为-255
                        byte[] src = shiftjis.GetBytes(sourceText);
                        byte[] buffer = new byte[3000];
                        TranTextFlowCJ(src, buffer, 3000, JC_DAT);
                        ret = gbk.GetString(buffer);
                        MTEndCJ();
                    }
                    catch (Exception ex)
                    {
                        Environment.CurrentDirectory = currentpath;
                        errorInfo = ex.Message;
                        return null;
                    }
                }
                else if (srcLang == "en")
                {
                    try
                    {
                        Directory.SetCurrentDirectory(workingDirectory);
                        MTInitEC(EC_DAT); //返回值为-255
                        byte[] src = utf8.GetBytes(sourceText);
                        byte[] buffer = new byte[3000];
                        TranTextFlowEC(src, buffer, 3000, EC_DAT);
                        ret = gbk.GetString(buffer);
                        MTEndEC();
                    }
                    catch (Exception ex)
                    {
                        Environment.CurrentDirectory = currentpath;
                        errorInfo = ex.Message;
                        return null;
                    }
                }
                else {
                    errorInfo = "语言不支持";
                    return null;
                }
            }
            else
            {
                errorInfo = "语言不支持";
                return null;
            }

            return ret;
        }

        public void TranslatorInit(string param1, string param2 = "")
        {
            FilePath = param1;
        }
    }
}
