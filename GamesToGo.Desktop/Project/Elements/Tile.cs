using System.Collections.Generic;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Tile : ProjectElement
    {
        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nueva casilla");
        public override Dictionary<string, Image> Images => new Dictionary<string, Image>(new KeyValuePair<string, Image>[]
        {
            new KeyValuePair<string, Image>("Frente", null)
        });

        public override string ToSaveable()
        {
            return "3|" + base.ToSaveable();
        }
    }
}
