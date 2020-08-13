using System.ComponentModel;

namespace GamesToGo.Desktop.Project
{
    public enum ChatRecommendation
    {
        [Description("Ninguno")]
        None = 0,
        [Description("Chat de voz")]
        Voice = 1,
        [Description("Chat de texto")]
        Text = 2,
        [Description("Chat de voz y texto")]
        Both = Voice | Text,
        [Description("Frente a frente")]
        Presential = 4,
    }
}
