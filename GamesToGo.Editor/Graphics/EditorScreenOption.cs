using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace GamesToGo.Editor.Graphics
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public enum EditorScreenOption
    {
        [Description("Archivo")]
        Archivo,
        [Description("Inicio")]
        Inicio,
        [Description("Objetos")]
        Objetos,
        [Description("Eventos")]
        Eventos,
    }
}
