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
        /// Creates an invoker delegate for calling the specified constructor.
        /// </summary>
        /// <param name="constructorInfo">The constructor metadata to create an invoker for.</param>
        /// <returns>A delegate that invokes the constructor when called.</returns>
        public static ConstructorInvoker CreateConstructorInvoker(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

#if ENABLE_COMPILER
            // Create parameter expression for the arguments array
            var argsParameter = Expression.Parameter(typeof(object[]), "args");

            // Get constructor parameters
            var parameters = constructorInfo.GetParameters();

            // Create parameter expressions and convert each argument
            var parameterExpressions = new Expression[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                // Access the argument from the array: args[i]
                var indexExpression = Expression.Constant(i, typeof(int));
                var arrayAccessExpression = Expression.ArrayAccess(argsParameter, indexExpression);

                // Convert to the parameter type
                parameterExpressions[i] = Expression.Convert(arrayAccessExpression, parameters[i].ParameterType);
            }

            // Create constructor call expression (new expression)
            var newExpression = Expression.New(constructorInfo, parameterExpressions);

            // Convert result to object if necessary
            Expression bodyExpression = constructorInfo.DeclaringType == typeof(object)
                ? (Expression)newExpression
                : Expression.Convert(newExpression, typeof(object));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<ConstructorInvoker>(bodyExpression, argsParameter);
            return lambda.Compile();
#else
            return constructorInfo.Invoke;
#endif
        }

        /// <summary>
        /// Creates an invoker delegate for calling the specified parameterless constructor.
        /// </summary>
        /// <param name="constructorInfo">The constructor metadata to create an invoker for.</param>
        /// <param name="autoFillParameters">
        /// When true, automatically fills constructor parameters with default values if the constructor has parameters.
        /// When false, the constructor must be parameterless or an exception will be thrown.
        /// </param>
        /// <returns>A delegate that invokes the constructor when called.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="constructorInfo"/> has parameters and <paramref name="autoFillParameters"/> is false.
        /// </exception>
        public static ParameterlessConstructorInvoker CreateParameterlessConstructorInvoker(
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

            // Convert result to object if necessary
            Expression bodyExpression = constructorInfo.DeclaringType == typeof(object)
                ? newExpression
                : Expression.Convert(newExpression, typeof(object));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<ParameterlessConstructorInvoker>(bodyExpression);
            return lambda.Compile();
#else
            if (parameters.Length == 0)
            {
                return () => constructorInfo.Invoke(null);
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

            return () => constructorInfo.Invoke(args);
#endif
        }
    }
}
