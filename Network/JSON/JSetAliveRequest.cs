using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataManager.Network.JSON
{
    [JsonObject(MemberSerialization.OptOut)]
    public class JSetAliveRequest
    {
        /// <summary>
        /// Sender Id of the Data line
        /// </summary>
        public DateTime DateAlive { get; set; }

    }
}
