using System;
using System.Linq;

namespace GamesToGo.Desktop.Graphics
{
    public class LabeledDropdown<TEnum> : LabeledElement<GamesToGoDropdown<TEnum>, TEnum> where TEnum : Enum
    {
        public new GamesToGoDropdown<TEnum> Element
        {
            get => base.Element;
            set
            {
                value.Items = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
                base.Element = value;
            }
        }
    }
}
