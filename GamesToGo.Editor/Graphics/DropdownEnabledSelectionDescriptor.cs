using System;
using osu.Framework.Allocation;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class DropdownEnabledSelectionDescriptor<T> : ArgumentSelectionDescriptor where T : ArgumentDropdown
    {
        [Resolved]
        private ArgumentSelectorOverlay selectorOverlay { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            Action = () =>
            {
                T dropdown = (T)Activator.CreateInstance(typeof(T), (object)Current);
                dropdown.Position = ToSpaceOfOtherDrawable(new Vector2((Width - 4) / 2, DrawHeight), selectorOverlay);
                selectorOverlay.Show(dropdown);
            };
        }
    }
}
