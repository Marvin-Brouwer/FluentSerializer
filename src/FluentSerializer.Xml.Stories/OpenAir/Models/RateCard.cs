using FluentSerializer.Xml.Stories.OpenAir;

namespace FluentSerializer.Xml.Tests
{
    internal sealed class RateCard : IOpenAirEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
