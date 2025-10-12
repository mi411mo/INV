using Newtonsoft.Json;

namespace Domain.Models.Base
{
    public class BaseRequest
    {
        [JsonProperty(Required = Required.Default)]

        public string CustomerId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string Access_Token { get; set; }
    }
}
