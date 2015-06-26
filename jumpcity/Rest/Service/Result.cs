using System;
using System.Runtime.Serialization;

namespace Jumpcity.Rest.Service
{
    /// <summary>
    /// 用于承载服务返回结果集的类，该类为泛型类
    /// </summary>
    /// <typeparam name="T">返回的结果集中主体部分的数据类型</typeparam>
    [DataContract]
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
        [DataMember(Order = 1)]
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 获取或设置服务端返回的HttpStatusCode
        /// </summary>
        [DataMember(Order = 2)]
        public int StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        /// <summary>
        /// 获取或设置服务端返回的错误信息
        /// </summary>
        [DataMember(Order = 3)]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 获取或设置一个表示操作时间的时间戳
        /// </summary>
        [DataMember(Order = 4)]
        public string Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        /// <summary>
        /// 获取或设置服务端返回的数据主体
        /// </summary>
        [DataMember(Order = 5)]
        public T Results
        {
            get { return _result; }
            set { _result = value; }
        }

        #endregion 属性...

        #region 构造方法...

        /// <summary>
        /// 创建一个服务端返回的结果集对象
        /// </summary>
        /// <param name="url">设置请求的服务地址</param>
        /// <param name="statusCode">设置服务端返回的HttpStatusCode</param>
        /// <param name="description">设置服务端返回的错误信息</param>
        /// <param name="result">设置服务端返回的数据主体</param>
        public Result(string url, int statusCode, string description, T result)
        {
            this.URL = url;
            this.StatusCode = statusCode;
            this.Description = description;
            this.Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            this._result = result;        
        }

        /// <summary>
        /// 创建一个服务端返回的结果集对象
        /// </summary>
        /// <param name="url">设置请求的服务地址</param>
        /// <param name="statusCode">设置服务端返回的HttpStatusCode</param>
        /// <param name="description">设置服务端返回的错误信息</param>
        public Result(string url, int statusCode, string description) : this(url, statusCode, description, default(T)) { }
        
        /// <summary>
        /// 创建一个服务端返回的结果集对象
        /// </summary>
        /// <param name="url">设置请求的服务地址</param>
        public Result(string url) : this(url, 0, "") { }

        #endregion 构造方法...

        #region 成员方法...

        /// <summary>
        /// 设置并获取结果集的主体部分
        /// </summary>
        /// <param name="result">主体部分的数据对象</param>
        /// <returns>返回设置好的结果集</returns>
        public Result<T> GetResults(T result)
        {
            if (result != null)
                this._result = result;

            return this;
        }

        /// <summary>
        /// 设置结果集对象为响应成功的状态
        /// </summary>
        public void UpdateToSuccess()
        {
            this.StatusCode = 200;
            this.Description = "";
        }

        /// <summary>
        /// 设置结果集对象为响应失败的状态
        /// </summary>
        /// <param name="errorDescription">设置失败的错误信息</param>
        public void UpdateToError(string errorDescription)
        {
            this.StatusCode = 417;
            this.Description = errorDescription;
        }

        #endregion 成员方法...
    }
}
