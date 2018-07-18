using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataManager.Network.JSON
{
    [JsonObject(MemberSerialization.OptOut)]
    /// <summary>
    /// General message object
    /// </summary>
    public class JMessage
    {
        /// <summary>
        /// Sender of the message
        /// </summary>
        public String Sender { get; set; }

        /// <summary>
        /// Function in Innermessage
        /// </summary>
        public String Function { get; set; }

        /// <summary>
        /// Message 
        /// </summary>
        public String InnerMessage { get; set; }
    }
}
