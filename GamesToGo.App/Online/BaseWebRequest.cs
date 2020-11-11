using osu.Framework.IO.Network;

namespace GamesToGo.App.Online
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
