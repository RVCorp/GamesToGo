using System.Collections.Generic;
using GamesToGo.Game.LocalGame.Elements;

namespace GamesToGo.Game.Online.Models.OnlineProjectElements
{
    public class OnlineCard
    {
        public int ID { get; set; }

        public int TypeID { get; set; }

        public ElementOrientation Orientation { get; set; }

        public ElementPrivacy Privacy { get; set; }

        public bool FrontVisible { get; set; }

        public List<OnlineToken> Tokens { get; set; } = new List<OnlineToken>();

        public override bool Equals(object obj)
        {
            if (!(obj is OnlineCard other))
                return false;

            bool tokenEquals = Tokens.Count == other.Tokens.Count;

            for (int i = 0; i < Tokens.Count && tokenEquals; i++)
                tokenEquals &= Tokens[i].Equals(other.Tokens[i]);

            return tokenEquals &&
                   ID == other.ID &&
                   TypeID == other.TypeID &&
                   Orientation == other.Orientation &&
                   FrontVisible == other.FrontVisible;
        }
    }
}
