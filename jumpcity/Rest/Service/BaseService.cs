using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.ServiceModel.Web;
using System.Data.Objects;
using Jumpcity.Utility.Extend;

namespace Jumpcity.Rest.Service
{
    #region 相关委托...

    /// <summary>
    /// 用于实现自定义操作的委托
    /// </summary>
    /// <typeparam name="TContext">ADO.NET数据实体操作对象的类型</typeparam>
    /// <typeparam name="TReturn">要返回的数据类型</typeparam>
    /// <param name="context">ADO.NET数据实体操作对象</param>
    /// <param name="Arguments">自定义操作所需的参数列表</param>
    /// <returns>返回指定自定义类型的返回值</returns>
    public delegate TReturn ExecuteHandler<TContext, TReturn>(TContext context, IDictionary<string, object> Arguments);

    /// <summary>
    /// 用于实现自定义写入操作的委托
    /// </summary>
    /// <typeparam name="TContext">ADO.NET数据实体操作对象的类型</typeparam>
    /// <typeparam name="TEntity">要写入的数据实体对象的类型</typeparam>
    /// <typeparam name="TReturn">要返回的数据类型</typeparam>
    /// <param name="context">ADO.NET数据实体操作对象</param>
    /// <param name="entity">要写入的数据实体对象</param>
    /// <returns>返回指定自定义类型的返回值</returns>
    public delegate TReturn ChangeHandler<TContext, TEntity, TReturn>(TContext context, TEntity entity) where TEntity : class;

    #endregion 相关委托...

    /// <summary>
    /// 用于实现REST服务端操作的基类
    /// </summary>
    /// <typeparam name="TContext">ADO.NET数据实体操作对象的类型</typeparam>
    public class BaseService<TContext> where TContext : ObjectContext
    {
        private TContext _context = null;
        private WebOperationContext _webContext = null;

        #region 属性...

        /// <summary>
        /// 获取ADO.NET数据实体操作对象
        /// </summary>
        public TContext EntityContext
        {
            get
            {
                if (this._context == null)
                    this._context = Activator.CreateInstance<TContext>();
                return this._context;
            }
        }

        /// <summary>
        /// 获取当前Web操作的上下文
        /// </summary>
        public WebOperationContext CurrentContext
        {
            get { return this._webContext; }
        }

        /// <summary>
        /// 获取正在发送的Web响应上下文
        /// </summary>
        public OutgoingWebResponseContext Response
        {
            get { return _webContext.OutgoingResponse; }
        }

        /// <summary>
        /// 获取客户端提交的RequestBody的字节长度
        /// </summary>
        public int ContentLength
        {
            get
            {
                long length = this._webContext.IncomingRequest.ContentLength;
                if (length > Int32.MaxValue)
                    length = Int32.MaxValue;
                return Convert.ToInt32(length);
            }
        }

        /// <summary>
        /// 获取客户端提交的数据类型
        /// </summary>
        public string ContentType
        {
            get { return this._webContext.IncomingRequest.ContentType; }
        }

        #endregion 属性...

        #region 构造函数...

        /// <summary>
        /// 创建一个用于实现REST操作的对象
        /// </summary>
        public BaseService()
        {
            this._webContext = WebOperationContext.Current;
        }

        #endregion 构造函数...

        #region 成员方法...

        /// <summary>
        /// 执行自定义的操作，返回REST结果集
        /// </summary>
        /// <typeparam name="TReturn">REST结果集中代表结果主体的数据类型</typeparam>
        /// <param name="executeFunc">自定义操作，该参数是一个委托</param>
        /// <param name="Arguments">执行自定义操作时所需的参数列表</param>
        /// <returns>返回处理后的REST结果集</returns>
        public virtual Result<TReturn> Execute<TReturn>(ExecuteHandler<TContext, TReturn> executeFunc, IDictionary<string, object> Arguments)
        {
            this.SetHttpStatusCodeOK();

            Result<TReturn> result = CreateResult<TReturn>();

            try
            {
                TContext context = this.EntityContext;
                if (executeFunc != null)
                    result.Results = executeFunc.Invoke(context, Arguments);
            }
            catch (Exception ex)
            {
                string message = string.Empty;
                if (ex.InnerException != null)
                    message = ex.InnerException.Message;
                else
                    message = ex.Message;
                result.UpdateToError(message);
            }
            finally
            {
                if (this._context != null)
                    this._context.Dispose();
            }

            return result;
        }

