using System.Collections.Generic;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;

namespace FluentSerializer.Core.Profiles;

public interface ISerializerProfile
{
	[System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerStepThrough, 
	 System.Diagnostics.DebuggerHidden]
	IReadOnlyList<IClassMap> Configure(SerializerConfiguration configuration);
}