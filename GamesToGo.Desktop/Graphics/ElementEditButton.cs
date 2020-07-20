using System.Linq;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using Image = GamesToGo.Desktop.Project.Image;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class ElementEditButton<T> : Button where T : ProjectElement
    {
        public readonly T Element;
        private readonly Container borderContainer;
        private readonly SpriteText elementName;

        private IBindable<string> elementText = new Bindable<string>();
        private IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();
        private Sprite image;

        private bool selected => (currentEditing.Value?.ID ?? -1) == Element.ID;
        public ElementEditButton(T element)
        {
            Element = element;
            Size = new Vector2(165);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Masking = true;
            Children = new Drawable[]
            {
                borderContainer = new Container
                {
                    Masking = true,
                    CornerRadius = 10,
                    BorderThickness = 2,
                    BorderColour = Color4.White,
                    RelativeSizeAxes = Axes.Both,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.1f
                    }
                },
                new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(120),
                    Position = new Vector2(0, 15),
                    Child = image = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        FillMode = FillMode.Fit,
                    }
                },
                elementName = new SpriteText
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Position = new Vector2(0, 135),
                    Text = element.Name.Value,
                    MaxWidth = 150,
                    Font = new FontUsage(size: 20),
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {
            Action = () => editor.SelectElement(Element);
            elementName.Current.BindTo(Element.Name);

            currentEditing.BindTo(editor.CurrentEditingElement);
            currentEditing.BindValueChanged((_) => editingChanged(), true);
            Element.Images.Values.First().BindValueChanged(imageChanged, true);
            borderContainer.Alpha = selected ? 1 : 0;
        }
        private void editingChanged()
        {
            borderContainer.Colour = selected ? Color4.Gold : Color4.White;
            borderContainer.FadeTo(selected ? 1 : IsHovered ? 1 : 0, 125);
        }

        private void imageChanged(ValueChangedEvent<Image> value)
        {
            image.Texture = value.NewValue?.Texture ?? Element.DefaultImage.Texture;
        }

        protected override bool OnHover(HoverEvent e)
        {
            borderContainer.FadeIn(125);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            if (!selected)
                borderContainer.FadeOut(125);
        }
    }
}
