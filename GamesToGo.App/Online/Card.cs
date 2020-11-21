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

namespace GamesToGo.App.Online
{
    public class Card
    {
        public int ID { get; set; }

        public int TypeID { get; set; }

        public int Orientation { get; set; }

        public bool FrontVisible { get; set; }

        public List<Token> Tokens { get; set; }
    }
}
