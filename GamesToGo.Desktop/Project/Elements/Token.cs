using System.Collections.Generic;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Token : ProjectElement, IHasPrivacy
    {
        public override ElementType Type => ElementType.Token;
        public override Bindable<string> Name { get; } = new Bindable<string>(@"Nueva Ficha");

        public override Bindable<string> Description { get; } = new Bindable<string>(@"¡Describe esta ficha para poder identificarla mejor!");

        protected override string DefaultImageName => @"Token";

        public override Dictionary<string, Bindable<Image>> Images { get; } = new Dictionary<string, Bindable<Image>>(new[]
        {
            new KeyValuePair<string, Bindable<Image>>(@"Frente", new Bindable<Image>()),
            new KeyValuePair<string, Bindable<Image>>(@"Miniatura", new Bindable<Image>()),
        });
        public Bindable<ElementPrivacy> DefaultPrivacy { get; } = new Bindable<ElementPrivacy>();
    }
}
