using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Game.Online
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
