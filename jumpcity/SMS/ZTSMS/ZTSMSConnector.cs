using System;
using System.Collections.Generic;

namespace Jumpcity.SMS
{
    public class ZTSMSConnector : SMSConnector
    {
        public const string CONTENTSPLIT = "※";
        private string _productId = null;

        public string ProductID
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public ZTSMSConnector(string username, string password, string productId, List<string> mobileNumbers)
        {
            this.Host = new Uri("http://www.ztsms.cn:8800/");
            this.UserName = username;
            this.Password = password;
            this.MobileNumbers = mobileNumbers;
            this.MaxLength = 1000;
            this._productId = productId;
        }
        public ZTSMSConnector(string username, string password, string productId, string mobileNumber)
            : this(username, password, productId, new List<string> { mobileNumber }) { }
        public ZTSMSConnector(string username, string password, string productId)
            : this(username, password, productId, new List<string>()) { }

        public SendResult SendSms(string content, List<string> mobileNumbers, DateTime? sendTime)
        {
            string mobile = base.GetMobileNumberString(mobileNumbers, 500);
            if (mobile == string.Empty)
                return null;

            string host = this.Host.ToString();
            string method = this.Method;
            string username = this.UserName;
            string password = this.Password;
            string productId = this._productId;
            
            string dstime = null;
            if(sendTime.HasValue)
                dstime = ((DateTime)sendTime).ToString("yyyyMMddHHmmss");

            if (string.IsNullOrWhiteSpace(username))
            {
                this.ErrorMessage = "用户名必须提交";
                return null;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                this.ErrorMessage = "密码必须提交";
                return null;
            }

            if (string.IsNullOrWhiteSpace(productId))
            {
                this.ErrorMessage = "产品ID必须提交";
                return null;
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                this.ErrorMessage = "发送内容必须提交";
                return null;
            }

            if (content.IndexOf(CONTENTSPLIT) != -1)
                host += "sendManySms.do";
            else
                host += "sendSms.do";

            content = base.GetContent(content, this.MaxLength, this.ContentEncoding) + this.Sign;

            string queryString = string.Format(
                "username={0}&password={1}&productid={2}&mobile={3}&content={4}",
                username,
                password,
                productId,
                mobile,
                content
            );

            if (!string.IsNullOrWhiteSpace(dstime))
                queryString += string.Format("&dstime={0}", dstime);

            string responseText = base.RequestSms(host, queryString, method, this.ContentEncoding);
            return GetResult(responseText);           
        }
        public SendResult SendSms(string content, string mobileNumber, DateTime? sendTime)
        {
            return SendSms(content, new List<string> { mobileNumber }, sendTime);
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
                if (resposeText.IndexOf(",") != -1)
                    result.StatusCode = int.Parse(resposeText.Split(',')[0]);

                switch (result.StatusCode)
                {
                    case -1:
                        result.Value = "用户名或者密码不正确";
                        break;
                    case 1:
                        result.Value = "发送成功";
                        break;
                    case 0:
                        result.Value = "发送失败";
                        break;
                    case 2:
                        result.Value = "余额不足";
                        break;
                    case 3:
                        result.Value = "扣费失败";
                        break;
                    case 5:
                        result.Value = "短信定时成功";
                        break;
                    case 6:
                        result.Value = "有效号码为空";
                        break;
                    case 7:
                        result.Value = "短信内容为空";
                        break;
                    case 8:
                        result.Value = "无签名";
                        break;
                    case 9:
                        result.Value = "没有Url提交权限";
                        break;
                    case 10:
                        result.Value = "发送号码过多";
                        break;
                    case 11:
                        result.Value = "产品ID异常";
                        break;
                    case 12:
                        result.Value = "参数异常";
                        break;
                    case 13:
                        result.Value = "重复提交";
                        break;
                    case 14:
                        result.Value = "禁止提交";
                        break;
                    case 15:
                        result.Value = "Ip验证失败";
                        break;
                    case 19:
                        result.Value = "短信内容过长";
                        break;
                    case 20:
                        result.Value = "定时时间格式不正确";
                        break;
                    default:
                        result.Value = "未知错误";
                        break;
                }
            }

            return result;
        }
    }
}
