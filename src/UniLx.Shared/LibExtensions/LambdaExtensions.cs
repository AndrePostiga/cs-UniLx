using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UniLx.Shared.LibExtensions
{
    public static class LambdaExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return CombineExpressions(first, second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return CombineExpressions(first, second, Expression.OrElse);
        }

        private static Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second,
            Func<Expression, Expression, BinaryExpression> mergeFunction)
        {
            var parameter = Expression.Parameter(typeof(T));

            // Replace parameters in the second expression with the parameter of the first expression
            var firstBody = ReplaceParameter(first.Body, first.Parameters[0], parameter);
            var secondBody = ReplaceParameter(second.Body, second.Parameters[0], parameter);

            // Combine the expressions
            var body = mergeFunction(firstBody, secondBody);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression ReplaceParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return new ParameterReplacer(oldParameter, newParameter).Visit(expression);
        }

        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParameter ? _newParameter : base.VisitParameter(node);
            }
        }
    }
}
