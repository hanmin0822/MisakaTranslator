using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransOptimizationLibrary
{
    public class AfterTransHandle
    {
        NounTransOptimization nto;
        BeforeTransHandle bth;

        public AfterTransHandle(BeforeTransHandle b)
        {
            bth = b;
        }

        /// <summary>
        /// 外部调用的共用方法，自动进行翻译后处理
        /// </summary>
        /// <param name="text">通过翻译接口翻译后的句子</param>
        /// <returns>处理后句子</returns>
        public string AutoHandle(string text)
        {
            if (text == null || text == "")
            {
                return "";
            }

            //目前只支持名词的预处理，处理后检查是否有对话人名，如果有则加上
            if (bth.GetPeopleChatName() != "") {
                text = bth.GetPeopleChatName() + "： " + text;
            }

            return text;
        }


    }
}
