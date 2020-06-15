using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Card : ProjectElement
    {
        public override Bindable<string> Name { get; set; } = new Bindable<string>("Nueva Carta");

        public override Dictionary<string, Image> Images => new Dictionary<string, Image>();

        public override string ToSaveable()
        {
            return "2|" + base.ToSaveable();
        }
    }
}
