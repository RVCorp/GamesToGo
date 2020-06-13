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

        public Dictionary<string, Image> Images => new Dictionary<string, Image>();

        public Board()
        {

        }

        public string ToSaveable()
        {
            return "";
        }
    }
}
