using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Database.Migrations;
using GamesToGo.Desktop.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Overlays
{
    class EventCreationOverlay : OverlayContainer
    {
        private FillFlowContainer<ActionDescriptor> actionFillFlow;
        private Box shadowBox;
        private Container contentContainer;

        public EventCreationOverlay()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        shadowBox = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black,
                            Alpha = 0
                        },
                        contentContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding(20),
                                    Spacing = new Vector2(20),
                                    Children = new Drawable[]
                                    {
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            Height = 50,
                                            Child = new FillFlowContainer
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Children = new Drawable[]
                                                {
                                                    new SpriteText
                                                    {
                                                        Text = "Nombre del evento:"
                                                    },
                                                    new BasicTextBox
                                                    {
                                                        Height = 30,
                                                        Width = 400,
                                                    }
                                                }
                                            },
                                        },
                                        new EventDescriptor{},
                                        new BasicScrollContainer
                                        {
                                            Height =800,
                                            Width = 900,
                                            ClampExtension = 30,
                                            Child = actionFillFlow = new FillFlowContainer<ActionDescriptor>
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                            }
                                        },
                                        new GamesToGoButton
                                        {
                                            Text = "Añadir accion",
                                            Height = 30,
                                            Width = 100,
                                            Action = addAction
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private void addAction()
        {
            actionFillFlow.Add(new ActionDescriptor());
        }

        protected override void PopIn()
        {
            shadowBox.FadeTo(0.5f, 250);
            contentContainer.FadeIn();
        }

        protected override void PopOut()
        {
            shadowBox.FadeOut(250, Easing.OutExpo);
            contentContainer.FadeOut();
        }
    }
}
