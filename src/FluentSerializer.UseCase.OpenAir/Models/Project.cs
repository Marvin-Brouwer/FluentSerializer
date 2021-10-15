using System;
using FluentSerializer.UseCase.OpenAir.Models.Base;

namespace FluentSerializer.UseCase.OpenAir.Models
{
    internal sealed class Project : OpenAirEntity
    {
        public string? Name { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Active { get; set; }
        public string? RateCardId { get; set; }
        public DateTime? CustomDate { get; set; }
        public string? ExternalId { get; set; }
    }
}
