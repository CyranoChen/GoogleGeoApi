using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace GoogleGeoApi.Web.Api
{
    [ServiceContract(Namespace = "GoogleGeoApi.Web.Api")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GeoList
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";

        [WebGet]
        [OperationContract]
        public List<State> States()
        {
            var list = new List<State>();

            list.Add(new State { Code = "SH", CountryCode = "CN", Name = "Shang Hai", GeoCode = "Shanghai" });
            list.Add(new State { Code = "BJ", CountryCode = "CN", Name = "Bei Jing", GeoCode = "Beijing" });

            return list;
        }

        [WebGet]
        [OperationContract]
        public List<Country> Countries()
        {
            var list = new List<Country>();

            list.Add(new Country { Code = "CN", Name = "China PR", GeoCode = "China" });
            list.Add(new Country { Code = "UK", Name = "Great Britain", GeoCode = "United Kingdom" });


            return list;
        }

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

        [DataContract]
        public class Country
        {
            [DataMember]
            public string Code { get; set; }
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public string GeoCode { get; set; }
        }
    }
}

