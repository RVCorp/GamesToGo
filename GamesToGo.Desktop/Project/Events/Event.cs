using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public abstract class Event
    {
        public abstract EventType Type { get; set; }

        public int Priority;

        public List<EventAction> Actions;
    }
}