        /// <summary>
        /// 执行自定义的操作，返回REST结果集
        /// </summary>
        /// <typeparam name="TReturn">REST结果集中代表结果主体的数据类型</typeparam>
        /// <param name="executeFunc">自定义操作，该参数是一个委托</param>
        /// <param name="ArgumentName">执行自定义操作时所需的参数名称</param>
        /// <param name="ArgumentValue">执行自定义操作时所需的参数值</param>
        /// <returns>返回处理后的REST结果集</returns>
        public virtual Result<TReturn> Execute<TReturn>(ExecuteHandler<TContext, TReturn> executeFunc, string ArgumentName, object ArgumentValue)
        {
            if (string.IsNullOrWhiteSpace(ArgumentName))
                return null;

            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add(ArgumentName, ArgumentValue);

            return Execute<TReturn>(executeFunc, args);
        }

        /// <summary>
        /// 执行自定义的操作，返回REST结果集
        /// </summary>
        /// <typeparam name="TReturn">REST结果集中代表结果主体的数据类型</typeparam>
        /// <param name="executeFunc">自定义操作，该参数是一个委托</param>
        /// <returns>返回处理后的REST结果集</returns>
        public virtual Result<TReturn> Execute<TReturn>(ExecuteHandler<TContext, TReturn> executeFunc)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            return Execute<TReturn>(executeFunc, args);
        }

        /// <summary>
        /// 执行自定义的操作，返回REST结果集
        /// </summary>
        /// <typeparam name="TEntity">要操作的数据实体对象的类型</typeparam>
        /// <typeparam name="TReturn">要返回的数据类型</typeparam>
        /// <param name="changeFunc">自定义写入操作，该参数是一个委托</param>
        /// <param name="entity">要写入的数据实体对象</param>
        /// <returns>返回处理后的REST结果集</returns>
        public virtual Result<TReturn> Execute<TEntity, TReturn>(ChangeHandler<TContext, TEntity, TReturn> changeFunc, TEntity entity)
            where TEntity : class
        {
            this.SetHttpStatusCodeOK();

            Result<TReturn> result = CreateResult<TReturn>();

            try
            {
                TContext context = this.EntityContext;
                if (changeFunc != null)
                    result.Results = changeFunc.Invoke(context, entity);
            }
            catch (Exception ex)
            {
                result.UpdateToError(ex.Message);
            }
            finally
            {
                if (this._context != null)
                    this._context.Dispose();
            }

            return result;    
        }

        /// <summary>
        /// 将HTTP状态码设置为OK(200)以保证客户端总能顺利获得服务器响应
        /// </summary>
        protected virtual void SetHttpStatusCodeOK()
        {
            this._webContext.OutgoingResponse.StatusCode = HttpStatusCode.OK;
        }

        /// <summary>
        /// 创建一个用以保存服务端返回结果集的对象
        /// </summary>
        /// <typeparam name="TReturn">指定结果集主体的数据类型</typeparam>
        /// <returns>返回创建好的结果集对象</returns>
        protected virtual Result<TReturn> CreateResult<TReturn>()
        {
            Result<TReturn> result = new Result<TReturn>(
                this._webContext.IncomingRequest.UriTemplateMatch.RequestUri.ToString(),
                200,
                string.Empty
            );

            return result;
        }
        
        #endregion 成员方法...
    }
}