using System;
using System.Collections.Generic;
using System.Linq;
using GamesToGo.Editor.Project;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class ElementSelectionDescriptor<T> : DropdownEnabledSelectionDescriptor<ElementSelectionDropdown<T>> where T : ProjectElement
    {
        private SpriteText text;
        private Sprite image;

        [Resolved]
        private WorkingProject project { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectionContainer.Add(new FillFlowContainer
            {
                AutoSizeAxes = Axes.X,
                Direction = FillDirection.Horizontal,
                Height = 25,
                Spacing = new Vector2(4),
                Children = new Drawable[]
                {
                    new Container
                    {
                        Size = new Vector2(25),
                        Child = image = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            FillMode = FillMode.Fit,
                        },
                    },
                    text = new SpriteText
                    {
                        Font = FontUsage.Default.With(size: 25),
                    },
                },
            });

            Current.BindValueChanged(v =>
            {
                ProjectElement element = project.ProjectElements.FirstOrDefault(e => e.ID == v.NewValue);

                if (!(element is T))
                {
                    if (v.NewValue != null)
                        throw new ArgumentException(
                            $"An argument looking for an element had it's result set to a non existent element or an argument type different to {typeof(T)}",
                            nameof(v.NewValue));

                    return;
                }
                text.Text = element.Name?.Value ?? string.Empty;
                image.Texture = element.GetImageWithFallback().Texture;
            }, true);
        }
    }

    public class ElementSelectionDropdown<T> : ArgumentDropdown where T : ProjectElement
    {
        [Resolved]
        private WorkingProject project { get; set; }

        public ElementSelectionDropdown(Bindable<int?> target) : base(target)
        {
        }

        protected override IEnumerable<ArgumentItem> CreateItems()
        {
            IEnumerable<ProjectElement> elements = Array.Empty<ProjectElement>();

            if (typeof(T) == typeof(Card))
                elements = project.ProjectCards;

            if (typeof(T) == typeof(Token))
                elements = project.ProjectTokens;

            if (typeof(T) == typeof(Board))
                elements = project.ProjectBoards;

            if (typeof(T) == typeof(Tile))
                elements = project.ProjectTiles;

            foreach (var element in elements)
            {
                yield return new ElementItem(element);
            }
        }

        private class ElementItem : ArgumentItem
        {
            private readonly ProjectElement value;
            private Sprite image;

            public ElementItem(ProjectElement value) : base(value.ID)
            {
                this.value = value;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                Add(new FillFlowContainer
                {
                    AutoSizeAxes = Axes.X,
                    Direction = FillDirection.Horizontal,
                    Height = ARGUMENT_HEIGHT,
                    Spacing = new Vector2(4),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Size = new Vector2(ARGUMENT_HEIGHT),
                            Child = image = new Sprite
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativeSizeAxes = Axes.Both,
                                FillMode = FillMode.Fit,
                            },
                        },
                        new SpriteText
                        {
                            Text = value.Name.Value,
                            Font = FontUsage.Default.With(size: ARGUMENT_HEIGHT),
                        },
                    },
                });

                image.Texture = value.GetImageWithFallback().Texture;
            }
        }
    }
}
