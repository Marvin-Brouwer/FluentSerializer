// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using FluentSerializer.Json.DataNodes;

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
	Justification = "This refers to an Enum", Scope = "member", Target = "~M:FluentSerializer.Json.Converting.IUseJsonConverters.Enum~FluentSerializer.Json.Converting.IJsonConverter")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
	Justification = "This refers to an Enum", Scope = "member", Target = "~M:FluentSerializer.Json.Converting.UseJsonConverters.Enum~FluentSerializer.Json.Converting.IJsonConverter")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
	Justification = "This refers to an Enum", Scope = "member", Target = "~M:FluentSerializer.Json.Converting.IUseJsonConverters.Enum(FluentSerializer.Core.Converting.Converters.EnumFormats,System.Boolean)~System.Func{FluentSerializer.Json.Converting.IJsonConverter}")]

[assembly: SuppressMessage("Naming", "CA1720:Identifier contains type name",
	Justification = "This refers to a JsonObject", Scope = "member", Target = "~M:FluentSerializer.Json.JsonBuilder.Object(FluentSerializer.Json.DataNodes.IJsonObjectContent[])~FluentSerializer.Json.DataNodes.IJsonObject")]
[assembly: SuppressMessage("Naming", "CA1720:Identifier contains type name",
	Justification = "This refers to a JsonObject", Scope = "member", Target = "~M:FluentSerializer.Json.JsonBuilder.Object(System.Collections.Generic.IEnumerable{FluentSerializer.Json.DataNodes.IJsonObjectContent}@)~FluentSerializer.Json.DataNodes.IJsonObject")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
	Justification = "This refers to a JsonProperty", Scope = "member", Target = "~M:FluentSerializer.Json.Profiles.IJsonProfileBuilder`1.Property``1(System.Linq.Expressions.Expression{System.Func{`0,``0}}@,FluentSerializer.Core.Configuration.SerializerDirection@,System.Func{FluentSerializer.Core.Naming.NamingStrategies.INamingStrategy}@,System.Func{FluentSerializer.Json.Converting.IJsonConverter}@)~FluentSerializer.Json.Profiles.IJsonProfileBuilder{`0}")]

[assembly: SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions",
	Justification = "We're building highly optimized code", Scope = "member", Target = "~M:FluentSerializer.Json.DataNodes.Nodes.JsonObject.GetProperty(System.String@)~FluentSerializer.Json.DataNodes.IJsonProperty")]