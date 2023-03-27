using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Services.FileManager.Model
{
    [DataContract]
    public class Advertisement
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public int AuthorId { get; set; }
        [DataMember]
        public AdvAttachment[] Attachments { get; set; }

        public Advertisement()
        {

        }

        public Advertisement (Advertisement parent)
        {
            this.Id = parent.Id;
            this.Title = parent.Title;
            this.Description = parent.Description;
            this.Status = parent.Status;
            this.CategoryId = parent.CategoryId;
            this.AuthorId = parent.AuthorId;
            this.Attachments = parent.Attachments;
        }

    }
}
