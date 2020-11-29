using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online
{
    public class BaseWebRequest : WebRequest
    {
        public BaseWebRequest(string uri)
            : base(uri)
        {
        }

        protected override string UserAgent => APIController.UserAgent;
    }
}
