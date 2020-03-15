using System;

namespace SettingsHelper
{
    /// <summary>
    /// 读取对象的接口
    /// </summary>
    interface ISettingsReader
    {
        /// <summary>
        /// 获取设置的接口
        /// </summary>
        /// <returns>设置对象</returns>
        void GetSettings();
    }
}
