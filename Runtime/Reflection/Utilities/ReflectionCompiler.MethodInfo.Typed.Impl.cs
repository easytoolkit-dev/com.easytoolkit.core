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
#if ENABLE_COMPILER
        #region Static Method Compilers

        private static StaticInvoker<TResult> CompileStaticInvoker<TResult>(MethodInfo methodInfo)
        {
            var callExpression = Expression.Call(null, methodInfo);
            var bodyExpression = ConvertReturnType<TResult>(callExpression, methodInfo.ReturnType);
            var lambda = Expression.Lambda<StaticInvoker<TResult>>(bodyExpression);
            return lambda.Compile();
        }

        private static StaticInvoker<TArg1, TResult> CompileStaticInvoker<TArg1, TResult>(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var callExpression = Expression.Call(null, methodInfo, convertedArg1);
            var bodyExpression = ConvertReturnType<TResult>(callExpression, methodInfo.ReturnType);
            var lambda = Expression.Lambda<StaticInvoker<TArg1, TResult>>(bodyExpression, arg1Parameter);
            return lambda.Compile();
        }

        private static StaticInvoker<TArg1, TArg2, TResult> CompileStaticInvoker<TArg1, TArg2, TResult>(
            MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var callExpression = Expression.Call(null, methodInfo, convertedArg1, convertedArg2);
            var bodyExpression = ConvertReturnType<TResult>(callExpression, methodInfo.ReturnType);
            var lambda =
                Expression.Lambda<StaticInvoker<TArg1, TArg2, TResult>>(bodyExpression, arg1Parameter, arg2Parameter);
            return lambda.Compile();
        }

        private static StaticInvoker<TArg1, TArg2, TArg3, TResult> CompileStaticInvoker<TArg1, TArg2, TArg3, TResult>(
            MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var arg3Parameter = Expression.Parameter(typeof(TArg3), "arg3");
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var convertedArg3 = ConvertParameter(arg3Parameter, parameters[2].ParameterType);
            var callExpression = Expression.Call(null, methodInfo, convertedArg1, convertedArg2, convertedArg3);
            var bodyExpression = ConvertReturnType<TResult>(callExpression, methodInfo.ReturnType);
            var lambda = Expression.Lambda<StaticInvoker<TArg1, TArg2, TArg3, TResult>>(bodyExpression, arg1Parameter,
                arg2Parameter, arg3Parameter);
            return lambda.Compile();
        }

        private static StaticInvoker<TArg1, TArg2, TArg3, TArg4, TResult> CompileStaticInvoker
            <TArg1, TArg2, TArg3, TArg4, TResult>(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var arg3Parameter = Expression.Parameter(typeof(TArg3), "arg3");
            var arg4Parameter = Expression.Parameter(typeof(TArg4), "arg4");
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var convertedArg3 = ConvertParameter(arg3Parameter, parameters[2].ParameterType);
            var convertedArg4 = ConvertParameter(arg4Parameter, parameters[3].ParameterType);
            var callExpression = Expression.Call(null, methodInfo, convertedArg1, convertedArg2, convertedArg3,
                convertedArg4);
            var bodyExpression = ConvertReturnType<TResult>(callExpression, methodInfo.ReturnType);
            var lambda = Expression.Lambda<StaticInvoker<TArg1, TArg2, TArg3, TArg4, TResult>>(bodyExpression,
                arg1Parameter,
                arg2Parameter, arg3Parameter, arg4Parameter);
            return lambda.Compile();
        }

        #endregion

        #region Static Void Method Compilers

        private static StaticVoidInvoker<TArg1> CompileStaticVoidInvoker<TArg1>(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var callExpression = Expression.Call(null, methodInfo, convertedArg1);
            var lambda = Expression.Lambda<StaticVoidInvoker<TArg1>>(callExpression, arg1Parameter);
            return lambda.Compile();
        }

        private static StaticVoidInvoker<TArg1, TArg2> CompileStaticVoidInvoker<TArg1, TArg2>(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var callExpression = Expression.Call(null, methodInfo, convertedArg1, convertedArg2);
            var lambda =
                Expression.Lambda<StaticVoidInvoker<TArg1, TArg2>>(callExpression, arg1Parameter, arg2Parameter);
            return lambda.Compile();
        }

        private static StaticVoidInvoker<TArg1, TArg2, TArg3> CompileStaticVoidInvoker<TArg1, TArg2, TArg3>(
            MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var arg3Parameter = Expression.Parameter(typeof(TArg3), "arg3");
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var convertedArg3 = ConvertParameter(arg3Parameter, parameters[2].ParameterType);
            var callExpression = Expression.Call(null, methodInfo, convertedArg1, convertedArg2, convertedArg3);
            var lambda = Expression.Lambda<StaticVoidInvoker<TArg1, TArg2, TArg3>>(callExpression, arg1Parameter,
                arg2Parameter, arg3Parameter);
            return lambda.Compile();
        }

        private static StaticVoidInvoker<TArg1, TArg2, TArg3, TArg4> CompileStaticVoidInvoker
            <TArg1, TArg2, TArg3, TArg4>(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var arg3Parameter = Expression.Parameter(typeof(TArg3), "arg3");
            var arg4Parameter = Expression.Parameter(typeof(TArg4), "arg4");
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var convertedArg3 = ConvertParameter(arg3Parameter, parameters[2].ParameterType);
            var convertedArg4 = ConvertParameter(arg4Parameter, parameters[3].ParameterType);
            var callExpression = Expression.Call(null, methodInfo, convertedArg1, convertedArg2, convertedArg3,
                convertedArg4);
            var lambda = Expression.Lambda<StaticVoidInvoker<TArg1, TArg2, TArg3, TArg4>>(callExpression, arg1Parameter,
                arg2Parameter, arg3Parameter, arg4Parameter);
            return lambda.Compile();
        }

        #endregion

        #region Instance Method Compilers

        private static InstanceInvoker<TInstance, TResult> CompileInstanceInvoker<TInstance, TResult>(
            MethodInfo methodInfo)
        {
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var callExpression = Expression.Call(convertedInstance, methodInfo);
            var bodyExpression = ConvertReturnType<TResult>(callExpression, methodInfo.ReturnType);
            var lambda = Expression.Lambda<InstanceInvoker<TInstance, TResult>>(bodyExpression, instanceParameter);
            return lambda.Compile();
        }

        private static InstanceInvoker<TInstance, TArg1, TResult> CompileInstanceInvoker<TInstance, TArg1, TResult>(
            MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var callExpression = Expression.Call(convertedInstance, methodInfo, convertedArg1);
            var bodyExpression = ConvertReturnType<TResult>(callExpression, methodInfo.ReturnType);
            var lambda = Expression.Lambda<InstanceInvoker<TInstance, TArg1, TResult>>(bodyExpression,
                instanceParameter,
                arg1Parameter);
            return lambda.Compile();
        }

        private static InstanceInvoker<TInstance, TArg1, TArg2, TResult> CompileInstanceInvoker
            <TInstance, TArg1, TArg2, TResult>(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var callExpression = Expression.Call(convertedInstance, methodInfo, convertedArg1, convertedArg2);
            var bodyExpression = ConvertReturnType<TResult>(callExpression, methodInfo.ReturnType);
            var lambda = Expression.Lambda<InstanceInvoker<TInstance, TArg1, TArg2, TResult>>(bodyExpression,
                instanceParameter, arg1Parameter, arg2Parameter);
            return lambda.Compile();
        }

        private static InstanceInvoker<TInstance, TArg1, TArg2, TArg3, TResult> CompileInstanceInvoker
            <TInstance, TArg1, TArg2, TArg3, TResult>(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var arg3Parameter = Expression.Parameter(typeof(TArg3), "arg3");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var convertedArg3 = ConvertParameter(arg3Parameter, parameters[2].ParameterType);
            var callExpression =
                Expression.Call(convertedInstance, methodInfo, convertedArg1, convertedArg2, convertedArg3);
            var bodyExpression = ConvertReturnType<TResult>(callExpression, methodInfo.ReturnType);
            var lambda = Expression.Lambda<InstanceInvoker<TInstance, TArg1, TArg2, TArg3, TResult>>(bodyExpression,
                instanceParameter, arg1Parameter, arg2Parameter, arg3Parameter);
            return lambda.Compile();
        }

        private static InstanceInvoker<TInstance, TArg1, TArg2, TArg3, TArg4, TResult> CompileInstanceInvoker
            <TInstance, TArg1, TArg2, TArg3, TArg4, TResult>(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var arg3Parameter = Expression.Parameter(typeof(TArg3), "arg3");
            var arg4Parameter = Expression.Parameter(typeof(TArg4), "arg4");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var convertedArg3 = ConvertParameter(arg3Parameter, parameters[2].ParameterType);
            var convertedArg4 = ConvertParameter(arg4Parameter, parameters[3].ParameterType);
            var callExpression = Expression.Call(convertedInstance, methodInfo, convertedArg1, convertedArg2,
                convertedArg3,
                convertedArg4);
            var bodyExpression = ConvertReturnType<TResult>(callExpression, methodInfo.ReturnType);
            var lambda = Expression.Lambda<InstanceInvoker<TInstance, TArg1, TArg2, TArg3, TArg4, TResult>>(
                bodyExpression,
                instanceParameter, arg1Parameter, arg2Parameter, arg3Parameter, arg4Parameter);
            return lambda.Compile();
        }

        #endregion

        #region Instance Void Method Compilers

        private static InstanceVoidInvoker<TInstance> CompileInstanceVoidInvoker<TInstance>(MethodInfo methodInfo)
        {
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var callExpression = Expression.Call(convertedInstance, methodInfo);
            var lambda = Expression.Lambda<InstanceVoidInvoker<TInstance>>(callExpression, instanceParameter);
            return lambda.Compile();
        }

        private static InstanceVoidInvoker<TInstance, TArg1> CompileInstanceVoidInvoker<TInstance, TArg1>(
            MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var callExpression = Expression.Call(convertedInstance, methodInfo, convertedArg1);
            var lambda = Expression.Lambda<InstanceVoidInvoker<TInstance, TArg1>>(callExpression, instanceParameter,
                arg1Parameter);
            return lambda.Compile();
        }

        private static InstanceVoidInvoker<TInstance, TArg1, TArg2> CompileInstanceVoidInvoker<TInstance, TArg1, TArg2>(
            MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var callExpression = Expression.Call(convertedInstance, methodInfo, convertedArg1, convertedArg2);
            var lambda = Expression.Lambda<InstanceVoidInvoker<TInstance, TArg1, TArg2>>(callExpression,
                instanceParameter,
                arg1Parameter, arg2Parameter);
            return lambda.Compile();
        }

        private static InstanceVoidInvoker<TInstance, TArg1, TArg2, TArg3> CompileInstanceVoidInvoker
            <TInstance, TArg1, TArg2, TArg3>(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var arg3Parameter = Expression.Parameter(typeof(TArg3), "arg3");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var convertedArg3 = ConvertParameter(arg3Parameter, parameters[2].ParameterType);
            var callExpression =
                Expression.Call(convertedInstance, methodInfo, convertedArg1, convertedArg2, convertedArg3);
            var lambda = Expression.Lambda<InstanceVoidInvoker<TInstance, TArg1, TArg2, TArg3>>(callExpression,
                instanceParameter, arg1Parameter, arg2Parameter, arg3Parameter);
            return lambda.Compile();
        }

        private static InstanceVoidInvoker<TInstance, TArg1, TArg2, TArg3, TArg4> CompileInstanceVoidInvoker
            <TInstance, TArg1, TArg2, TArg3, TArg4>(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var arg1Parameter = Expression.Parameter(typeof(TArg1), "arg1");
            var arg2Parameter = Expression.Parameter(typeof(TArg2), "arg2");
            var arg3Parameter = Expression.Parameter(typeof(TArg3), "arg3");
            var arg4Parameter = Expression.Parameter(typeof(TArg4), "arg4");
            var convertedInstance = Expression.Convert(instanceParameter, methodInfo.DeclaringType);
            var convertedArg1 = ConvertParameter(arg1Parameter, parameters[0].ParameterType);
            var convertedArg2 = ConvertParameter(arg2Parameter, parameters[1].ParameterType);
            var convertedArg3 = ConvertParameter(arg3Parameter, parameters[2].ParameterType);
            var convertedArg4 = ConvertParameter(arg4Parameter, parameters[3].ParameterType);
            var callExpression = Expression.Call(convertedInstance, methodInfo, convertedArg1, convertedArg2,
                convertedArg3,
                convertedArg4);
            var lambda = Expression.Lambda<InstanceVoidInvoker<TInstance, TArg1, TArg2, TArg3, TArg4>>(callExpression,
                instanceParameter, arg1Parameter, arg2Parameter, arg3Parameter, arg4Parameter);
            return lambda.Compile();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Converts a parameter expression to the target type if necessary.
        /// </summary>
        /// <param name="parameter">The parameter expression to convert.</param>
        /// <param name="targetType">The target type to convert to.</param>
        /// <returns>The original expression if types match, or a converted expression.</returns>
        private static Expression ConvertParameter(Expression parameter, Type targetType)
        {
            return parameter.Type == targetType ? parameter : Expression.Convert(parameter, targetType);
        }

        /// <summary>
        /// Converts a method call result to the specified return type if necessary.
        /// </summary>
        /// <typeparam name="TResult">The expected return type.</typeparam>
        /// <param name="callExpression">The method call expression.</param>
        /// <param name="actualReturnType">The actual return type of the method.</param>
        /// <returns>An expression that returns the method result with the correct type.</returns>
        private static Expression ConvertReturnType<TResult>(Expression callExpression, Type actualReturnType)
        {
            if (typeof(TResult) == actualReturnType)
            {
                return callExpression;
            }

            return Expression.Convert(callExpression, typeof(TResult));
        }

        #endregion

#else
        #region Fallback Implementations for IL2CPP

        private static StaticInvoker<TResult> CompileStaticInvoker<TResult>(MethodInfo methodInfo) =>
            () => (TResult)methodInfo.Invoke(null, null);

        private static StaticInvoker<TArg1, TResult> CompileStaticInvoker<TArg1, TResult>(MethodInfo methodInfo) =>
            arg1 => (TResult)methodInfo.Invoke(null, new object[] { arg1 });

        private static StaticInvoker<TArg1, TArg2, TResult> CompileStaticInvoker<TArg1, TArg2, TResult>(
            MethodInfo methodInfo) =>
            (arg1, arg2) => (TResult)methodInfo.Invoke(null, new object[] { arg1, arg2 });

        private static StaticInvoker<TArg1, TArg2, TArg3, TResult> CompileStaticInvoker<TArg1, TArg2, TArg3, TResult>(
            MethodInfo methodInfo) =>
            (arg1, arg2, arg3) => (TResult)methodInfo.Invoke(null, new object[] { arg1, arg2, arg3 });

        private static StaticInvoker<TArg1, TArg2, TArg3, TArg4, TResult> CompileStaticInvoker
            <TArg1, TArg2, TArg3, TArg4, TResult>(MethodInfo methodInfo) =>
            (arg1, arg2, arg3, arg4) => (TResult)methodInfo.Invoke(null, new object[] { arg1, arg2, arg3, arg4 });

        private static StaticVoidInvoker<TArg1> CompileStaticVoidInvoker<TArg1>(MethodInfo methodInfo) =>
            arg1 => methodInfo.Invoke(null, new object[] { arg1 });

        private static StaticVoidInvoker<TArg1, TArg2> CompileStaticVoidInvoker<TArg1, TArg2>(MethodInfo methodInfo) =>
            (arg1, arg2) => methodInfo.Invoke(null, new object[] { arg1, arg2 });

        private static StaticVoidInvoker<TArg1, TArg2, TArg3> CompileStaticVoidInvoker<TArg1, TArg2, TArg3>(
            MethodInfo methodInfo) =>
            (arg1, arg2, arg3) => methodInfo.Invoke(null, new object[] { arg1, arg2, arg3 });

        private static StaticVoidInvoker<TArg1, TArg2, TArg3, TArg4> CompileStaticVoidInvoker
            <TArg1, TArg2, TArg3, TArg4>(MethodInfo methodInfo) =>
            (arg1, arg2, arg3, arg4) => methodInfo.Invoke(null, new object[] { arg1, arg2, arg3, arg4 });

        private static InstanceInvoker<TInstance, TResult> CompileInstanceInvoker<TInstance, TResult>(
            MethodInfo methodInfo) =>
            (ref TInstance instance) => (TResult)methodInfo.Invoke(instance, null);

        private static InstanceInvoker<TInstance, TArg1, TResult> CompileInstanceInvoker<TInstance, TArg1, TResult>(
            MethodInfo methodInfo) =>
            (ref TInstance instance, TArg1 arg1) => (TResult)methodInfo.Invoke(instance, new object[] { arg1 });

        private static InstanceInvoker<TInstance, TArg1, TArg2, TResult> CompileInstanceInvoker
            <TInstance, TArg1, TArg2, TResult>(MethodInfo methodInfo) =>
            (ref TInstance instance, TArg1 arg1, TArg2 arg2) => (TResult)methodInfo.Invoke(instance, new object[] { arg1, arg2 });

        private static InstanceInvoker<TInstance, TArg1, TArg2, TArg3, TResult> CompileInstanceInvoker
            <TInstance, TArg1, TArg2, TArg3, TResult>(MethodInfo methodInfo) =>
            (ref TInstance instance, TArg1 arg1, TArg2 arg2, TArg3 arg3) =>
                (TResult)methodInfo.Invoke(instance, new object[] { arg1, arg2, arg3 });

        private static InstanceInvoker<TInstance, TArg1, TArg2, TArg3, TArg4, TResult> CompileInstanceInvoker
            <TInstance, TArg1, TArg2, TArg3, TArg4, TResult>(MethodInfo methodInfo) =>
            (ref TInstance instance, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) =>
                (TResult)methodInfo.Invoke(instance, new object[] { arg1, arg2, arg3, arg4 });

        private static InstanceVoidInvoker<TInstance> CompileInstanceVoidInvoker<TInstance>(MethodInfo methodInfo) =>
            (ref TInstance instance) => methodInfo.Invoke(instance, null);

        private static InstanceVoidInvoker<TInstance, TArg1> CompileInstanceVoidInvoker<TInstance, TArg1>(
            MethodInfo methodInfo) =>
            (ref TInstance instance, TArg1 arg1) => methodInfo.Invoke(instance, new object[] { arg1 });

        private static InstanceVoidInvoker<TInstance, TArg1, TArg2> CompileInstanceVoidInvoker<TInstance, TArg1, TArg2>(
            MethodInfo methodInfo) =>
            (ref TInstance instance, TArg1 arg1, TArg2 arg2) => methodInfo.Invoke(instance, new object[] { arg1, arg2 });

        private static InstanceVoidInvoker<TInstance, TArg1, TArg2, TArg3> CompileInstanceVoidInvoker
            <TInstance, TArg1, TArg2, TArg3>(MethodInfo methodInfo) =>
            (ref TInstance instance, TArg1 arg1, TArg2 arg2, TArg3 arg3) =>
                methodInfo.Invoke(instance, new object[] { arg1, arg2, arg3 });

        private static InstanceVoidInvoker<TInstance, TArg1, TArg2, TArg3, TArg4> CompileInstanceVoidInvoker
            <TInstance, TArg1, TArg2, TArg3, TArg4>(MethodInfo methodInfo) =>
            (ref TInstance instance, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) =>
                methodInfo.Invoke(instance, new object[] { arg1, arg2, arg3, arg4 });

        #endregion

#endif

        #region Validation Methods

        /// <summary>
        /// Validates that the method is static and has the expected number of parameters.
        /// </summary>
        private static void ValidateStaticMethod(MethodInfo methodInfo, int expectedParameterCount)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (!methodInfo.IsStatic)
            {
                throw new ArgumentException($"Method '{methodInfo.Name}' is not static.", nameof(methodInfo));
            }

            ValidateParameterCount(methodInfo, expectedParameterCount);
        }

        /// <summary>
        /// Validates that the method is static, returns void, and has the expected number of parameters.
        /// </summary>
        private static void ValidateStaticVoidMethod(MethodInfo methodInfo, int expectedParameterCount)
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

            ValidateParameterCount(methodInfo, expectedParameterCount);
        }

        /// <summary>
        /// Validates that the method is an instance method and has the expected number of parameters.
        /// </summary>
        private static void ValidateInstanceMethod(MethodInfo methodInfo, int expectedParameterCount)
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

            ValidateParameterCount(methodInfo, expectedParameterCount);
        }

        /// <summary>
        /// Validates that the method is an instance method, returns void, and has the expected number of parameters.
        /// </summary>
        private static void ValidateInstanceVoidMethod(MethodInfo methodInfo, int expectedParameterCount)
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

            ValidateParameterCount(methodInfo, expectedParameterCount);
        }

        /// <summary>
        /// Validates that the method has the expected number of parameters.
        /// </summary>
        private static void ValidateParameterCount(MethodInfo methodInfo, int expectedParameterCount)
        {
            var actualParameterCount = methodInfo.GetParameters().Length;
            if (actualParameterCount != expectedParameterCount)
            {
                throw new ArgumentException(
                    $"Method '{methodInfo.Name}' must have {expectedParameterCount} parameter(s), but has {actualParameterCount}.",
                    nameof(methodInfo));
            }
        }

        #endregion
    }
}
