using Domain.Models.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Application.Utils
{
    public class URLDecoder
    {
       public static async Task<GeneralFilterDto> DecodeURL(HttpRequestMessage httprequest)
       {
            try
            {
                Uri requestUrl = httprequest.RequestUri;

                //Uri uri = new Uri(requestUrl);

                var queryParams = HttpUtility.ParseQueryString(requestUrl.Query).ToString();

                //var parames = queryParams.ToString();

                var pairs = queryParams.Split('&');

                // Create a dictionary to hold the key-value pairs
                var dictionary = new Dictionary<string, string>();

                foreach (var pair in pairs)
                {
                    // Split each pair by '='
                    var keyValue = pair.Split('=');
                    if (keyValue.Length == 2)
                    {
                        // Add to dictionary
                        dictionary[keyValue[0]] = keyValue[1];
                    }
                }

                string jsonString = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
                //Console.WriteLine(jsonString);
                return JsonConvert.DeserializeObject<GeneralFilterDto>(jsonString);

            }
            catch(Exception ex)
            {
                return new GeneralFilterDto()
                {

                };
            }
       }
    }
}
