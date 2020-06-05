using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Card : IProjectElement
    {
        public int ID { get; set; }
        public Bindable<string> Name { get; set; } = new Bindable<string>("Nueva Carta");

        public Card()
        {

        }

        public Drawable Image(bool size)
        {
            throw new NotImplementedException();
        }
    }
}
