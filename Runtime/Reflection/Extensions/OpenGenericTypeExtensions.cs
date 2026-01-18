using System;
using System.Collections.Generic;
using System.Linq;
using EasyToolKit.Core.Diagnostics;

namespace EasyToolKit.Core.Reflection
{
    public static class OpenGenericTypeExtensions
    {
        public static Type[] GetGenericArgumentsRelativeTo(this Type openGenericType, Type genericTypeDefinition)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType));
            if (genericTypeDefinition == null)
                throw new ArgumentNullException(nameof(genericTypeDefinition));

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

            var typeArguments = openGenericType.GetCompletedGenericArguments(providedTypeArguments);
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
        public static Type[] GetCompletedGenericArguments(this Type openGenericType,
            params Type[] providedTypeArguments)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType), "Open generic type cannot be null.");

            if (!openGenericType.IsGenericType)
                throw new ArgumentException("Type must be a generic type.", nameof(openGenericType));

            Type[] existingArgs = openGenericType.GetGenericArguments();
            int placeholderCount = existingArgs.Count(t => t.IsGenericParameter);
            int providedCount = providedTypeArguments.Length;

            if (providedCount != placeholderCount)
            {
                throw new ArgumentException(
                    $"Number of provided type arguments ({providedCount}) does not match " +
                    $"the number of remaining generic parameters ({placeholderCount}).",
                    nameof(providedTypeArguments));
            }

            Type[] result = new Type[existingArgs.Length];
            int providedIndex = 0;

            for (int i = 0; i < existingArgs.Length; i++)
            {
                if (existingArgs[i].IsGenericParameter)
                {
                    result[i] = providedTypeArguments[providedIndex++];
                }
                else
                {
                    result[i] = existingArgs[i];
                }
            }

            return result;
        }

        public static Type[] ExtractGenericTypeArguments(this Type openGenericType, Type concreteType,
            bool allowTypeInheritance = false)
        {
            if (openGenericType == null || concreteType == null)
            {
                throw new ArgumentNullException();
            }

            if (openGenericType.IsArray != concreteType.IsArray ||
                openGenericType.IsSZArray != concreteType.IsSZArray)
            {
                return new Type[] { };
            }

            if (concreteType.IsArray)
            {
                return new[] { concreteType.GetElementType() };
            }

            if (openGenericType.IsGenericParameter)
            {
                return new Type[] { concreteType };
            }

            if (!openGenericType.IsGenericType)
            {
                return new Type[] { };
            }

            if (!concreteType.IsGenericType ||
                openGenericType.GetGenericTypeDefinition() != concreteType.GetGenericTypeDefinition())
            {
                if (!allowTypeInheritance)
                {
                    return new Type[] { };
                }
            }

            var sourceArgs = openGenericType.GetGenericArguments();
            var targetArgs =
                concreteType.GetGenericArgumentsRelativeTo(openGenericType.GetGenericTypeDefinition());
            if (targetArgs.Length == 0)
            {
                return new Type[] { };
            }

            Assert.IsTrue(sourceArgs.Length == targetArgs.Length);

            var missingArgs = new List<Type>();
            for (int i = 0; i < sourceArgs.Length; i++)
            {
                if (sourceArgs[i].IsGenericParameter)
                {
                    missingArgs.Add(targetArgs[i]);
                }
            }

            return missingArgs.ToArray();
        }
    }
}
