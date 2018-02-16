using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GoogleGeoApi.Web.Models
{
    [DataContract]
    public class State
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string GeoCode { get; set; }
    }
}