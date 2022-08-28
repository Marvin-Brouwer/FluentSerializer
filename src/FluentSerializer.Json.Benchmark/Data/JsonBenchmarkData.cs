using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Json.DataNodes;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Benchmark.Data;

public sealed class JsonBenchmarkData : DataCollectionFactory<IJsonObject>
{
	public static readonly JsonBenchmarkData Default = new();

	protected override string GetStringFileName(int dataCount) => $"{nameof(JsonBenchmarkData)}-{dataCount}.json";
	protected override IJsonObject ConvertToData(List<ResidentialArea> data, int residentialAreaCount, long houseCount, long peopleCount) =>
		Object(
			Property(nameof(residentialAreaCount), Value(residentialAreaCount.ToString(CultureInfo.InvariantCulture))),
			Property(nameof(houseCount), Value(houseCount.ToString(CultureInfo.InvariantCulture))),
			Property(nameof(peopleCount), Value(peopleCount.ToString(CultureInfo.InvariantCulture))),
			Property(nameof(data), Array(data.Select(area => area.ToJsonElement())))
		);

	protected override IJsonObject GetDataFromSpan(string stringValue) => JsonParser.Parse(stringValue);
}