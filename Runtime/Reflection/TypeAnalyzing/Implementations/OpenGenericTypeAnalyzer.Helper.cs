using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EasyToolkit.Core.Reflection.Implementations
{
    public partial class OpenGenericTypeAnalyzer
    {
        private static class Helper
        {
            public static Type[] GetGenericArgumentsRelativeTo(Type openGenericType, Type genericTypeDefinition)
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
                        return GetGenericArgumentsRelativeTo(openElementType, targetElementType);
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

            public static bool IsImplementsGenericDefinition(Type openGenericType, Type genericTypeDefinition)
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
                    if (IsImplementsGenericDefinition(iface, genericTypeDefinition))
                        return true;
                }

                return false;
            }

            public static Type[] GetCompletedGenericArguments([NotNull] Type openGenericType, [NotNull] Type targetType,
                bool allowTypeInheritance)
            {
                if (openGenericType == null)
                    throw new ArgumentNullException(nameof(openGenericType));
                if (targetType == null)
                    throw new ArgumentNullException(nameof(targetType));
                // Handle the case where openGenericType is a generic interface/base type of arrays (e.g., IList<T>)
                // and targetType is a concrete array type (e.g., int[])
                // Note: Use IsGenericType instead of IsGenericTypeDefinition to handle cases like IList<T2>
                // where T2 is a generic parameter from another type
                if (!openGenericType.IsArray && targetType.IsArray && openGenericType.IsGenericType)
                {
                    // Find the generic interface that the array implements matching the open generic type
                    foreach (var iface in targetType.GetInterfaces())
                    {
                        if (iface.IsGenericType && iface.GetGenericTypeDefinition() == openGenericType.GetGenericTypeDefinition())
                        {
                            // Found the matching generic interface, extract its type arguments
                            var genericArguments = iface.GetGenericArguments();
                            var existingTypeArguments = openGenericType.GetGenericArguments();
                            var missingTypeArguments = new List<Type>();

                            for (var i = 0; i < existingTypeArguments.Length; i++)
                            {
                                var existingTypeArgument = existingTypeArguments[i];
                                if (existingTypeArgument.IsGenericParameter)
                                {
                                    missingTypeArguments.Add(genericArguments[i]);
                                }
                                else
                                {
                                    if (existingTypeArgument != genericArguments[i])
                                    {
                                        throw new ArgumentException(
                                            $"Type arguments do not match. Open generic type: {openGenericType}, Target type: {targetType}");
                                    }
                                }
                            }

                            return missingTypeArguments.ToArray();
                        }
                    }

                    throw new ArgumentException(
                        $"The array type '{targetType}' does not implement generic type '{openGenericType}'.",
                        nameof(targetType));
                }

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
                    return GetCompletedGenericArguments(openElementType, targetElementType, allowTypeInheritance);
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
                    if (!IsImplementsGenericDefinition(targetType, openGenericType.GetGenericTypeDefinition()))
                    {
                        throw new ArgumentException(
                            $"The target type '{targetType}' must be derived from the open generic type '{openGenericType}'.",
                            nameof(targetType));
                    }
                }
                else
                {
                    if (!targetType.IsGenericType ||
                        targetType.GetGenericTypeDefinition() != openGenericType.GetGenericTypeDefinition())
                    {
                        throw new ArgumentException(
                            $"The generic type definition of target type '{targetType}' " +
                            $"must be same as the generic type definition of open generic type '{openGenericType}'.",
                            nameof(targetType));
                    }
                }

                {
                    var genericTypeDefinition = openGenericType.GetGenericTypeDefinition();

                    var typeArguments = allowTypeInheritance
                        ? GetGenericArgumentsRelativeTo(targetType, genericTypeDefinition)
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
    }
}
