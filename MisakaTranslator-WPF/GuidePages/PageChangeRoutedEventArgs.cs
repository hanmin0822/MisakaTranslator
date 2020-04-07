using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MisakaTranslator_WPF.GuidePages
{
    public class PageChangeRoutedEventArgs : RoutedEventArgs
    {
        public PageChangeRoutedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

        /// <summary>
        /// 下一页的XAML地址
        /// </summary>
        public string XamlPath { get; set; }

        /// <summary>
        /// 部分方法需要用到的额外参数
        /// </summary>
        public object ExtraArgs;
    }

    public class PageChange
    {
        public string XamlPath;

        //声明和注册路由事件
        public static readonly RoutedEvent PageChangeRoutedEvent =
            EventManager.RegisterRoutedEvent("PageChange", RoutingStrategy.Bubble, typeof(EventHandler<PageChangeRoutedEventArgs>), typeof(PageChange));
        
    }

    
}
