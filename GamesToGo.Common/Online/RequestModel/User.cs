using Newtonsoft.Json;

namespace GamesToGo.Common.Online.RequestModel
{
    public class User
    {
        [JsonProperty("Id")]
        public int ID { get; set; }

        public string Username { get; set; }
    }
}
