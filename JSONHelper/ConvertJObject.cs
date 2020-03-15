//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace JSONHelper
//{
//    public static class ConvertJObject
//    {
//        /// <summary>
//        /// 将JObject向下转型
//        /// </summary>
//        /// <typeparam name="TChildOfJObject">JObject的派生类</typeparam>
//        /// <param name="jObject">要转换的实例</param>
//        /// <returns>转换的实例</returns>
//        static public TChildOfJObject Convert<TChildOfJObject>(JObject jObject) where TChildOfJObject : JObject , new()
//        {
//            TChildOfJObject child = new TChildOfJObject();
//            var properties = typeof(JObject).GetProperties().Where(x => !x.GetIndexParameters().Any());
//            foreach (var property in properties)
//            {
//                if(property.CanRead && property.CanWrite)
//                {
//                    property.SetValue(child, property.GetValue(jObject,null),null);
//                }
//            }
//            return child;
//        }

//    }
//}
