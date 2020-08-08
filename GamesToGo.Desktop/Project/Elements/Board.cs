using System.Collections.Generic;
using osu.Framework.Bindables;
using osuTK;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Board : ProjectElement, IHasSize, IHasElements<Tile>
    {
        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nuevo Tablero");
        public override Bindable<string> Description { get; set; } = new Bindable<string>("¡Describe este tablero para poder identificarlo mejor!");

        protected override string DefaultImageName => @"Board";

        public override Dictionary<string, Bindable<Image>> Images { get; } = new Dictionary<string, Bindable<Image>>(new KeyValuePair<string, Bindable<Image>>[]
        {
            new KeyValuePair<string, Bindable<Image>>("Fondo", new Bindable<Image>()),
        });

        public Bindable<Vector2> Size { get; } = new Bindable<Vector2>(new Vector2(1920, 1080));

        public List<Tile> Subelements { get; } = new List<Tile>();

        public Queue<int> PendingSubelements { get; } = new Queue<int>();

        public override string ToSaveableString()
        {
            return "3|" + base.ToSaveableString();
        }
    }
}
