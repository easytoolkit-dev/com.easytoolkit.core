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
        #region Static Methods with Return Value

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified static method without parameters.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the static method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not static.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> has parameters.</exception>
        public static StaticInvoker<TResult> CreateStaticMethodInvoker<TResult>(MethodInfo methodInfo)
        {
            ValidateStaticMethod(methodInfo, expectedParameterCount: 0);
            return CompileStaticInvoker<TResult>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified static method with one parameter.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the static method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not static.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly one parameter.</exception>
        public static StaticInvoker<TArg1, TResult> CreateStaticMethodInvoker<TArg1, TResult>(MethodInfo methodInfo)
        {
            ValidateStaticMethod(methodInfo, expectedParameterCount: 1);
            return CompileStaticInvoker<TArg1, TResult>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified static method with two parameters.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the static method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not static.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly two parameters.</exception>
        public static StaticInvoker<TArg1, TArg2, TResult> CreateStaticMethodInvoker<TArg1, TArg2, TResult>(
            MethodInfo methodInfo)
        {
            ValidateStaticMethod(methodInfo, expectedParameterCount: 2);
            return CompileStaticInvoker<TArg1, TArg2, TResult>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified static method with three parameters.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the static method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not static.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly three parameters.</exception>
        public static StaticInvoker<TArg1, TArg2, TArg3, TResult> CreateStaticMethodInvoker<TArg1, TArg2, TArg3, TResult>(
            MethodInfo methodInfo)
        {
            ValidateStaticMethod(methodInfo, expectedParameterCount: 3);
            return CompileStaticInvoker<TArg1, TArg2, TArg3, TResult>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified static method with four parameters.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth parameter.</typeparam>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the static method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not static.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly four parameters.</exception>
        public static StaticInvoker<TArg1, TArg2, TArg3, TArg4, TResult> CreateStaticMethodInvoker
            <TArg1, TArg2, TArg3, TArg4, TResult>(MethodInfo methodInfo)
        {
            ValidateStaticMethod(methodInfo, expectedParameterCount: 4);
            return CompileStaticInvoker<TArg1, TArg2, TArg3, TArg4, TResult>(methodInfo);
        }

        #endregion

        #region Static Void Methods without Return Value

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified static void method with one parameter.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the static void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not static or does not return void.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly one parameter.</exception>
        public static StaticVoidInvoker<TArg1> CreateStaticVoidMethodInvoker<TArg1>(MethodInfo methodInfo)
        {
            ValidateStaticVoidMethod(methodInfo, expectedParameterCount: 1);
            return CompileStaticVoidInvoker<TArg1>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified static void method with two parameters.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the static void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not static or does not return void.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly two parameters.</exception>
        public static StaticVoidInvoker<TArg1, TArg2> CreateStaticVoidMethodInvoker<TArg1, TArg2>(
            MethodInfo methodInfo)
        {
            ValidateStaticVoidMethod(methodInfo, expectedParameterCount: 2);
            return CompileStaticVoidInvoker<TArg1, TArg2>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified static void method with three parameters.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the static void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not static or does not return void.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly three parameters.</exception>
        public static StaticVoidInvoker<TArg1, TArg2, TArg3> CreateStaticVoidMethodInvoker<TArg1, TArg2, TArg3>(
            MethodInfo methodInfo)
        {
            ValidateStaticVoidMethod(methodInfo, expectedParameterCount: 3);
            return CompileStaticVoidInvoker<TArg1, TArg2, TArg3>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified static void method with four parameters.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth parameter.</typeparam>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the static void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not static or does not return void.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly four parameters.</exception>
        public static StaticVoidInvoker<TArg1, TArg2, TArg3, TArg4> CreateStaticVoidMethodInvoker
            <TArg1, TArg2, TArg3, TArg4>(MethodInfo methodInfo)
        {
            ValidateStaticVoidMethod(methodInfo, expectedParameterCount: 4);
            return CompileStaticVoidInvoker<TArg1, TArg2, TArg3, TArg4>(methodInfo);
        }

        #endregion

        #region Instance Methods with Return Value

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified instance method without parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the instance method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> has parameters.</exception>
        public static InstanceInvoker<TInstance, TResult> CreateInstanceMethodInvoker<TInstance, TResult>(
            MethodInfo methodInfo)
        {
            ValidateInstanceMethod(methodInfo, expectedParameterCount: 0);
            return CompileInstanceInvoker<TInstance, TResult>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified instance method with one parameter.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the instance method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly one parameter.</exception>
        public static InstanceInvoker<TInstance, TArg1, TResult> CreateInstanceMethodInvoker<TInstance, TArg1, TResult>(
            MethodInfo methodInfo)
        {
            ValidateInstanceMethod(methodInfo, expectedParameterCount: 1);
            return CompileInstanceInvoker<TInstance, TArg1, TResult>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified instance method with two parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the instance method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly two parameters.</exception>
        public static InstanceInvoker<TInstance, TArg1, TArg2, TResult> CreateInstanceMethodInvoker
            <TInstance, TArg1, TArg2, TResult>(MethodInfo methodInfo)
        {
            ValidateInstanceMethod(methodInfo, expectedParameterCount: 2);
            return CompileInstanceInvoker<TInstance, TArg1, TArg2, TResult>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified instance method with three parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the instance method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly three parameters.</exception>
        public static InstanceInvoker<TInstance, TArg1, TArg2, TArg3, TResult> CreateInstanceMethodInvoker
            <TInstance, TArg1, TArg2, TArg3, TResult>(MethodInfo methodInfo)
        {
            ValidateInstanceMethod(methodInfo, expectedParameterCount: 3);
            return CompileInstanceInvoker<TInstance, TArg1, TArg2, TArg3, TResult>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified instance method with four parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth parameter.</typeparam>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the instance method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly four parameters.</exception>
        public static InstanceInvoker<TInstance, TArg1, TArg2, TArg3, TArg4, TResult> CreateInstanceMethodInvoker
            <TInstance, TArg1, TArg2, TArg3, TArg4, TResult>(MethodInfo methodInfo)
        {
            ValidateInstanceMethod(methodInfo, expectedParameterCount: 4);
            return CompileInstanceInvoker<TInstance, TArg1, TArg2, TArg3, TArg4, TResult>(methodInfo);
        }

        #endregion

        #region Instance Void Methods without Return Value

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified instance void method without parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the instance void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method or does not return void.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> has parameters.</exception>
        public static InstanceVoidInvoker<TInstance> CreateInstanceVoidMethodInvoker<TInstance>(MethodInfo methodInfo)
        {
            ValidateInstanceVoidMethod(methodInfo, expectedParameterCount: 0);
            return CompileInstanceVoidInvoker<TInstance>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified instance void method with one parameter.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the instance void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method or does not return void.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly one parameter.</exception>
        public static InstanceVoidInvoker<TInstance, TArg1> CreateInstanceVoidMethodInvoker<TInstance, TArg1>(
            MethodInfo methodInfo)
        {
            ValidateInstanceVoidMethod(methodInfo, expectedParameterCount: 1);
            return CompileInstanceVoidInvoker<TInstance, TArg1>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified instance void method with two parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the instance void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method or does not return void.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly two parameters.</exception>
        public static InstanceVoidInvoker<TInstance, TArg1, TArg2> CreateInstanceVoidMethodInvoker<TInstance, TArg1, TArg2>(
            MethodInfo methodInfo)
        {
            ValidateInstanceVoidMethod(methodInfo, expectedParameterCount: 2);
            return CompileInstanceVoidInvoker<TInstance, TArg1, TArg2>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified instance void method with three parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the instance void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method or does not return void.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly three parameters.</exception>
        public static InstanceVoidInvoker<TInstance, TArg1, TArg2, TArg3> CreateInstanceVoidMethodInvoker
            <TInstance, TArg1, TArg2, TArg3>(MethodInfo methodInfo)
        {
            ValidateInstanceVoidMethod(methodInfo, expectedParameterCount: 3);
            return CompileInstanceVoidInvoker<TInstance, TArg1, TArg2, TArg3>(methodInfo);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified instance void method with four parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
        /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
        /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth parameter.</typeparam>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the instance void method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method or does not return void.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> does not have exactly four parameters.</exception>
        public static InstanceVoidInvoker<TInstance, TArg1, TArg2, TArg3, TArg4> CreateInstanceVoidMethodInvoker
            <TInstance, TArg1, TArg2, TArg3, TArg4>(MethodInfo methodInfo)
        {
            ValidateInstanceVoidMethod(methodInfo, expectedParameterCount: 4);
            return CompileInstanceVoidInvoker<TInstance, TArg1, TArg2, TArg3, TArg4>(methodInfo);
        }

        #endregion
    }
}
