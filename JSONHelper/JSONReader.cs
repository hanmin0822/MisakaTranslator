using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace JSONHelper
{
    public class JSONReader
    {
        private string Path//文件路径
        {
            get;
            set;
        }
        public JSONReader(string path)
        {
            Regex regex = new Regex("\\.[jJ][sS][oO][nN]");
            if (!regex.IsMatch(path))//判断拓展名
            {
                throw new ArgumentOutOfRangeException("path", "传入的不是JSON格式文件");//抛出异常
            }
            this.Path = path;
        }

        /// <summary>
        /// 读入JSON文件
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <returns>返回的JSON对象</returns>
        public T ReadJSON<T>()
        {
            try
            {
                /*
                    ArgumentException
                        path 为空字符串 ("")。
                    ArgumentNullException
                        path 或 encoding 为 null。
                    FileNotFoundException
                        无法找到该文件。
                    DirectoryNotFoundException
                        指定的路径无效，例如位于未映射的驱动器上。
                    NotSupportedException
                        path 包括不正确或无效的文件名、目录名或卷标的语法。
                 */
                using (StreamReader streamReader = new StreamReader(Path))
                {
                    return JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd());
                }
            }
            catch(InvalidCastException ex)//捕获类型转换异常
            {
                throw ex;
            }
            catch (Exception ex)//捕获Exception下的所有异常，在上层处理
            {
                throw ex;
            }
        }
    }
}
