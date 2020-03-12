/*
 *Namespace         MisakaTranslator
 *Class             Program
 *Description       程序的入口
 *Author            Hanmin Qi
 *LastModifyTime    2020-03-12
 * ===============================================================
 * 以下是修改记录（任何一次修改都应被记录）
 * 日期   修改内容    作者
 * 2020-03-12       代码注释完成      果冻
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
