using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RESTful_API.Models
{
    [DataContract(Name = "Family")]
    public class FamilyViewModel
    {
        [DataMember(Name = "contact_number")]
        public string ContactNumber { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "address_line")]
        public string AddressLine { get; set; }
        [DataMember(Name = "address_area")]
        public string AddressArea { get; set; }
        [DataMember(Name = "address_postcode")]
        public string AddressPostcode { get; set; }
        [DataMember(Name = "children_url")]
        public List<string> Children { get; set; }
        [DataMember(Name = "parent_url")]
        public List<string> Parent { get; set; }
    }
}