using System;
using System.Linq;
using GamesToGo.Editor.Project.Arguments;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace GamesToGo.Editor.Graphics
{
    public class ArgumentDescriptor : Container
    {
        private readonly Argument model;
        private FillFlowContainer descriptionContainer;
        private Container selectionContainer;

        public ArgumentDescriptor(Argument model)
        {
            this.model = model;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Both;
            Masking = true;
            CornerRadius = 4;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.2f,
                },
                new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        descriptionContainer = new FillFlowContainer
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                        },
                        selectionContainer = new Container
                        {
                            AutoSizeAxes = Axes.Both,
                        },
                    },
                },
            };

            if (model is IHasResult resolved)
            {
                descriptionContainer.FadeOut();
                selectionContainer.FadeIn();
                selectionContainer.Add(getSelectorForType(resolved));
            }
            else
            {
                descriptionContainer.FadeIn();
                selectionContainer.FadeOut();

                for (int i = 0; i < model.ExpectedArguments.Length; i++)
                {
                    descriptionContainer.AddRange(new Drawable[]
                    {
                        new SpriteText
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Padding = new MarginPadding(4),
                            Text = model.Text[i],
                            Font = new FontUsage(size: 25),
                        },
                        new ArgumentChanger(model.ExpectedArguments[i], model.Arguments[i]),
                    });
                }

                if (model.ExpectedArguments.Length < model.Text.Length)
                {
                    descriptionContainer.Add(new SpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Padding = new MarginPadding(4),
                        Text = model.Text.Last(),
                        Font = new FontUsage(size: 25),
                    });
                }
            }
        }

        private ArgumentSelectionDescriptor getSelectorForType(IHasResult resolved)
        {
            ArgumentSelectionDescriptor selectionDescriptor = model.Type switch
            {
                ArgumentType.Privacy => new EnumArgumentDescriptor<ElementPrivacy>(),
                ArgumentType.Orientation => new EnumArgumentDescriptor<ElementOrientation>(),
                ArgumentType.SingleNumber => new NumberSelectionDescriptor(),
                ArgumentType.CardType => new ElementSelectionDescriptor<Card>(),
                ArgumentType.TileType => new ElementSelectionDescriptor<Tile>(),
                ArgumentType.TokenType => new ElementSelectionDescriptor<Token>(),
                ArgumentType.BoardType => new ElementSelectionDescriptor<Board>(),
                _ => throw new ArgumentOutOfRangeException(nameof(model.Type), model.Type,
                    "Can't create selector for given model, as no selection is available"),
            };

            selectionDescriptor.Current = resolved.Result;

            return selectionDescriptor;
        }
    }
}
