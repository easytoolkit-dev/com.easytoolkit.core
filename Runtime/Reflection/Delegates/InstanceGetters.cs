using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Delegate for getting values from instance members (fields or properties).
    /// </summary>
    /// <param name="instance">The instance to get the value from.</param>
    /// <returns>The value retrieved from the instance member.</returns>
    public delegate object InstanceGetter(object instance);

    /// <summary>
    /// Generic delegate for getting values from instance members (fields or properties).
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TValue">The type of value to retrieve.</typeparam>
    /// <param name="instance">The instance to get the value from. Passed by reference to support value types.</param>
    /// <returns>The value retrieved from the instance member.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// </remarks>
    public delegate TValue InstanceGetter<TInstance, out TValue>(ref TInstance instance);
}
