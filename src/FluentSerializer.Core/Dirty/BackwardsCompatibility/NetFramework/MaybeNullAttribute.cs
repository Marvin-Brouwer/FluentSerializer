#if NETSTANDARD2_0
namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// Specifies that an output may be null even if the corresponding type disallows it.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
public sealed class MaybeNullAttribute : Attribute
{

}
#else
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Diagnostics.CodeAnalysis.MaybeNullAttribute))]
#endif