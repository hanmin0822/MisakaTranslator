using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppEnvironmentLibrary;

namespace TransOptimizationLibrary
{
    public class BeforeTransHandle
    {

        NounTransOptimization nto;



        public BeforeTransHandle(string gameName,string srcLang,string dstLang) {
            if (!Directory.Exists(AppEnvironment.LocalFolder + "\\TransOptimization"))
                Directory.CreateDirectory(AppEnvironment.LocalFolder + "\\TransOptimization");

            nto = new NounTransOptimization(gameName,srcLang,dstLang);
        }

        /// <summary>
        /// 外部调用的共用方法，自动进行翻译前处理
        /// </summary>
        /// <param name="text">Hook或OCR得到的原句</param>
        /// <returns>处理后句子</returns>
        public string AutoHandle(string text) {
            if (text == null || text == "") {
                return "";
            }

            //暂支持人名地名预处理
            return nto.ReplacePeoPleLocNameInSentence(text);

        }

        /// <summary>
        /// 显示在结果中的对话人名 以 人名：对话 的形式展示
        /// </summary>
        /// <returns></returns>
        public string GetPeopleChatName() {
            return nto.PeopleChatName;
        }
    }
}
