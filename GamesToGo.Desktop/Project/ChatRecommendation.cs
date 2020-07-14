using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project
{
    public enum ChatRecommendation
    {
        None = 0,
        Voice = 1,
        Text = 2,
        Both = Voice | Text,
        Presential = 4,
    }
}
