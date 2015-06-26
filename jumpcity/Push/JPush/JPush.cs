using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Jumpcity.Utility;
using Jumpcity.Utility.Extend;

namespace Jumpcity.Push
{
    public class JPush
    {
        private string _host = "http://api.jpush.cn:8800/v2/push";
        private string _method = HttpMethod.POST;
        private string _contentType = MimeType.UrlEncoded;
        private Encoding _encoding = Encoding.UTF8;

        private string _appKey = "";
        private string _masterSecret = "";
        
        private int _sendId = 0;
        private int _receiverType = JPushReceiverType.Alias;
        private List<string> _receiverValue = null;
        private int _msgType = JPushMessageType.Notice;
        private JPushContent _content = null;
        private string _sendDescription = "";
        private string _platform = JPushPlatForm.Both;
        private int _timeToLive = 864000;
        private string _overrideId = "";

        public string AppKey
        {
            get { return _appKey; }
            set { _appKey = value; }
        }

        public string MasterSecert
        {
            get { return _masterSecret; }
            set { _masterSecret = value; }
        }

        public int SendID
        {
            get { return _sendId; }
            set { _sendId = value; }
        }

        public int ReceiverType
        {
            get { return _receiverType; }
            set { _receiverType = value; }
        }

        public List<string> ReceiverValue
        {
            get
            {
                if (_receiverValue == null)
                    _receiverValue = new List<string>();
                return _receiverValue;
            }
            set { _receiverValue = value; }
        }

        public int MessageType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        public JPushContent Content
        {
            get
            {
                if (_content == null)
                    _content = new JPushContent();
                return _content;
            }
        }

        public string SendDescription
        {
            get { return _sendDescription; }
            set { _sendDescription = value; }
        }

        public string PlatForm
        {
            get { return _platform; }
            set { _platform = value; }
        }

        public int TimeToLive
        {
            get { return _timeToLive; }
            set { _timeToLive = value; }
        }

        public string OverrideID
        {
            get { return _overrideId; }
            set { _overrideId = value; }
        }

        public JPushResult Push(JPushContent content, int receiverType, List<string> receiverValue, int messageType, string platform, int sendId, int? timeToLive, string overrideId)
        {
            JPushResult result = null; 

            if (string.IsNullOrWhiteSpace(this._appKey) || string.IsNullOrWhiteSpace(this._masterSecret) || General.IsNullable(receiverValue) || content == null)
            {
                result = new JPushResult();
                result.Code = 500;
                result.Message = "关键参数不能为空";
                return result;
            }

            string receiver = string.Join(",", receiverValue);

            StringBuilder query = new StringBuilder();

            query.AppendFormat(
                "sendno={0}&app_key={1}&receiver_type={2}", 
                sendId, 
                this._appKey, 
                receiverType
            );

            if (receiverType != JPushReceiverType.All)
                query.AppendFormat("&receiver_value={0}", receiver);

            query.AppendFormat(
                "&verification_code={0}&msg_type={1}&msg_content={2}&platform={3}", 
                GetVerificationCode(sendId, receiverType, receiver),
                messageType,
                JsonConvert.SerializeObject(content),
                platform
            );

            if (!string.IsNullOrWhiteSpace(this._sendDescription))
                query.AppendFormat("&send_description={0}", this._sendDescription);

            if (timeToLive.HasValue)
                query.AppendFormat("&time_to_live={0}", timeToLive);

            if (!string.IsNullOrWhiteSpace(overrideId))
                query.AppendFormat("&override_msg_id={0}", overrideId);

            result = JPushResult.Parse(
                Submit(this._host, query.ToString())
            );

            return result;
        }

        public JPushResult Push(JPushContent content, int receiverType, List<string> receiverValue, int messageType, string platform, int sendId)
        {
            return Push(content, receiverType, receiverValue, messageType, platform, sendId, this._timeToLive, this._overrideId);
        }

        public JPushResult Push(JPushContent content, int receiverType, List<string> receiverValue)
        {
            return Push(content, receiverType, receiverValue, this._msgType, this._platform, this._sendId, this._timeToLive, this._overrideId);
        }

        public JPushResult Push(JPushContent content, List<string> receiverValue)
        {
            return Push(content, this._receiverType, receiverValue, this._msgType, this._platform, this._sendId, this._timeToLive, this._overrideId);
        }

        public JPushResult Push()
        {
            return Push(this._content, this._receiverType, this._receiverValue, this._msgType, this._platform, this._sendId, this._timeToLive, this._overrideId);
        }


        protected string GetVerificationCode(int sendId, int receiverType, string receiverValue)
        {
            string input = sendId.ToString() + receiverType + receiverValue + this._masterSecret;
            return Encryption.EncrypForMD5(input);
        }

        protected string Submit(string url, string queryString)
        {
            StreamReader reader = null;
            Stream requestStream = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            string responseText = string.Empty;

            try
            {
                byte[] postData = this._encoding.GetBytes(queryString);
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = this._method;
                request.ContentType = this._contentType;
                request.ContentLength = postData.GetLongLength(0);
                requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), this._encoding);
                if (reader != null)
                    responseText = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                responseText = ex.Message;
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
    }
}
