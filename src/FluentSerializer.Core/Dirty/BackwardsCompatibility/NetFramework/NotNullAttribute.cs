#if NETSTANDARD2_0
namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// Specifies that an output is not null even if the corresponding type allows it.
///  Specifies that an input argument was not null when the call returns.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
public sealed class NotNullAttribute : Attribute
{

}
#else
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Diagnostics.CodeAnalysis.NotNullAttribute))]
#endif