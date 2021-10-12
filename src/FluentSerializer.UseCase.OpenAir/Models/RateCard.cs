using System;

namespace FluentSerializer.UseCase.OpenAir.Models
{
    internal sealed class RateCard : IOpenAirEntity
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
