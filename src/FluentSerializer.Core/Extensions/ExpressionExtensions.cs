using System.Linq.Expressions;
using System.Reflection;

namespace FluentSerializer.Core.Extensions
{
    public static class ExpressionExtensions
    {
        public static PropertyInfo GetProperty(this LambdaExpression lambda)
        {
            if (lambda.Body is UnaryExpression unaryExpression)
                return (PropertyInfo)((MemberExpression)unaryExpression.Operand).Member;

            return (PropertyInfo)((MemberExpression)lambda.Body).Member;
        }
    }
}
