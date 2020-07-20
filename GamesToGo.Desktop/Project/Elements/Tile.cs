using System.Collections.Generic;
using osu.Framework.Bindables;
using osuTK;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Tile : ProjectElement, IHasSize, IHasEvents
    {
        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nueva casilla");

        protected override string DefaultImageName => @"Tile";

        public override Dictionary<string, Bindable<Image>> Images => new Dictionary<string, Bindable<Image>>(new KeyValuePair<string, Bindable<Image>>[]
        {
            new KeyValuePair<string, Bindable<Image>>("Frente", new Bindable<Image>())
        });

        public BindableList<int> Events { get; } = new BindableList<int>();

        public Bindable<Vector2> Size { get; } = new Bindable<Vector2>(new Vector2(400));

        public override string ToSaveable()
        {
            return "2|" + base.ToSaveable();
        }
    }
}
