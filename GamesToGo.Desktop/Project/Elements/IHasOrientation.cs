namespace GamesToGo.Desktop.Project.Elements
{
    public interface IHasOrientation
    {
        ElementOrientation DefaultOrientation { get; set; }

        public string ToSaveable()
        {
            return $"Orient={DefaultOrientation}";
        }

    }
}
