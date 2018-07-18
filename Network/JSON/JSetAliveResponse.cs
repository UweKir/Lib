using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataManager.Network.JSON
{
    [JsonObject(MemberSerialization.OptOut)]
    public class JSetAliveResponse
    {
        /// <summary>
        /// Acknowlage flag
        /// </summary>
        public bool Ack { get; set; }

        /// <summary>
        /// When Alive was received
        /// </summary>
        public DateTime DateAck { get; set; }

    }
}
