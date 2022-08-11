using System.Collections.Generic;
using System.Linq;
using FluentSerializer.UseCase.Mavenlink.Extensions;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;

namespace FluentSerializer.UseCase.Mavenlink.Models;

internal static class EntityMappings
{
	// These overrides are just examples of what a real world example may look like
	// Our code base doesn't contain any of these, otherwise nameof(WorkspaceStatus) would've been used.
	private static readonly Dictionary<string, string> SingularOverrides = new()
	{
		[nameof(Project)] = "workspace",
		["WorkspaceStatus"] = "workspace_status_change"
	};
	private static readonly Dictionary<string, string> PluralOverrides = new()
	{
		["Story"] = "stories"
	};

	internal static string GetDataItemName(in string entityName)
	{
		if (SingularOverrides.ContainsKey(entityName))
			return SingularOverrides[entityName];

		return ConvertName(in entityName);
	}

	internal static string GetDataGroupName(in string entityName)
	{
		if (PluralOverrides.ContainsKey(entityName))
			return PluralOverrides[entityName];

		return string.Concat(GetDataItemName(in entityName), 's');
	}

	internal static string GetDataReferenceName(in string entityName)
	{
		return string.Concat(GetDataItemName(in entityName), "_id");
	}

	internal static string GetDataReferenceGroupName(in string entityName)
	{
		return string.Concat(GetDataItemName(in entityName), "_ids");
	}

	private static string ConvertName(in string originalName)
	{
		const char separator = '_';

		return originalName
			.Select(character => char.IsUpper(character)
				? string.Concat(separator, character)
				: character.ToString())
			.Join(string.Empty)
			.TrimStart(separator)
			.ToLowerInvariant();
	}
}