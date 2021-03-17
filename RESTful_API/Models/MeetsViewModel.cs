using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RESTful_API.Models
{
    [DataContract(Name = "Meet")]
    public class MeetsViewModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "venue")]
        public string Venue { get; set; }
        [DataMember(Name = "date")]
        public System.DateTime Date { get; set; }
        [DataMember(Name = "pool_length")]
        public int PoolLength { get; set; }
        [DataMember(Name = "event_url")]
        public List<String> Events { get; set; }
    }
}