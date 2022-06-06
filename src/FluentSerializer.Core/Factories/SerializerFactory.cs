namespace FluentSerializer.Core.Factories;

/// <summary>
/// The Statically accessible Factory class for any <see cref="ISerializerFactory{TSerializer, TConfiguration, TSerializerProfile}"/>
/// </summary>
public sealed class SerializerFactory
{
	private SerializerFactory() { }

	/// <summary>
	/// Get a <see cref="ISerializerFactory{TSerializer, TConfiguration, TSerializerProfile}"/> for your requested type
	/// </summary>

	public static readonly SerializerFactory For = new ();
}
