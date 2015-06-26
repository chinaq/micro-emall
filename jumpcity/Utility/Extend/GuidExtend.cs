using System;

namespace Jumpcity.Utility.Extend
{
    /// <summary>
    /// 自定义针对System.Guid的扩展，该类为静态类
    /// </summary>
    public static class GuidExtend
    {
        /// <summary>  
        /// 将GUID转换为十六长度的唯一字符串  
        /// </summary>  
        /// <param name="guid">要转换的GUID对象</param>
        /// <param name="isUpper">指示转换后的字符串是否要把其中的英文字母转换为大写的</param>
        /// <returns>返回转换后的字符串，长度固定为十六</returns> 
        public static string ToUniqueString(this Guid guid, bool isUpper = false)
        {
            long i = 1;
            byte[] buffer = guid.ToByteArray();

            foreach (byte b in buffer)
                i *= ((int)b + 1);

            string unqiue = string.Format("{0:x}", i - DateTime.Now.Ticks);
            if (isUpper)
                unqiue = unqiue.ToUpper();

            return unqiue;
        }

        /// <summary>
        /// 将GUID转换为一个整数位数为19位或22位的唯一数字序列
        /// </summary>
        /// <param name="guid">要转换的GUID对象</param>
        /// <param name="size">指定转换后数字整数部分的位数，只能是19或22，默认为19</param>
        /// <returns>返回转换后的数字序列</returns>
        public static decimal ToUniqueNumber(this Guid guid, int size = 19)
        {
            int uSize = (size == 19 || size == 22) ? size : 19;
            byte[] buffer = guid.ToByteArray();

            if (size == 19)
            {
                return BitConverter.ToInt64(buffer, 0);
            }
            else
            {
                //保证时间毫秒部分不重复 
                System.Threading.Thread.Sleep(1);

                //将GUID转为为一个随机数的种子
                int seed = BitConverter.ToInt32(buffer, 0);

                //因GUID全局唯一，所以随机数永不重复
                Random d = new Random(seed);

                decimal unique = decimal.Parse(DateTime.Now.ToString("yyyyMMddHHmmssffff") + d.Next(1000, 9999));

                return unique;
            }
        }
    }
}
