using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Jumpcity.Push
{
    [Serializable]
    public class JPushContent
    {
        private JPushExtras _extras = null;

        [JsonProperty(PropertyName = "n_content")]
        public string Content
        {
            get { return string.Empty; }
        }

        [JsonProperty(PropertyName = "n_extras")]
        public JPushExtras Extras
        {
            get
            {
                if (_extras == null)
                    _extras = new JPushExtras();
                return _extras;
            }
        }
    }

    [Serializable]
    public class JPushExtras
    {
        private IOSApns _ios = null;
        private int _builderId = 0;
        
        [JsonProperty(PropertyName = "ios")]
        public IOSApns IOS
        {
            get
            {
                if (this._ios == null)
                    this._ios = new IOSApns();
                return this._ios;
            }
        }

        [JsonProperty(PropertyName = "builder_id")]
        public int BuilderID
        {
            get { return this._builderId; }
            set
            {
                int id = value;
                id = id < 0 ? 0 : id;
                id = id > 1000 ? 1000 : id;
                this._builderId = id;
            }
        }

        [JsonProperty(PropertyName = "user_msg_id")]
        public string MessageID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "user_msg_title")]
        public string MessageTitle
        {
            get;
            set;
        }
    }

    [Serializable]
    public class IOSApns
    {
        [JsonProperty(PropertyName = "badge")]
        public int Badge { get; set; }

        [JsonProperty(PropertyName = "sound")]
        public string Sound { get; set; }

        public void Create(int badge, string sound)
        {
            this.Badge = badge;
            this.Sound = sound;
        }
    }
}
