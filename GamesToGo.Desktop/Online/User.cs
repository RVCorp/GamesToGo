using Newtonsoft.Json;

namespace GamesToGo.Desktop.Online
{
    public class User
    {
        [JsonProperty(@"Id")]
        public int ID { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [JsonProperty(@"UsertypeId")]
        public int UserTypeID { get; set; }

        public string Image { get; set; }

    }
}
