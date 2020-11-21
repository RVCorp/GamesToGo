using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace GamesToGo.App.Graphics
{
    public class EmptyBox : Box
    {
        public EmptyBox()
        {
            Colour = Colour4.Black;
            Alpha = 0;
        }
    }
}
