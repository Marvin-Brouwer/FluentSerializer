namespace FluentSerializer.Core.Configuration;

/// <summary>
/// Direction of serialization. <br />
/// This includes both constaining to a direction and context of what the current direction is.
/// </summary>
public enum SerializerDirection
{
	/// <summary>
	/// [Default] This serializer can work in both directions
	/// </summary>
	Both,
	/// <summary>
	/// This serializer can only serialize / The current action is serializing
	/// </summary>
	Serialize,
	/// <summary>
	/// This serializer can only deserialize / The current action is deserializing
	/// </summary>
	Deserialize
}