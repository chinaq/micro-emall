using System;

namespace Jumpcity.Web
{
    /// <summary>
    /// 用于在Session中保存登陆后的账户信息
    /// </summary>
    [Serializable]
    public class Account
    {
        /// <summary>
        /// 获取或设置账户ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 湖区或设置登陆名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 获取或设置登陆者的权限ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 获取或设置登陆者的权限名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 获取或设置登陆昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置登陆者的头像地址
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 获取或设置登陆时间
        /// </summary>
        public DateTime LoginDate { get; set; }

        /// <summary>
        /// 获取或设置登陆者账号的注册时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
