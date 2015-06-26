using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jumpcity.Utility.Extend
{
    /// <summary>
    /// 自定义针对System.String类的扩展，该类为静态类
    /// </summary>
    public static class StringExtend
    {
        /// <summary>
        /// 获取字符串对象的实际字节长度
        /// </summary>
        /// <param name="str">要获取长度的字符串</param>
        /// <returns>字符串对象的实际字节长度</returns>
        public static int GetByteLength(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return 0;

            int strlen = 0;
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();

            //将字符串转换为ASCII编码的字节数组
            byte[] strBytes = asciiEncoding.GetBytes(str);
            strlen = strBytes.Length;

            for (int i = 0; i < strBytes.Length; i++)
            {
                //中文都将编码为ASCII编码63，即"?"号。
                //一个中文字符算作两个字节
                if (strBytes[i] == 63)  
                    strlen++;
            }
            return strlen;
        }

        /// <summary>
        /// 如果是空字符串，则用指定的字符串替代，该方法是扩展方法。
        /// </summary>
        /// <param name="str">为空的字符串对象</param>
        /// <param name="replace">用来替代空字符串的指定字符串对象</param>
        /// <returns>如果str不为空，直接返回str，否则返回replace</returns>
        [Obsolete("该方法已被废弃，请使用Jumpcity.Utility.Extend.General.ToNullable(string str, string replace)方法替代该功能")]
        public static string ToEmpty(this string str, string replace = "---")
        {
            if (!string.IsNullOrWhiteSpace(str))
                return str;

            return replace;
        }

        /// <summary>
        /// 指示当前字符串对象是否是由null，空，全空格或者指定为空替代字符串组成。
        /// </summary>
        /// <param name="str">为空的字符串对象</param>
        /// <param name="emptyRep">用来替代空字符串的指定字符串对象</param>
        /// <returns>如果str不满足上述条件，返回False，否则返回True</returns>
        [Obsolete("该方法已被废弃，请使用Jumpcity.Utility.Extend.General.IsNullable(string str, string emptyRep)方法替代该功能")]
        public static bool IsNullOrEmpty(this string str, string emptyRep = "---")
        {
            return (string.IsNullOrWhiteSpace(str) || str == emptyRep);
        }

        /// <summary>
        /// 将当前字符串对象按照指定的分隔符转换为字符串数组
        /// </summary>
        /// <param name="str">当前要转换的字符串对象</param>
        /// <param name="split">指定的分隔字符列表</param>
        /// <param name="opt">指定转换后的数组是否要包含为空的项，默认为不包含</param>
        /// <returns>如果当前字符串为空，返回NULL，否则返回转换后的数组对象</returns>
        public static string[] ToArray(this string str, string split = ",", StringSplitOptions opt = StringSplitOptions.RemoveEmptyEntries)
        {
            if (General.IsNullable(str))
                return null;

            string[] strArray = null;
            if (General.IsNullable(split) || str.IndexOf(split) == -1)
                strArray = new string[] { str };
            else
                strArray = str.Split(split.ToCharArray(), opt);

            return strArray;
        }

        /// <summary>
        /// 将当前字符串对象按照指定的分隔符转换为字符串集合
        /// </summary>
        /// <param name="str">当前要转换的字符串对象</param>
        /// <param name="split">指定的分隔字符列表</param>
        /// <param name="opt">指定转换后的集合是否要包含为空的项，默认为不包含</param>
        /// <returns>如果当前字符串为空，返回NULL，否则返回转换后的集合对象</returns>
        public static List<string> ToList(this string str, string split = ",", StringSplitOptions opt = StringSplitOptions.RemoveEmptyEntries)
        {
            return str.ToArray(split, opt).ToList();
        }

        /// <summary>
        /// 将当前字符串对象按照指定的分隔符转换为整数数组
        /// </summary>
        /// <param name="str">当前要转换的字符串对象</param>
        /// <param name="split">指定的分隔字符列表</param>
        /// <param name="opt">指定转换后的数组是否要包含为空的项，默认为不包含</param>
        /// <returns>如果当前字符串为空，返回NULL，否则返回转换后的数组对象</returns>
        public static int[] ToIntArray(this string str, string split = ",")
        {
            string[] strArray = str.ToArray(split, StringSplitOptions.RemoveEmptyEntries);
            int[] intArray = null; 
            
            if (!General.IsNullable(strArray))
            {
                intArray = new int[strArray.Length];
                for (int i = 0, c = strArray.Length; i < c; i++)
                {
                    string s = strArray[i];
                    int n = 0;
                    if (int.TryParse(s, out n))
                        intArray[i] = n;
                }
            }

            return intArray;
        }

        /// <summary>
        /// 将字符串转换为32位整型对象，该方法是扩展方法。
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="isAbs">指定转换后的整数值是否应该是个绝对值，也就是不能为负数</param>
        /// <param name="failValue">指定转换失败时返回的值，默认为零，该值不受isAbs参数设置的影响</param>
        /// <returns>返回转换后的32位整型对象，如果转换失败返回指定值(未指定则返回默认值)</returns>
        public static int ToInt32(this string str, bool isAbs = false, int failValue = 0)
        {
            int i;
            if (int.TryParse(str, out i))
            {
                if (isAbs)
                    i = Math.Abs(i);
            }
            else
                i = failValue;
            return i;
        }

        /// <summary>
        /// 将字符串转换为32位可空整型对象，该方法是扩展方法。
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="isAbs">指定转换后的整数值是否应该是个绝对值，也就是不能为负数</param>
        /// <param name="failValue">指定转换失败时返回的值，默认为零，该值不受isAbs参数设置的影响</param>
        /// <returns>返回转换后的32位可空整型对象，如果转换失败返回指定值</returns>
        public static int? ToNullableInt32(this string str, bool isAbs = false, int? failValue = null)
        {
            int? i = null;
            try
            {
                i = int.Parse(str);
                if (isAbs)
                    i = Math.Abs((int)i);
            }
            catch
            {
                i = failValue;
            }
            return i;
        }

        /// <summary>
        /// 将字符串转换为64位整型对象，该方法是扩展方法。
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="isAbs">指定转换后的整数值是否应该是个绝对值，也就是不能为负数</param>
        /// <param name="failValue">指定转换失败时返回的值，默认为零，该值不受isAbs参数设置的影响</param>
        /// <returns>返回转换后的64位整型对象，如果转换失败返回指定值(未指定则返回默认值)</returns>
        public static long ToInt64(this string str, bool isAbs = false, long failValue = 0)
        {
            long i;
            if (long.TryParse(str, out i))
            {
                if (isAbs)
                    i = Math.Abs(i);
            }
            else
                i = failValue;
            return i;
        }

        /// <summary>
        /// 将字符串转换为双精度浮点数对象，该方法是扩展方法。
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="isAbs">指定转换后的双精度浮点数是否应该是个绝对值，也就是不能为负数</param>
        /// <param name="failValue">指定转换失败时返回的值，默认为零，该值不受isAbs参数设置的影响</param>
        /// <returns>返回转换后的双精度浮点数对象，如果转换失败返回指定值</returns>
        public static double? ToDouble(this string str, bool isAbs = false, double? failValue = null)
        {
            double? d = null;
            try
            {
                d = double.Parse(str);
                if (isAbs)
                    d = Math.Abs((double)d);
            }
            catch
            {
                d = failValue;
            }
            return d;
        }

        /// <summary>
        /// 将字符串转换为GUID结构，该方法是扩展方法。
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="format">指示当前字符串正在使用的确切格式：“N”、“D”、“B”、“P”或“X”,默认为“D”</param>
        /// <returns>如果字符串内容格式正确，返回转换后的GUID结构，否则返回Guid.Empty</returns>
        public static Guid ToGuid(this string str, string format = "D")
        {
            if (string.IsNullOrWhiteSpace(format))
                format = "D";
            else
                format = format.ToUpper();

            if (format != "N" && format != "D" && format != "B" && format != "P" && format != "X")
                format = "D";

            Guid guid;

            if (!Guid.TryParseExact(str, format, out guid))
                guid = Guid.Empty;

            return guid;
        }

        /// <summary>
        /// 将当前的字符串中指定位置的字符转换成ASCII码
        /// </summary>
        /// <param name="str">当前要处理的字符串</param>
        /// <param name="index">设置要处理字符串中的字符位置索引</param>
        /// <returns>返回转换后的ASCII编码</returns>
        public static int ToASCII(this string str, int index = 0)
        {
            int asc = -1;

            if (str.Length - 1 >= index)
            {
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                asc = (int)asciiEncoding.GetBytes(str[index].ToString())[0];
            }

            return asc;
        }

        /// <summary>
        /// 检测当前字符串对象是否为一个中国地区的手机号码。
        /// </summary>
        /// <param name="str">要检测的字符串对象</param>
        /// <returns>如果是手机号码返回True，否则返回False</returns>
        public static bool IsMobileNumber(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            Regex rx = new Regex(@"^(13|14|15|18)\d{9}$");
            return rx.Match(str).Success;
        }
    }
}