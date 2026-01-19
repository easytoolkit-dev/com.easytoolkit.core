using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Delegate for invoking constructors with variable parameters.
    /// </summary>
    /// <param name="args">The constructor arguments.</param>
    /// <returns>The created instance.</returns>
    public delegate object ConstructorInvoker(params object[] args);

    /// <summary>
    /// Delegate for invoking parameterless constructors.
    /// </summary>
    /// <returns>The created instance.</returns>
    public delegate object ParameterlessConstructorInvoker();

    /// <summary>
    /// Delegate for invoking parameterless constructors.
    /// </summary>
    /// <typeparam name="TInstance">The type of the constructed instance value.</typeparam>
    /// <returns>The created instance.</returns>
    public delegate TInstance ParameterlessConstructorInvoker<out TInstance>();
}
