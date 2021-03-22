using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RESTful_API.Models
{
    [DataContract(Name = "Participant")]
    public class ParticipantViewModel
    {
        [DataMember(Name = "event")]
        public string @event { get; set; }
        [DataMember(Name = "swimmer")]
        public string children { get; set; }
        [DataMember(Name = "lane")]
        public int Lane { get; set; }
        [DataMember(Name = "time")]
        public TimeSpan? Time { get; set; }
    }
}