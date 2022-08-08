using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSerializer.Core.Benchmark.Profiles.Experiments._81;

/// <summary>
/// This test is not correct at all, it wil result in <c><![CDATA[<SomeTag>SomeTextMoreText</SomeTag>]]></c>.
/// However, we only care about skipping the whitespace anyway.
/// </summary>
[MemoryDiagnoser]
public partial class AdjustForWhiteSpaceProfile
{
	private const string Text = "<SomeTag>\n  SomeText            MoreText\n</SomeTag>";

	[Benchmark(Baseline = true), BenchmarkCategory("ParseText")]
	public string ParseText()
	{
		var text = Text.AsSpan();
		var result = string.Empty;
		var offset = 0;

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharactersAtOffset(in offset, '/', '>'))
			{
				offset.AdjustForToken('/');
				break;
			}

			if (text.HasWhitespaceAtOffset(in offset))
			{
				offset.Increment();
				continue;
			}

			result += text[offset];
			offset.Increment();
		}

		return result;
	}

	[Benchmark, BenchmarkCategory("ParseText")]
	public string ParseText_AdjustForWhiteSpace()
	{
		var text = Text.AsSpan();
		var result = string.Empty;
		var offset = 0;

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharactersAtOffset(in offset, '/', '>'))
			{
				offset.AdjustForToken('/');
				break;
			}

			offset.AdjustForWhiteSpace(in text);

			result += text[offset];
			offset.Increment();
		}

		return result;
	}
}