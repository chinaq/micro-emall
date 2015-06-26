using System;
using System.Text;

namespace Jumpcity.Utility.Extend
{
    /// <summary>
    /// 自定义针对基础数字类型的扩展，该类为静态类
    /// </summary>
    public static class NumberExtend
    {
        /// <summary>
        /// 检测当前的32位整数是否处于指定的范围内
        /// </summary>
        /// <param name="number">要检测的32位整数对象</param>
        /// <param name="min">匹配范围的的起始值</param>
        /// <param name="max">匹配范围的的最大值</param>
        /// <param name="mode">指定匹配模式，默认为包含起始值和最大值</param>
        /// <returns>检测成功返回True，否则返回False</returns>
        public static bool Range(this int number, int min = int.MinValue, int max = int.MaxValue, NumberRangeMode mode = NumberRangeMode.AllEqual)
        {
            int rm = (int)mode;

            switch (rm)
            {
                case 0:
                    return number >= min && number <= max;
                case 1:
                    return number >= min && number < max;
                case 2:
                    return number > min && number <= max;
                case 3:
                    return number > min && number < max;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 检测当前的64位整数是否处于指定的范围内
        /// </summary>
        /// <param name="number">要检测的64位整数对象</param>
        /// <param name="min">匹配范围的的起始值</param>
        /// <param name="max">匹配范围的的最大值</param>
        /// <param name="mode">指定匹配模式，默认为包含起始值和最大值</param>
        /// <returns>检测成功返回True，否则返回False</returns>
        public static bool Range(this long number, long min = long.MinValue, long max = long.MaxValue, NumberRangeMode mode = NumberRangeMode.AllEqual)
        {
            int rm = (int)mode;

            switch (rm)
            {
                case 0:
                    return number >= min && number <= max;
                case 1:
                    return number >= min && number < max;
                case 2:
                    return number > min && number <= max;
                case 3:
                    return number > min && number < max;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 检测当前的双精度浮点数是否处于指定的范围内
        /// </summary>
        /// <param name="number">要检测的双精度浮点数对象</param>
        /// <param name="min">匹配范围的的起始值</param>
        /// <param name="max">匹配范围的的最大值</param>
        /// <param name="mode">指定匹配模式，默认为包含起始值和最大值</param>
        /// <returns>检测成功返回True，否则返回False</returns>
        public static bool Range(this double number, double min = double.MinValue, double max = double.MaxValue, NumberRangeMode mode = NumberRangeMode.AllEqual)
        {
            int rm = (int)mode;

            switch (rm)
            {
                case 0:
                    return number >= min && number <= max;
                case 1:
                    return number >= min && number < max;
                case 2:
                    return number > min && number <= max;
                case 3:
                    return number > min && number < max;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 将Unix时间戳转换为DateTime对象
        /// </summary>
        /// <param name="unixTimeStamp">当前的Unix时间戳</param>
        /// <returns>返回转换后的DateTime对象</returns>
        public static DateTime ToDateTime(this int unixTimeStamp)
        {
            return DateTime.Now.ConvertFromStamp(unixTimeStamp);
        }

        /// <summary>
        /// 将Windows时间戳转换为DateTime对象
        /// </summary>
        /// <param name="unixTimeStamp">当前的Windows时间戳</param>
        /// <returns>返回转换后的DateTime对象</returns>
        public static DateTime ToDateTime(this double winTimeStamp)
        {
            return DateTime.Now.ConvertFromStamp(winTimeStamp);
        }

        /// <summary>
        /// 将当前ASCII编码值转换成相应的字符串
        /// </summary>
        /// <param name="ascNumber">当前的ASCII编码</param>
        /// <returns>返回转换后的字符串</returns>
        public static string ConvertFromASCII(this int ascNumber)
        {
            string character = null;
            
            if (ascNumber.Range(0, 255))
            {
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)ascNumber };
                character = asciiEncoding.GetString(byteArray);
            }

            return character;
        }
    }

    /// <summary>
    /// 指定数字范围的匹配模式
    /// </summary>
    public enum NumberRangeMode
    {
        /// <summary>
        /// 指示匹配范围应包含起始值和最大值
        /// </summary>
        AllEqual = 0,

        /// <summary>
        /// 指示匹配范围应包含起始值
        /// </summary>
        MinEqual = 1,

        /// <summary>
        /// 指示匹配范围应包含最大值
        /// </summary>
        MaxEqual = 2,

        /// <summary>
        /// 指示匹配范围不应包含起始值和最大值
        /// </summary>
        NotEqual = 3
    }
}
