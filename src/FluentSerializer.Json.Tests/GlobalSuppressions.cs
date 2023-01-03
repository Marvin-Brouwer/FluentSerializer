// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores",
	Justification = "Underscores are common in test method naming", Scope = "module")]

[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider",
	Justification = "Not important",
	Scope = "member", Target = "~M:FluentSerializer.Json.Tests.Tests.Converting.Converters.ParsableConverterTests.Serialize_TryParse_ThrowsNotSupported")]
