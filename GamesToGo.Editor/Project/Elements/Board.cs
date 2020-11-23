using System.Collections.Generic;
using osu.Framework.Bindables;
using osuTK;

namespace GamesToGo.Editor.Project.Elements
{
    public class Board : ProjectElement, IHasSize, IHasElements
    {
        public override ElementType Type => ElementType.Board;
        public override Bindable<string> Name { get; } = new Bindable<string>(@"Nuevo Tablero");
        public override Bindable<string> Description { get; } = new Bindable<string>(@"¡Describe este tablero para poder identificarlo mejor!");

        protected override string DefaultImageName => @"Board";

        public override Dictionary<string, Bindable<Image>> Images { get; } = new Dictionary<string, Bindable<Image>>(new[]
        {
            new KeyValuePair<string, Bindable<Image>>(@"Fondo", new Bindable<Image>()),
        });

        public Bindable<Vector2> Size { get; } = new Bindable<Vector2>(new Vector2(1920, 1080));

        public List<ProjectElement> Elements { get; } = new List<ProjectElement>();

        public Queue<int> PendingElements { get; } = new Queue<int>();

        public ElementType NestedElementType => ElementType.Tile;

        public override ElementPreviewMode PreviewMode => ElementPreviewMode.ChildrenOfSelf;
    }
}
