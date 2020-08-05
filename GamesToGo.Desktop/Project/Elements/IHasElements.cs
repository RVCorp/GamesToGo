using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Elements
{
    public interface IHasElements<T> where T : ProjectElement
    {
        void AddElement(int ID);
    }
}
