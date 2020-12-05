// ReSharper disable UnusedMember.Global
using System.ComponentModel;

namespace GamesToGo.Editor.Project
{
    public enum CommunityStatus
    {
        [Description("Guardado")]
        Saved,
        [Description("Subido")]
        Clouded,
        [Description("Probado")]
        Tested,
        [Description("Publicado")]
        Published,
    }
}
