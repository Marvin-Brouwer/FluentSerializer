using FluentSerializer.Json.DataNodes;

using System.IO;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests;

internal readonly struct AllJsonNodes
{
	internal static IJsonObject GetInstance(bool format) => Object(
		format
			? Comment("object level comment")
			: MultilineComment("object level comment"),
		MultilineComment(
			"object level comment\n" +
			"With a new line"),
		Property("prop", Value($"\"Test\"")),
		Property("prop2", Object(
			Property("array", Array(
				format
					? Comment("array level comment")
					: MultilineComment("array level comment"),
                MultilineComment(
                    "array level comment\n" +
                    "With a new line"),
                Object(),
                Array()
            )),
            Property("prop3", Value("1")),
            Property("prop4", Value("true")),
            Property("prop5", Value(null))
        )),
		Property("prop6", Object(
			Property("prop7", Object()),
			Property("prop8", Object())
		)),
		Property("prop9", Array(
			Value("true"),
			Value("\"value\""),
			Value("96"))
		)
	);

	internal static string GetJson(bool format) => format ?
		File.ReadAllText("./Tests/AllJsonNodes.Formatted.json") :
		File.ReadAllText("./Tests/AllJsonNodes.Slim.json");
}
