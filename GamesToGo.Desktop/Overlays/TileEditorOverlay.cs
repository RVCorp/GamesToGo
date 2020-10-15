﻿using System.Globalization;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Desktop.Overlays
{
    public class TileEditorOverlay : OverlayContainer
    {
        [Resolved]
        private ProjectEditor editor { get; set; }
        private ProjectElement oldCurrentEditing;
        public ProjectElement SelectedTile;

        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new GamesToGoButton
                {
                    Height = 35,
                    Width = 175,
                    Text = @"Regresar",
                    Position = new Vector2(10,10),
                    Action = () => editor.SelectElement(oldCurrentEditing),
                },
            };
        }

        public void ShowElement(ProjectElement element)
        {
            oldCurrentEditing = editor.CurrentEditingElement.Value;
            SelectedTile = element;
            Show();
        }

        protected override void PopIn()
        {
            this.FadeIn();
        }

        protected override void PopOut()
        {
            this.FadeOut();
        }
    }
}
