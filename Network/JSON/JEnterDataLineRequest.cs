using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataManager.Network.JSON
{
 
    [JsonObject(MemberSerialization.OptOut)]
    public class JEnterDataLineRequest
    {
        public DateTime dtCreated;

        public string SourceFile { get; set; }

        public int LineNumber { get; set; }
        /// <summary>
        /// List of data columns in the dataline
        /// </summary>
        public List<JDataValue> DataColumns { get; set; }
    }
}
