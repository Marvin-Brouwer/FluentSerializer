using FluentSerializer.Xml.Stories.OpenAir;

namespace FluentSerializer.Xml.Tests
{
    internal sealed class RateCard : IOpenAirEntity
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
    }
}
