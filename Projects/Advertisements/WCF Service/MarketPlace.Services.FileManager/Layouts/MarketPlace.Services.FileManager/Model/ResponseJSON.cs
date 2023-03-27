using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Services.FileManager.Model
{
    [DataContract]
    public class ResponseJSON<T>
    {
        [DataMember]
        public string _metadata { get; set; }
        [DataMember]
        public T d { get; set; }

        public ResponseJSON (string _metadata, T Data)
        {
            this._metadata = _metadata;
            this.d = Data;
        }

        public ResponseJSON(T Data)
        {
            this.d = Data;
        }
    }
}
