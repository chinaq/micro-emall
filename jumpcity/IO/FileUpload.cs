using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Text;

namespace Jumpcity.IO
{
    /// <summary>
    /// 用来实现文件上传的类
    /// </summary>
    public class FileUpload
    {
        #region 字段...

        private string _host = string.Empty;
        private string _path = "~/images";
        private string _name = DateTime.Now.ToFileTime().ToString();
        private string _extion = ".jpg";
        private int _currentLength = 0;
        private int _maxLength = 0;
        private string _errorMessage = string.Empty;

        #endregion 字段...

        #region 构造方法...

        /// <summary>
        /// 初始化类的新实例
        /// </summary>
        /// <param name="host">当前文件要上传到的服务器主机名称</param>
        /// <param name="path">当前文件要保存到的服务器路径</param>
        /// <param name="name">当前文件要保存成的文件名</param>
        /// <param name="extion">当前文件要使用的扩展名</param>
        /// <param name="currentLength">当前文件的长度</param>
        /// <param name="maxLength">指定可上传的最大长度</param>
        public FileUpload(string host, string path, string name, string extion, int currentLength, int maxLength)
        {
            this.Init(host, path, name, extion, currentLength, maxLength);
        }

        /// <summary>
        /// 初始化类的新实例
        /// </summary>
        /// <param name="path">当前文件要保存到的服务器路径</param>
        /// <param name="name">当前文件要保存成的文件名</param>
        /// <param name="extion">当前文件要使用的扩展名</param>
        /// <param name="currentLength">当前文件的长度</param>
        /// <param name="maxLength">指定可上传的最大长度</param>
        public FileUpload(string path, string name, string extion, int currentLength, int maxLength)
        {
            this.Init("", path, name, extion, currentLength, maxLength);     
        }

