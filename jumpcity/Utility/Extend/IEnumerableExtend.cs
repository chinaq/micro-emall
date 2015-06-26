using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data.Objects.DataClasses;

namespace Jumpcity.Utility.Extend
{
    /// <summary>
    /// 对System.Collections.IEnumerable类型的扩展
    /// </summary>
    public static class IEnumerableExtend
    {
        private static object syncProperty = new object();
        private static object syncCommon = new object();
        
        //缓存类型的PropertyInfo数组
        private static Dictionary<Type, PropertyInfo[]> PropertyDictionary = new Dictionary<Type, PropertyInfo[]>(); 
        
        //缓存两种类型的公共属性对应关系
        private static Dictionary<string, IEnumerable<CommonProperty>> CommonPropertyDictionary = new Dictionary<string, IEnumerable<CommonProperty>>(); 

        private class CommonProperty
        {
            public PropertyInfo SourceProperty { get; set; }
            public PropertyInfo TargetProperty { get; set; }
        }

        private static PropertyInfo[] GetPropertyInfoArray(Type type)
        {
            if (!IEnumerableExtend.PropertyDictionary.ContainsKey(type))
            {
                lock (syncProperty)
                {
                    //双重检查
                    if (!IEnumerableExtend.PropertyDictionary.ContainsKey(type)) 
                    {
                        PropertyInfo[] properties = type.GetProperties();
                        
                        //Type是单例的(Singleton)，可以直接作为Key
                        IEnumerableExtend.PropertyDictionary.Add(type, properties); 
                    }
                }
            }

            return IEnumerableExtend.PropertyDictionary[type];
        }

        private static IEnumerable<CommonProperty> GetCommonProperties(Type sourceType, Type targetType)
        {
            string key = sourceType.ToString() + targetType.ToString();

            if (!IEnumerableExtend.CommonPropertyDictionary.ContainsKey(key))
            {
                lock (syncCommon)
                {
                    //双重检查
                    if (!IEnumerableExtend.CommonPropertyDictionary.ContainsKey(key)) 
                    {
                        //获取源对象所有属性
                        PropertyInfo[] sourceTypeProperties = GetPropertyInfoArray(sourceType);
                        
                        //获取目标对象所有属性
                        PropertyInfo[] targetTypeProperties = GetPropertyInfoArray(targetType);

                        IEnumerable<CommonProperty> commonProperties = (
                            from SP in sourceTypeProperties
                            join TP in targetTypeProperties on SP.Name.ToLower() equals TP.Name.ToLower()
                            select new CommonProperty
                            {
                                SourceProperty = SP,
                                TargetProperty = TP
                            }
                        );

                        IEnumerableExtend.CommonPropertyDictionary.Add(key, commonProperties);
                    }
                }
            }

            return IEnumerableExtend.CommonPropertyDictionary[key];
        }

        [EdmFunction("SqlServer", "NEWID")]
        private static Guid NewId()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 对元素进行随机排序，该方法为扩展方法
        /// </summary>
        /// <typeparam name="T">要排序的数据源的类型</typeparam>
        /// <param name="source">要排序的数据源</param>
        /// <returns>返回随机排序后的结果集</returns>
        public static IQueryable<T> OrderByRandom<T>(this IEnumerable<T> source)
        { 
            return source.AsQueryable().OrderBy(d => NewId());
        }

        /// <summary>
        /// 将当前的迭代枚举器转换为指定类型的集合。
        /// </summary>
        /// <typeparam name="TResult">要转换成的类型</typeparam>
        /// <param name="source">包含要转换的元素的迭代枚举器对象</param>
        /// <returns>一个指定类型的泛型集合，包含已转换为指定类型的源序列的每个元素</returns>
        public static List<TResult> ConvertToList<TResult>(this IEnumerable source) where TResult : new()
        {
            if (source == null)
                return null;
            
            //源类型于目标类型一致，可以直接转换
            if (source is IEnumerable<TResult>)
                return source.Cast<TResult>().ToList();

            List<TResult> result = new List<TResult>();

            //循环控制变量
            bool hasGetElementType = false;
            
            //公共属性(按属性名称进行匹配)
            IEnumerable<CommonProperty> commonProperties = null; 

            foreach (var s in source)
            {
                //访问第一个元素时，取得属性对应关系，后续的元素不再重新计算
                if (!hasGetElementType) 
                {
                    if (s is TResult) 
                    {
                        //如果源类型是目标类型的子类，可以直接调用Cast<T>扩展方法
                        return source.Cast<TResult>().ToList();
                    }

                    commonProperties = GetCommonProperties(s.GetType(), typeof(TResult));
                    hasGetElementType = true;
                }

                TResult t = new TResult();
                
                //逐个属性拷贝
                foreach (CommonProperty commonProperty in commonProperties) 
                {
                    object value = commonProperty.SourceProperty.GetValue(s, null);
                    commonProperty.TargetProperty.SetValue(t, value, null);
                }

                result.Add(t);
            }

            return result;
        }

        /// <summary>
        /// 将当前的迭代枚举器转换为指定类型的实体。
        /// </summary>
        /// <typeparam name="TResult">要转换成的类型</typeparam>
        /// <param name="source">包含要转换的元素的迭代枚举器对象</param>
        /// <returns>一个指定类型的实体对象，包含已转换为指定类型的源序列的每个成员</returns>
        public static TResult ConvertToEntity<TResult>(this IEnumerable source) where TResult : new()
        {
            List<TResult> list = ConvertToList<TResult>(source);

            return (!General.IsNullable(list) ? list[0] : default(TResult));
        }
    }
}
