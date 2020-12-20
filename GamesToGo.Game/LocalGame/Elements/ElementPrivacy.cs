// ReSharper disable UnusedMember.Global

using System.ComponentModel;

namespace GamesToGo.Game.LocalGame.Elements
{
    public enum ElementPrivacy
    {
        [Description("Publico")]
        Public,
        [Description("Privado")]
        Private,
        Invisible,
    }
}
