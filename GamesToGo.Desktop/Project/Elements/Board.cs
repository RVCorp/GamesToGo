using System.Collections.Generic;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Board : ProjectElement
    {
        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nuevo Tablero");

        public override Dictionary<string, Image> Images => new Dictionary<string, Image>(new KeyValuePair<string, Image>[]
        {
            new KeyValuePair<string, Image>("Fondo", null),
        });

        public override string ToSaveable()
        {
            return "1|" + base.ToSaveable();
        }
    }
}
