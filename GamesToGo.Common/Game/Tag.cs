using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace GamesToGo.Common.Game
{
    [Flags]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum Tag : uint
    {
        [Category(TagCategory.Time)]
        [Description(@"<10 mins.")]
        Tag5 = 1 << 0,
        [Category(TagCategory.Time)]
        [Description(@"10-20 mins.")]
        Tag2 = 1 << 1,
        [Category(TagCategory.Time)]
        [Description(@"20-30 mins.")]
        Tag1 = 1 << 2,
        [Category(TagCategory.Time)]
        [Description(@">30 mins.")]
        Tag3 = 1 << 3,
        [Category(TagCategory.Type)]
        [Description(@"Cartas")]
        Tag4 = 1 << 4,
        [Category(TagCategory.Type)]
        [Description(@"Fichas")]
        Tag8 = 1 << 5,
        [Category(TagCategory.Type)]
        [Description(@"Rol")]
        Tag6 = 1 << 6,
        [Category(TagCategory.Type)]
        [Description(@"Tablero tradicional")]
        Tag7 = 1 << 7,
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
