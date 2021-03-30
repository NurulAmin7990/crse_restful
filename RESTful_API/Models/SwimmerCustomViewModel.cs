using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RESTful_API.Models
{
    [DataContract(Name = "my_record")]
    public class SwimmerCustomViewModel
    {
        [DataMember(Name = "my_name")]
        public string Name { get; set; }
        [DataMember(Name = "my_date_of_birth")]
        public DateTime DateOfBirth { get; set; }
        [DataMember(Name = "my_gender")]
        public string Gender { get; set; }
        [DataMember(Name = "my_events")]
        public List<EventViewModel> Events { get; set; }
        [DataMember(Name = "my_meets")]
        public List<MeetsViewModel> Meets { get; set; }
        [DataMember(Name = "my_races")]
        public List<ParticipantViewModel> Races { get; set; }
    }
}