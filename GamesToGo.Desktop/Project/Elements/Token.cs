using System.Collections.Generic;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Token : ProjectElement
    {
        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nueva Ficha");

        public override Dictionary<string, Image> Images => new Dictionary<string, Image>(new KeyValuePair<string, Image>[]
        {
            new KeyValuePair<string, Image>("Frente", null),
            new KeyValuePair<string, Image>("Miniatura", null),
        });

        public override string ToSaveable()
        {
            return "0|" + base.ToSaveable();
        }
    }
}
