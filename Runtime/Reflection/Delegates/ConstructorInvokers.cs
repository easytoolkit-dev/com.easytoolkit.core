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
    /// <typeparam name="TReturn">The type of the return value (the constructed instance type).</typeparam>
    /// <returns>The created instance.</returns>
    public delegate TReturn ConstructorInvoker<out TReturn>();
}
