using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using GoogleGeoApi.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        public IEnumerable<State> States()
        {
            var file_path = ConfigurationManager.AppSettings["JsonFilePath"] + "state.json";

            using (var sr = new StreamReader(file_path))
            {
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                //serializer.NullValueHandling = NullValueHandling.Ignore;

                //构建Json.net的读取流  
                JsonReader reader = new JsonTextReader(sr);

                //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                var list = serializer.Deserialize<List<State>>(reader);

                return list.OrderBy(x => x.CountryCode).ThenBy(x => x.Code);
            }
        }

        [WebGet]
        [OperationContract]
        public IEnumerable<Country> Countries()
        {
            var file_path = ConfigurationManager.AppSettings["JsonFilePath"] + "country.json";

            using (var sr = new StreamReader(file_path))
            {
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                //serializer.NullValueHandling = NullValueHandling.Ignore;

                //构建Json.net的读取流  
                JsonReader reader = new JsonTextReader(sr);

                //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                var list = serializer.Deserialize<List<Country>>(reader);

                return list.OrderBy(x => x.Code);
            }
        }
    }
}

