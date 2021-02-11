using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using GamesToGo.Common.Online.Requests;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online.Requests
{
    public class ReportGameRequest : APIRequest<string>
    {
        private string reason;
        private int gameID;
        private int reportType;
        public ReportGameRequest(string reason, int gameID, int reportType)
        {
            this.reason = reason;
            this.gameID = gameID;
            this.reportType = reportType;
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            req.Method = HttpMethod.Post;
            req.AddParameter("reason", @$"{reason}");
            req.AddParameter("gameID", @$"{gameID}");
            req.AddParameter("type", @$"{reportType}");
            return req;
        }
        protected override string Target => "Reports/ReportGame";
    }
}
