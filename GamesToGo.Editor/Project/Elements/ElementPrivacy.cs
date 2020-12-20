// ReSharper disable UnusedMember.Global

using System.ComponentModel;

namespace GamesToGo.Editor.Project.Elements
{
    public enum ElementPrivacy
    {
        [Description(@"Publico")]
        Public,
        [Description(@"Privado")]
        Private,
        Invisible,
    }
}
