using System;
using System.Collections;
using System.Collections.Generic;

namespace Jumpcity.Utility.Extend
{
    /// <summary>
    /// 自定义针对System.Collections.ICollection派生类的扩展，该类为静态类
    /// </summary>
    public static class CollectionsExtend
    {
        #region 通用扩展...

        /// <summary>
        /// 判断集合对象是否为NULL或长度小于等于指定长度
        /// </summary>
        /// <param name="list">当前被判断的集合对象</param>
        /// <param name="minCount">集合必须大于的长度数</param>
        /// <returns>如果集合对象为NULL或长度小于等于指定长度，返回TRUE，否则为FALSE</returns>
        [Obsolete("该方法已被废弃，请使用Jumpcity.Utility.Extend.General.IsNullable(ICollection collection, int minCount = 0)方法替代该功能")]
        public static bool IsNullOrEmpty(this ICollection collection, int minCount = 0)
        {
            return (collection == null || collection.Count <= minCount);
        }

        /// <summary>
        /// 对集合对象进行深度复制
        /// </summary>
        /// <typeparam name="T">要复制的集合对象类型</typeparam>
        /// <param name="collection">要进行复制的集合对象</param>
        /// <returns>复制成功返回新的集合对象副本，否则返回NULL</returns>
        public static T Clone<T>(this ICollection collection) where T : class, ICollection
        {
            return General.Clone<T>(collection as T);
        }

        #endregion 通用扩展...

        #region 针对IList的扩展...

        /// <summary>
        /// 对IList集合中的首、尾项进行交换，交换后其它位置的项按指定方向顺延。
        /// </summary>
        /// <param name="list">要进行交换的Ilist集合</param>
        /// <param name="right">如果为True,则代表将集合的最后一项交换到集合开始处，否则代表将集合的第一项交换到集合的结尾处，默认为False</param>
        public static void ExChange(this IList list, bool right = false)
        {
            if (!General.IsNullable(list, 1))
            {
                object temp = null;
                int index = (right ? list.Count - 1 : 0); 
                temp = list[index];
                list.RemoveAt(index);

                if (right)
                    list.Insert(0, temp);
                else
                    list.Add(temp);
            }
        }

        #endregion 针对IList的扩展...

        #region 针对IDictionary<string, object>的扩展...

        /// <summary>
        /// 获取与指定的键相关联的Int32类型的值，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键</param>
        /// <param name="failValue">获取失败时返回的默认值</param>
        /// <returns>获取成功时返回获取到的值，否则返回failValue指定的值</returns>
        public static int GetInt32(this IDictionary<string, object> dictionary, string keyName, int failValue = 0)
        {
            object value = GetValue(dictionary, keyName);
            try
            {
                int intValue = Convert.ToInt32(value);
                return intValue;
            }
            catch
            {
                return failValue;
            }
        }

        /// <summary>
        /// 获取与指定的键相关联的可空Int32类型的值，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键</param>
        /// <param name="failValue">获取失败时返回的默认值</param>
        /// <returns>获取成功时返回获取到的值，否则返回failValue指定的值</returns>
        public static int? GetNullableInt32(this IDictionary<string, object> dictionary, string keyName, int? failValue = null)
        {
            object value = GetValue(dictionary, keyName);
            if (value == null)
                return null;

            try
            {
                int? intValue = Convert.ToInt32(value);
                return intValue;
            }
            catch
            {
                return failValue;
            }
        }

        /// <summary>
        /// 获取与指定的键相关联的Int64类型的值，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键</param>
        /// <param name="failValue">获取失败时返回的默认值</param>
        /// <returns>获取成功时返回获取到的值，否则返回failValue指定的值</returns>
        public static long GetInt64(this IDictionary<string, object> dictionary, string keyName, long failValue = 0L)
        {
            object value = GetValue(dictionary, keyName);
            try
            {
                long longValue = Convert.ToInt64(value);
                return longValue;
            }
            catch
            {
                return failValue;
            }
        }

        /// <summary>
        /// 获取与指定的键相关联的双精度浮点数类型的值，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键</param>
        /// <param name="failValue">获取失败时返回的默认值</param>
        /// <returns>获取成功时返回获取到的值，否则返回failValue指定的值</returns>
        public static double GetDouble(this IDictionary<string, object> dictionary, string keyName, double failValue = 0.0)
        {
            object value = GetValue(dictionary, keyName);
            try
            {
                double doubleValue = Convert.ToDouble(value);
                return doubleValue;
            }
            catch
            {
                return failValue;
            }
        }

