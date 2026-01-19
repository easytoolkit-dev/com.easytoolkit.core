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
}
