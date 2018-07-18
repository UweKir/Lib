using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataManager.Network.JSON
{
    [JsonObject(MemberSerialization.OptOut)]
    public class JEnterDataLineReply
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public String ErrorText { get; set; }
    }
}
