using System;

namespace FluentSerializer.Xml.Stories.OpenAir.Models
{
    internal sealed class Project : IOpenAirEntity
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Active { get; set; }
        public int? RateCardId { get; set; }
        public DateTime? CustomDate { get; set; }
    }
}
