using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;

namespace JSONHelper
{
    public class JSONWriter
    {
        private string Path//文件路径
        {
            get;
            set;
        }
        public JSONWriter(string path)
        {
            this.Path = path;
        }
        /// <summary>
        /// 写入JSON文件
        /// </summary>
        /// <typeparam name="T">要传入的对象类型</typeparam>
        /// <param name="jsonText"></param>
        public void WriteJSON<T>(T jsonText)
        {
            try
            {
                /*            
                 UnauthorizedAccessException
                    拒绝访问。
                 ArgumentException
                    path 为空字符串 ("")。
                    或 path 包含系统设备的名称（com1、com2 等等）。
                 ArgumentNullException
                    path 为 null。
                 DirectoryNotFoundException
                    指定的路径无效（例如，它位于未映射的驱动器上）。
                 PathTooLongException
                    指定的路径和/或文件名超过了系统定义的最大长度。
                 IOException
                    path 包含不正确或无效的文件名、目录名或卷标签的语法。
                 SecurityException
                    调用方没有所要求的权限。
                 */
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter streamWriter = new StreamWriter(Path))
                using (JsonWriter writer = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(writer, jsonText);
                }
            }
            catch (Exception ex)//在上层处理异常
            {
                throw ex;
            }
        }
    }
}
