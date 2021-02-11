// ReSharper disable UnusedMember.Global

using System.ComponentModel;

namespace GamesToGo.Common.Game
{
    public enum CommunityStatus
    {
        [Description(@"Guardado")]
        Saved,
        [Description(@"Subido")]
        Clouded,
        [Description(@"Probado")]
        Tested,
        [Description(@"Publicado")]
        Published,
    }
}
