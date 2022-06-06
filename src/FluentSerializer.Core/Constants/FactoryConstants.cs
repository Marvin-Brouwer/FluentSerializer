using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Core.Constants;

/// <summary>
/// Constant class for usage in Serializer factories
/// </summary>
public struct FactoryConstants
{
	/// <summary>
	/// The default <see cref="DefaultObjectPoolProvider"/> to use when none are configured
	/// </summary>
	public static readonly DefaultObjectPoolProvider DefaultObjectPoolProvider = new();
}
