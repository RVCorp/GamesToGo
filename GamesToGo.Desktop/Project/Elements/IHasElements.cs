using System.Collections.Generic;

namespace GamesToGo.Desktop.Project.Elements
{
    public interface IHasElements
    {
        public void QueueSubelement(int ID)
        {
            PendingSubelements.Enqueue(ID);
        }

        public Queue<int> PendingSubelements { get; }
    }

    public interface IHasElements<T> : IHasElements where T : ProjectElement
    {
        List<T> Subelements { get; }
    }
}
