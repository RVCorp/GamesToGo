using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace GamesToGo.Game.Online
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class User
    {
        [JsonProperty(@"Id")]
        public int ID { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [JsonProperty(@"UsertypeId")]
        public UserType UserType { get; set; }

        // ReSharper disable once UnusedMember.Global
        public string Image { get; set; }

    }

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PasswordedUser : User
    {
        public string Password { get; set; }
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum UserType
    {
        User = 1,
        Admin = 2,
    }
}
