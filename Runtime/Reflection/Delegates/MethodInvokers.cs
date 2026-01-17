using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Delegate for invoking static methods with variable parameters.
    /// </summary>
    /// <param name="args">The method arguments.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate object StaticFuncInvoker(params object[] args);

    /// <summary>
    /// Delegate for invoking instance methods with variable parameters.
    /// </summary>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="args">The method arguments.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// When setting properties on a struct instance, modifications would be lost without <c>ref</c> as the setter
    /// would operate on a copy. Using <c>ref</c> ensures the original instance is modified.
    /// </remarks>
    public delegate object InstanceFuncInvoker(ref object instance, params object[] args);

    /// <summary>
    /// Generic delegate for invoking functions with a ref object instance and variable parameters.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="args">The method arguments.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// This delegate provides a strongly-typed alternative to <see cref="InstanceFuncInvoker"/>
    /// while maintaining the ability to pass the instance by reference for struct support.
    /// </remarks>
    public delegate TResult FuncRefObject<in TArgs, out TResult>(ref object instance, TArgs args);
}
