using System.Collections.Specialized;
using System.Web;

namespace Jumpcity.Utility.Extend
{
    /// <summary>
    /// 针对HttpRequest对象的扩展
    /// </summary>
    public static class HttpRequestExtend
    {
        /// <summary>
        /// 获取客户端的主机IP地址
        /// </summary>
        /// <param name="request">HttpRequest对象</param>
        /// <returns>返回真实的客户端主机IP地址</returns>
        public static string GetUserIP(this HttpRequest request)
        {
            NameValueCollection variables = request.ServerVariables;

            if (variables["HTTP_VIA"] != null)
                return variables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
            else
                return variables["REMOTE_ADDR"];
        }

        /// <summary>
        /// 获取指定成员内容的字符串对象
        /// </summary>
        /// <param name="request">HttpRequest对象</param>
        /// <param name="key">要获取的成员名称</param>
        /// <param name="empty">获取失败后的替代值</param>
        /// <returns>返回指定成员内容的字符串对象或者替代值</returns>
        public static string GetString(this HttpRequest request, string key, string empty = "")
        {
            return request[key] != null ? request[key] : empty;
        }
        
        /// <summary>
        /// 获取指定成员内容的32位整型对象
        /// </summary>
        /// <param name="request">HttpRequest对象</param>
        /// <param name="key">要获取的成员名称</param>
        /// <param name="error">获取失败后的替代值</param>
        /// <returns>返回指定成员内容的32位整型对象或者替代值</returns>
        public static int GetInt32(this HttpRequest request, string key, int error = 0)
        {
            string value = request[key];
            return value != null ? value.ToInt32(failValue: error) : error;
        }
    }
}
