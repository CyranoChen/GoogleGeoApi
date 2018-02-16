using GoogleGeoApi.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace GoogleGeoApi.Web.Api
{
    /// <summary>
    /// Summary description for GeoCodeGenerator
    /// </summary>
    public class GeoCodeGenerator : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var response = "{}";

            if (!string.IsNullOrEmpty(context.Request.QueryString["type"]))
            {
                try
                {
                    var type = context.Request.QueryString["type"];

                    if (type.Equals(nameof(State), StringComparison.OrdinalIgnoreCase))
                    {
                        // Generate GeoCode of Model State
                        var list = new List<State>();

                        var file_path = ConfigurationManager.AppSettings["JsonFilePath"] + "state.json";

                        using (var sr = new StreamReader(file_path))
                        {
                            var serializer = new JsonSerializer();
                            //serializer.Converters.Add(new JavaScriptDateTimeConverter());
                            //serializer.NullValueHandling = NullValueHandling.Ignore;

                            //构建Json.net的读取流  
                            using (JsonReader reader = new JsonTextReader(sr))
                            {
                                //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                                list = serializer.Deserialize<List<State>>(reader);
                            }
                        }

                        if (list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                var code = GetGoogleGeoCode($"{item.Name}, {item.CountryCode}");
                                if (!string.IsNullOrEmpty(code))
                                {
                                    item.GeoCode = code;
                                }
                            }

                            using (StreamWriter sw = new StreamWriter(file_path))
                            {
                                var serializer = new JsonSerializer();
                                //serializer.Converters.Add(new JavaScriptDateTimeConverter());
                                //serializer.NullValueHandling = NullValueHandling.Ignore;

                                //构建Json.net的写入流  
                                using (JsonWriter writer = new JsonTextWriter(sw))
                                {
                                    //把模型数据序列化并写入Json.net的JsonWriter流中  
                                    serializer.Serialize(writer, list);
                                }
                            }
                        }

                        response = JsonConvert.SerializeObject(new { result = "success", message = $"total {list.Count} {type}" });
                    }

                    if (type.Equals(nameof(Country), StringComparison.OrdinalIgnoreCase))
                    {
                        // Generate GeoCode of Model Country
                        var list = new List<Country>();

                        var file_path = ConfigurationManager.AppSettings["JsonFilePath"] + "country.json";

                        using (var sr = new StreamReader(file_path))
                        {
                            var serializer = new JsonSerializer();
                            //serializer.Converters.Add(new JavaScriptDateTimeConverter());
                            //serializer.NullValueHandling = NullValueHandling.Ignore;

                            //构建Json.net的读取流  
                            using (JsonReader reader = new JsonTextReader(sr))
                            {
                                //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                                list = serializer.Deserialize<List<Country>>(reader);
                            }
                        }

                        if (list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                var code = GetGoogleGeoCode(item.Name);
                                if (!string.IsNullOrEmpty(code))
                                {
                                    item.GeoCode = code;
                                }
                            }

                            using (StreamWriter sw = new StreamWriter(file_path))
                            {
                                var serializer = new JsonSerializer();
                                //serializer.Converters.Add(new JavaScriptDateTimeConverter());
                                //serializer.NullValueHandling = NullValueHandling.Ignore;

                                //构建Json.net的写入流  
                                using (JsonWriter writer = new JsonTextWriter(sw))
                                {
                                    //把模型数据序列化并写入Json.net的JsonWriter流中  
                                    serializer.Serialize(writer, list);
                                }
                            }
                        }

                        response = JsonConvert.SerializeObject(new { result = "success", message = $"total {list.Count} {type}" });
                    }
                }
                catch (Exception ex)
                {
                    response = JsonConvert.SerializeObject(new { result = "error", message = ex.Message });
                }
            }

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.Write(response);
            context.Response.End();
        }

        private string GetGoogleGeoCode(string address)
        {
            const string GOOGLE_MAP_API_ADDRESS = "https://maps.googleapis.com/maps/api/geocode/json";
            const string API_KEY = "AIzaSyASawSYd14E234TX0YN7vhXOBjeIuswxsw";

            try
            {
                var result = string.Empty;

                var url = $"{GOOGLE_MAP_API_ADDRESS}?address={address.Replace(" ", "+")}&key={API_KEY}";

                var response = GetResponse(url, out string statusCode);

                var jsonObj = JObject.Parse(response);
                var array = (JArray)jsonObj.GetValue("results");

                if (array.Count > 0)
                {
                    return array[0]["formatted_address"].ToString(); ;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetResponse(string url, out string statusCode)
        {
            if (url.StartsWith("https")) { ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls; }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    statusCode = response.StatusCode.ToString();

                    return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : null;
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}