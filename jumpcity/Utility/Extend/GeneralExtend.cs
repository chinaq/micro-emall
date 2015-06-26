using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Jumpcity.Utility.Extend
{
    /// <summary>
    /// 该类用以存放通用的工具方法
    /// </summary>
    public static class General
    {
        /// <summary>
        /// 如果是空字符串，则用指定的字符串替代，该方法是扩展方法。
        /// </summary>
        /// <param name="str">为空的字符串对象</param>
        /// <param name="replace">用来替代空字符串的指定字符串对象</param>
        /// <returns>如果str不为空，直接返回str，否则返回replace</returns>
        public static string ToNullable(string str, string replace = "---")
        {
            if (!string.IsNullOrWhiteSpace(str))
                return str;

            return replace;
        }

        /// <summary>
        /// 输出指定对象的字符串表示形式，如果对象为NULL，则用指定的字符串替代，该方法是扩展方法。
        /// </summary>
        /// <param name="source">要输出的对象</param>
        /// <param name="replace">用来替代的指定字符串对象</param>
        /// <returns>如果source不为NULL，直接返回其字符串表示形式，否则返回replace</returns>
        public static string ToNullable(object source, string replace = "---")
        {
            if (source != null)
                return source.ToString();

            return replace;
        }

        /// <summary>
        /// 指示当前字符串对象是否是由null，空，全空格或者指定为空替代字符串组成。
        /// </summary>
        /// <param name="str">为空的字符串对象</param>
        /// <param name="emptyRep">用来替代空字符串的指定字符串对象</param>
        /// <returns>如果str不满足上述条件，返回False，否则返回True</returns>
        public static bool IsNullable(string str, string emptyRep = "---")
        {
            return (string.IsNullOrWhiteSpace(str) || str == emptyRep); 
        }

        /// <summary>
        /// 判断集合对象是否为NULL或长度小于等于指定长度
        /// </summary>
        /// <param name="collection">当前被判断的集合对象</param>
        /// <param name="minCount">集合必须大于的长度数(默认为1)</param>
        /// <returns>如果集合对象为NULL或长度小于等于指定长度，返回TRUE，否则为FALSE</returns>
        public static bool IsNullable(ICollection collection, int minCount = 1)
        {
            return (collection == null || collection.Count < minCount);
        }

        /// <summary>
        /// 判断传入的字符串对象是否是一个基于System.Guid的十六位的唯一字符串，如果不是则将其转换为唯一字符串
        /// </summary>
        /// <param name="unique">要检测的字符串对象</param>
        /// <returns>如果传入的字符串符合要求，则原样返回，否则返回转换后的字符串</returns>
        public static string UniqueString(string unique = null)
        {
            if (IsNullable(unique) || unique.Length != 16)
                unique = Guid.NewGuid().ToUniqueString();
            return unique;
        }

        /// <summary>
        /// 对引用类型的对象实现深度复制
        /// </summary>
        /// <typeparam name="T">要进行复制的对象类型，必须为类类型</typeparam>
        /// <param name="entity">要进行深度复制的对象</param>
        /// <returns>如果对象为NULL，返回NULL，否则返回一个新的对象副本</returns>
        public static T Clone<T>(T entity) where T : class
        {
            if (entity != null)
            {
                using (Stream stream = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(stream, entity);
                    stream.Seek(0, SeekOrigin.Begin);
                    return (T)serializer.Deserialize(stream);
                }
            }

            return null;
        }

        /// <summary>
        /// 获取指定类型的属性成员数组
        /// </summary>
        /// <param name="type">指定的类型</param>
        /// <param name="argumnetIndex">对于泛型类型,则指定要获取的泛型类型列表项的索引</param>
        /// <returns>返回按照搜索规则获取的属性成员列表数组</returns>
        public static PropertyInfo[] GetPropertyInfo(Type type, int argumnetIndex = 0)
        {
            PropertyInfo[] infoArray = null;
            if (type.IsGenericType)
            {
                Type[] Generics = type.GetGenericArguments();
                infoArray = Generics[argumnetIndex].GetProperties();
            }
            else
                infoArray = type.GetProperties();

            return infoArray;
        }
    }
}
