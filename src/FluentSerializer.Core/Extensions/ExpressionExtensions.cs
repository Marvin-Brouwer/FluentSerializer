using System.Linq.Expressions;
using System.Reflection;

namespace FluentSerializer.Core.Extensions;

/// <summary>
/// Extention method to help with using <see cref="Expression{TDelegate}"/>
/// </summary>
public static class ExpressionExtensions
{
	/// <summary>
	/// Get the property referred to in the passed <paramref name="lambda"/>
	/// </summary>
	public static PropertyInfo GetProperty(this LambdaExpression lambda)
	{
		if (lambda.Body is UnaryExpression unaryExpression)
			return (PropertyInfo)((MemberExpression)unaryExpression.Operand).Member;

		return (PropertyInfo)((MemberExpression)lambda.Body).Member;
	}
}