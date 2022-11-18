using FluentSerializer.Core.Converting;
using FluentSerializer.Xml.DataNodes;

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Xml.Tests.Extensions;

internal static class ConverterExtensions
{
	/// <summary>
	/// Cast a converter that implements <see cref="IConverter{TSerialContainer, TDataNode}"/> explicitly, to
	/// it's interface type (for <see cref="IXmlText"/>) so the correct method is tested.
	/// </summary>
	[DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IConverter<IXmlText, IXmlNode> ForText<TConverter>(this TConverter converter)
		where TConverter : IConverter<IXmlText, IXmlNode> => converter;

	/// <summary>
	/// Cast a converter that implements <see cref="IConverter{TSerialContainer, TDataNode}"/> explicitly, to
	/// it's interface type (for <see cref="IXmlAttribute"/>) so the correct method is tested.
	/// </summary>
	[DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IConverter<IXmlAttribute, IXmlNode> ForAttribute<TConverter>(this TConverter converter)
		where TConverter : IConverter<IXmlAttribute, IXmlNode> => converter;

	/// <summary>
	/// Cast a converter that implements <see cref="IConverter{TSerialContainer, TDataNode}"/> explicitly, to
	/// it's interface type (for <see cref="IXmlElement"/>) so the correct method is tested.
	/// </summary>
	[DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IConverter<IXmlElement, IXmlNode> ForElement<TConverter>(this TConverter converter)
		where TConverter : IConverter<IXmlElement, IXmlNode> => converter;
}
