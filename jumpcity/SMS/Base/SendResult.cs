using System;
using System.Collections.Generic;
using System.Text;

namespace Jumpcity.SMS
{
    public class SendResult
    {
        private int _statusCode;
        private string _value;
        
        public int StatusCode 
        {
            get{ return _statusCode; } 
            set{ _statusCode = value; } 
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public SendResult(int statusCode, string value)
        {
            this._statusCode = statusCode;
            this._value = value;
        }
        public SendResult() { }
    }
}
