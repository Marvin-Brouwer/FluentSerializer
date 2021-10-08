using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FluentSerializer.Xml.Profiles
{
    public interface ISerializerContext
    {
        PropertyInfo Property { get; set; }
        INamingStrategy NamingStrategy { get; set; }
        IXmlSerializer CurrentSerializer { get; set; }
    }
}
