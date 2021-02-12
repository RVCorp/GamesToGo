using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Online.Requests;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online.Requests
{
    public class SearchTagRequest : APIRequest<List<OnlineGame>>
    {
        private uint tags;

        public SearchTagRequest(uint tags)
        {
            this.tags = tags;
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            req.Method = HttpMethod.Post;
            req.AddParameter("tags", @$"{tags}");
            return req;
        }

        protected override string Target => "Games/SearchTag";
    }
}
