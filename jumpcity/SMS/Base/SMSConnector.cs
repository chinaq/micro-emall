using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using Jumpcity.Utility;
using Jumpcity.Utility.Extend;

namespace Jumpcity.SMS
{
    /// <summary>
    /// 在派生类中重写时，用于实现短信发送，该类为抽象类。
    /// </summary>
    public abstract class SMSConnector
    {
        #region 成员变量...

        private Uri _host = null;
        private string _username = null;
        private string _password = null;
        private List<string> _mobileNumbers = null;
        private string _content = null;
        private Encoding _encoding = Encoding.UTF8;
        private int _maxLength = 140;
        private string _method = HttpMethod.POST;
        private string _sign = string.Empty;
        private string _errorMessage = null;

        #endregion 成员变量...

        #region 成员属性...

        public Uri Host
        {
            get { return _host; }
            set { _host = value; }
        }

        public string UserName 
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public List<string> MobileNumbers
        {
            get 
            {
                if (_mobileNumbers == null)
                    _mobileNumbers = new List<string>();
                return _mobileNumbers;
            }
            set { _mobileNumbers = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public Encoding ContentEncoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public virtual string Sign
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_sign))
                    return "【" + _sign + "】";
                else
                    return string.Empty;
            }
            set { _sign = value; }
        }

        public virtual string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        #endregion 成员属性...

        #region 成员方法...

        protected virtual string GetContent(string content, int maxLength, Encoding encoding)
        {
            content = content.Trim();
            int byteLength = content.GetByteLength();
            while (byteLength > maxLength)
            {
                content = content.Remove(content.Length - 1);
                byteLength = content.GetByteLength();
            }
            return HttpUtility.UrlEncode(content, encoding);
        }
        protected virtual string GetContent(string content)
        {
            return GetContent(content, this._maxLength, this._encoding);
        }
        
        protected virtual string GetMobileNumberString(List<string> mobileNumbers, int maxCount = 0)
        {
            if (General.IsNullable(mobileNumbers))
            {
                this._errorMessage = "接收短信的手机号码列表长度不能为零";
                return string.Empty;
            }
            if (maxCount > 0 && mobileNumbers.Count > maxCount)
            {
                this._errorMessage = string.Format("每次接收短信的手机号码数量不能超过{0}个", maxCount);
                return string.Empty;
            }

            List<string> mobileList = new List<string>();
            foreach (string number in mobileNumbers)
            {
                if (number.IsMobileNumber())
                    mobileList.Add(number);
            }

            if (!General.IsNullable(mobileList))
                return string.Join(",", mobileList);
            else
                return string.Empty;
        }

        protected virtual string RequestSms(string url, string queryString, string method, Encoding encoding)
        {
            StreamReader reader = null;
            Stream requestStream = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            string responseText = string.Empty;

            try
            {
                if (method == HttpMethod.POST)
                {
                    byte[] postData = encoding.GetBytes(queryString);
                    request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                    request.Method = method;
                    request.ContentType = MimeType.UrlEncoded;
                    request.ContentLength = postData.GetLongLength(0);
                    requestStream = request.GetRequestStream();
                    requestStream.Write(postData, 0, postData.Length);
                }
                else if (method == HttpMethod.GET)
                {
                    url += "?" + queryString;
                    request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                }
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), encoding);
                if (reader != null)
                    responseText = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }

            return responseText;
        }
        
        /// <summary>
        /// 在派生类重写时，用于发送短信。
        /// </summary>
        /// <param name="content">要发送的短信内容</param>
        /// <returns></returns>
        public abstract SendResult SendSms(string content);
            
        #endregion 成员方法...  
    }
}
