using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RESTful_API.Models
{
    [DataContract(Name = "Participant")]
    public class RoleModel
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "role")]
        public string Role { get; set; }
    }
}