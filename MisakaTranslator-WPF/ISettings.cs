using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Config.Net;
using HandyControl.Controls;

namespace MisakaTranslator_WPF
{
    public interface ISettings
    {
        #region 界面设置
        #region 前景色设置
        [Option(Alias = "Appearance.Foreground", DefaultValue = "#ffcccc")]
        string ForegroundHex { get; set; }
        #endregion
        #endregion
        //API设置
        //翻译设置
    }
}
