namespace GamesToGo.Desktop.Project.Elements
{
    public interface IHasPrivacy
    {
        ElementPrivacy DefaultPrivacy { get; set; }

        public string ToSaveable()
        {
            return $"Privacy={DefaultPrivacy}";
        }
    }
}
