using System.Collections.Generic;
using osu.Framework.Bindables;
using osuTK;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Card : ProjectElement, IHasSize, IHasEvents
    {
        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nueva Carta");

        public override Dictionary<string, Bindable<Image>> Images => new Dictionary<string, Bindable<Image>>(new List<KeyValuePair<string, Bindable<Image>>>
        {
            new KeyValuePair<string, Bindable<Image>>("Frente", new Bindable<Image>()),
            new KeyValuePair<string, Bindable<Image>>("Posterior", new Bindable<Image>()),
        });

        public Bindable<Vector2> Size { get; } = new Bindable<Vector2>();

        public BindableList<int> Events { get; } = new BindableList<int>();

        public override string ToSaveable()
        {
            return "1|" + base.ToSaveable();
        }
    }
}
