using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Board : IProjectElement
    {
        public int ID { get; set; }
        public Bindable<string> Name { get; set; } = new Bindable<string>("Nuevo Tablero");

        public Board()
        {

        }

        public Drawable Image(bool size)
        {
            return null;
        }

        public int CompareTo([AllowNull] IProjectElement other)
        {
            throw new NotImplementedException();
        }
    }
}
