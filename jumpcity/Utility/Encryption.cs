using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.Configuration;
using System.Text;

namespace Jumpcity.Utility
{
    /// <summary>
    /// 数据加密工具类
    /// </summary>
    public sealed class Encryption
    {
        /// <summary>
        /// 为指定的字符串加密
        /// </summary>
        /// <param name="eText">要加密的字符串</param>
        /// <param name="pwdFormat">用于存储密码的加密格式枚举</param>
        /// <returns>加密后的字符串散列码</returns>
        public static string Encryp(string eText, FormsAuthPasswordFormat pwdFormat)
        {
            if (string.IsNullOrEmpty(eText))
                return null;
            string pFormat = string.Empty;
            switch (pwdFormat)
            {
                case FormsAuthPasswordFormat.MD5:
                    pFormat = "MD5";
                    break;
                case FormsAuthPasswordFormat.SHA1:
                    pFormat = "SHA1";
                    break;
                case FormsAuthPasswordFormat.Clear:
                    pFormat = "Clear";
                    break;
                default: break;
            }
            return FormsAuthentication.HashPasswordForStoringInConfigFile(eText, pFormat);
        }

        /// <summary>
        /// 为指定的字符串加密,并用MD5加密格式保存
        /// </summary>
        /// <param name="eText">要加密的字符串</param>
        /// <returns></returns>
        public static string EncrypForMD5(string eText)
        {
            return Encryp(eText, FormsAuthPasswordFormat.MD5);
        }

        /// <summary>
        /// 为指定的字符串加密,并用SHA1加密格式保存
        /// </summary>
        /// <param name="eText">要加密的字符串</param>
        /// <returns></returns>
        public static string EncrypForSHA1(string eText)
        {
            return Encryp(eText, FormsAuthPasswordFormat.SHA1);
        }

        /// <summary>
        /// 为指定字符串使用Base64编码加密
        /// </summary>
        /// <param name="eText">要加密的字符串</param>
        /// <param name="IsBranch">如果为true，编码时每76个字符中插入分行符</param>
        /// <returns></returns>
        public static string EncrypBase64(string eText, bool IsBranch)
        {
            if (string.IsNullOrEmpty(eText))
                throw new Exception("要加密的字符串不能为空!");
            byte[] Arr = Encoding.Default.GetBytes(eText.Replace("+", "%2B"));
            Base64FormattingOptions option = IsBranch ? 
                Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None;
            return Convert.ToBase64String(Arr, option);
        }

        /// <summary>
        /// 为指定字符串使用Base64编码加密
        /// </summary>
        /// <param name="eText">要加密的字符串</param>
        /// <returns></returns>
        public static string EncrypBase64(string eText)
        {
            return EncrypBase64(eText, false);
        }

        /// <summary>
        /// 为指定的使用Base64编码加密的字符串解密
        /// </summary>
        /// <param name="eText">想要解密的,用Base64编码加密的字符串</param>
        /// <returns></returns>
        public static string DecrypBase64(string eText)
        {
            if (string.IsNullOrEmpty(eText))
                throw new Exception("要解密的字符串不能为空!");
            byte[] Arr = Convert.FromBase64String(eText);
            return Encoding.Default.GetString(Arr);
        }

        /// <summary>
        /// 自定义简易加密方法
        /// </summary>
        /// <param name="eText">要加密的字符串</param>
        /// <param name="xCode">密钥数字(解密时需用到)</param>
        /// <returns></returns>
        public static string EncrypXCode(string eText, int xCode)
        {
            if (string.IsNullOrEmpty(eText))
                throw new Exception("要操作的字符串不能为空!");

            byte[] Arr = Encoding.Default.GetBytes(eText.Replace("+", "%2B"));
            for (int i = 0; i < Arr.Length; i++)
                Arr[i] = (byte)((int)Arr[i] ^ xCode);
            
            return Encoding.Default.GetString(Arr);
        }

        /// <summary>
        /// 自定义简易加密方法
        /// </summary>
        /// <param name="eText">要加密的字符串</param>
        /// <returns></returns>
        public static string EncrypXCode(string eText)
        {
            return EncrypXCode(eText, 321654);
        }

        /// <summary>
        /// 解密自定义简易加密方法加密的字符串
        /// </summary>
        /// <param name="eText">要解密的字符串</param>
        /// <param name="xCode">加密时用到的密钥数字</param>
        /// <returns></returns>
        public static string DecrypXCode(string eText, int xCode)
        {
            return EncrypXCode(eText, xCode);
        }

        /// <summary>
        /// 解密自定义简易加密方法加密的字符串
        /// </summary>
        /// <param name="eText">要解密的字符串</param>
        /// <returns></returns>
        public static string DecrypXCode(string eText)
        {
            return EncrypXCode(eText, 321654);
        }
    }
}
