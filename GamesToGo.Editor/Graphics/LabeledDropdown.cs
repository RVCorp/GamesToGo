using System;
using System.Linq;
using System.Reflection;

namespace GamesToGo.Editor.Graphics
{
    public class LabeledDropdown<TEnum> : LabeledElement<GamesToGoDropdown<TEnum>, TEnum> where TEnum : Enum
    {
        public new GamesToGoDropdown<TEnum> Element
        {
            init
            {
                var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
                value.Items = values.Except(values.Where(isIgnored));
                base.Element = value;
            }
        }

        private static bool isIgnored(TEnum value)
        {
            return value.GetType().GetField(value.ToString())!
                .GetCustomAttribute<IgnoreItemAttribute>() != null;
        }
    }
}
