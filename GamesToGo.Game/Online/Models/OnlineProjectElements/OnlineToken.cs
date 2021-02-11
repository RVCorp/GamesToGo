namespace GamesToGo.Game.Online.Models.OnlineProjectElements
{
    public class OnlineToken
    {
        public int TypeID { get; set; }

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
