using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Screens
{
    public class EmptyScreen : Screen
    {
        private readonly FillFlowContainer nextScreensContainer;
        private readonly SpriteText screenText;
        private readonly BasicButton backButton;

        private Action gameExitAction;

        protected virtual IEnumerable<Type> FollowingScreens => null;

        public EmptyScreen()
        {
            InternalChildren = new Drawable[]
            {
                backButton = new BasicButton
                {
                    AutoSizeAxes = Axes.X,
                    Height = 50,
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                },
                screenText = new SpriteText
                {
                    Text = GetType().Name,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    X = 1f / 16,
                },
                nextScreensContainer = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    AutoSizeAxes = Axes.Y,
                }
            };

            if(FollowingScreens != null)
            {
                foreach(var screen in FollowingScreens)
                {
                    nextScreensContainer.Add(new BasicButton
                    {
                        AutoSizeAxes = Axes.X,
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        Height = 50,
                        Text = $@"{screen.Name}",
                        BackgroundColour = getColorFor(screen.Name),
                        HoverColour = getColorFor(screen.Name).Lighten(0.2f),
                        Action = delegate { this.Push(Activator.CreateInstance(screen) as Screen); }
                    });
                }
            }
        }

        [BackgroundDependencyLoader]
        private void load(GamesToGoEditor editor)
        {
            gameExitAction = editor.Exit;
        }

        public override bool OnExiting(IScreen next)
        {
            screenText.MoveToX(1f / 16, 1000, Easing.OutExpo);
            this.FadeOut(1000, Easing.OutExpo);

            return base.OnExiting(next);
        }

        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);

            screenText.MoveToX(-1f / 16, 1000, Easing.OutExpo);
            this.FadeOut(1000, Easing.OutExpo);
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            screenText.MoveToX(0f, 1000, Easing.OutExpo);
            this.FadeIn(1000, Easing.OutExpo);
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            screenText.MoveToX(0f, 1000, Easing.OutExpo);
            this.FadeInFromZero(1000, Easing.OutExpo);

            backButton.Text = last?.GetType().Name ?? "Exit";
            backButton.BackgroundColour = last == null ? Color4.IndianRed : getColorFor(last.GetType().Name);
            backButton.Action = last == null ? gameExitAction : this.Exit;
        }

        private static Color4 getColorFor(object type)
        {
            int hash = type.GetHashCode();
            byte r = (byte)Math.Clamp(((hash & 0xFF0000) >> 16) * 0.8f, 20, 255);
            byte g = (byte)Math.Clamp(((hash & 0x00FF00) >> 8) * 0.8f, 20, 255);
            byte b = (byte)Math.Clamp((hash & 0x0000FF) * 0.8f, 20, 255);
            return new Color4(r, g, b, 255);
        }

    }
}
