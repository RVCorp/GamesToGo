using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.IO.Network;

namespace GamesToGo.Desktop.Online
{
    public class BaseJsonWebRequest<T> : JsonWebRequest<T>
    {
        public BaseJsonWebRequest(string uri)
            : base(uri)
        {
        }

        public BaseJsonWebRequest()
        {
        }

        protected override string UserAgent => APIController.UserAgent;
    }
}
