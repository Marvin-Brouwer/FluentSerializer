// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
	Justification = "This is a Stack", Scope = "type", Target = "~T:FluentSerializer.Core.Configuration.IConfigurationStack`1")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
	Justification = "This is a Stack", Scope = "type", Target = "~T:FluentSerializer.Core.Configuration.ConfigurationStack`1")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
	Justification = "This is a Collection", Scope = "type", Target = "~T:FluentSerializer.Core.Mapping.IClassMapCollection")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
	Justification = "This is a Collection", Scope = "type", Target = "~T:FluentSerializer.Core.Mapping.ClassMapCollection")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
	Justification = "This is a Collection", Scope = "type", Target = "~T:FluentSerializer.Core.Mapping.IPropertyMapCollection")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
	Justification = "This is a Collection", Scope = "type", Target = "~T:FluentSerializer.Core.Mapping.PropertyMapCollection")]

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
	Justification = "This refers to a Property", Scope = "member", Target = "~P:FluentSerializer.Core.Context.ISerializerContext.Property")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
	Justification = "This refers to a Property", Scope = "member", Target = "~P:FluentSerializer.Core.Mapping.IPropertyMap.Property")]

[assembly: SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions",
	Justification = "We're writing highly optimized code", Scope = "member", Target = "~M:FluentSerializer.Core.Converting.Converters.EnumConverterBase.GetEnumFromName(System.String@,System.Type@)~System.Object")]

[assembly: SuppressMessage("Warning", "S2436: Reduce the number of generic parameters in the 'ISerializerFactory' class to no more than the 2 authorized.",
	Justification = "This is necessary", Scope = "type", Target = "~T:FluentSerializer.Core.Factories.ISerializerFactory`3")]
[assembly: SuppressMessage("Warning", "S2436: Reduce the number of generic parameters in the 'IConfiguredSerializerFactory' class to no more than the 2 authorized.",
	Justification = "This is necessary", Scope = "type", Target = "~T:FluentSerializer.Core.Factories.IConfiguredSerializerFactory`3")]
[assembly: SuppressMessage("Warning", "S2436: Reduce the number of generic parameters in the 'BaseSerializerFactory' class to no more than the 2 authorized.",
	Justification = "This is necessary", Scope = "type", Target = "~T:FluentSerializer.Core.Factories.BaseSerializerFactory`3")]

[assembly: SuppressMessage("Warning", "S5766: Make sure not performing data validation after deserialization is safe here.",
	Justification = "There is no validation to begin with", Scope = "type", Target = "~T:FluentSerializer.Core.SerializerException.ConverterNotSupportedException")]

[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
	Justification = "This is a similar construction as Array.Empty<T>()",
	Scope = "type", Target = "~T:FluentSerializer.Core.DataNodes.SingleItemCollection")]
[assembly: SuppressMessage("Usage", "CA2201:Do not raise reserved exception types",
	Justification = "This is the correct exception type",
	Scope = "member", Target = "~P:FluentSerializer.Core.DataNodes.SingleItemCollection`1.Item(System.Int32)")]
