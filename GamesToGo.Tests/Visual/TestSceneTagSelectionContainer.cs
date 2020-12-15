﻿using System;
using System.Linq;
using GamesToGo.Editor.Graphics;
using GamesToGo.Editor.Project;
using GamesToGo.Editor.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;

namespace GamesToGo.Tests.Visual
{
    public class TestSceneTagSelectionContainer : TestScene
    {
        private TagSelectionContainer tags;
        private SpriteText valueText;
        private Bindable<Tag> current = new Bindable<Tag>();

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(tags = new TagSelectionContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.X,
                Height = ProjectHomeScreen.TEXT_ELEMENT_SIZE,
                Current = { BindTarget = current },
            });
            Add(valueText = new SpriteText
            {
                Text = $"Value: {current.Value}",
            });

            AddStep(@"Set value to 0", () => current.Value = (Tag)0);
            AddStep(@"Set value to All tags", () => current.Value = (Tag)Enum.GetValues(typeof(Tag)).Cast<Tag>().Sum(t => (uint)t));

            current.BindValueChanged(_ => valueText.Text = $"Value: {current.Value}");
        }
    }
}
