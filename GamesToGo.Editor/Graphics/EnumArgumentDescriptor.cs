using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GamesToGo.Editor.Graphics
{
    public class EnumArgumentDescriptor<T> : DropdownEnabledSelectionDescriptor<EnumArgumentDropdown<T>> where T : Enum
    {
        private SpriteText text;

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectionContainer.Add(text = new SpriteText
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Font = new FontUsage(size: 25),
            });
            Current.BindValueChanged(v => text.Text = v.NewValue.HasValue ? ((T)(object)v.NewValue.Value).GetDescription() : string.Empty, true);
        }
    }

    public class EnumArgumentDropdown<T> : ArgumentDropdown where T : Enum
    {
        public EnumArgumentDropdown(Bindable<int?> target) : base(target)
        {
        }

        protected override IEnumerable<ArgumentItem> CreateItems()
        {
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                yield return new EnumItem(value);
            }
        }

        private class EnumItem : ArgumentItem
        {
            private readonly T value;
            public EnumItem(T value) : base(Convert.ToInt32(value))
            {
                this.value = value;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                Add(new SpriteText
                {
                    Text = value.GetDescription(),
                    Font = FontUsage.Default.With(size: ARGUMENT_HEIGHT),
                });
            }
        }
    }
}
