using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace GamesToGo.Editor.Project
{
    [Flags]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum Tag : uint
    {
        [Category(TagCategory.Time)]
        [Description(@"20-30 mins.")]
        Tag1 = 1 << 0,
        [Category(TagCategory.Time)]
        [Description(@"10-20 mins.")]
        Tag2 = 1 << 1,
        [Category(TagCategory.Time)]
        [Description(@">30 mins.")]
        Tag3 = 1 << 2,
        [Category(TagCategory.Type)]
        [Description(@"Poker")]
        Tag4 = 1 << 3,
        Tag5 = 1 << 4,
        Tag6 = 1 << 5,
        Tag7 = 1 << 6,
        Tag8 = 1 << 7,
        Tag9 = 1 << 8,
        Tag10 = 1 << 9,
        Tag11 = 1 << 10,
        Tag12 = 1 << 11,
        Tag13 = 1 << 12,
        Tag14 = 1 << 13,
        Tag15 = 1 << 14,
    }

    public class CategoryAttribute : Attribute
    {
        public readonly TagCategory Category;

        public CategoryAttribute(TagCategory category)
        {
            Category = category;
        }
    }

    public enum TagCategory
    {
        [Description(@"Sin categoria")]
        None,
        [Description(@"Tipo de juego")]
        Type,
        [Description(@"Tiempo de juego")]
        Time,
    }

    public static class TagExtensions
    {
        public static TagCategory GetCategory(this Tag tag) => tag.GetType().GetField(tag.ToString())!
            .GetCustomAttribute<CategoryAttribute>()?.Category ?? TagCategory.None;

        public static Tag[] GetSetFlags(this Tag tag) => Enum.GetValues(typeof(Tag)).Cast<Tag>().Where(t => tag.HasFlag(t)).ToArray();
    }
}
