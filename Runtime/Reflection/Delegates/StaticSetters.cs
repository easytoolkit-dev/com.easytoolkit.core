using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Delegate for setting values to static members (fields or properties).
    /// </summary>
    /// <param name="value">The value to set.</param>
    public delegate void StaticSetter(object value);

    /// <summary>
    /// Generic delegate for setting values to static members (fields or properties).
    /// </summary>
    /// <typeparam name="TValue">The type of value to set.</typeparam>
    /// <param name="value">The value to set.</param>
    public delegate void StaticSetter<in TValue>(TValue value);
}
