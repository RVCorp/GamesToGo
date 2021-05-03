using GamesToGo.Game.LocalGame.Elements;

namespace GamesToGo.Game.Online.Models.OnlineProjectElements
{
    public class OnlineToken
    {
        public int ID { get; set; }
        public int TypeID { get; set; }
        public ElementPrivacy Privacy { get; set; }
        public int Count { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is OnlineToken other))
                return false;

            return TypeID == other.TypeID &&
                   Count == other.Count;
        }
    }
}
