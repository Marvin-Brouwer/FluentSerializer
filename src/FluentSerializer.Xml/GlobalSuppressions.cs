// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
	Justification = "This refers to an Enum", Scope = "member", Target = "~M:FluentSerializer.Xml.Converting.IUseXmlConverters.Enum~FluentSerializer.Xml.Converting.IXmlConverter")]

[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
	Justification = "This refers to an XmlAttribute", Scope = "type", Target = "~T:FluentSerializer.Xml.DataNodes.IXmlAttribute")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
	Justification = "This refers to an XmlAttribute", Scope = "type", Target = "~T:FluentSerializer.Xml.DataNodes.Nodes.XmlAttribute")]

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
	Justification = "This refers to an XmlAttribute", Scope = "member", Target = "~M:FluentSerializer.Xml.Converting.IUseXmlConverters.Enum(FluentSerializer.Core.Converting.Converters.EnumFormats)~System.Func{FluentSerializer.Xml.Converting.IXmlConverter}")]
