using System;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Delegate for setting values to instance members.
    /// </summary>
    /// <param name="instance">The instance containing the member to set. Passed by reference to support value types.</param>
    /// <param name="value">The value to set.</param>
    /// <remarks>
    /// <para>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// When setting properties on a struct instance, modifications would be lost without <c>ref</c> as the setter
    /// would operate on a copy. Using <c>ref</c> ensures the original instance is modified.
    /// </para>
    /// <para><b>Why boxed instances require <c>ref</c>:</b></para>
    /// <para>
    /// Even though a boxed struct is passed as a reference type (<c>object</c>), the setter needs to modify
    /// fields/properties on the boxed value. Without <c>ref</c>, the setter operates on a copy created during
    /// unboxing, leaving the original boxed instance unchanged. With <c>ref</c>, the boxed reference itself
    /// can be replaced with a new boxed instance containing the modifications.
    /// </para>
    /// </remarks>
    public delegate void InstanceSetter(ref object instance, object value);

    /// <summary>
    /// Generic delegate for setting values to instance members.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TValue">The type of value to set.</typeparam>
    /// <param name="instance">The instance containing the member to set. Passed by reference to support value types.</param>
    /// <param name="value">The value to set.</param>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// When setting properties on a struct instance, modifications would be lost without <c>ref</c> as the setter
    /// would operate on a copy. Using <c>ref</c> ensures the original instance is modified.
    /// </remarks>
    public delegate void InstanceSetter<TInstance, in TValue>(ref TInstance instance, TValue value);
}
