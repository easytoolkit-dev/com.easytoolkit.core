using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EasyToolkit.Core.Reflection
{
    public static class TypeUtility
    {
        private static readonly Dictionary<string, Type> TypeByNameCache = new();
        private static readonly Dictionary<Type, string> TypeNameByTypeCache = new();
        private static readonly object CacheLock = new();

        /// <summary>
        /// Gets the type name for the specified type with caching.
        /// </summary>
        /// <param name="type">The type to get the name for.</param>
        /// <returns>
        /// The assembly-qualified name, full name, or simple name of the type.
        /// </returns>
        public static string GetTypeName([NotNull] Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Check cache first
            lock (CacheLock)
            {
                if (TypeNameByTypeCache.TryGetValue(type, out var cachedTypeName))
                {
                    return cachedTypeName;
                }
            }

            // Generate the type name
            string typeName = type.AssemblyQualifiedName ?? type.FullName ?? type.Name;

            // Cache the result
            lock (CacheLock)
            {
                TypeNameByTypeCache[type] = typeName;
            }

            return typeName;
        }

        /// <summary>
        /// Attempts to find a Type from the specified type name.
        /// </summary>
        /// <param name="typeName">
        /// The assembly-qualified name of the type to get.
        /// If the type is in the currently executing assembly or in Mscorlib.dll,
        /// it is sufficient to supply the type name qualified by its namespace.
        /// </param>
        /// <returns>The found Type object.</returns>
        [CanBeNull] public static Type FindType(string typeName)
        {
            ValidateTypeName(typeName);

            // Check cache first
            lock (CacheLock)
            {
                if (TypeByNameCache.TryGetValue(typeName, out var cachedType))
                {
                    return cachedType;
                }
            }

            // First, try Type.GetType which handles most common scenarios
            Type type = Type.GetType(typeName, throwOnError: false);
            if (type != null)
            {
                lock (CacheLock)
                {
                    TypeByNameCache[typeName] = type;
                }
                return type;
            }

            // If not found, search through all loaded assemblies
            type = SearchAllAssemblies(typeName);
            if (type != null)
            {
                lock (CacheLock)
                {
                    TypeByNameCache[typeName] = type;
                }
                return type;
            }

            // Cache negative result to avoid repeated searches
            lock (CacheLock)
            {
                TypeByNameCache[typeName] = null;
            }
            return null;
        }

        /// <summary>
        /// Searches for a type across all currently loaded assemblies.
        /// </summary>
        /// <param name="typeName">The name of the type to find.</param>
        /// <returns>
        /// The found Type object, or null if the type was not found.
        /// </returns>
        /// <remarks>
        /// This method searches through all loaded assemblies for the specified type.
        /// It first tries to match by assembly-qualified name, then by full name
        /// (namespace + type name), and finally by simple type name.
        /// </remarks>
        private static Type SearchAllAssemblies(string typeName)
        {
            var assemblies = AssemblyUtility.GetAllAssemblies();

            // First pass: exact match with assembly-qualified name
            foreach (var assembly in assemblies)
            {
                try
                {
                    Type type = assembly.GetType(typeName, throwOnError: false);
                    if (type != null)
                    {
                        return type;
                    }
                }
                catch (Exception)
                {
                    // Continue searching other assemblies
                    continue;
                }
            }

            // Second pass: match by full name (namespace + type name) or simple type name
            // Combined into a single pass to avoid duplicate GetTypes() calls
            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        // Check full name first (more specific)
                        if (type.FullName != null &&
                            string.Equals(type.FullName, typeName, StringComparison.Ordinal))
                        {
                            return type;
                        }

                        // Fall back to simple name (less specific)
                        if (string.Equals(type.Name, typeName, StringComparison.Ordinal))
                        {
                            return type;
                        }
                    }
                }
                catch (Exception)
                {
                    // Some assemblies may not be accessible
                    continue;
                }
            }

            return null;
        }

        /// <summary>
        /// Validates that the provided type name is not null or empty.
        /// </summary>
        /// <param name="typeName">The type name to validate.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="typeName"/> is null or empty.
        /// </exception>
        private static void ValidateTypeName(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentNullException(
                    nameof(typeName),
                    "Type name cannot be null or empty. " +
                    "Please provide a valid type name.");
            }
        }
    }
}
