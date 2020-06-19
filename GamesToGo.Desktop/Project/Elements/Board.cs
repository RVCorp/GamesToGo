using System.Collections.Generic;
using osu.Framework.Bindables;
using osuTK;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Board : ProjectElement, IHasSize
    {
        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nuevo Tablero");

        public override Dictionary<string, Bindable<Image>> Images => new Dictionary<string, Bindable<Image>>(new KeyValuePair<string, Bindable<Image>>[]
        {
            new KeyValuePair<string, Bindable<Image>>("Fondo", new Bindable<Image>()),
        });

        public Bindable<Vector2> Size { get; } = new Bindable<Vector2>();

        public override string ToSaveable()
        {
            return "3|" + base.ToSaveable();
        }
    }
}
