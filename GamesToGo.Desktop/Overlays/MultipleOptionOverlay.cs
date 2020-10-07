using System;
using System.Collections.Generic;
using GamesToGo.Desktop.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Desktop.Overlays
{
    public class MultipleOptionOverlay : OverlayContainer
    {
        private TextFlowContainer upperText;
        private Box shadowBox;
        private Container popUpContent;
        private FillFlowContainer<OptionButton> options;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                shadowBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black,
                    Alpha = 0,
                },
                popUpContent = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Width = 1/3f,
                    BorderColour = new Colour4(70, 68, 66, 255),
                    BorderThickness = 4,
                    Masking = true,
                    CornerRadius = 15,
                    Scale = Vector2.Zero,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(106, 100, 104, 255),
                        },
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Y,
                            RelativeSizeAxes = Axes.X,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(25),
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Padding = new MarginPadding(50),
                            Children = new Drawable[]
                            {
                                new SpriteText
                                {
                                    Font = new FontUsage(size: 60),
                                    Text = @"¡Cuidado!",
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                },
                                upperText = new TextFlowContainer(font => { font.Font = new FontUsage(size: 40); })
                                {
                                    AutoSizeAxes = Axes.Y,
                                    RelativeSizeAxes = Axes.X,
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Padding = new MarginPadding { Horizontal = 20 },
                                    TextAnchor = Anchor.TopCentre,
                                },
                                options = new FillFlowContainer<OptionButton>
                                {
                                    Direction = FillDirection.Vertical,
                                    AutoSizeAxes = Axes.Y,
                                    RelativeSizeAxes = Axes.X,
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Spacing = Vector2.Zero,
                                },
                            },
                        },
                    },
                },
            };
        }

        private void recreateItems(IEnumerable<OptionItem> items)
        {
            options.Clear();
            foreach (var item in items)
            {
                Action closeAction = Hide;
                closeAction += item.Action;
                var toAddButton = new OptionButton(item, closeAction)
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 80,
                };
                toAddButton.Enabled.Value = false;
                toAddButton.Action += Hide;
                options.Add(toAddButton);
            }
        }

        public void Show(string textUpper, IEnumerable<OptionItem> items)
        {
            if (shadowBox.LatestTransformEndTime > Clock.CurrentTime)
                Scheduler.AddDelayed(() => show(textUpper, items), shadowBox.LatestTransformEndTime - Clock.CurrentTime);
            else
                show(textUpper, items);
        }

        private void show(string textUpper, IEnumerable<OptionItem> items)
        {
            upperText.Text = textUpper;
            recreateItems(items);
            Show();
        }

        protected override void PopIn()
        {
            shadowBox.FadeTo(0.5f, 250);
            popUpContent.Delay(150)
                .ScaleTo(1, 550, Easing.OutExpo)
                .OnComplete(_ =>
                {
                    foreach (var button in options)
                    {
                        button.Enabled.Value = true;
                    }
                });
        }

        protected override void PopOut()
        {
            foreach (var button in options)
            {
                button.Enabled.Value = false;
            }
            shadowBox.FadeOut(250, Easing.OutExpo);
            popUpContent.ScaleTo(0, 250, Easing.OutExpo);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            return true;
        }

        protected override bool OnClick(ClickEvent e)
        {
            return true;
        }
    }

    public struct OptionItem
    {
        public string Text;
        public Action Action;
        public OptionType Type;
    }

    public enum OptionType
    {
        Destructive,
        Neutral,
        Additive,
    }
}
