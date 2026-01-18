using System;
using System.Linq;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Provides static APIs for resolving Type objects from type names.
    /// </summary>
    public static class TypeResolver
    {
        /// <summary>
        /// Attempts to resolve a Type from the specified type name.
        /// </summary>
        /// <param name="typeName">
        /// The assembly-qualified name of the type to get.
        /// If the type is in the currently executing assembly or in Mscorlib.dll,
        /// it is sufficient to supply the type name qualified by its namespace.
        /// </param>
        /// <returns>The resolved Type object.</returns>
        /// <exception cref="TypeLoadException">
        /// Thrown when the type cannot be found in any loaded assembly.
        /// </exception>
        public static Type ResolveType(string typeName)
        {
            ValidateTypeName(typeName);

            // First, try Type.GetType which handles most common scenarios
            Type type = Type.GetType(typeName, throwOnError: false);
            if (type != null)
            {
                return type;
            }

            // If not found, search through all loaded assemblies
            type = SearchAllAssemblies(typeName);
            if (type != null)
            {
                return type;
            }

            throw new TypeLoadException(
                $"Could not load type '{typeName}'. " +
                "The type was not found in any loaded assembly. " +
                "Please verify the assembly-qualified name and ensure " +
                "the containing assembly is referenced.");
        }

        /// <summary>
        /// Attempts to resolve a Type from the specified type name without throwing an exception.
        /// </summary>
        /// <param name="typeName">The name of the type to resolve.</param>
        /// <param name="type">
        /// When this method returns, contains the resolved Type object if successful;
        /// otherwise, null.
        /// </param>
        /// <returns>
        /// true if the type was successfully resolved; otherwise, false.
        /// </returns>
        public static bool TryResolveType(string typeName, out Type type)
        {
            type = null;

            if (string.IsNullOrWhiteSpace(typeName))
            {
                return false;
            }

            try
            {
                type = Type.GetType(typeName, throwOnError: false);
                if (type != null)
                {
                    return true;
                }

                type = SearchAllAssemblies(typeName);
                return type != null;
            }
            catch (Exception)
            {
                // Suppress all exceptions for the Try pattern
                return false;
            }
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
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

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

            // Second pass: match by full name (namespace + type name)
            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes()
                        .Where(t => t.FullName != null &&
                                    t.FullName.Equals(typeName, StringComparison.Ordinal))
                        .ToArray();

                    if (types.Length == 1)
                    {
                        return types[0];
                    }
                }
                catch (Exception)
                {
                    // Some assemblies may not be accessible
                    continue;
                }
            }

            // Third pass: match by simple type name (without namespace)
            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes()
                        .Where(t => t.Name.Equals(typeName, StringComparison.Ordinal))
                        .ToArray();

                    if (types.Length == 1)
                    {
                        return types[0];
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