        /// <summary>
        /// 获取与指定的键相关联的Boolean类型的值，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键</param>
        /// <returns>获取成功时返回获取到的值，否则返回一个空布尔值</returns>
        public static bool? GetBoolean(this IDictionary<string, object> dictionary, string keyName)
        {
            object value = GetValue(dictionary, keyName);
            if (value == null)
                return null;
            
            bool? flag = null;
            try
            {
                flag = Convert.ToBoolean(value);
            }
            catch { }
            return flag;
        }

        /// <summary>
        /// 获取与指定的键相关联的字符串类型的值，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键</param>
        /// <returns>获取成功时返回获取到的值，否则返回NULL</returns>
        public static string GetString(this IDictionary<string, object> dictionary, string keyName)
        {
            return GetValue<string>(dictionary, keyName);
        }

        /// <summary>
        /// 获取与指定的键相关联的DateTime类型的值，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键</param>
        /// <returns>获取成功时返回获取到的值，否则返回一个空DateTime值</returns>
        public static DateTime? GetDateTime(this IDictionary<string, object> dictionary, string keyName)
        {
            object value = GetValue(dictionary, keyName);
            DateTime? dateTime = null;
            try
            {
                dateTime = Convert.ToDateTime(value);
            }
            catch { }

            return dateTime;
        }

        /// <summary>
        /// 获取与指定的键相关联的文件流对象，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键</param>
        /// <returns>获取成功时返回获取到的值，否则返回NULL</returns>
        public static System.IO.Stream GetStream(this IDictionary<string, object> dictionary, string keyName)
        {
            return GetValue<System.IO.Stream>(dictionary, keyName);
        }

        /// <summary>
        /// 获取字典集合中用于表示当前页码项的值，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键，默认为PageIndex</param>
        /// <param name="isStartZero">如果当前页码从零开始为True，从一开始为False，默认为True</param>
        /// <returns>如果获取失败则返回指定的开始页码，否则返回相应的页码</returns>
        public static int GetPageIndex(this IDictionary<string, object> dictionary, string keyName = "PageIndex", bool isStartZero = true)
        {
            int pageIndex = GetInt32(dictionary, keyName);
            
            if (isStartZero)
            {
                if (pageIndex < 0)
                    pageIndex = 0;
            }
            else
            {
                if (pageIndex < 0)
                    pageIndex = 1;
                else
                    pageIndex++;
            }

            return pageIndex;
        }

        /// <summary>
        /// 获取字典集合中用于表示每页行数项的值，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键，默认为PageSize</param>
        /// <param name="failSize">获取失败时所使用的每页行数值</param>
        /// <returns>如果获取失败则返回默认的每页行数，否则返回相应的每页行数</returns>
        public static int GetPageSize(this IDictionary<string, object> dictionary, string keyName = "PageSize", int failSize = 10)
        {
            int pageSize = GetInt32(dictionary, keyName);
            if (pageSize <= 0)
                pageSize = failSize;
            return pageSize;
        }

        /// <summary>
        /// 获取与指定的键相关联的值，该方法为扩展方法。
        /// </summary>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键</param>
        /// <returns>获取成功时返回获取到的值，否则返回NULL</returns>
        public static object GetValue(this IDictionary<string, object> dictionary, string keyName)
        {
            object value = null;
            if (!string.IsNullOrWhiteSpace(keyName))
                dictionary.TryGetValue(keyName, out value);
            return value;
        }

        /// <summary>
        /// 获取与指定的键相关联的指定类型的值，该方法为扩展方法。
        /// </summary>
        /// <typeparam name="T">指定获取到的值的类型</typeparam>
        /// <param name="dictionary">正在调用该方法的IDictionary对象</param>
        /// <param name="keyName">要获取其值的键</param>
        /// <returns>获取成功时返回获取到的指定类型的值，否则返回NULL</returns>
        public static T GetValue<T>(this IDictionary<string, object> dictionary, string keyName)
            where T : class
        {
            return GetValue(dictionary, keyName) as T;
        }

        #endregion 针对IDictionary<string, object>的扩展...
    }
}
