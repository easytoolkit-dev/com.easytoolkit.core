#if UNITY_EDITOR || !ENABLE_IL2CPP
#define ENABLE_COMPILER
#endif

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyToolkit.Core.Reflection
{
    public static partial class ReflectionCompiler
    {
        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified constructor.
        /// </summary>
        /// <typeparam name="TInstance">The type of the constructed instance value.</typeparam>
        /// <param name="constructorInfo">The constructor metadata to create an invoker for.</param>
        /// <param name="autoFillParameters">
        /// When true, automatically fills constructor parameters with default values if the constructor has parameters.
        /// When false, the constructor must be parameterless or an exception will be thrown.
        /// </param>
        /// <returns>A strongly-typed delegate that invokes the constructor when called.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="constructorInfo"/> has parameters and <paramref name="autoFillParameters"/> is false.
        /// </exception>
        public static ParameterlessConstructorInvoker<TInstance> CreateParameterlessConstructorInvoker<TInstance>(
            ConstructorInfo constructorInfo,
            bool autoFillParameters = false)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            var parameters = constructorInfo.GetParameters();

#if ENABLE_COMPILER
            Expression newExpression;

            if (parameters.Length == 0)
            {
                // Parameterless constructor
                newExpression = Expression.New(constructorInfo);
            }
            else if (autoFillParameters)
            {
                // Constructor with parameters - auto-fill with default values
                var parameterExpressions = new Expression[parameters.Length];
                for (var i = 0; i < parameters.Length; i++)
                {
                    parameterExpressions[i] = Expression.Default(parameters[i].ParameterType);
                }

                newExpression = Expression.New(constructorInfo, parameterExpressions);
            }
            else
            {
                throw new ArgumentException(
                    $"Constructor must be parameterless, but has {parameters.Length} parameter(s).",
                    nameof(constructorInfo));
            }

            // Convert result to TInstance if necessary
            Expression bodyExpression = typeof(TInstance) == constructorInfo.DeclaringType
                ? newExpression
                : Expression.Convert(newExpression, typeof(TInstance));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<ParameterlessConstructorInvoker<TInstance>>(bodyExpression);
            return lambda.Compile();
#else
            if (parameters.Length == 0)
            {
                return () => (TInstance)constructorInfo.Invoke(null);
            }

            if (!autoFillParameters)
            {
                throw new ArgumentException(
                    $"Constructor must be parameterless, but has {parameters.Length} parameter(s).",
                    nameof(constructorInfo));
            }

            // Create default values for parameters
            var args = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                args[i] = parameters[i].ParameterType.IsValueType
                    ? Activator.CreateInstance(parameters[i].ParameterType)
                    : null;
            }

            return () => (TInstance)constructorInfo.Invoke(args);
#endif
        }
    }
}
