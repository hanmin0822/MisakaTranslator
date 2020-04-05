using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextHookLibrary;

namespace MisakaTranslator_WPF
{
    public class Common
    {
        public static IAppSettings appSettings;
        public static TextHookHandle textHooker;


        public static bool ExportTextractorHistory()
        {
            try
            {
                if (textHooker != null)
                {
                    FileStream fs = new FileStream("TextractorOutPutHistory.txt", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine("=================以下是Textractor的历史输出记录================");
                    string[] history = textHooker.TextractorOutPutHistory.ToArray();
                    for (int i = 0; i < history.Length; i++)
                    {
                        sw.WriteLine(history[i]);
                    }

                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    return true;
                }
                else {
                    return false;
                }
            }
            catch (System.NullReferenceException) {
                return false;
            }
        }
    }
}
