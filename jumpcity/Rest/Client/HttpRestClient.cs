using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Jumpcity.Utility;
using Microsoft.Http;
using Newtonsoft.Json;

namespace Jumpcity.Rest.Client
{
    /// <summary>
    /// 用于实现Rest客户端操作的类
    /// </summary>
    public class HttpRestClient
    {
        #region 字段...

        private string _host;
        private string _serviceName;
        private string _uriTemplate;
        private string _method;
        private string _contentType;
        private Encoding _encoding = Encoding.UTF8;

        #endregion 字段...

        #region 构造方法...

        /// <summary>
        /// 创建一个新的Rest客户端操作对象
        /// </summary>
        /// <param name="host">服务端的主机地址</param>
        /// <param name="serviceName">服务类别名称</param>
        /// <param name="method">提交方式</param>
        /// <param name="contentType">提交数据的类型</param>
        /// <param name="encoding">提交数据的编码格式</param>
        public HttpRestClient(string host, string serviceName, string method, string contentType, Encoding encoding)
        {
            this._host = host;
            this._serviceName = serviceName;
            this._method = method;
            this._contentType = contentType;

            if (encoding != null)
                this._encoding = encoding;
        }

        /// <summary>
        /// 创建一个新的Rest客户端操作对象
        /// </summary>
        /// <param name="host">服务端的主机地址</param>
        /// <param name="serviceName">服务类别名称</param>
        public HttpRestClient(string host, string serviceName) 
            : this(host, serviceName, null, null, null) { }

        /// <summary>
        /// 创建一个新的Rest客户端操作对象
        /// </summary>
        public HttpRestClient() { }

        #endregion 构造方法...

        #region 属性...

        /// <summary>
        /// 获取或设置服务端的主机地址
        /// </summary>
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        /// <summary>
        /// 获取或设置服务类别名称
        /// </summary>
        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        /// 获取或设置服务根节点名称
        /// </summary>
        public string UriTemplate
        {
            get { return _uriTemplate; }
            set { _uriTemplate = value; }
        }

        /// <summary>
        /// 获取或设置提交方式
        /// </summary>
        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }

        /// <summary>
        /// 获取或设置提交数据的类型
        /// </summary>
        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        /// <summary>
        /// 获取或设置提交数据的编码格式
        /// </summary>
        public Encoding HttpEncoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        #endregion 属性...

        #region 成员方法...

        #region 创建HttpContent的相关方法...

        /// <summary>
        /// 创建一个用于封装提交数据的HttpContent
        /// </summary>
        /// <typeparam name="TEntity">要提交的实体数据类型</typeparam>
        /// <param name="entity">要提交的数据实体对象</param>
        /// <param name="encoding">设置提交数据的编码格式</param>
        /// <param name="contentType">设置提交数据的类型</param>
        /// <returns>返回封装好的HttpContent对象</returns>
        public virtual HttpContent CreateHttpContent<TEntity>(TEntity entity, Encoding encoding, string contentType)
            where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (string.IsNullOrWhiteSpace(contentType))
                throw new ArgumentNullException("contentType");
            
