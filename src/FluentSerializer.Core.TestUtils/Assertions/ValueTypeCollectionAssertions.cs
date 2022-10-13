using FluentAssertions.Execution;
using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Collections;

namespace FluentSerializer.Core.TestUtils.Assertions;

public sealed class ValueTypeCollectionAssertions<T> : GenericCollectionAssertions<T>
	where T : struct
{
	public ValueTypeCollectionAssertions(IEnumerable<T> instance) : base(instance) { }

	protected override string Identifier => Subject?.ToString() ?? string.Empty;

	public AndConstraint<GenericCollectionAssertions<T>> AllBeUnique()
	{
		Execute.Assertion
			.Given(() => Subject.Distinct().Count())
			.ForCondition(result => result.Equals(Subject.Count()))
			.FailWith(
			$"Expected collection to contain only unique values, {Environment.NewLine}" +
		            "Values at these positions were duplicated: {0}",
			_ => GetValueGroups());
		return new AndConstraint<GenericCollectionAssertions<T>>(this);
	}

	private string GetValueGroups()
	{
		var duplicates = Subject
			.GroupBy(value => value)
			.Where(group => group.Count() > 1)
			.Select(group => group.First());

		return new StringBuilder(128)
			.AppendLine()
			.AppendJoin(Environment.NewLine, duplicates.Select(GetValueGroup))
			.AppendLine()
			.ToString();
	}

	private StringBuilder GetValueGroup(T value)
	{
		var indexes = FindIndexes(value);

		var stringBuilder = new StringBuilder(64)
			.Append("\tindexes: [")
			.AppendJoin(',', indexes)
			.AppendFormat("]; value: {0};", value);

		return stringBuilder;
	}

	private IEnumerable<int> FindIndexes(T value) =>
		Subject
			.Select((element, index) => element.Equals(value) ? index : -1)
			.Where(i => i >= 0);
}