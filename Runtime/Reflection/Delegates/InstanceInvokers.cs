namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Delegate for invoking instance methods with variable parameters.
    /// </summary>
    /// <param name="instance">The instance to invoke the method on.</param>
    /// <param name="args">The method arguments.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate object InstanceInvoker(object instance, params object[] args);

    /// <summary>
    /// Delegate for invoking instance methods without return values with variable parameters.
    /// </summary>
    /// <param name="instance">The instance to invoke the method on.</param>
    /// <param name="args">The method arguments.</param>
    public delegate void InstanceVoidInvoker(object instance, params object[] args);

    #region Generic Instance Invokers with Return Value

    /// <summary>
    /// Generic delegate for invoking instance methods without parameters.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate TResult InstanceInvoker<TInstance, out TResult>(ref TInstance instance);

    /// <summary>
    /// Generic delegate for invoking instance methods with one parameter.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate TResult InstanceInvoker<TInstance, out TResult, in TArg1>(ref TInstance instance, TArg1 arg1);

    /// <summary>
    /// Generic delegate for invoking instance methods with two parameters.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate TResult InstanceInvoker<TInstance, out TResult, in TArg1, in TArg2>(ref TInstance instance, TArg1 arg1, TArg2 arg2);

    /// <summary>
    /// Generic delegate for invoking instance methods with three parameters.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate TResult InstanceInvoker<TInstance, out TResult, in TArg1, in TArg2, in TArg3>(ref TInstance instance, TArg1 arg1, TArg2 arg2, TArg3 arg3);

    /// <summary>
    /// Generic delegate for invoking instance methods with four parameters.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
    /// <typeparam name="TArg4">The type of the fourth parameter.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <param name="arg4">The fourth parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate TResult InstanceInvoker<TInstance, out TResult, in TArg1, in TArg2, in TArg3, in TArg4>(ref TInstance instance, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);

    #endregion

    #region Generic Instance Void Invokers without Return Value

    /// <summary>
    /// Generic delegate for invoking instance methods without parameters and without return value.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate void InstanceVoidInvoker<TInstance>(ref TInstance instance);

    /// <summary>
    /// Generic delegate for invoking instance methods with one parameter and without return value.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate void InstanceVoidInvoker<TInstance, in TArg1>(ref TInstance instance, TArg1 arg1);

    /// <summary>
    /// Generic delegate for invoking instance methods with two parameters and without return value.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate void InstanceVoidInvoker<TInstance, in TArg1, in TArg2>(ref TInstance instance, TArg1 arg1, TArg2 arg2);

    /// <summary>
    /// Generic delegate for invoking instance methods with three parameters and without return value.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate void InstanceVoidInvoker<TInstance, in TArg1, in TArg2, in TArg3>(ref TInstance instance, TArg1 arg1, TArg2 arg2, TArg3 arg3);

    /// <summary>
    /// Generic delegate for invoking instance methods with four parameters and without return value.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TArg1">The type of the first parameter.</typeparam>
    /// <typeparam name="TArg2">The type of the second parameter.</typeparam>
    /// <typeparam name="TArg3">The type of the third parameter.</typeparam>
    /// <typeparam name="TArg4">The type of the fourth parameter.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <param name="arg4">The fourth parameter.</param>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate void InstanceVoidInvoker<TInstance, in TArg1, in TArg2, in TArg3, in TArg4>(ref TInstance instance, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);

    #endregion
}
