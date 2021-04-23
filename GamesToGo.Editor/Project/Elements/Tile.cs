using System.Collections.Generic;
using GamesToGo.Editor.Project.Events;
using osu.Framework.Bindables;
using osuTK;

namespace GamesToGo.Editor.Project.Elements
{
    public class Tile : ProjectElement, IHasSize, IHasEvents, IHasOrientation, IHasPosition, IHasLogicalArrangement
    {
        public override ElementType Type => ElementType.Tile;

        public override Bindable<string> Name { get; } = new Bindable<string>(@"Nueva casilla");

        public override Bindable<string> Description { get; } = new Bindable<string>(@"¡Describe esta casilla para poder identificarla mejor!");

        protected override string DefaultImageName => @"Tile";

        public override Dictionary<string, Bindable<Image>> Images { get; } = new Dictionary<string, Bindable<Image>>(new[]
        {
            new KeyValuePair<string, Bindable<Image>>(@"Frente", new Bindable<Image>()),
        });

        public BindableList<ProjectEvent> Events { get; } = new BindableList<ProjectEvent>();

        public Bindable<Vector2> Size { get; } = new Bindable<Vector2>(new Vector2(400));
        public Bindable<ElementOrientation> DefaultOrientation { get; } = new Bindable<ElementOrientation>();

        public Bindable<Vector2> Position { get; } = new Bindable<Vector2>(Vector2.Zero);

        public Bindable<Vector2> Arrangement { get; } = new Bindable<Vector2>(Vector2.Zero);

        public override ElementPreviewMode PreviewMode => ElementPreviewMode.ParentWithChildren;
    }
}
