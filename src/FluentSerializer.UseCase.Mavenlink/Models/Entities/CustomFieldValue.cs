using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.UseCase.Mavenlink.Models.Entities;

internal sealed class CustomFieldValue : IMavenlinkEntity
{
	public string Id { get; init; } = default!;

	/// <summary>
	/// If <see cref="Type"/> is of value 'string', this is a string. If it is of value 'single', it is an array of id's
	/// </summary>
	public IJsonValue? Value { get; init; }
	public string DisplayValue { get; init; } = default!;
	public string Type { get; init; } = default!;

	/// <summary>
	/// MavenlinkId of the field definition
	/// </summary>
	public string CustomFieldId { get; init; } = default!;
	public string CustomFieldName { get; init; } = default!;

	/// <summary>
	/// MavenlinkId of the associated object.
	/// </summary>
	public string SubjectId { get; init; } = default!;

	/// <summary>
	/// Type of associated object. 
	/// </summary>
	public CustomFieldSubjectType SubjectType { get; init; } = default!;
}