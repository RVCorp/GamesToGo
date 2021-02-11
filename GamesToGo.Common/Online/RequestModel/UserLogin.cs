namespace GamesToGo.Common.Online.RequestModel
{
    internal class UserLogin
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public User User { get; set; }
    }
}
