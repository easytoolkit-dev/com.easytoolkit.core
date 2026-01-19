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
        /// Creates an invoker delegate for calling the specified static method.
        /// </summary>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A delegate that invokes the static method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not a static method.</exception>
        public static StaticFuncInvoker CreateStaticMethodInvoker(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (!methodInfo.IsStatic)
            {
                throw new ArgumentException($"Method '{methodInfo.Name}' is not static.", nameof(methodInfo));
            }

#if ENABLE_COMPILER
            // Create parameter expression for the arguments array
            var argsParameter = Expression.Parameter(typeof(object[]), "args");

            // Get method parameters
            var parameters = methodInfo.GetParameters();

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

            // Create method call expression
            var callExpression = Expression.Call(null, methodInfo, parameterExpressions);

            // Convert result to object if necessary (void methods need special handling)
            Expression bodyExpression;
            if (methodInfo.ReturnType == typeof(void))
            {
                // For void methods, call the method and then return a boxed null
                bodyExpression = Expression.Block(callExpression, Expression.Default(typeof(object)));
            }
            else if (methodInfo.ReturnType == typeof(object))
            {
                bodyExpression = callExpression;
            }
            else
            {
                bodyExpression = Expression.Convert(callExpression, typeof(object));
            }

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<StaticFuncInvoker>(bodyExpression, argsParameter);
            return lambda.Compile();
#else
            return args => methodInfo.Invoke(null, args);
#endif
        }

        /// <summary>
        /// Creates an invoker delegate for calling the specified instance method.
        /// </summary>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A delegate that invokes the instance method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method (i.e., it is a static method).</exception>
        public static InstanceFuncInvoker CreateInstanceMethodInvoker(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (methodInfo.IsStatic)
            {
                throw new ArgumentException($"Method '{methodInfo.Name}' is not an instance method.",
                    nameof(methodInfo));
            }

#if ENABLE_COMPILER
            // Create parameter expressions
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var argsParameter = Expression.Parameter(typeof(object[]), "args");

            // Get method parameters
            var parameters = methodInfo.GetParameters();

            // Convert instance to the declaring type
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);

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

            // Create method call expression
            var callExpression = Expression.Call(convertedInstance, methodInfo, parameterExpressions);

            // Convert result to object if necessary (void methods need special handling)
            Expression bodyExpression;
            if (methodInfo.ReturnType == typeof(void))
            {
                // For void methods, call the method and then return a boxed null
                bodyExpression = Expression.Block(callExpression, Expression.Default(typeof(object)));
            }
            else if (methodInfo.ReturnType == typeof(object))
            {
                bodyExpression = callExpression;
            }
            else
            {
                bodyExpression = Expression.Convert(callExpression, typeof(object));
            }

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<InstanceFuncInvoker>(bodyExpression, instanceParameter, argsParameter);
            return lambda.Compile();
#else
            return methodInfo.Invoke;
#endif
        }
    }
}
