using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RESTful_API.Models
{
    [DataContract(Name = "Archive")]
    public class ArchiveParentViewModel
    {
        [DataMember(Name = "first_name")]
        public string Firstname { get; set; }
        [DataMember(Name = "last_name")]
        public string Lastname { get; set; }
        [DataMember(Name = "date_of_birth")]
        public System.DateTime DateOfBirth { get; set; }
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
    }
}