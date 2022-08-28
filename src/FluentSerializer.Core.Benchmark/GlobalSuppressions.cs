// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1822:Mark members as static",
	Justification = "BenchmarkDotNet", Scope = "module")]

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores",
	Justification = "Underscores are part of profile naming", Scope = "module")]
