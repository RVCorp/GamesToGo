using System.Collections.Generic;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Token : ProjectElement
    {
        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nueva Ficha");

        public override Dictionary<string, Bindable<Image>> Images => new Dictionary<string, Bindable<Image>>(new KeyValuePair<string, Bindable<Image>>[]
        {
            new KeyValuePair<string, Bindable<Image>>("Frente", new Bindable<Image>()),
            new KeyValuePair<string, Bindable<Image>>("Miniatura", new Bindable<Image>()),
        });

        public override string ToSaveable()
        {
            return "0|" + base.ToSaveable();
        }
    }
}
