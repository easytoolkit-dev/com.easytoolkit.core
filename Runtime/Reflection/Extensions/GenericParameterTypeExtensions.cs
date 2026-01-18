using System;
using System.Linq;
using System.Reflection;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Provides extension methods for <see cref="Type"/> objects representing generic type parameters.
    /// </summary>
    public static class GenericParameterTypeExtensions
    {
        /// <summary>
        /// Determines whether a target <see cref="Type"/> satisfies the constraints of a generic type parameter.
        /// </summary>
        /// <param name="genericParameter">The generic type parameter to check constraints against.</param>
        /// <param name="targetType">The <see cref="Type"/> to evaluate for compatibility with the generic parameter constraints.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="targetType"/> satisfies all constraints of the <paramref name="genericParameter"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="genericParameter"/> is not a generic parameter type.
        /// </exception>
        /// <remarks>
        /// This method validates whether the <paramref name="targetType"/> meets all constraints defined for the generic parameter,
        /// including class constraints, interface constraints, and special constraints (e.g., <c>new()</c>, <c>struct</c>, <c>class</c>).
        /// It returns <c>true</c> only if the target type is assignable to the generic parameter considering all its constraints.
        /// </remarks>
        public static bool SatisfiesGenericParameterConstraints(this Type genericParameter, Type targetType)
        {
            if (genericParameter == null)
                throw new ArgumentNullException(nameof(genericParameter), "Generic parameter type cannot be null.");
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType), "Target type cannot be null.");
            if (!genericParameter.IsGenericParameter)
                throw new ArgumentException("The specified type must be a generic parameter.",
                    nameof(genericParameter));

            // Check special constraints (new(), struct, class)
            if (!ValidateSpecialConstraints(genericParameter, targetType))
                return false;

            // Check type constraints (base class and interface constraints)
            var constraints = genericParameter.GetGenericParameterConstraints();
            foreach (var constraint in constraints)
            {
                if (!constraint.IsAssignableFrom(targetType))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Validates whether the target type satisfies the special constraints of the generic parameter.
        /// </summary>
        /// <param name="genericParameter">The generic type parameter.</param>
        /// <param name="targetType">The target type to validate.</param>
        /// <returns><c>true</c> if all special constraints are satisfied; otherwise, <c>false</c>.</returns>
        private static bool ValidateSpecialConstraints(Type genericParameter, Type targetType)
        {
            var attributes = genericParameter.GenericParameterAttributes;

            // Check 'struct' constraint (value type)
            if ((attributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
            {
                if (!targetType.IsValueType || (targetType.IsGenericType &&
                                                targetType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    return false;
            }

            // Check 'class' constraint (reference type)
            if ((attributes & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
            {
                if (targetType.IsValueType)
                    return false;
            }

            // Check 'new()' constraint (parameterless constructor)
            if ((attributes & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
            {
                if (targetType.IsAbstract || targetType.GetConstructor(Type.EmptyTypes) == null)
                {
                    // For value types, we consider them to have a default constructor even if not explicitly defined
                    if (!targetType.IsValueType)
                        return false;
                }
            }

            return true;
        }
    }
}
