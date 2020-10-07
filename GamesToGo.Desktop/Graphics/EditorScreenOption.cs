using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace GamesToGo.Desktop.Graphics
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
