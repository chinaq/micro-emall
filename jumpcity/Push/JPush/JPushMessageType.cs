using System;

namespace Jumpcity.Push
{
    /// <summary>
    /// 指定推送消息的类型，无法继承该类
    /// </summary>
    public sealed class JPushMessageType
    {
        /// <summary>
        /// 消息类型为通知
        /// </summary>
        public const int Notice = 1;

        /// <summary>
        /// 消息类型为自定义
        /// </summary>
        public const int Custom = 2;
    }
}
