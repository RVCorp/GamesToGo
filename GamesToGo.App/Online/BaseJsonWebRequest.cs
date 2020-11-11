using osu.Framework.IO.Network;

namespace GamesToGo.App.Online
{
    public class BaseJsonWebRequest<T> : JsonWebRequest<T>
    {
        public BaseJsonWebRequest(string uri)
            : base(uri)
        {
        }

        protected BaseJsonWebRequest()
        {
        }

        protected override string UserAgent => APIController.UserAgent;
    }
}