        /// <summary>
        /// 初始化类的新实例
        /// </summary>
        /// <param name="fileName">当前文件保存到服务器上的完整文件路径，以"~"开头，包括文件名和扩展名</param>
        /// <param name="currentLength">当前文件的长度</param>
        /// <param name="maxLength">指定可上传的最大长度</param>
        public FileUpload(string fileName, int currentLength, int maxLength) 
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                string path = fileName.Substring(0, fileName.LastIndexOf("/"));
                string name = fileName.Substring(fileName.LastIndexOf("/") + 1, fileName.LastIndexOf(".") - (fileName.LastIndexOf("/") + 1));
                string extion = fileName.Substring(fileName.LastIndexOf("."));
                this.Init("", path, name, extion, currentLength, maxLength);
            }
            else
            {
                this._currentLength = currentLength;
                this._maxLength = maxLength;
            }
        }

        /// <summary>
        /// 初始化类的新实例
        /// </summary>
        public FileUpload() { }

        #endregion 构造方法...

        #region 属性...

        /// <summary>
        /// 获取或设置文件要上传到的服务器主机名称
        /// </summary>
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        /// <summary>
        /// 获取或设置当前文件要保存到的服务器目录路径
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// 获取或设置当前文件要保存成的文件名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 获取或设置当前文件要使用的扩展名
        /// </summary>
        public string Extion
        {
            get { return _extion; }
            set { _extion = value; }
        }

        /// <summary>
        /// 获取操作发生错误时，返回的错误信息
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        /// <summary>
        /// 获取当前文件上传后完整的Http访问路径
        /// </summary>
        public string HttpFileName
        {
            get { return CreateFileName(true); }
        }

        /// <summary>
        /// 获取当前文件完整的路径
        /// </summary>
        public string FileName
        {
            get { return CreateFileName(false); }
        }

        #endregion 属性...

        #region 成员方法...

        /// <summary>
        /// 根据指定的文件路径上传文件到服务器
        /// </summary>
        /// <param name="stream">要上传的文件流</param>
        /// <returns>返回一个布尔值，该值指示上传是否成功</returns>
        public virtual bool Upload(Stream stream)
        {
            return Upload(stream, this._path, this._name, this._extion, this._currentLength, this._maxLength);
        }

        /// <summary>
        /// 根据指定的文件路径上传文件到服务器
        /// </summary>
        /// <param name="stream">要上传的文件流</param>
        /// <param name="currentLength">当前要上传的文件的长度(字节数)，该值如果为零代表忽略当前文件的长度</param>
        /// <param name="maxLength">指定的上传文件可用的最大长度(字节数)，该值如果为零代表不限制长度</param>
        /// <returns>返回一个布尔值，该值指示上传是否成功</returns>
        public virtual bool Upload(Stream stream, int currentLength, int maxLength)
        {
            return Upload(stream, this._path, this._name, this._extion, currentLength, maxLength);
        }

        /// <summary>
        /// 根据指定的文件路径上传文件到服务器
        /// </summary>
        /// <param name="stream">要上传的文件流</param>
        /// <param name="path">上传到的服务器路径(带"~")</param>
        /// <param name="name">指定上传到服务器后的文件名</param>
        /// <param name="extion">指定上传后的文件扩展名</param>
        /// <param name="currentLength">当前要上传的文件的长度(字节数)，该值如果为零代表忽略当前文件的长度</param>
        /// <param name="maxLength">指定的上传文件可用的最大长度(字节数)，该值如果为零代表不限制长度</param>
        /// <returns>返回一个布尔值，该值指示上传是否成功</returns>
        public virtual bool Upload(Stream stream, string path, string name, string extion, int currentLength, int maxLength)
        {
            bool flag = false;

            if (stream == null)
                this._errorMessage = "文件流不存在";
            else if (string.IsNullOrWhiteSpace(path))
                this._errorMessage = "文件保存路径未指定";
            else if (string.IsNullOrWhiteSpace(name))
                this._errorMessage = "文件名称未指定";
            else if (string.IsNullOrWhiteSpace(extion))
                this._errorMessage = "文件扩展名未指定";
            else if (currentLength > 0 && maxLength > 0 && currentLength > maxLength)
                this._errorMessage = string.Format("当前文件长度({0}字节)，超出指定范围({1}字节)", currentLength, maxLength);
            else
            {
                string mapFileName = null;
                if (FileHelper.CreateDirectory(path))
                    mapFileName = FileHelper.MapPath(System.IO.Path.Combine(path, name + extion));

                if (mapFileName != null)
                {
                    FileStream file = null;
                    try
                    {
                        int length = (int)stream.Length;
                        byte[] buffer = new byte[length];
                        stream.Read(buffer, 0, length);

                        if (extion == ".jpg" || extion == ".png" || extion == ".gif" || extion == ".bmp")
                        {
                            if (!FileHelper.AllowedImages(buffer))
                            {
                                this._errorMessage = "请上传图片文件";
                                if (stream != null)
                                    stream.Close();
                                return false;
                            }
                        }

                        file = new FileStream(mapFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        file.Write(buffer, 0, length);
                        file.Flush();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                            this._errorMessage = ex.InnerException.Message;
                        else
                            this._errorMessage = ex.Message;

                        flag = false;
                    }
                    finally
                    {
                        if (file != null)
                            file.Close();
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// 根据指定的文件路径上传文件到服务器
        /// </summary>
        /// <param name="request">客户端请求对象</param>
        /// <param name="index">当上传多个文件时为文件流列表项的索引，单个文件时应为零</param>
        /// <param name="path">上传到的服务器路径(带"~")</param>
        /// <param name="name">指定上传到服务器后的文件名</param>
        /// <param name="extion">指定上传后的文件扩展名</param>
        /// <param name="currentLength">当前要上传的文件的长度(字节数)，该值如果为零代表忽略当前文件的长度</param>
        /// <param name="maxLength">指定的上传文件可用的最大长度(字节数)，该值如果为零代表不限制长度</param>
        /// <returns>返回一个布尔值，该值指示上传是否成功</returns>
        public virtual bool Upload(HttpRequest request, int index, string path, string name, string extion, int currentLength, int maxLength)
        {
            Stream stream = FileHelper.GetRequestFile(request, index);
            return Upload(stream, path, name, extion, currentLength, maxLength);
        }

        /// <summary>
        /// 根据指定的文件路径上传文件到服务器
        /// </summary>
        /// <param name="request">客户端请求对象</param>
        /// <returns>返回一个布尔值，该值指示上传是否成功</returns>
        public virtual bool Upload(HttpRequest request)
        {
            return Upload(request, 0, this._path, this._name, this._extion, this._currentLength, this._maxLength);
        }

        /// <summary>
        /// 根据指定的文件路径上传文件到服务器
        /// </summary>
        /// <param name="request">客户端请求对象</param>
        /// <param name="index">当上传多个文件时为文件流列表项的索引，单个文件时应为零</param>
        /// <returns>返回一个布尔值，该值指示上传是否成功</returns>
        public virtual bool Upload(HttpRequest request, int index)
        {
            return Upload(request, index, this._path, this._name, this._extion, this._currentLength, this._maxLength);
        }

        /// <summary>
        /// 根据指定的文件路径上传文件到服务器
        /// </summary>
        /// <param name="request">客户端请求对象</param>
        /// <param name="currentLength">当前要上传的文件的长度(字节数)，该值如果为零代表忽略当前文件的长度</param>
        /// <param name="maxLength">指定的上传文件可用的最大长度(字节数)，该值如果为零代表不限制长度</param>
        /// <returns>返回一个布尔值，该值指示上传是否成功</returns>
        public virtual bool Upload(HttpRequest request, int currentLength, int maxLength)
        {
            return Upload(request, 0, this._path, this._name, this._extion, currentLength, maxLength);
        }

        /// <summary>
        /// 根据指定的文件路径上传文件到服务器
        /// </summary>
        /// <param name="request">客户端请求对象</param>
        /// <param name="index">当上传多个文件时为文件流列表项的索引，单个文件时应为零</param>
        /// <param name="currentLength">当前要上传的文件的长度(字节数)，该值如果为零代表忽略当前文件的长度</param>
        /// <param name="maxLength">指定的上传文件可用的最大长度(字节数)，该值如果为零代表不限制长度</param>
        /// <returns>返回一个布尔值，该值指示上传是否成功</returns>
        public virtual bool Upload(HttpRequest request, int index, int currentLength, int maxLength)
        {
            return Upload(request, index, this._path, this._name, this._extion, currentLength, maxLength);
        }

        #endregion 成员方法...

        #region 保护成员...

        /// <summary>
        /// 初始化类的新实例
        /// </summary>
        /// <param name="host">当前文件要上传到的服务器主机名称</param>
        /// <param name="path">当前文件要保存到的服务器路径</param>
        /// <param name="name">当前文件要保存成的文件名</param>
        /// <param name="extion">当前文件要使用的扩展名</param>
        /// <param name="currentLength">当前文件的长度</param>
        /// <param name="maxLength">指定可上传的最大长度</param>
        protected void Init(string host, string path, string name, string extion, int currentLength, int maxLength)
        {
            if (!string.IsNullOrWhiteSpace(host))
                this._host = host;
            else
            {
                this._host = ConfigurationManager.AppSettings["fileHost"];
                if (string.IsNullOrWhiteSpace(this._host))
                {
                    Uri uri = HttpContext.Current.Request.Url;
                    this._host = string.Format("{0}://{1}", uri.Scheme, uri.Authority);
                }
            }

            this._path = path;
            this._name = name;
            this._extion = extion;
            this._currentLength = currentLength;
            this._maxLength = maxLength;
        }

        /// <summary>
        /// 创建一个完整的文件名
        /// </summary>
        /// <param name="IsHttp">指示是否创建Http路径</param>
        /// <returns>返回一个完整的文件路径</returns>
        protected virtual string CreateFileName(bool IsHttp)
        {
            string fullFileName = System.IO.Path.Combine(_path, _name + _extion);

            if (IsHttp)
            {
                if (string.IsNullOrWhiteSpace(this._host))
                    throw new Exception("ImageHost为空时，无法获取Http规范的文件路径");

                fullFileName = fullFileName.Replace("~", this._host);
            }

            return fullFileName.Replace("\\","\\\\").Replace("\\\\", "/");
        }

        #endregion 保护成员...
    }
}
