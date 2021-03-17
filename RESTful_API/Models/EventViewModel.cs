using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RESTful_API.Models
{
    [DataContract(Name = "Event")]
    public class EventViewModel
    {
        [DataMember(Name = "meet_url")]
        public string Meet { get; set; }
        [DataMember(Name = "age_range")]
        public int AgeRange { get; set; }
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
        [DataMember(Name = "distance")]
        public int Distance { get; set; }
        [DataMember(Name = "stroke")]
        public string Stroke { get; set; }
        [DataMember(Name = "round")]
        public int Round { get; set; }
        [DataMember(Name = "start_time")]
        public System.TimeSpan StartTime { get; set; }
        [DataMember(Name = "end_time")]
        public System.TimeSpan EndTime { get; set; }
    }
}