#if UNITY_EDITOR || !ENABLE_IL2CPP
#define ENABLE_COMPILER
#endif

using System;
using System.Linq.Expressions;

namespace EasyToolkit.Core.Reflection
{
    public static partial class ReflectionCompiler
    {
#if ENABLE_COMPILER
        /// <summary>
        /// Creates an expression that converts the value parameter to the target type,
        /// using the default value when the input is null and the target is a value type.
        /// </summary>
        /// <param name="valueParameter">The value parameter expression of type object.</param>
        /// <param name="targetType">The target type to convert to.</param>
        /// <returns>An expression that performs the conversion with null handling.</returns>
        internal static Expression CreateValueConversionExpression(Expression valueParameter, Type targetType)
        {
            if (targetType.IsValueType)
            {
                var nullTest = Expression.Equal(valueParameter, Expression.Constant(null, typeof(object)));
                var defaultValue = Expression.Default(targetType);
                var convertExpr = Expression.Convert(valueParameter, targetType);
                return Expression.Condition(nullTest, defaultValue, convertExpr);
            }
            else
            {
                return Expression.Convert(valueParameter, targetType);
            }
        }
#endif
    }
}
