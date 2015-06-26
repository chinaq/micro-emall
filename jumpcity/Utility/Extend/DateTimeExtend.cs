using System;

namespace Jumpcity.Utility.Extend
{
    /// <summary>
    /// 自定义针对日期时间类型的扩展，该类为静态类
    /// </summary>
    public static class DateTimeExtend
    {
        private static readonly DateTime original = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));

        /// <summary>
        /// 将当前的日期时间转换为Windows时间戳
        /// </summary>
        /// <param name="time">当前的日期时间对象</param>
        /// <returns>返回转换后的Windows时间戳</returns>
        public static double ToTimeStamp(this DateTime time)
        {
            return (time - original).TotalSeconds;
        }

        /// <summary>
        /// 将当前的日期时间转换为Unix时间戳
        /// </summary>
        /// <param name="time">当前的日期时间对象</param>
        /// <returns>返回转换后的Unix时间戳</returns>
        public static int ToUnixTimeStamp(this DateTime time)
        {
            return (int)ToTimeStamp(time);
        }

        /// <summary>
        /// 根据指定的Windows时间戳生成一个DateTime对象
        /// </summary>
        /// <param name="time">当前调用该方法的日期时间对象，该对象只负责调用，对生成结果无任何影响</param>
        /// <param name="timeStamp">指定的Windows时间戳</param>
        /// <returns>返回转换后的日期时间对象</returns>
        public static DateTime ConvertFromStamp(this DateTime time, double timeStamp = 0)
        {
            timeStamp = timeStamp > 0 ? timeStamp : 0;
            return original.AddSeconds(timeStamp);
        }
    }
}
