using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public sealed class SerializerContext : ISerializerContext
    {
        public PropertyInfo Property { get; }
        public INamingStrategy NamingStrategy { get; }
        public IXmlSerializer CurrentSerializer { get; }

        public SerializerContext(PropertyInfo property, INamingStrategy namingStrategy, IXmlSerializer currentSerializer)
        {
            Property = property;
            NamingStrategy = namingStrategy;
            CurrentSerializer = currentSerializer;
        }
    }
}
