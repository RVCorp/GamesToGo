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
        public UserType UserType { get; set; }

        public string Image { get; set; }

    }

    public class PasswordedUser : User
    {
        public string Password { get; set; }
    }

    public enum UserType
    {
        User = 1,
        Admin = 2,
    }
}
