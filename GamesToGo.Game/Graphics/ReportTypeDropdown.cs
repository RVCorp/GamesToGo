using GamesToGo.Game.Online.Models.RequestModel;

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
