using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataManager.Network.JSON
{
    [JsonObject(MemberSerialization.OptOut)]
    public class JDataValue
    {
        private String key = "UNKNOWN";
        private String value = "0";
        private String unity = "UNKNOWN";
        private String device = "UNKNOWN";
        private String article = "UNKNOWN";

        #region Properties

        public String Key
        {
            get { return key; }
            set { key = value; }
        }
        public String Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        public String Unity
        {
            get { return unity; }
            set { unity = value; }
        }
        public String Device
        {
            get { return device; }
            set { device = value; }
        }
        public String Article
        {
            get { return article; }
            set { article = value; }
        }

        #endregion

    }
}
