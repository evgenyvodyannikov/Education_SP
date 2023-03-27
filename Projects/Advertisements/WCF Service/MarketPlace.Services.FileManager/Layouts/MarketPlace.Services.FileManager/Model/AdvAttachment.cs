using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Services.FileManager.Model
{
    [DataContract]
    public class AdvAttachment
    {

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int AdvertisementId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Data { get; set; }
        [DataMember]
        public string Url { get; set; }

        public AdvAttachment ()
        {

        }

        public AdvAttachment(int Id, string Name, string Url)
        {
            this.Id = Id;
            this.Name = Name;
            this.Url = Url;
        }
    }
}
