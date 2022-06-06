using System.Collections.Generic;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;

namespace FluentSerializer.Core.Profiles;

/// <summary>
/// A profile representation of how to (de)serialize a Class to and from any serializable format.
/// </summary>
public interface ISerializerProfile<TConfiguration> //: ISerializerProfile
	where TConfiguration : ISerializerConfiguration
{
	/// <summary>
	/// Configures profile mappings
	/// </summary>
	[System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerStepThrough,
	 System.Diagnostics.DebuggerHidden]
	IReadOnlyList<IClassMap> Configure(in TConfiguration configuration);
}