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
