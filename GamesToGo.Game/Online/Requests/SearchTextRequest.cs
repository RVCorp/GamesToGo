using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Online.Requests;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online.Requests
{
    public class SearchTextRequest : APIRequest<List<OnlineGame>>
    {
        private string text;

        public SearchTextRequest(string text)
        {
            this.text = text;
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            req.Method = HttpMethod.Post;
            req.AddParameter("text", @$"{text}");
            return req;
        }

        protected override string Target => "Games/SearchString";
    }
}
