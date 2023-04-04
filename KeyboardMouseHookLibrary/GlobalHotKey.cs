using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KeyboardMouseHookLibrary
{

    //代码来源：https://www.cnblogs.com/margin-gu/p/5887853.html

    public class GlobalHotKey
    {
        //引入系统API
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, Keys vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        private int _keyId = 10;     //区分不同的快捷键
        private readonly Dictionary<int, HotKeyCallBackHandler> _keymap = new Dictionary<int, HotKeyCallBackHandler>();   //每一个key对于一个处理函数
        public delegate void HotKeyCallBackHandler();

        //组合控制键
        public enum HotkeyModifiers
        {
            Alt = 1,
            Control = 2,
            Shift = 4,
            Win = 8
        }

        //注册快捷键
        public bool RegisterGlobalHotKey(IntPtr hWnd, int modifiers, Keys vk, HotKeyCallBackHandler callBack)
        {
            int id = _keyId++;
            bool registerHotkeyResult = RegisterHotKey(hWnd, id, modifiers, vk);
            _keymap[id] = callBack;
            return registerHotkeyResult;
        }

        // 注销快捷键
        public void UnRegisterGlobalHotKey(IntPtr hWnd, HotKeyCallBackHandler callBack)
        {
            foreach (KeyValuePair<int, HotKeyCallBackHandler> var in _keymap)
            {
                if (var.Value == callBack)
                {
                    UnregisterHotKey(hWnd, var.Key);
                    return;
                }
            }
        }

        // 快捷键消息处理
        public void ProcessHotKey(Message m)
        {
            if (m.Msg == 0x312)
            {
                int id = m.WParam.ToInt32();
                if (_keymap.TryGetValue(id, out HotKeyCallBackHandler callback))
                    callback();
            }
        }

        /// <summary>
        /// 根据键值组合字符串注册
        /// </summary>
        /// <param name="str"></param>
        /// <param name="handle"></param>
        /// <param name="callback"></param>
        public bool RegisterHotKeyByStr(string str, IntPtr handle, HotKeyCallBackHandler callback)
        {
            if (str == "")
                return false;
            int modifiers = 0;
            Keys vk = Keys.None;
            foreach (string value in str.Split('+'))
            {
                switch (value.Trim())
                {
                    case "Ctrl":
                        modifiers += (int)HotkeyModifiers.Control;
                        break;
                    case "Alt":
                        modifiers += (int)HotkeyModifiers.Alt;
                        break;
                    case "Shift":
                        modifiers += (int)HotkeyModifiers.Shift;
                        break;
                    default:
                    {
                        string pattern = Regex.IsMatch(value, @"[0-9]") ?  "D" + value.Trim() : value.Trim();
                        if (!Enum.TryParse<Keys>(pattern, out vk))
                            return false;
                        break;
                    }
                }
            }


            //这里注册了Ctrl+Alt+E 快捷键
            return RegisterGlobalHotKey(handle, modifiers, vk, callback);
        }

    }
}
