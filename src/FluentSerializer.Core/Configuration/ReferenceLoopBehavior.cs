namespace FluentSerializer.Core.Configuration;

/// <summary>
/// Defines serialization behavior when referenced recursively. <br />
/// The serialization behavior is not specified since JSON will never include these references so the data will just not be there.
/// </summary>
public enum ReferenceLoopBehavior
{
	/// <summary>
	/// [Default] Throw an error when trying to serialize a recursive reference
	/// </summary>
	Throw,
	/// <summary>
	/// Skip writing out the value when trying to serialize a recursive reference
	/// </summary>
	Ignore
}