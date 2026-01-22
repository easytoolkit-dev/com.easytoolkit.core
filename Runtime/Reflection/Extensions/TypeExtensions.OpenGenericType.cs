using System;

namespace EasyToolKit.Core.Reflection
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Gets the generic type arguments relative to the specified generic type definition.
        /// </summary>
        /// <param name="openGenericType">A partially constructed generic type or generic type definition.</param>
        /// <param name="genericTypeDefinition">The generic type definition to compare with. Must be a generic type definition.</param>
        /// <returns>
        /// An array of type arguments that correspond to the generic parameters of <paramref name="genericTypeDefinition"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="genericTypeDefinition"/> is not a generic type definition.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the analyzed type does not implement or inherit from <paramref name="genericTypeDefinition"/>.
        /// </exception>
        public static Type[] GetGenericArgumentsRelativeTo(this Type openGenericType, Type genericTypeDefinition)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType));
            if (genericTypeDefinition == null)
                throw new ArgumentNullException(nameof(genericTypeDefinition));

            return TypeAnalyzerFactory.GetOpenGenericTypeAnalyzer(openGenericType)
                .GetGenericArgumentsRelativeTo(genericTypeDefinition);
        }

        public static int GetGenericParameterCount(this Type openGenericType)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType));

            return TypeAnalyzerFactory.GetOpenGenericTypeAnalyzer(openGenericType)
                .GenericParameters.Count;
        }

        /// <summary>
        /// Determines whether the analyzed type implements the given generic type definition,
        /// including interface implementations and inheritance hierarchy.
        /// </summary>
        /// <param name="openGenericType">A partially constructed generic type or generic type definition.</param>
        /// <param name="genericTypeDefinition">The generic type definition to compare with. Must be a generic type definition.</param>
        /// <returns>
        /// <c>true</c> if the analyzed type implements or inherits from a type constructed from
        /// <paramref name="genericTypeDefinition"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="genericTypeDefinition"/> is not a generic type definition.
        /// </exception>
        public static bool IsImplementsGenericDefinition(this Type openGenericType, Type genericTypeDefinition)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType));
            if (genericTypeDefinition == null)
                throw new ArgumentNullException(nameof(genericTypeDefinition));
            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new ArgumentException("The specified type must be a generic type definition.",
                    nameof(genericTypeDefinition));

            IOpenGenericTypeAnalyzer analyzer;
            try
            {
                analyzer = TypeAnalyzerFactory.GetOpenGenericTypeAnalyzer(openGenericType);
            }
            catch (Exception e)
            {
                return false;
            }

            return analyzer.IsImplementsGenericDefinition(genericTypeDefinition);
        }

        public static bool SatisfiesConstraints(
            this Type openGenericType,
            params Type[] providedTypeArguments)
        {
            try
            {
                return openGenericType.MakeGenericTypeExtended(providedTypeArguments) != null;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Makes a concrete generic type by merging the partially constructed generic type's
        /// arguments with the provided type arguments.
        /// </summary>
        /// <param name="openGenericType">A partially constructed generic type, array type with generic elements, or generic type definition.</param>
        /// <param name="providedTypeArguments">Additional type arguments to fill the remaining generic parameters.</param>
        /// <returns>A concrete type constructed from the generic definition with all type parameters resolved.</returns>
        /// <exception cref="ArgumentException">Thrown when openGenericType is not a generic type or array,
        /// or when the number of provided type arguments doesn't match the number of remaining generic parameters.</exception>
        public static Type MakeGenericTypeExtended(
            this Type openGenericType,
            params Type[] providedTypeArguments)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType));

            if (providedTypeArguments == null)
                throw new ArgumentNullException(nameof(providedTypeArguments));

            // Handle array types
            if (openGenericType.IsArray)
            {
                return MakeGenericArrayType(openGenericType, providedTypeArguments);
            }

            // Handle regular generic types
            if (!openGenericType.IsGenericType)
                throw new ArgumentException($"Type '{openGenericType}' must be a generic type.",
                    nameof(openGenericType));

            if (openGenericType.IsGenericTypeDefinition)
            {
                return openGenericType.MakeGenericType(providedTypeArguments);
            }

            var typeArguments = openGenericType.GetMergedGenericArguments(providedTypeArguments);
            var genericTypeDefinition = openGenericType.GetGenericTypeDefinition();
            return genericTypeDefinition.MakeGenericType(typeArguments);
        }

        /// <summary>
        /// Handles the creation of array types with generic element types.
        /// </summary>
        /// <param name="arrayType">The array type with generic elements (e.g., T[], List<T>[,], etc.).</param>
        /// <param name="providedTypeArguments">Type arguments to substitute for generic parameters in the element type.</param>
        /// <returns>A concrete array type with all generic parameters resolved.</returns>
        private static Type MakeGenericArrayType(Type arrayType, Type[] providedTypeArguments)
        {
            if (!arrayType.IsArray)
                throw new ArgumentException($"Type '{arrayType}' must be an array.", nameof(arrayType));

            Type elementType = arrayType.GetElementType();

            // If the element type is not generic, just return the original array type
            if (!elementType.ContainsGenericParameters)
                return arrayType;

            // Recursively process the element type to resolve its generic parameters
            Type concreteElementType;

            if (elementType.IsArray)
            {
                // For multi-dimensional or jagged arrays, process recursively
                concreteElementType = MakeGenericArrayType(elementType, providedTypeArguments);
            }
            else if (elementType.IsGenericType)
            {
                // For generic element types, resolve the generic parameters
                concreteElementType = elementType.MakeGenericTypeExtended(providedTypeArguments);
            }
            else if (elementType.IsGenericParameter)
            {
                // For direct generic parameter element types (e.g., T[])
                int placeholderCount = 1; // Single generic parameter
                int providedCount = providedTypeArguments.Length;

                if (providedCount != placeholderCount)
                {
                    throw new ArgumentException(
                        $"Number of provided type arguments ({providedCount}) does not match " +
                        $"the number of remaining generic parameters ({placeholderCount}) in array element type.",
                        nameof(providedTypeArguments));
                }

                concreteElementType = providedTypeArguments[0];
            }
            else
            {
                // Non-generic element type
                return arrayType;
            }

            // Recreate the array with the same dimensions
            int rank = arrayType.GetArrayRank();

            if (rank == 1)
            {
                // Single-dimensional array
                return concreteElementType.MakeArrayType();
            }
            else
            {
                // Multi-dimensional array
                return concreteElementType.MakeArrayType(rank);
            }
        }

        /// <summary>
        /// Completes the generic type arguments by merging the partially constructed generic type's
        /// arguments with the provided type arguments.
        /// </summary>
        /// <param name="openGenericType">A partially constructed generic type or generic type definition.</param>
        /// <param name="providedTypeArguments">Additional type arguments to fill the remaining generic parameters.</param>
        /// <returns>An array of completed generic type arguments.</returns>
        /// <exception cref="ArgumentException">Thrown when openGenericType is not a generic type,
        /// or when the number of provided type arguments doesn't match the number of remaining generic parameters.</exception>
        public static Type[] GetMergedGenericArguments(this Type openGenericType,
            params Type[] providedTypeArguments)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType), "Open generic type cannot be null.");

            if (!openGenericType.IsGenericType)
                throw new ArgumentException("Type must be a generic type.", nameof(openGenericType));

            var existingArgs = openGenericType.GetGenericArguments();

            var result = new Type[existingArgs.Length];
            var providedIndex = 0;

            for (var i = 0; i < existingArgs.Length; i++)
            {
                if (existingArgs[i].IsGenericParameter)
                {
                    if (providedTypeArguments.Length > providedIndex)
                    {
                        result[i] = providedTypeArguments[providedIndex++];
                        if (!existingArgs[i].SatisfiesGenericParameterConstraints(result[i]))
                        {
                            throw new ArgumentException(
                                $"Type '{result[i]}' does not satisfy the constraints of generic parameter '{existingArgs[i]}'.");
                        }
                        continue;
                    }
                }
                result[i] = existingArgs[i];
            }

            return result;
        }

        /// <summary>
        /// Gets the supplementary generic type arguments from a target type relative to the analyzed open generic type.
        /// Supports array types (e.g., T[], List&lt;T&gt;[]) by recursively processing element types.
        /// </summary>
        /// <param name="openGenericType">The open generic type to compare against.</param>
        /// <param name="targetType">The target type from which to extract type arguments.</param>
        /// <param name="allowTypeInheritance">If true, allows type inheritance when extracting generic arguments.</param>
        /// <returns>An array of supplementary generic type arguments.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when array ranks don't match or when types are incompatible.
        /// </exception>
        public static Type[] GetCompletedGenericArguments(this Type openGenericType, Type targetType,
            bool allowTypeInheritance = false)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType), "Open generic type cannot be null.");

            return TypeAnalyzerFactory.GetOpenGenericTypeAnalyzer(openGenericType)
                .GetCompletedGenericArguments(targetType, allowTypeInheritance);
        }
    }
}
