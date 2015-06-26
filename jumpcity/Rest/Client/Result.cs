using System;
using Newtonsoft.Json;

namespace Jumpcity.Rest.Client
{
    /// <summary>
    /// 用于接收服务端返回的结果集，该类为泛型类
    /// </summary>
    /// <typeparam name="T">返回的结果集中主体部分的数据类型</typeparam>
    [Serializable]
    public class Result<T>
    {
        #region 字段...

        private string _url;
        private int _statusCode;
        private string _description;
        private string _timestamp;
        private T _result;

        #endregion 字段...

        #region 属性...

        /// <summary>
        /// 获取或设置请求的服务地址
        /// </summary>
        [JsonProperty(Order = 1)]
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 获取或设置服务端返回的HttpStatusCode
        /// </summary>
        [JsonProperty(Order = 2)]
        public int StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        /// <summary>
        /// 获取或设置服务端返回的错误信息
        /// </summary>
        [JsonProperty(Order = 3)]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 获取或设置一个表示操作时间的时间戳
        /// </summary>
        [JsonProperty(Order = 4)]
        public string Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        /// <summary>
        /// 获取或设置服务端返回的数据主体
        /// </summary>
        [JsonProperty(Order = 5)]
        public T Results
        {
            get { return _result; }
            set { _result = value; }
        }

        #endregion 属性...
    }
}
