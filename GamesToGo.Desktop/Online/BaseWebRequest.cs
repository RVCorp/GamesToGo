using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.IO.Network;

namespace GamesToGo.Desktop.Online
{
    public class BaseWebRequest : WebRequest
    {
        public BaseWebRequest(string uri)
            : base(uri)
        {
        }

        public BaseWebRequest()
        {
        }

        protected override string UserAgent => APIController.UserAgent;
    }
}
