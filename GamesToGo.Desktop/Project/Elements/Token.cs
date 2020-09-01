using System.Collections.Generic;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Token : ProjectElement
    {
        public override ElementType Type => ElementType.Token;

        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nueva Ficha");

        public override Bindable<string> Description { get; set; } = new Bindable<string>("¡Describe esta ficha para poder identificarla mejor!");

        protected override string DefaultImageName => @"Token";

        public override Dictionary<string, Bindable<Image>> Images { get; } = new Dictionary<string, Bindable<Image>>(new KeyValuePair<string, Bindable<Image>>[]
        {
            new KeyValuePair<string, Bindable<Image>>("Frente", new Bindable<Image>()),
            new KeyValuePair<string, Bindable<Image>>("Miniatura", new Bindable<Image>()),
        });

    }
}
