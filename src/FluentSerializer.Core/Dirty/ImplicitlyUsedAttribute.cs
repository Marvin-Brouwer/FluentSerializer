using System;
using System.Xml.Serialization;

namespace FluentSerializer.Core.Dirty
{
    /// <summary>
    /// This attribute purely exists to indicate that this class is not an unused class
    /// The reason that we don't use the ReSharper version is that we can't assume people will be using ReSharper, hence we don't depend on their NuGet.
    /// This still works because libraries like ReSharper will check if the class an XmlElement which this attribute inherits from.
    /// Since <see cref="XmlElementAttribute"/> is a system type, no additional dependencies are required.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    [System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerStepThrough]
    public sealed class ImplicitlyUsedAttribute : XmlElementAttribute { }
}
