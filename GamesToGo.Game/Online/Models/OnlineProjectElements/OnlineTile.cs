using System;
using System.Collections.Generic;

namespace GamesToGo.Game.Online.Models.OnlineProjectElements
{
    public class OnlineTile
    {
         public int TypeID { get; set; }

         public List<OnlineToken> Tokens { get; set; } = new List<OnlineToken>();

         public List<OnlineCard> Cards { get; set; } = new List<OnlineCard>();

         public override bool Equals(object obj)
         {
             if (!(obj is OnlineTile other))
                 return false;

             bool tokensEqual = Tokens.Count == other.Tokens.Count;

             for (int i = 0; i < Tokens.Count && tokensEqual; i++)
                 tokensEqual &= Tokens[i].Equals(other.Tokens[i]);

             if (!tokensEqual)
                 return false;

             bool cardsEqual = Cards.Count == other.Cards.Count;

             for (int i = 0; i < Cards.Count && cardsEqual; i++)
                 cardsEqual &= Cards[i].Equals(other.Cards[i]);

             if (!cardsEqual)
                 return false;

             return TypeID == other.TypeID;
         }
    }
}
