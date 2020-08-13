using System;
using GamesToGo.Desktop.Project.Events;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
{
    public interface IHasEvents
    {
        BindableList<Event> Events { get; }

        Type[] CompatibleEvents { get; }
    }
}