            string request = "";
            if (contentType == MimeType.JSON)
            {
                JsonSerializerSettings settings = GetJsonSettings();
                request = JsonConvert.SerializeObject(entity, settings);
            }
            else if (contentType == MimeType.XML)
            {
                XmlSerializer xml = new XmlSerializer(typeof(TEntity));
                StringBuilder xmlString = new StringBuilder();
                using (TextWriter writer = new StringWriter(xmlString))
                {
                    xml.Serialize(writer, entity);
                }
                request = xmlString.ToString();
            }
            byte[] stream = encoding.GetBytes(request);
            return HttpContent.Create(stream, contentType);
        }

        /// <summary>
        /// 创建一个用于封装提交数据的HttpContent
        /// </summary>
        /// <typeparam name="TEntity">要提交的实体数据类型</typeparam>
        /// <param name="entity">要提交的数据实体对象</param>
        /// <param name="contentType">设置提交数据的类型</param>
        /// <returns>返回封装好的HttpContent对象</returns>
        public HttpContent CreateHttpContent<TEntity>(TEntity entity, string contentType)
            where TEntity : class
        {
            return CreateHttpContent<TEntity>(entity, this._encoding, contentType);
        }

        /// <summary>
        /// 创建一个用于封装提交数据的HttpContent
        /// </summary>
        /// <typeparam name="TEntity">要提交的实体数据类型</typeparam>
        /// <param name="entity">要提交的数据实体对象</param>
        /// <returns>返回封装好的HttpContent对象</returns>
        public HttpContent CreateHttpContent<TEntity>(TEntity entity)
            where TEntity : class
        {
            return CreateHttpContent<TEntity>(entity, this._encoding, this._contentType);
        }

        /// <summary>
        /// 创建一个用于封装提交数据的HttpContent
        /// </summary>
        /// <param name="dictionary">要提交的数据集合对象</param>
        /// <param name="encoding">设置提交数据的编码格式</param>
        /// <param name="contentType">设置提交数据的类型</param>
        /// <returns>返回封装好的HttpContent对象</returns>
        public HttpContent CreateHttpContent(IDictionary<string, object> dictionary, Encoding encoding, string contentType)
        {
            return CreateHttpContent<IDictionary<string, object>>(dictionary, encoding, contentType);
        }

        /// <summary>
        /// 创建一个用于封装提交数据的HttpContent
        /// </summary>
        /// <param name="dictionary">要提交的数据集合对象</param>
        /// <param name="contentType">设置提交数据的类型</param>
        /// <returns>返回封装好的HttpContent对象</returns>
        public HttpContent CreateHttpContent(IDictionary<string, object> dictionary, string contentType)
        {
            return CreateHttpContent(dictionary, this._encoding, contentType);
        }

        /// <summary>
        /// 创建一个用于封装提交数据的HttpContent
        /// </summary>
        /// <param name="dictionary">要提交的数据集合对象</param>
        /// <returns>返回封装好的HttpContent对象</returns>
        public HttpContent CreateHttpContent(IDictionary<string, object> dictionary)
        {
            return CreateHttpContent(dictionary, this._encoding, this._contentType);
        }

        /// <summary>
        /// 创建一个用于封装二进制数据的HttpContent
        /// </summary>
        /// <param name="stream">要提交的二进制数据</param>
        /// <returns>返回封装好的HttpContent对象</returns>
        public virtual HttpContent CreateStreamContent(Stream stream)
        {
            return HttpContent.Create(stream);
        }

        /// <summary>
        /// 创建一个空数据的HttpContent
        /// </summary>
        /// <returns>返回封装好的HttpContent对象</returns>
        public virtual HttpContent CreateEmptyContent()
        {
            return HttpContent.CreateEmpty();
        }

        #endregion 创建HttpContent的相关方法...

        #region 提交操作请求的相关方法...

        /// <summary>
        /// 向服务端发起操作请求，并提交相关数据，接收服务端响应后返回的数据。
        /// </summary>
        /// <typeparam name="TReturn">指定服务端返回的主体数据类型</typeparam>
        /// <param name="host">设置服务端的主机地址</param>
        /// <param name="serviceName">设置服务类别名称</param>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <param name="method">设置提交类型</param>
        /// <param name="httpContent">设置要提交的数据对象</param>
        /// <returns>返回一个与服务端约定好的结果集对象</returns>
        public virtual Result<TReturn> Submit<TReturn>(string host, string serviceName, string uriTemplate, string method, HttpContent httpContent)
        {
            HttpResponseMessage response = GetHttpResponse(host, serviceName, uriTemplate, method, httpContent);
            Result<TReturn> result = null;

            if (response != null)
            {
                string contentType = response.Content.ContentType;
                if (contentType.Contains(MimeType.JSON))
                {
                    result = JsonConvert.DeserializeObject<Result<TReturn>>(response.Content.ReadAsString());
                }
                else if (contentType.Contains(MimeType.XML))
                {
                    result = response.Content.ReadAsXmlSerializable<Result<TReturn>>();
                }
            }

            return result;
        }

        /// <summary>
        /// 向服务端发起操作请求，并提交相关数据，接收服务端响应后返回的数据。
        /// </summary>
        /// <typeparam name="TEntity">指定要提交的实体数据的类型</typeparam>
        /// <typeparam name="TReturn">指定服务端返回的主体数据类型</typeparam>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <param name="entity">设置要求提交的实体数据</param>
        /// <param name="encoding">设置提交数据的编码格式</param>
        /// <param name="contentType">设置提交数据的类型</param>
        /// <returns>返回一个与服务端约定好的结果集对象</returns>
        public Result<TReturn> Submit<TEntity, TReturn>(string uriTemplate, TEntity entity, Encoding encoding, string contentType)
            where TEntity : class
        {
            HttpContent httpContent = CreateHttpContent(entity, encoding, contentType);

            string method = this._method;
            if(string.IsNullOrWhiteSpace(method))
                method = Jumpcity.Utility.HttpMethod.POST;

            return Submit<TReturn>(this._host, this._serviceName, uriTemplate, method, httpContent);    
        }

        /// <summary>
        /// 向服务端发起操作请求，并提交相关数据，接收服务端响应后返回的数据。
        /// </summary>
        /// <typeparam name="TEntity">指定要提交的实体数据的类型</typeparam>
        /// <typeparam name="TReturn">指定服务端返回的主体数据类型</typeparam>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <param name="entity">设置要求提交的实体数据</param>
        /// <param name="contentType">设置提交数据的类型</param>
        /// <returns>返回一个与服务端约定好的结果集对象</returns>
        public Result<TReturn> Submit<TEntity, TReturn>(string uriTemplate, TEntity entity, string contentType)
            where TEntity : class
        {
            return Submit<TEntity, TReturn>(uriTemplate, entity, this._encoding, contentType);
        }

        /// <summary>
        /// 向服务端发起操作请求，并提交相关数据，接收服务端响应后返回的数据。
        /// </summary>
        /// <typeparam name="TEntity">指定要提交的实体数据的类型</typeparam>
        /// <typeparam name="TReturn">指定服务端返回的主体数据类型</typeparam>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <param name="entity">设置要求提交的实体数据</param>
        /// <returns>返回一个与服务端约定好的结果集对象</returns>
        public Result<TReturn> Submit<TEntity, TReturn>(string uriTemplate, TEntity entity)
            where TEntity : class
        {
            return Submit<TEntity, TReturn>(uriTemplate, entity, this._encoding, this._contentType);
        }

        /// <summary>
        /// 向服务端发起操作请求，并提交相关数据，接收服务端响应后返回的数据。
        /// </summary>
        /// <typeparam name="TReturn">指定服务端返回的主体数据类型</typeparam>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <param name="dictionary">设置要提交的数据集合对象</param>
        /// <param name="encoding">设置提交数据的编码格式</param>
        /// <param name="contentType">设置提交数据的类型</param>
        /// <returns>返回一个与服务端约定好的结果集对象</returns>
        public Result<TReturn> Submit<TReturn>(string uriTemplate, IDictionary<string, object> dictionary, Encoding encoding, string contentType)
        {
            return Submit<IDictionary<string, object>, TReturn>(uriTemplate, dictionary, encoding, contentType);
        }

        /// <summary>
        /// 向服务端发起操作请求，并提交相关数据，接收服务端响应后返回的数据。
        /// </summary>
        /// <typeparam name="TReturn">指定服务端返回的主体数据类型</typeparam>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <param name="dictionary">设置要提交的数据集合对象</param>
        /// <param name="contentType">设置提交数据的类型</param>
        /// <returns>返回一个与服务端约定好的结果集对象</returns>
        public Result<TReturn> Submit<TReturn>(string uriTemplate, IDictionary<string, object> dictionary, string contentType)
        {
            return Submit<TReturn>(uriTemplate, dictionary, this._encoding, contentType);
        }

        /// <summary>
        /// 向服务端发起操作请求，并提交相关数据，接收服务端响应后返回的数据。
        /// </summary>
        /// <typeparam name="TReturn">指定服务端返回的主体数据类型</typeparam>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <param name="dictionary">设置要提交的数据集合对象</param>
        /// <returns>返回一个与服务端约定好的结果集对象</returns>
        public Result<TReturn> Submit<TReturn>(string uriTemplate, IDictionary<string, object> dictionary)
        {
            return Submit<TReturn>(uriTemplate, dictionary, this._encoding, this._contentType);
        }

        /// <summary>
        /// 向服务端发起操作请求，并提交相关数据，接收服务端响应后返回的数据。
        /// </summary>
        /// <typeparam name="TReturn">指定服务端返回的主体数据类型</typeparam>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <param name="stream">设置要提交的二进制流对象</param>
        /// <returns>返回一个与服务端约定好的结果集对象</returns>
        public Result<TReturn> Submit<TReturn>(string uriTemplate, Stream stream)
        {
            HttpContent httpContent = CreateStreamContent(stream);
            return Submit<TReturn>(this._host, this._serviceName, uriTemplate, Jumpcity.Utility.HttpMethod.POST, httpContent);
        }

        /// <summary>
        /// 向服务端发起GET操作请求，并提交相关数据，接收服务端响应后返回的数据。
        /// </summary>
        /// <typeparam name="TReturn">指定服务端返回的主体数据类型</typeparam>
        /// <param name="host">设置服务端的主机地址</param>
        /// <param name="serviceName">设置服务类别名称</param>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <returns>返回一个与服务端约定好的结果集对象</returns>
        public Result<TReturn> Submit<TReturn>(string host, string serviceName, string uriTemplate)
        {
            return Submit<TReturn>(host, serviceName, uriTemplate, Jumpcity.Utility.HttpMethod.GET, null);
        }

        /// <summary>
        /// 向服务端发起GET操作请求，并提交相关数据，接收服务端响应后返回的数据。
        /// </summary>
        /// <typeparam name="TReturn">指定服务端返回的主体数据类型</typeparam>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <returns>返回一个与服务端约定好的结果集对象</returns>
        public Result<TReturn> Submit<TReturn>(string uriTemplate)
        {
            return Submit<TReturn>(this._host, this._serviceName, uriTemplate, Jumpcity.Utility.HttpMethod.GET, null);
        }

        #endregion 提交操作请求的相关方法...

        #region 私有(保护)成员方法...

        /// <summary>
        /// 向服务端发起请求，获得服务端响应对象
        /// </summary>
        /// <param name="host">设置要请求的服务端主机地址</param>
        /// <param name="serviceName">设置要请求的服务类别名称</param>
        /// <param name="uriTemplate">设置要请求的服务根节点名称</param>
        /// <param name="method">设置请求类型</param>
        /// <param name="httpContent">设置要求提交到服务端的数据</param>
        /// <returns>返回服务端处理后的响应对象</returns>
        protected virtual HttpResponseMessage GetHttpResponse(string host, string serviceName, string uriTemplate, string method, HttpContent httpContent)
        {
            HttpClient _client = new HttpClient();
            HttpResponseMessage _response = null;

            try
            {
                Uri _url = GetRequestURL(host, serviceName, uriTemplate);

                if (method == Jumpcity.Utility.HttpMethod.GET)
                {
                    _response = _client.Get(_url);
                }
                else if (method == Jumpcity.Utility.HttpMethod.POST)
                {
                    if (httpContent == null)
                        httpContent = CreateEmptyContent();
                    _response = _client.Post(_url, httpContent);
                }

                return _response.EnsureStatusIsSuccessful();
            }
            catch (Exception ex)
            {
                if (_response != null)
                    _response.Dispose();

                throw ex;
            }
            finally
            {
                if (_client != null)
                    _client.Dispose();
            }
        }

        /// <summary>
        /// 获得一个JsonSerializerSettings对象用于设置Json序列化与反序列化
        /// </summary>
        /// <returns>返回设置好的JsonSerializerSettings对象</returns>
        private JsonSerializerSettings GetJsonSettings()
        {
            //创建JSON序列化规范设置对象
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //在序列化时使用微软的格式规范以保证和.NET服务器使用的规范保持一致
            settings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;

            //设置缩进
            settings.Formatting = Formatting.Indented;

            return settings;
        }

        /// <summary>
        /// 通过主机地址，服务类别名称和根节点名称构造一个URI资源标识符
        /// </summary>
        /// <param name="host">服务端主机地址</param>
        /// <param name="serviceName">服务类别名称</param>
        /// <param name="uriTemplate">服务根节点名称</param>
        /// <returns>返回构造好的URI资源标识符</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        private Uri GetRequestURL(string host, string serviceName, string uriTemplate)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentNullException("host");

            if (string.IsNullOrWhiteSpace(serviceName))
                throw new ArgumentNullException("serviceName");

            if (string.IsNullOrWhiteSpace(uriTemplate))
                throw new ArgumentNullException("uriTemplate");

            string url = Path.Combine(host, serviceName, uriTemplate);
            return new Uri(url);
        }

        #endregion 私有(保护)成员方法...

        #endregion 成员方法...
    }

}
