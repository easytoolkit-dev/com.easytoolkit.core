using System;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Delegate for invoking static methods with variable parameters.
    /// </summary>
    /// <param name="args">The method arguments.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate object StaticInvoker(params object[] args);

    /// <summary>
    /// Delegate for invoking static methods without return values with variable parameters.
    /// </summary>
    /// <param name="args">The method arguments.</param>
    public delegate void StaticVoidInvoker(params object[] args);

    #region Generic Static Invokers with Return Value

    /// <summary>
    /// Generic delegate for invoking static methods without parameters.
    /// </summary>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticInvoker<out TResult>();

    /// <summary>
    /// Generic delegate for invoking static methods with one parameter.
    /// </summary>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticInvoker<in TArg1, out TResult>(TArg1 arg1);

    /// <summary>
    /// Generic delegate for invoking static methods with two parameters.
    /// </summary>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticInvoker<in TArg1, in TArg2, out TResult>(TArg1 arg1, TArg2 arg2);

    /// <summary>
    /// Generic delegate for invoking static methods with three parameters.
    /// </summary>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticInvoker<in TArg1, in TArg2, in TArg3, out TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3);

    /// <summary>
    /// Generic delegate for invoking static methods with four parameters.
    /// </summary>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
    /// <typeparam name="TArg4">The type of the fourth parameter.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <param name="arg4">The fourth parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticInvoker<in TArg1, in TArg2, in TArg3, in TArg4, out TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);

    #endregion

    #region Generic Static Void Invokers without Return Value

    /// <summary>
    /// Generic delegate for invoking static methods with one parameter and without return value.
    /// </summary>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    public delegate void StaticVoidInvoker<in TArg1>(TArg1 arg1);

    /// <summary>
    /// Generic delegate for invoking static methods with two parameters and without return value.
    /// </summary>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    public delegate void StaticVoidInvoker<in TArg1, in TArg2>(TArg1 arg1, TArg2 arg2);

    /// <summary>
    /// Generic delegate for invoking static methods with three parameters and without return value.
    /// </summary>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    public delegate void StaticVoidInvoker<in TArg1, in TArg2, in TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3);

    /// <summary>
    /// Generic delegate for invoking static methods with four parameters and without return value.
    /// </summary>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
    /// <typeparam name="TArg4">The type of the fourth parameter.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <param name="arg4">The fourth parameter.</param>
    public delegate void StaticVoidInvoker<in TArg1, in TArg2, in TArg3, in TArg4>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);

    #endregion
}
