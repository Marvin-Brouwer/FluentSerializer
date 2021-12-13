using System.Collections.Generic;

namespace FluentSerializer.Core.BenchmarkUtils.TestData;

public sealed class House
{
	public string Type { get; set; } = string.Empty;
	public string StreetName { get; set; } = string.Empty;
	public int HouseNumber { get; set; }
	public string City { get; set; } = string.Empty;
	public string ZipCode { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;

	public List<Person> Residents { get; set; } = new();
}