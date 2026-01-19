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
        public static StaticInvoker CreateStaticMethodInvoker(MethodInfo methodInfo)
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
            var argsParameter = Expression.Parameter(typeof(object[]), "args");
            var parameterExpressions = BuildParameterExpressions(methodInfo, argsParameter);
            var callExpression = Expression.Call(null, methodInfo, parameterExpressions);
            var bodyExpression = CreateReturnExpression(callExpression, methodInfo.ReturnType);

            var lambda = Expression.Lambda<StaticInvoker>(bodyExpression, argsParameter);
            return lambda.Compile();
#else
            return args => methodInfo.Invoke(null, args);
#endif
        }

        /// <summary>
        /// Creates an invoker delegate for calling the specified static method that returns void.
        /// </summary>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A delegate that invokes the static void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not a static method or does not return void.</exception>
        public static StaticVoidInvoker CreateStaticVoidMethodInvoker(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (!methodInfo.IsStatic)
            {
                throw new ArgumentException($"Method '{methodInfo.Name}' is not static.", nameof(methodInfo));
            }

            if (methodInfo.ReturnType != typeof(void))
            {
                throw new ArgumentException($"Method '{methodInfo.Name}' does not return void.", nameof(methodInfo));
            }

#if ENABLE_COMPILER
            var argsParameter = Expression.Parameter(typeof(object[]), "args");
            var parameterExpressions = BuildParameterExpressions(methodInfo, argsParameter);
            var callExpression = Expression.Call(null, methodInfo, parameterExpressions);

            var lambda = Expression.Lambda<StaticVoidInvoker>(callExpression, argsParameter);
            return lambda.Compile();
#else
            return args => methodInfo.Invoke(null, args);
#endif
        }

        /// <summary>
        /// Creates an invoker delegate for calling the specified instance method that returns void.
        /// </summary>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A delegate that invokes the instance void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method or does not return void.</exception>
        public static InstanceVoidInvoker CreateInstanceVoidMethodInvoker(MethodInfo methodInfo)
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

            if (methodInfo.ReturnType != typeof(void))
            {
                throw new ArgumentException($"Method '{methodInfo.Name}' does not return void.", nameof(methodInfo));
            }

#if ENABLE_COMPILER
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var argsParameter = Expression.Parameter(typeof(object[]), "args");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var parameterExpressions = BuildParameterExpressions(methodInfo, argsParameter);
            var callExpression = Expression.Call(convertedInstance, methodInfo, parameterExpressions);

            var lambda = Expression.Lambda<InstanceVoidInvoker>(callExpression, instanceParameter, argsParameter);
            return lambda.Compile();
#else
            return (instance, args) => methodInfo.Invoke(instance, args);
#endif
        }

        /// <summary>
        /// Creates an invoker delegate for calling the specified instance method.
        /// </summary>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A delegate that invokes the instance method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method (i.e., it is a static method).</exception>
        public static InstanceInvoker CreateInstanceMethodInvoker(MethodInfo methodInfo)
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
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var argsParameter = Expression.Parameter(typeof(object[]), "args");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var parameterExpressions = BuildParameterExpressions(methodInfo, argsParameter);
            var callExpression = Expression.Call(convertedInstance, methodInfo, parameterExpressions);
            var bodyExpression = CreateReturnExpression(callExpression, methodInfo.ReturnType);

            var lambda = Expression.Lambda<InstanceInvoker>(bodyExpression, instanceParameter, argsParameter);
            return lambda.Compile();
#else
            return methodInfo.Invoke;
#endif
        }

#if ENABLE_COMPILER
        /// <summary>
        /// Builds parameter expressions for method invocation from an arguments array.
        /// </summary>
        /// <param name="methodInfo">The method to get parameters from.</param>
        /// <param name="argsParameter">The expression representing the arguments array.</param>
        /// <returns>An array of parameter expressions ready for method invocation.</returns>
        private static Expression[] BuildParameterExpressions(MethodInfo methodInfo, ParameterExpression argsParameter)
        {
            var parameters = methodInfo.GetParameters();
            var parameterExpressions = new Expression[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var indexExpression = Expression.Constant(i, typeof(int));
                var arrayAccessExpression = Expression.ArrayAccess(argsParameter, indexExpression);
                parameterExpressions[i] = Expression.Convert(arrayAccessExpression, parameters[i].ParameterType);
            }

            return parameterExpressions;
        }

        /// <summary>
        /// Creates a body expression that converts the method call result to object type.
        /// </summary>
        /// <param name="callExpression">The method call expression.</param>
        /// <param name="returnType">The return type of the method.</param>
        /// <returns>An expression that returns the method result as an object.</returns>
        private static Expression CreateReturnExpression(MethodCallExpression callExpression, Type returnType)
        {
            if (returnType == typeof(void))
            {
                return Expression.Block(callExpression, Expression.Default(typeof(object)));
            }

            if (returnType == typeof(object))
            {
                return callExpression;
            }

            return Expression.Convert(callExpression, typeof(object));
        }
#endif
    }
}
