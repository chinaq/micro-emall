using System;

namespace Jumpcity.Push
{
    /// <summary>
    /// 指定极光推送的接收者类型，无法继承该类
    /// </summary>
    public sealed class JPushReceiverType
    {
        /// <summary>
        /// 推送到指定的tag
        /// </summary>
        public const int Tag = 2;

        /// <summary>
        /// 推送到指定的alias
        /// </summary>
        public const int Alias = 3;

        /// <summary>
        /// 推送到所有用户
        /// </summary>
        public const int All = 4;
    }
}
