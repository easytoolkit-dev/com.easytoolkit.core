using System;

namespace EasyToolKit.Core.Reflection
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
}
