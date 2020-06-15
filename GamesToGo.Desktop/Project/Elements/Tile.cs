using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Tile : ProjectElement
    {
        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nueva casilla");
        public override Dictionary<string, Image> Images => new Dictionary<string, Image>();

        public override string ToSaveable()
        {
            return "3|" + base.ToSaveable();
        }
    }
}
