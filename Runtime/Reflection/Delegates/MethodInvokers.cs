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
    /// <param name="instance">The instance to invoke the method on.</param>
    /// <param name="args">The method arguments.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate object InstanceFuncInvoker(object instance, params object[] args);
}
