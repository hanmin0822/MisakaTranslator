/*
 *Namespace         MisakaTranslator
 *Class             Program
 *Description       程序的入口
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MisakaTranslator
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Common.HookCode = "";
            Common.HistoryTextInfos = new Queue<TextInfo>(5);
            Common.TextractorOutPutHistory = new Queue<string>(1000);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
