using System;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Delegate for getting values from static members (fields or properties).
    /// </summary>
    /// <returns>The value retrieved from the static member.</returns>
    public delegate object StaticGetter();

    /// <summary>
    /// Generic delegate for getting values from static members (fields or properties).
    /// </summary>
    /// <typeparam name="TValue">The type of value to retrieve.</typeparam>
    /// <returns>The value retrieved from the static member.</returns>
    public delegate TValue StaticGetter<out TValue>();
}
