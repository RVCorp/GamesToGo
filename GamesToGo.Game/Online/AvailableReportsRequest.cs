using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Game.Online
{
    public class AvailableReportsRequest : APIRequest<List<ReportType>>
    {
        protected override string Target => "Reports/Available";
    }
}
