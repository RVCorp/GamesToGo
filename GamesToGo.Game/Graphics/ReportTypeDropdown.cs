using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Game.Online;

namespace GamesToGo.Game.Graphics
{
    public class ReportTypeDropdown : GamesToGoDropdown<ReportType>
    {
        protected override string GenerateItemText(ReportType item)
        {
            return item.Description;
        }
    }
}
