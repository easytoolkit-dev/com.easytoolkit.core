#if UNITY_EDITOR || !ENABLE_IL2CPP
#define ENABLE_COMPILER
#endif

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyToolKit.Core.Reflection
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
        /// Creates a strongly-typed invoker delegate for calling the specified parameterless constructor.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return value (the constructed instance type).</typeparam>
        /// <param name="constructorInfo">The constructor metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the parameterless constructor when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="constructorInfo"/> has parameters.</exception>
        public static ConstructorInvoker<TReturn> CreateConstructorInvoker<TReturn>(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            var parameters = constructorInfo.GetParameters();
            if (parameters.Length != 0)
            {
                throw new ArgumentException(
                    $"Constructor must be parameterless, but has {parameters.Length} parameter(s).",
                    nameof(constructorInfo));
            }

#if ENABLE_COMPILER
            // Create constructor call expression (new expression)
            var newExpression = Expression.New(constructorInfo);

            // Convert result to TReturn if necessary
            Expression bodyExpression = typeof(TReturn) == constructorInfo.DeclaringType
                ? (Expression)newExpression
                : Expression.Convert(newExpression, typeof(TReturn));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<ConstructorInvoker<TReturn>>(bodyExpression);
            return lambda.Compile();
#else
            return () => (TReturn)constructorInfo.Invoke(null);
#endif
        }
    }
}
