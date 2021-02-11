using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Common.Online.Requests;
using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class AvailableReportsRequest : APIRequest<List<ReportType>>
    {
        protected override string Target => "Reports/Available";
    }
}
