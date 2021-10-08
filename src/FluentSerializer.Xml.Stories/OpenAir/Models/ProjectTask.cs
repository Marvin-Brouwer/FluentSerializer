using FluentSerializer.Json.Stories.OpenAir;
using FluentSerializer.Xml.Stories.OpenAir.Converters;

namespace FluentSerializer.Json.Tests
{
    internal sealed class ProjectTask : IOpenAirEntity, ICustomXmlRoot
    {
        public string XmlRoot => "Projecttask";
        public int Id { get; set; }
    }
}
