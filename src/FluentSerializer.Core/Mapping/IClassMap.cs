using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;

namespace FluentSerializer.Core.Mapping;

/// <summary>
/// A set of poperties describing how a Class should be mapped to a serializable representation
/// </summary>
public interface IClassMap : IPropertyMapCollection
{
	/// <summary>
	/// The <see cref="INamingStrategy"/> attributed to this class
	/// </summary>
	INamingStrategy NamingStrategy { get; }

	/// <summary>
	/// The <see cref="IPropertyMapCollection"/> attributed to this class
	/// </summary>
	/// <remarks>
	/// Please prefer the <see cref="IPropertyMapCollection"/>s methods forwarded on the <see cref="IClassMap"/>
	/// instead of using this collection directly
	/// </remarks>
	IPropertyMapCollection PropertyMapCollection { get; }

	/// <summary>
	/// The actual top level instance type of this class
	/// </summary>
	Type ClassType { get; }

	/// <summary>
	/// The <see cref="SerializerDirection"/> this mapping is allowed to be used in
	/// </summary>
	SerializerDirection Direction { get; }
}