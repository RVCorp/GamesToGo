using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Game.Graphics
{
    public class TagFlowContainer : FillFlowContainer<TagContainer> , IHasCurrentValue<uint>
    {
        public Bindable<uint> Current { get; set; } = new Bindable<uint>();

        public override void Add(TagContainer drawable)
        {
            drawable.IsSelected.BindValueChanged(v =>
            {
                if (v.NewValue)                
                    Current.Value += drawable.Value;                
                else
                    Current.Value -= drawable.Value;
            });
            base.Add(drawable);
        }
    }
}
