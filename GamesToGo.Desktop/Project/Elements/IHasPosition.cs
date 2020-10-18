using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Bindables;
using osuTK;

namespace GamesToGo.Desktop.Project.Elements
{
    interface IHasPosition
    {
        Bindable<Vector2> Position { get; }

        public string ToSaveable()
        {
            return $"Position={Position.Value.X}|{Position.Value.Y}";
        }
    }
}
