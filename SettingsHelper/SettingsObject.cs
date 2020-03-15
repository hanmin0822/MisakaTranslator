using JSONHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace SettingsHelper
{
    public class SettingsObject : ISettingsReader, ISettingsWriter
    {
        private readonly string SettingsPath = String.Empty;//设置路径
        public JObject Value;
        public SettingsObject()
        {
            SettingsPath = Environment.CurrentDirectory + "\\settings\\settings.json";
            GetSettings();
        }

        /// <summary>
        /// 找不到设置的委托
        /// </summary>
        /// <param name="sender">事件的发布者</param>
        /// <param name="args">传入的参数</param>
        public delegate void SettingsNotFoundEventHandler(object sender, SettingsNotFoundEventArgs args);
        public event SettingsNotFoundEventHandler SettingsNotFound;//声明事件
        /// <summary>
        /// 发布事件的保护方法，可以在派生类中重写
        /// </summary>
        /// <param name="args">传入的参数</param>
        protected virtual void PostEvent(SettingsNotFoundEventArgs args)
        {
            SettingsNotFound?.Invoke(this, args);//发布事件，我不明白为啥这样写，但VS建议我将判空语句简化
        }
        public void SaveSettings()
        {
            //没有能力处理异常(参见JSONWriter类中的异常类型)
            JSONWriter writer = new JSONWriter(SettingsPath);
            writer.WriteJSON<JObject>(this.Value);
            return;
        }

        public void GetSettings()
        {
            try
            {
                JSONReader reader = new JSONReader(SettingsPath);
                Value = reader.ReadJSON<JObject>();
            }
            catch (IOException ex)//捕获FileNotFoundException和DirectoryNotFoundException(参见JSONReader类中的异常类型)a
            {
                PostEvent(new SettingsNotFoundEventArgs("找不到配置文件，将重新生成配置"));
                Value = new JObject();//返回空对象
            }
        }
    }
}
