using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils
{
    public class JSONHelper
    {
        public static string DeserializeAndMakePassword(string jsonstring)
        {
            var obj = JsonConvert.DeserializeObject<dynamic>(jsonstring);

            MakePasswordProperty(obj);

            return JsonConvert.SerializeObject(obj);
        }
        public static void MakePasswordProperty(dynamic obj)
        {
            if (obj is JObject jobject)
            {
                foreach (var property in jobject.Properties())
                {
                    if (property.Name.ToLower() == "password")
                    {
                        var passwordvalue = property.Value?.ToString();
                        if (!string.IsNullOrEmpty(passwordvalue))
                        {

                            var makedpassword = new string('*', passwordvalue.Length);
                            property.Value = makedpassword;
                        }
                    }


                }
            }
        }
    }
    public class JSONHelper<T>
    {
        readonly static JsonSerializerSettings serializationOptions = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore/*, DefaultValueHandling = DefaultValueHandling.Ignore*/ ,
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
        };
        readonly static JsonSerializerSettings deserializationOptions = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore/*, DefaultValueHandling = DefaultValueHandling.Ignore*/
        };

        /// <summary>
        /// Serialize model parameters into JSON text
        /// </summary>
        /// <param name="typedModel"></param>
        /// <returns></returns>
        public static string GetJSONStr(T typedModel)
        {
            var str = JsonConvert.SerializeObject(typedModel, Formatting.Indented, serializationOptions);
            return str;
        }

        /// <summary>
        /// Deserialize JSON text into model class
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T GetTypedModel(string jsonStr)
        {

            var typedModel = JsonConvert.DeserializeObject<T>(jsonStr, deserializationOptions);
            return typedModel;
        }
    }
}
