using System;
using System.Collections.Generic;
using GamesToGo.Common.Game;
using GamesToGo.Editor.Project;
using GamesToGo.Editor.Project.Arguments;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class ArgumentTypeListing : VisibilityContainer
    {
        private IReadOnlyList<Argument> possibilities;

        private FillFlowContainer<ArgumentTypeButton> list;

        private readonly Bindable<Argument> current = new Bindable<Argument>();
        private Container content;

        protected override bool StartHidden => true;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => State.Value == Visibility.Visible;

        protected override bool ReceivePositionalInputAtSubTree(Vector2 screenSpacePos) => State.Value == Visibility.Visible;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                content = new Container
                {
                    Size = Vector2.Zero,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Masking = true,
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Colour4.Black,
                                },
                                list = new FillFlowContainer<ArgumentTypeButton>
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                },
                            },
                        },
                    },
                },
            };

            var possibilitiesList = new List<Argument>();

            foreach (var type in WorkingProject.AvailableArguments.Values)
            {
                if(Activator.CreateInstance(type) is Argument defaultArgument)
                    possibilitiesList.Add(defaultArgument);
            }

            possibilities = possibilitiesList;
        }

        public void ShowFor(ArgumentReturnType type, Bindable<Argument> argument, Vector2 position)
        {
            current.UnbindAll();
            current.BindTo(argument);

            list.Clear();
            foreach (var arg in possibilities)
            {
                if(arg.Type == type)
                    list.Add(new ArgumentTypeButton(arg));
            }

            content.MoveTo(position);
            Show();
        }

        private void ChangeCurrentTo(Argument newArgument)
        {
            if(current.Value == null)
                throw new InvalidOperationException( @$"{nameof(current)} has not yet been initialized, no argument is current");

            current.Value = newArgument;
        }

        protected override void PopIn()
        {
            this.FadeIn();
        }

        protected override void PopOut()
        {
            this.FadeOut();
        }

        protected override bool Handle(UIEvent e)
        {
            switch (e)
            {
                case ClickEvent _:
                    Hide();
                    return false;
            }

            return true;
        }

        private class ArgumentTypeButton : GamesToGoButton
        {
            private readonly Container content = new Container
            {
                AutoSizeAxes = Axes.Both,
            };

            private readonly Argument type;

            [Resolved]
            private ArgumentTypeListing argumentList { get; set; }

            public ArgumentTypeButton(Argument type)
            {
                this.type = type;
            }

            protected override Container<Drawable> Content => content;

            [BackgroundDependencyLoader]
            private void load()
            {
                AddInternal(content);
                AutoSizeAxes = Axes.Both;
                BackgroundColour = Colour4.Transparent;
                HoverColour = new Colour4(55, 55, 55, 255);
                Action = () =>
                {
                    argumentList.ChangeCurrentTo(Activator.CreateInstance(type.GetType()) as Argument);
                    argumentList.Hide();
                };
                SpriteText.Text = string.Join(' ', type.Text);
            }

            protected override SpriteText CreateText()
            {
                var text = base.CreateText();

                text.Colour = Colour4.White;
                text.Font = new FontUsage(size: 28);
                text.Margin = new MarginPadding { Horizontal = 6.5f };

                return text;
            }
        }
    }
}
