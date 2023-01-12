#if NETSTANDARD2_0
namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// Specifies that null is allowed as an input even if the corresponding type disallows it.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
public sealed class AllowNullAttribute : Attribute
{

}
#else
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Diagnostics.CodeAnalysis.AllowNullAttribute))]
#endif