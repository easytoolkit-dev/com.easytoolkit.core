using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyToolKit.Core.Reflection
{
    public static partial class TypeExtensions
    {
        public static Type[] GetGenericArgumentsRelativeTo(this Type openGenericType, Type genericTypeDefinition)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType));
            if (genericTypeDefinition == null)
                throw new ArgumentNullException(nameof(genericTypeDefinition));

            // Handle array types
            if (openGenericType.IsArray)
            {
                // If the target is also an array, process element types recursively
                if (genericTypeDefinition.IsArray)
                {
                    // Validate array ranks match
                    var openRank = openGenericType.GetArrayRank();
                    var targetRank = genericTypeDefinition.GetArrayRank();

                    if (openRank != targetRank)
                    {
                        throw new ArgumentException(
                            $"Array ranks do not match. Open generic array rank: {openRank}, Target array rank: {targetRank}");
                    }

                    // Get array element types
                    var openElementType = openGenericType.GetElementType();
                    var targetElementType = genericTypeDefinition.GetElementType();

                    if (openElementType == null || targetElementType == null)
                    {
                        throw new ArgumentException("Failed to get element type of array.");
                    }

                    // Recursively process element types
                    return openElementType.GetGenericArgumentsRelativeTo(targetElementType);
                }

                // If the target is a non-array generic definition (e.g., IList<T>),
                // get generic arguments from the implemented generic interface
                if (genericTypeDefinition.IsGenericTypeDefinition)
                {
                    var elementType = openGenericType.GetElementType();
                    if (elementType == null)
                    {
                        throw new ArgumentException("Failed to get element type of array.");
                    }

                    // Special case: if the element type is a generic parameter (e.g., T in T[]),
                    // return the generic parameter directly
                    if (elementType.IsGenericParameter)
                    {
                        // Find the corresponding type argument in the generic definition
                        var genericParams = genericTypeDefinition.GetGenericArguments();
                        if (genericParams.Length == 1)
                        {
                            return new[] { elementType };
                        }
                    }

                    // For concrete arrays (e.g., int[], string[]), find the implemented
                    // generic interface and extract its type arguments
                    foreach (var iface in openGenericType.GetInterfaces())
                    {
                        if (iface.IsGenericType && iface.GetGenericTypeDefinition() == genericTypeDefinition)
                        {
                            return iface.GetGenericArguments();
                        }
                    }

                    throw new InvalidOperationException(
                        $"Array type '{openGenericType.FullName}' does not implement generic type definition '{genericTypeDefinition.FullName}'");
                }

                throw new ArgumentException(
                    $"When the open type is an array, the target type must be either an array or a generic type definition.",
                    nameof(genericTypeDefinition));
            }

            // Handle generic parameters
            if (openGenericType.IsGenericParameter)
            {
                // If the open generic definition is a generic parameter,
                // return the corresponding type
                return new[] { genericTypeDefinition };
            }

            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new ArgumentException("genericTypeDefinition must be a generic type definition",
                    nameof(genericTypeDefinition));

            var typeMapping = BuildTypeMapping(openGenericType);
            var result = FindGenericArgumentsInHierarchy(openGenericType, genericTypeDefinition, typeMapping);

            if (result == null)
                throw new InvalidOperationException(
                    $"Type {openGenericType.FullName} does not implement or inherit from {genericTypeDefinition.FullName}");

            return result;
        }

        private static Type[] FindGenericArgumentsInHierarchy(Type typeToSearch, Type genericTypeDefinition,
            Dictionary<Type, Type> typeMapping)
        {
            if (typeToSearch.IsGenericType)
            {
                var genericDef = typeToSearch.GetGenericTypeDefinition();
                if (genericDef == genericTypeDefinition)
                {
                    return ResolveTypeArguments(typeToSearch.GetGenericArguments(), typeMapping);
                }
            }

            var baseType = typeToSearch.BaseType;
            if (baseType != null)
            {
                var baseResult = FindGenericArgumentsInHierarchy(baseType, genericTypeDefinition, typeMapping);
                if (baseResult != null)
                    return baseResult;
            }

            foreach (var interfaceType in typeToSearch.GetInterfaces())
            {
                var interfaceResult =
                    FindGenericArgumentsInHierarchy(interfaceType, genericTypeDefinition, typeMapping);
                if (interfaceResult != null)
                    return interfaceResult;
            }

            return null;
        }

        private static Dictionary<Type, Type> BuildTypeMapping(Type type)
        {
            var mapping = new Dictionary<Type, Type>();

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var genericParams = type.GetGenericTypeDefinition().GetGenericArguments();
                var actualArgs = type.GetGenericArguments();

                for (int i = 0; i < genericParams.Length; i++)
                {
                    mapping[genericParams[i]] = actualArgs[i];
                }
            }

            return mapping;
        }

        private static Type[] ResolveTypeArguments(Type[] typeArguments, Dictionary<Type, Type> typeMapping)
        {
            var result = new Type[typeArguments.Length];

            for (int i = 0; i < typeArguments.Length; i++)
            {
                var arg = typeArguments[i];

                if (arg.IsGenericParameter && typeMapping.TryGetValue(arg, out var resolvedType))
                {
                    result[i] = resolvedType;
                }
                else
                {
                    result[i] = arg;
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified open generic type is derived from or implements
        /// the given generic type definition, including interface implementations.
        /// </summary>
        /// <param name="openGenericType">The type to check. This can be a concrete type, generic type definition, or constructed generic type.</param>
        /// <param name="genericTypeDefinition">The generic type definition to compare with. Must be a generic type definition.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="openGenericType"/> is derived from or implements
        /// a type constructed from <paramref name="genericTypeDefinition"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="genericTypeDefinition"/> is not a generic type definition.
        /// </exception>
        public static bool IsDerivedFromGenericDefinition(this Type openGenericType, Type genericTypeDefinition)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType));
            if (genericTypeDefinition == null)
                throw new ArgumentNullException(nameof(genericTypeDefinition));
            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new ArgumentException("The specified type must be a generic type definition.",
                    nameof(genericTypeDefinition));

            // Quick check: if the types are the same generic definition
            if (openGenericType == genericTypeDefinition)
                return true;

            // Check base types (including the type itself)
            Type currentType = openGenericType;
            while (currentType != null)
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == genericTypeDefinition)
                    return true;
                currentType = currentType.BaseType;
            }

            // Check all implemented interfaces recursively
            var interfaces = openGenericType.GetInterfaces();
            foreach (var iface in interfaces)
            {
                if (iface.IsGenericType && iface.GetGenericTypeDefinition() == genericTypeDefinition)
                    return true;

                // Check if any of the interface's base interfaces match
                if (IsDerivedFromGenericDefinition(iface, genericTypeDefinition))
                    return true;
            }

            return false;
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
                                $"Type '{result[i]}' does not satisfy the constraints of generic parameter '{existingArgs[i]}'.",
                                nameof(providedTypeArguments));
                        }
                        continue;
                    }
                }
                result[i] = existingArgs[i];
            }

            return result;
        }

        /// <summary>
        /// Gets the supplementary generic type arguments from a target type relative to an open generic type.
        /// Supports array types (e.g., T[], MyClass&lt;T&gt;[]) by recursively processing element types.
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
            // Handle array types by processing their element types recursively
            if (openGenericType.IsArray)
            {
                if (!targetType.IsArray)
                {
                    throw new ArgumentException($"The target type '{targetType}' must both be arrays.",
                        nameof(targetType));
                }

                // Validate array ranks match
                var openRank = openGenericType.GetArrayRank();
                var targetRank = targetType.GetArrayRank();

                if (openRank != targetRank)
                {
                    throw new ArgumentException(
                        $"Array ranks do not match. Open generic array rank: {openRank}, Target array rank: {targetRank}");
                }

                // Get array element types
                var openElementType = openGenericType.GetElementType();
                var targetElementType = targetType.GetElementType();

                if (openElementType == null || targetElementType == null)
                {
                    throw new ArgumentException("Failed to get element type of array.");
                }

                // Recursively process element types
                return openElementType.GetCompletedGenericArguments(targetElementType, allowTypeInheritance);
            }

            // Handle non-generic type parameters (like T) by extracting from the target type
            // This case occurs when processing array element types that are generic parameters
            if (openGenericType.IsGenericParameter)
            {
                return new[] { targetType };
            }

            // Original logic for non-array, non-generic-parameter types
            if (!openGenericType.IsGenericType)
            {
                throw new ArgumentException(
                    "The openGenericType must be a generic type, generic parameter, or array of such types.");
            }

            if (allowTypeInheritance)
            {
                if (!targetType.IsDerivedFromGenericDefinition(openGenericType.GetGenericTypeDefinition()))
                {
                    throw new ArgumentException(
                        $"The target type '{targetType}' must be derived from the open generic type '{openGenericType}'.",
                        nameof(targetType));
                }
            }
            else
            {
                if (targetType.GetGenericTypeDefinition() != openGenericType.GetGenericTypeDefinition())
                {
                    throw new ArgumentException(
                        $"The generic type definition of target type '{targetType}' " +
                        $"must be same as the generic type definition of open generic type '{openGenericType}'.",
                        nameof(targetType));
                }
            }

            var genericTypeDefinition = openGenericType.GetGenericTypeDefinition();

            var typeArguments = allowTypeInheritance
                ? targetType.GetGenericArgumentsRelativeTo(genericTypeDefinition)
                : targetType.GetGenericArguments();
            var existingTypeArguments = openGenericType.GetGenericArguments();
            var missingTypeArguments = new List<Type>();

            for (var i = 0; i < existingTypeArguments.Length; i++)
            {
                var existingTypeArgument = existingTypeArguments[i];
                if (existingTypeArgument.IsGenericParameter)
                {
                    missingTypeArguments.Add(typeArguments[i]);
                }
                else
                {
                    if (existingTypeArgument != typeArguments[i])
                    {
                        throw new ArgumentException(
                            $"Type arguments do not match. Open generic type: {openGenericType}, Target type: {targetType}");
                    }
                }
            }

            return missingTypeArguments.ToArray();
        }
    }
}
