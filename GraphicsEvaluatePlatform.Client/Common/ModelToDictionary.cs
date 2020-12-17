using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GraphicsEvaluatePlatform.Client.Common
{
    public static class ModelToDictionary
    {

        public  static string ValueOf(object obj)
        {
            return (obj == null) ? "null" : obj.ToString(); ;
        }
        /// <summary>
        /// 将model转为Dictionaty键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> EntityToDictionary<T>(T obj)
        {
            //初始化定义一个键值对，注意最后的括号
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //返回当前 Type 的所有公共属性Property集合
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo p in props)
            {
                var property = obj.GetType().GetProperty(p.Name);//获取property对象
                var value = p.GetValue(obj, null);//获取属性值
                dic.Add(p.Name, ValueOf(value));
            }
            return dic;
        }
    }
}
