using System;
using System.Reflection;

using FluentSerializer.Core.Context;

using Moq;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming;

internal struct NameTestData
{
	public static readonly INamingContext NamingContext = new Mock<INamingContext>().Object;

	internal class ShortNamedClass
	{
		public static bool ShortNamedProperty => true;


		public static readonly Type ClassType = typeof(ShortNamedClass);
		public static readonly PropertyInfo PropertyInfo = ClassType.GetProperty(nameof(ShortNamedProperty))!;
	}

	internal class LongNamedWrapperClass
	{
		internal class LongNamedInnerClass
		{
			public static bool SomeVeryLongNamedProperty_WithSomeAddionalText_ForTestingOfCourse => true;


			public static readonly Type ClassType = typeof(LongNamedInnerClass);
			public static readonly PropertyInfo PropertyInfo = ClassType.GetProperty(nameof(SomeVeryLongNamedProperty_WithSomeAddionalText_ForTestingOfCourse))!;
		}
	}
}