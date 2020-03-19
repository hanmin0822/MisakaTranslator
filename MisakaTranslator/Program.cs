/*
 *Namespace         MisakaTranslator
 *Class             Program
 *Description       程序的入口
 */

using Config.Net;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            Common.settings = new ConfigurationBuilder<IAppSettings>().UseIniFile(Environment.CurrentDirectory + "\\settings\\settings.ini").Build();
            Common.repeatRsettings = new ConfigurationBuilder<IRepeatRepairSettings>().UseIniFile(Environment.CurrentDirectory + "\\settings\\TextRepeatRepair.ini").Build();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
