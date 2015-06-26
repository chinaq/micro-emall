using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Jumpcity.Push
{
    [Serializable]
    public class JPushResult
    {
        [JsonProperty(PropertyName = "errcode")]
        public int Code
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "errmsg")]
        public string Message
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "msg_id")]
        public string PushID
        {
            get;
            set;
        }

        public static JPushResult Parse(string resultString)
        {
            JPushResult result = null;
            try
            {
                result = JsonConvert.DeserializeObject<JPushResult>(resultString);
            }
            catch (Exception ex)
            {
                result = new JPushResult();
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
