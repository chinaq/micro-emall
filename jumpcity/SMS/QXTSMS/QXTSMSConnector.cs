using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumpcity.Utility;

namespace Jumpcity.SMS
{
    public class QXTSMSConnector : SMSConnector
    {
        public QXTSMSConnector(string username, string password, List<string> mobileNumbers)
        {
            this.Host = new Uri("http://221.122.112.136:8080/sms/");
            this.UserName = username;
            this.Password = password;
            this.Method = HttpMethod.GET;
            this.MobileNumbers = mobileNumbers;
            this.MaxLength = 500;
            this.ContentEncoding = Encoding.GetEncoding("gbk");
        }
        public QXTSMSConnector(string username, string password, string mobileNumber)
            : this(username, password, new List<string> { mobileNumber }) { }
        public QXTSMSConnector(string username, string password) 
            : this(username, password, new List<string>()) { }

        public SendResult SendSms(string content, List<string> mobileNumbers, string spCode)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                this.ErrorMessage = "短信内容不能为空";
                return null;
            }

            if (string.IsNullOrWhiteSpace(this.UserName))
            {
                this.ErrorMessage = "用户名必须提交";
                return null;
            }

            if (string.IsNullOrWhiteSpace(this.Password))
            {
                this.ErrorMessage = "密码必须提交";
                return null;
            }

            string phones = base.GetMobileNumberString(mobileNumbers, 50);
            if (phones == string.Empty)
                return null;
            
            string url = this.Host.ToString() + "mt.jsp";
            string method = this.Method;
            string cpName = this.UserName;
            string cpPwd = this.Password;
            
            Encoding encoding = this.ContentEncoding;
            string msg = base.GetContent(content + this.Sign, this.MaxLength, encoding);

            string queryString = string.Format(
                "cpName={0}&cpPwd={1}&phones={2}&msg={3}",
                cpName,
                cpPwd,
                phones,
                msg    
            );
            if (!string.IsNullOrWhiteSpace(spCode))
                queryString += "&spCode=" + spCode;

            string responseText = base.RequestSms(url, queryString, method, encoding);
            return GetResult(responseText);           
        }
        public SendResult SendSms(string content, string mobileNumber, string spCode)
        {
            return SendSms(content, new List<string> { mobileNumber }, spCode);
        }
        public SendResult SendSms(string content, string mobileNumber)
        {
            return SendSms(content, mobileNumber, null);
        }
        public override SendResult SendSms(string content)
        {
            return SendSms(content, this.MobileNumbers, null);   
        }

        private SendResult GetResult(string resposeText)
        {
            SendResult result = new SendResult();
            if (!string.IsNullOrWhiteSpace(resposeText))
            {
                if (resposeText.IndexOf("&") == -1)
                {
                    result.StatusCode = int.Parse(resposeText);
                    result.Value = "发送成功";
                }
                else
                {
                    result.Value = resposeText.Split('&')[1];
                    switch (result.Value)
                    {
                        case "您的余额不足以支持此次发送，请您充值":
                            result.StatusCode = 1;
                            result.Value = "余额不足";
                            break;
                        default:
                            result.StatusCode = 3;
                            result.Value = "发送失败";
                            break;
                    }
                }
            }

            return result;
        }
    }
}
