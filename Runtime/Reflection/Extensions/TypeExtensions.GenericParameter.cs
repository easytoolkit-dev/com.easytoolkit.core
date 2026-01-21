using System;
using System.Linq;
using System.Reflection;

namespace EasyToolKit.Core.Reflection
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified type satisfies all constraints of this generic parameter.
        /// </summary>
        /// <param name="genericParameter">The generic type parameter to check constraints against.</param>
        /// <param name="targetType">The type to check against the generic parameter constraints.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="targetType"/> satisfies all special constraints and type constraints;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool SatisfiesGenericParameterConstraints(this Type genericParameter, Type targetType)
        {
            if (genericParameter == null)
                throw new ArgumentNullException(nameof(genericParameter), "Generic parameter type cannot be null.");
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType), "Target type cannot be null.");
            if (!genericParameter.IsGenericParameter)
                throw new ArgumentException("The specified type must be a generic parameter.",
                    nameof(genericParameter));

            return TypeAnalyzerFactory.GetGenericParameterAnalyzer(genericParameter)
                .SatisfiesConstraints(targetType);
        }
    }
}
