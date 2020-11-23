using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Newtonsoft.Json;

// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace GamesToGo.Editor.Online
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class OnlineProject
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string LastEdited
        {
            get => DateTimeLastEdited.ToString(@"yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
            set => DateTimeLastEdited = DateTime.ParseExact(value, @"yyyyMMddHHmmssfff", CultureInfo.InvariantCulture).ToLocalTime();
        }

        [JsonIgnore]
        public DateTime DateTimeLastEdited { get; private set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Description { get; set; }
        // TODO: Make this actually work
        public int Status { get; set; }
        public int Minplayers { get; set; }
        public int Maxplayers { get; set; }
        public int CreatorId { get; set; }
    }
}
