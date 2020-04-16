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
        static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, Keys vk);
        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        

        int keyid = 10;     //区分不同的快捷键
        Dictionary<int, HotKeyCallBackHanlder> keymap = new Dictionary<int, HotKeyCallBackHanlder>();   //每一个key对于一个处理函数
        public delegate void HotKeyCallBackHanlder();

        //组合控制键
        public enum HotkeyModifiers
        {
            Alt = 1,
            Control = 2,
            Shift = 4,
            Win = 8
        }

        //注册快捷键
        public bool RegistGlobalHotKey(IntPtr hWnd, int modifiers, Keys vk, HotKeyCallBackHanlder callBack)
        {
            int id = keyid++;
            bool res = RegisterHotKey(hWnd, id, modifiers, vk);
            keymap[id] = callBack;
            return res;
        }

        // 注销快捷键
        public void UnRegistGlobalHotKey(IntPtr hWnd, HotKeyCallBackHanlder callBack)
        {
            foreach (KeyValuePair<int, HotKeyCallBackHanlder> var in keymap)
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
                HotKeyCallBackHanlder callback;
                if (keymap.TryGetValue(id, out callback))
                    callback();
            }
        }

        /// <summary>
        /// 根据键值组合字符串注册
        /// </summary>
        /// <param name="str"></param>
        public bool RegistHotKeyByStr(string str,IntPtr Handle, HotKeyCallBackHanlder callback)
        {
            if (str == "")
                return false;
            int modifiers = 0;
            Keys vk = Keys.None;
            foreach (string value in str.Split('+'))
            {
                if (value.Trim() == "Ctrl")
                    modifiers = modifiers + (int)HotkeyModifiers.Control;
                else if (value.Trim() == "Alt")
                    modifiers = modifiers + (int)HotkeyModifiers.Alt;
                else if (value.Trim() == "Shift")
                    modifiers = modifiers + (int)HotkeyModifiers.Shift;
                else
                {
                    if (Regex.IsMatch(value, @"[0-9]"))
                    {
                        vk = (Keys)Enum.Parse(typeof(Keys), "D" + value.Trim());
                    }
                    else
                    {
                        vk = (Keys)Enum.Parse(typeof(Keys), value.Trim());
                    }
                }
            }

            
            //这里注册了Ctrl+Alt+E 快捷键
            return RegistGlobalHotKey(Handle, modifiers, vk, callback);
        }

    }
}
