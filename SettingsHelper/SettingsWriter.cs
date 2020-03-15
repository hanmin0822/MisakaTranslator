using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsHelper
{
    /// <summary>
    /// 保存设置的接口
    /// </summary>
    interface ISettingsWriter
    {
        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="settings">设置对象</param>
        void SaveSettings();
    }
}
