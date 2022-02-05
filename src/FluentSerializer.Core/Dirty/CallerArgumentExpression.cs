namespace System.Runtime.CompilerServices;

#if !NET6_0_OR_GREATER

/// <summary>
/// https://stackoverflow.com/a/70034587/2319865
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
internal sealed class CallerArgumentExpressionAttribute : Attribute
{
	public CallerArgumentExpressionAttribute(string parameterName)
	{
		ParameterName = parameterName;
	}

	public string ParameterName { get; }
}

#endif
