using FluentSerializer.Core.Converting;
using FluentSerializer.Json.Converting.Converters;
using System;

namespace FluentSerializer.Json.Tests.ObjectMother
{
	internal struct ObjectMotherConstants
	{
		/// <summary>
		/// Converter used by Tests, it is basically the simplest converter because it can handle primitives and saves us writing a
		/// mock of <see cref="IConverter" />
		/// </summary>
		internal static readonly Func<IConverter> TestConverter = () => new ConvertibleConverter();
	}
}
