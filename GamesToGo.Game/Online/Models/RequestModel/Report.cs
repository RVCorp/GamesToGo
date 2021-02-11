using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Common.Online.RequestModel;

namespace GamesToGo.Game.Online.Models.RequestModel
{
    public class Report
    {
        public int Id { get; set; }
        public string Reason { get; set; }

        public DateTime TimeReported { get; set; }
        public virtual OnlineGame Game { get; set; }
        public virtual ReportType ReportType { get; set; }
        public virtual User User { get; set; }
    }
}
