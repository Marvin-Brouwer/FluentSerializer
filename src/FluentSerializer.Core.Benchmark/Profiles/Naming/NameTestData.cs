using System;
using System.Reflection;

using FluentSerializer.Core.Context;

using Moq;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming;

internal struct NameTestData
{
	internal class ShortNamedClass
	{
		public static bool ShortNamedProperty => true;


		public static readonly Type ClassType = typeof(ShortNamedClass);
		public static readonly PropertyInfo PropertyInfo = ClassType.GetProperty(nameof(ShortNamedProperty))!;
		public static readonly INamingContext NamingContext = new Mock<INamingContext>().Object;
	}
}