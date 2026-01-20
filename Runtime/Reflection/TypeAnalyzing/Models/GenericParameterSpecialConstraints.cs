using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Represents special constraints that can be applied to generic type parameters.
    /// </summary>
    [Flags]
    public enum GenericParameterSpecialConstraints
    {
        /// <summary>
        /// No special constraints.
        /// </summary>
        None = 0,

        /// <summary>
        /// The type parameter must be a reference type (class constraint).
        /// </summary>
        ReferenceType = 1 << 0,

        /// <summary>
        /// The type parameter must be a value type (struct constraint).
        /// </summary>
        ValueType = 1 << 1,

        /// <summary>
        /// The type parameter must have a public parameterless constructor (new() constraint).
        /// </summary>
        DefaultConstructor = 1 << 2
    }
}
